using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public static class Retryer
    {
        /// <summary>
        /// Executes the method the maximum number of retries waiting the specified wait time between each retry.
        /// </summary>
        /// <param name="waitTime">The time to wait between each successive retry.</param>
        /// <param name="maxRetries">The maximum number of retries to allow before failing.</param>
        /// <param name="retryMethod">The method to retry.</param>
        /// <returns>A value containing the results of the operation.</returns>
        public static RetryerResults Retry(TimeSpan waitTime, int maxRetries, Action retryMethod)
        {
            WaitTimeIncrementalRetryer retryer = new WaitTimeIncrementalRetryer(waitTime, maxRetries);
            return Retry(retryer, retryMethod);
        }


        public static RetryerResults Retry(int maximumRetries, Action retryMethod)
        {
            IncrementalRetryer retryer = new IncrementalRetryer(maximumRetries);
            return Retry(retryer, retryMethod);
        }


        /// <summary>
        /// Executes the method until the condition returns true, or the maximum number of retries has been reached.
        /// </summary>
        /// <typeparam name="TRetry"> </typeparam>
        /// <param name="retry"> </param>
        /// <param name="retryMethod">The method to retry.</param>
        /// <returns>A value containing the results of the operation.</returns>
        public static RetryerResults Retry<TRetry>(TRetry retry, Action retryMethod) where TRetry : Retry
        {
            ValidateRetrier(retry);
            List<Exception> exceptions = new List<Exception>();
            retry.HasRun = true;
            do
            {
                try
                {
                    retryMethod();
                    return new RetryerResults(retry);
                }
                catch (Exception ex)
                {
                    //Determine if the exception is in the whitelist and if so, exit and return.
                    if (retry.Options.GetIsIgnoredException(ex))
                    {
                        break;
                    }

                    exceptions.Add(ex);

                    //Determine if the exception is in the blacklist and if not continue to the next retry
                    if (retry.Options.GetIsBlackListed(ex))
                    {
                        if (retry.Options.OnBlackListExceptionThrow)
                        {
                            throw;
                        }
                    }

                    retry.OnRetry(ex);
                    bool canRetry = retry.CanRetry;
                    if (canRetry)
                        continue;
                    if (retry.Options.ThrowFinalException)
                    {
                        throw;
                    }
                    break;
                }
            } while (true);
            return new RetryerResults(retry, exceptions);
        }

        private static void ValidateRetrier<TRetry>(TRetry retry) where TRetry : Retry
        {
            if (retry.HasRun)
            {
                throw new InvalidOperationException("The supplied retrier has already been used.  You must provide a new instance of a retrier for each retry operation.");
            }
        }
    }

}
