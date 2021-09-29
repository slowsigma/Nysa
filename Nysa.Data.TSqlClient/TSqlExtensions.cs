using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.TSql;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.TSqlClient
{

    public static class TSqlExtensions
    {
        private static U Map<T, U>(this T value, Func<T, U> transform) => transform(value);

        private static void Process<T>(this T value, Action<T> action) => action(value);


        public static Func<TSqlResultReader, T?> ReadValue<T>(this Func<SqlDataReader, T?> transform)
            where T : struct
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return (T?)null;
            };

        public static Func<TSqlResultReader, T?> ReadRecord<T>(this Func<SqlDataReader, T> transform)
            where T : struct
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return (T?)null;
            };

        public static Func<TSqlResultReader, T?> ReadObject<T>(this Func<SqlDataReader, T> transform)
            where T : class?
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return null;
            };

        public static Func<TSqlResultReader, List<T>> ReadValues<T>(this Func<SqlDataReader, T?> transform)
            where T : struct
            => rr =>
            {
                var values = new List<T>();

                if (rr.ReadResult())
                {
                    while (rr.ReadRow())
                        transform(rr.Row).Process(t => { if (t.HasValue) values.Add(t.Value); });
                }

                return values;
            };

        public static Func<TSqlResultReader, List<T>> ReadRecords<T>(this Func<SqlDataReader, T> transform)
            where T : struct
            => rr =>
            {
                var values = new List<T>();

                if (rr.ReadResult())
                {
                    while (rr.ReadRow())
                        values.Add(transform(rr.Row));
                }

                return values;
            };

        public static Func<TSqlResultReader, List<T>> ReadObjects<T>(this Func<SqlDataReader, T> transform)
            where T : class
            => rr =>
            {
                var objects = new List<T>();

                if (rr.ReadResult())
                {
                    while (rr.ReadRow())
                        objects.Add(transform(rr.Row));
                }

                return objects;
            };

        public static Func<TSqlResultReader, T> Then<P, N, T>(this Func<TSqlResultReader, P> prior, Func<TSqlResultReader, N> next, Func<P, N, T> transform)
            => rr => prior(rr).Map(p => next(rr).Map(n => transform(p, n)));

        public static Func<SqlConnection, T> ForQuery<T>(this Func<TSqlResultReader, T> resultTransform, String query, Int32? timeout = null)
            => connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    if (timeout != null)
                        command.CommandTimeout = timeout.Value;

                    using (var reader = new TSqlResultReader(command.ExecuteReader(CommandBehavior.Default)))
                    {
                        return resultTransform(reader);
                    }
                }
            };

        public static Func<SqlConnection, Unit> ToExecute(this TSqlScript script, Int32? batchTimeout = null)
            => connection =>
            {
                foreach (var batch in script.Batches())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = batch;
                        command.CommandType = CommandType.Text;

                        if (batchTimeout != null)
                            command.CommandTimeout = batchTimeout.Value;

                        command.ExecuteNonQuery();
                    }
                }

                return Unit.Value;
            };

        public static Func<SqlConnection, T> Then<P, N, T>(this Func<SqlConnection, P> prior, Func<SqlConnection, N> next, Func<P, N, T> transform)
            => connection => prior(connection).Map(p => next(connection).Map(n => transform(p, n)));

        public static Func<T> ExecuteOn<T>(this Func<SqlConnection, T> query, String connectionString)
            => () =>
            {
                T result;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    result = query(connection);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return result;
            };

        public static Func<Unit> ExecuteOn(this TSqlScript script, String connectionString, Int32? batchTimeout = null)
            => () =>
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in script.Batches())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = batch;
                            command.CommandType = CommandType.Text;

                            if (batchTimeout != null)
                                command.CommandTimeout = batchTimeout.Value;

                            command.ExecuteNonQuery();
                        }
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return Unit.Value;
            };

        public static Func<Task<Unit>> ExecuteOnAsync(this TSqlScript script, String connectionString, Int32? batchTimeout = null)
            => async () =>
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in script.Batches())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = batch;
                            command.CommandType = CommandType.Text;
                            
                            if (batchTimeout != null)
                                command.CommandTimeout = batchTimeout.Value;

                            var result = await command.ExecuteNonQueryAsync();
                        }
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return Unit.Value;
            };

        public static Func<Task<Unit>> ExecuteOnAsync(this IEnumerable<String> batches, String connectionString, Int32? batchTimeout = null)
            => async () =>
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in batches)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = batch;
                            command.CommandType = CommandType.Text;
                            
                            if (batchTimeout != null)
                                command.CommandTimeout = batchTimeout.Value;

                            var result = await command.ExecuteNonQueryAsync();
                        }
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return Unit.Value;
            };

    }

}
