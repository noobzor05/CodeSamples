using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoDataLayerAbstraction
{
    public static class RetryConnectionExtensions
    {

        private class RetryConnection : IDbRetryConnection
        {

            private readonly IDbConnection _Connection;

            public RetryConnection(IDbConnection connection, int retryCount)
            {
                _Connection = connection;
                Retryer = new DbRetryer(retryCount);
            }

            public void Dispose()
            {
                _Connection.Dispose();
            }

            public IDbTransaction BeginTransaction()
            {
                return _Connection.BeginTransaction();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                return _Connection.BeginTransaction(il);
            }

            public void Close()
            {
                _Connection.Close();
            }

            public void ChangeDatabase(string databaseName)
            {
                _Connection.ChangeDatabase(databaseName);
            }

            public IDbCommand CreateCommand()
            {
                return _Connection.CreateCommand();
            }

            public void Open()
            {
                Retryer.Retry(() => _Connection.Open());
            }

            public string ConnectionString
            {
                get
                {
                    return _Connection.ConnectionString;
                }
                set
                {
                    _Connection.ConnectionString = value;
                }
            }

            public int ConnectionTimeout
            {
                get
                {
                    return _Connection.ConnectionTimeout;
                }
            }

            public string Database
            {
                get
                {
                    return _Connection.Database;
                }
            }

            public ConnectionState State
            {
                get
                {
                    return _Connection.State;
                }
            }

            public DbRetryer Retryer { get; private set; }
        }

        /// <summary>
        /// Gets an <see cref="IDbConnection"/> instance that will retry open operations.
        /// </summary>
        /// <param name="connection">The connection for which to return a connection instance.</param>
        /// <returns>A connection instance.</returns>
        public static IDbRetryConnection Retry(this IDbConnection connection)
        {
            return Retry(connection, DbRetryer.DefaultRetryCount);
        }

        /// <summary>
        /// Gets an <see cref="IDbConnection"/> instance that will retry open operations.
        /// </summary>
        /// <param name="connection">The connection for which to return a connection instance.</param>
        /// <param name="retryCount">The number of retry operations to perform.</param>
        /// <returns></returns>
        public static IDbRetryConnection Retry(this IDbConnection connection, int retryCount)
        {
            return new RetryConnection(connection, retryCount);
        }

    }

}
