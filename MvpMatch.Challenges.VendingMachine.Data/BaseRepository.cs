using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using MvpMatch.Challenges.VendingMachine.Common.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Data
{
    public abstract class BaseRepository
    {
        static BaseRepository()
        {
            SqlMapperExtensions.TableNameMapper = (type) => type.Name;
        }

        protected void Execute<T>(Action<SqlConnection> action)
        {
            try
            {
                using (var connection = new SqlConnection(VendingMachineSettings.ConnectionString))
                {
                    connection.Open();
                    action(connection);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected T Execute<T>(Func<SqlConnection, T> func)
        {
            try
            {
                using (var connection = new SqlConnection(VendingMachineSettings.ConnectionString))
                {
                    connection.Open();
                    return func(connection);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected IEnumerable<T> Execute<T>(Func<SqlConnection, IEnumerable<T>> func)
        {
            try
            {
                using (var connection = new SqlConnection(VendingMachineSettings.ConnectionString))
                {
                    connection.Open();
                    return func(connection) ?? Enumerable.Empty<T>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected IEnumerable<T> ExecuteText<T>(string sqlQuery)
        {
            return Execute(connection =>
                connection.Query<T>(
                    commandType: CommandType.Text,
                    sql: sqlQuery
                )
            );
        }

        protected IEnumerable<T> ExecuteText<T>(string sqlQuery, object input)
        {
            return Execute(connection =>
                connection.Query<T>(
                    commandType: CommandType.Text,
                    sql: sqlQuery,
                    param: input
                )
            );
        }

        protected IEnumerable<T> ExecuteStoredProcedure<T>(string procedureName, object input)
        {
            return Execute(connection =>
                connection.Query<T>(
                    commandType: CommandType.StoredProcedure,
                    sql: procedureName,
                    param: input
                )
            );
        }
    }

    public abstract class BaseRepository<T> : BaseRepository
        where T : class
    {
        public void Delete(int id)
        {
            Execute<T>(conn =>
            {
                var entity = conn.Get<T>(id);
                if (entity != null)
                {
                    conn.Delete(entity);
                }
            });
        }

        public T Get(int id)
            => Execute(conn => conn.Get<T>(id));

        public T Create(T entity)
        {
            return Execute(conn =>
            {
                var newId = conn.Insert(entity);
                return conn.Get<T>(newId);
            });
        }

        public T Update(T entity)
            => Execute(conn => {
                conn.Update(entity);
                return entity;
            });
    }
}
