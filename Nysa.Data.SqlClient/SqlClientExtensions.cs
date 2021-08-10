﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.TSql;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.SqlClient
{

    public static class SqlClientExtensions
    {
        private static U Map<T, U>(this T value, Func<T, U> transform) => transform(value);

        private static void Process<T>(this T value, Action<T> action) => action(value);


        public static Func<SqlResultReader, T?> ReadValue<T>(this Func<SqlDataReader, T?> transform)
            where T : struct
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return (T?)null;
            };

        public static Func<SqlResultReader, T?> ReadRecord<T>(this Func<SqlDataReader, T> transform)
            where T : struct
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return (T?)null;
            };

        public static Func<SqlResultReader, T?> ReadObject<T>(this Func<SqlDataReader, T> transform)
            where T : class?
            => rr =>
            {
                if (rr.ReadResult() && rr.ReadRow())
                    return transform(rr.Row);

                return null;
            };

        public static Func<SqlResultReader, List<T>> ReadValues<T>(this Func<SqlDataReader, T?> transform)
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

        public static Func<SqlResultReader, List<T>> ReadRecords<T>(this Func<SqlDataReader, T> transform)
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

        public static Func<SqlResultReader, List<T>> ReadObjects<T>(this Func<SqlDataReader, T> transform)
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

        public static Func<SqlResultReader, T> Then<P, N, T>(this Func<SqlResultReader, P> prior, Func<SqlResultReader, N> next, Func<P, N, T> transform)
            => rr => prior(rr).Map(p => next(rr).Map(n => transform(p, n)));

        public static Func<SqlConnection, T> ForQuery<T>(this Func<SqlResultReader, T> resultTransform, String query, CommandType commandType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default)
            => connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = commandType;

                    using (var reader = new SqlResultReader(command.ExecuteReader(commandBehavior)))
                    {
                        return resultTransform(reader);
                    }
                }
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

        public static Func<Unit> ExecuteOn(this SqlScript @this, String connectionString)
            => () =>
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in @this.Batches())
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

        public static Func<Task<Unit>> ExecuteOnAsync(this SqlScript @this, String connectionString)
            => async () =>
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var batch in @this.Batches())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = batch;
                            command.CommandType = CommandType.Text;

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
