using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AdoDataLayerAbstraction
{
    /// <summary>
    /// Allows retrying sql commands before failing.
    /// </summary>
    public class DbRetryer : GraduatedWaitTimeIncrementalRetryer
    {

        /// <summary>
        /// The default retry count for this retryer
        /// </summary>
        public const int DefaultRetryCount = 5;

        /// <summary>
        /// A default logging message that can be used for logging information.
        /// </summary>
        public const string DefaultMessage = "Retrying Query";

        private const int SqlDeadLockErrorNumber = 1205;
        private const int SqlTimeoutErrorNumber = -2;
        private const int SqlBulkLoadSchemaChangeErrorNumber1 = 4891;
        private const int SqlBulkLoadSchemaChangeErrorNumber2 = 4892;
        private const int SqlErrorDuringLoginProcess = 233;
        private const int ConnectionForciblyClosed = 10054;

        private static readonly List<int> SqlServerErrorCodes = new List<int>
        {
            SqlDeadLockErrorNumber,
            SqlTimeoutErrorNumber,
            SqlBulkLoadSchemaChangeErrorNumber1,
            SqlBulkLoadSchemaChangeErrorNumber2,
            SqlErrorDuringLoginProcess,
            ConnectionForciblyClosed,
        };

        protected internal DbRetryer(int retryCount)
            : base(TimeSpan.FromSeconds(5), retryCount, WaitTimeIncrement)
        {
            Options.GetIsBlackListed = GetIsBlackListed;
            Options.OnBlackListExceptionThrow = true;
            Options.ThrowFinalException = true;
        }

        public override void OnRetry(Exception exception)
        {
            if (LoggingMethod != null)
            {
                LoggingMethod(exception);
            }
        }

        /// <summary>
        /// Gets or sets the logging method.
        /// </summary>
        public Action<Exception> LoggingMethod { get; set; }

        protected virtual bool GetIsBlackListed(Exception exception)
        {
            SqlException sqlEx = exception as SqlException;
            //All exceptions that are not sql exceptions are blacklisted.
            //All sql exceptions are blacklisted except deadlock and timeout.
            return sqlEx == null || !SqlServerErrorCodes.Contains(sqlEx.Number);
        }

        protected static TimeSpan WaitTimeIncrement(TimeSpan currentWaitTime, int currentRetryCount)
        {
            return TimeSpan.FromSeconds(5 * currentRetryCount);
        }

        /// <summary>
        /// Retries the method <see cref="DefaultRetryCount"/> times.
        /// </summary>
        /// <typeparam name="TReturnType">The value type that is returned from the method.</typeparam>
        /// <param name="retryMethod">The method to retry.</param>
        /// <returns>The value returned from the retry method.</returns>
        public TReturnType Retry<TReturnType>(Func<TReturnType> retryMethod)
        {
            return Retry(DefaultRetryCount, retryMethod);
        }

        /// <summary>
        /// Retries the method <see cref="retryCount"/> times.
        /// </summary>
        /// <typeparam name="TReturnType">The value type that is returned from the method.</typeparam>
        /// <param name="retryCount">The number of times to try the operation before failing.</param>
        /// <param name="retryMethod">The method to retry.</param>
        /// <returns>The value returned from the retry method.</returns>
        public TReturnType Retry<TReturnType>(int retryCount, Func<TReturnType> retryMethod)
        {
            TReturnType returnValue = default(TReturnType);
            Retryer.Retry(this, () => { returnValue = retryMethod(); });
            return returnValue;
        }

        /// <summary>
        /// Retries the method <see cref="DefaultRetryCount"/> times.
        /// </summary>
        /// <param name="retryMethod">The method to retry.</param>
        public void Retry(Action retryMethod)
        {
            Retry(DefaultRetryCount, retryMethod);
        }

        /// <summary>
        /// Retries the method <see cref="retryCount"/> times.
        /// </summary>
        /// <param name="retryCount">The number of times to try the operation before failing.</param>
        /// <param name="retryMethod">The method to retry.</param>
        public void Retry(int retryCount, Action retryMethod)
        {
            Retryer.Retry(this, retryMethod);
        }

    }

}
