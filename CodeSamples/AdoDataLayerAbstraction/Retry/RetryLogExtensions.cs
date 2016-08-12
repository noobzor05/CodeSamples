using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoDataLayerAbstraction
{
    public static class RetryLogExtensions
    {

        /// <summary>
        /// Logs any retry operations on the command with the specified source information.
        /// </summary>
        /// <param name="command">The command to log.</param>
        /// <param name="source">The source information to log.</param>
        /// <returns>The same supplied command object.</returns>
        public static IDbCommand LogOnRetry(this IDbRetryCommand command, string source)
        {
            command.Retryer.LoggingMethod = exception => Log(command.CommandText, source, DbRetryer.DefaultMessage);
            return command;
        }

        /// <summary>
        /// Logs any retry operations on the command with the specified source information.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="source">The source information to log.</param>
        /// <param name="message">The mesage to log.</param>
        /// <returns></returns>
        public static IDbCommand LogOnRetry(this IDbRetryCommand command, string source, string message)
        {
            command.Retryer.LoggingMethod = exception => Log(command.CommandText, source, message);
            return command;
        }

        /// <summary>
        /// Logs any retry operations on the connection with the specified source information.
        /// </summary>
        /// <param name="connection">The connection to log.</param>
        /// <param name="source">The source information to log.</param>
        /// <returns></returns>
        public static IDbConnection LogOnRetry(this IDbRetryConnection connection, string source)
        {
            connection.Retryer.LoggingMethod = exception => Log(connection.ConnectionString, source, DbRetryer.DefaultMessage);
            return connection;
        }

        /// <summary>
        /// Logs any retry operations on the connection with the specified source information.
        /// </summary>
        /// <param name="connection">The connection to log.</param>
        /// <param name="source">The source information to log.</param>
        /// <param name="message">The mesage to log.</param>
        /// <returns></returns>
        public static IDbConnection LogOnRetry(this IDbRetryConnection connection, string source, string message)
        {
            connection.Retryer.LoggingMethod = exception => Log(connection.ConnectionString, source, message);
            return connection;
        }

        private static void Log(string debugInfo, string source, string message)
        {
            //TODO: Write to log (the actual logger used was proprietary, so I removed it)
        }


    }
}
