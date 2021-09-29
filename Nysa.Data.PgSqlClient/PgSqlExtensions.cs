using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Nysa.Logics;

using Npgsql;

namespace Nysa.Data.PgSqlClient
{

    public static class PgSqlExtensions
    {
        private static U Map<T, U>(this T value, Func<T, U> transform) => transform(value);

        private static void Process<T>(this T value, Action<T> action) => action(value);


        public static Func<PgSqlResultReader, T?> ReadValue<T>(this Func<NpgsqlDataReader, T?> transform)
            where T : struct
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return (T?)null;
            };

        public static Func<PgSqlResultReader, T?> ReadRecord<T>(this Func<NpgsqlDataReader, T> transform)
            where T : struct
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return (T?)null;
            };

        public static Func<PgSqlResultReader, T?> ReadObject<T>(this Func<NpgsqlDataReader, T> transform)
            where T : class?
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return null;
            };

        public static Func<PgSqlResultReader, List<T>> ReadValues<T>(this Func<NpgsqlDataReader, T?> transform)
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

        public static Func<PgSqlResultReader, List<T>> ReadRecords<T>(this Func<NpgsqlDataReader, T> transform)
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

        public static Func<PgSqlResultReader, List<T>> ReadObjects<T>(this Func<NpgsqlDataReader, T> transform)
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

        public static Func<PgSqlResultReader, T> Then<P, N, T>(this Func<PgSqlResultReader, P> prior, Func<PgSqlResultReader, N> next, Func<P, N, T> transform)
            => rr => prior(rr).Map(p => next(rr).Map(n => transform(p, n)));

        public static Func<NpgsqlConnection, T> ForQuery<T>(this Func<PgSqlResultReader, T> resultTransform, String query, Int32? timeout = null)
            => connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    if (timeout != null)
                        command.CommandTimeout = timeout.Value;

                    using (var reader = new PgSqlResultReader(command.ExecuteReader(CommandBehavior.Default)))
                    {
                        return resultTransform(reader);
                    }
                }
            };

        public static Func<NpgsqlConnection, Unit> ToExecute(this PgSqlScript script, Int32? batchTimeout = null)
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

        public static Func<NpgsqlConnection, T> Then<P, N, T>(this Func<NpgsqlConnection, P> prior, Func<NpgsqlConnection, N> next, Func<P, N, T> transform)
            => connection => prior(connection).Map(p => next(connection).Map(n => transform(p, n)));

        public static Func<T> ExecuteOn<T>(this Func<NpgsqlConnection, T> query, String connectionString)
            => () =>
            {
                T result;

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    result = query(connection);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return result;
            };

        public static Func<Unit> ExecuteOn(this IEnumerable<String> batches, String connectionString)
            => () =>
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in batches)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = batch;
                            command.CommandType = CommandType.Text;

                            command.ExecuteNonQuery();
                        }
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return Unit.Value;
            };

        public static Func<Unit> ExecuteOn(this PgSqlScript @this, String connectionString, Int32? batchTimeout = null)
            => () =>
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in @this.Batches())
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

        public static Func<Task<Unit>> ExecuteOnAsync(this PgSqlScript @this, String connectionString, Int32? batchTimeout = null)
            => async () =>
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in @this.Batches())
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

        public static Func<Task<Unit>> ExecuteOnAsync(this IEnumerable<String> batches, String connectionString, Int32? timeout = null)
            => async () =>
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in batches)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = batch;
                            command.CommandType = CommandType.Text;
                            
                            if (timeout != null)
                                command.CommandTimeout = timeout.Value;

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
