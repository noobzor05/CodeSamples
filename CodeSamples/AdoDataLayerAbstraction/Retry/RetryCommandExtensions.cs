using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoDataLayerAbstraction
{
    public static class RetryCommandExtensions
    {

        private class RetryCommand : IDbRetryCommand
        {

            private readonly IDbCommand _Command;

            public RetryCommand(IDbCommand command, int retryCount)
            {
                _Command = command;
                Retryer = new DbRetryer(retryCount);
            }

            public void Dispose()
            {
                _Command.Dispose();
            }

            public void Prepare()
            {
                _Command.Prepare();
            }

            public void Cancel()
            {
                _Command.Cancel();
            }

            public IDbDataParameter CreateParameter()
            {
                return _Command.CreateParameter();
            }

            public int ExecuteNonQuery()
            {
                return Retryer.Retry(() => _Command.ExecuteNonQuery());
            }

            public IDataReader ExecuteReader()
            {
                return Retryer.Retry(() => _Command.ExecuteReader());
            }

            public IDataReader ExecuteReader(CommandBehavior behavior)
            {
                return Retryer.Retry(() => _Command.ExecuteReader(behavior));
            }

            public object ExecuteScalar()
            {
                return Retryer.Retry(() => _Command.ExecuteScalar());
            }

            public IDbConnection Connection
            {
                get
                {
                    return _Command.Connection;
                }
                set
                {
                    _Command.Connection = value;
                }
            }

            public IDbTransaction Transaction
            {
                get
                {
                    return _Command.Transaction;
                }
                set
                {
                    _Command.Transaction = value;
                }
            }

            public string CommandText
            {
                get
                {
                    return _Command.CommandText;
                }
                set
                {
                    _Command.CommandText = value;
                }
            }

            public int CommandTimeout
            {
                get
                {
                    return _Command.CommandTimeout;
                }
                set
                {
                    _Command.CommandTimeout = value;
                }
            }

            public CommandType CommandType
            {
                get
                {
                    return _Command.CommandType;
                }
                set
                {
                    _Command.CommandType = value;
                }
            }

            public IDataParameterCollection Parameters
            {
                get
                {
                    return _Command.Parameters;
                }
            }

            public UpdateRowSource UpdatedRowSource
            {
                get
                {
                    return _Command.UpdatedRowSource;
                }
                set
                {
                    _Command.UpdatedRowSource = value;
                }
            }

            public DbRetryer Retryer { get; private set; }
        }

        /// <summary>
        /// Gets an <see cref="IDbCommand"/> instance that will retry execute operations.
        /// </summary>
        /// <param name="command">The command object for which to return a command instance.</param>
        /// <returns>A command instance.</returns>
        public static IDbRetryCommand Retry(this IDbCommand command)
        {
            return Retry(command, DbRetryer.DefaultRetryCount);
        }

        /// <summary>
        /// Gets an <see cref="IDbCommand"/> instance that will retry execute operations.
        /// </summary>
        /// <param name="command">The command object for which to return a command instance.</param>
        /// <param name="retryCount">The number of retry operations to perform.</param>
        /// <returns>A command instance.</returns>
        public static IDbRetryCommand Retry(this IDbCommand command, int retryCount)
        {
            return new RetryCommand(command, retryCount);
        }

    }

}
