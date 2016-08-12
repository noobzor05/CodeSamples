using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class IORetryer : GraduatedWaitTimeIncrementalRetryer
    {
        /// <summary>
        /// Default constructor that sets default values. Initial wait time is 4 seconds and maximum tries is 4.
        /// </summary>
        public IORetryer()
            : this(TimeSpan.FromSeconds(5), 4, IncrementWaitTime)
        {
        }


        /// <summary>
        /// Creates a new default instance with an initial wait time, maximum retries and a wait time increment function.
        /// </summary>
        /// <param name="initialWaitTime">The initial wait time for the first retry.</param>
        /// <param name="maxRetries">The maximum number of retries allowed.</param>
        /// <param name="waitTimeIncrement">A method that returns a new wait time.</param>
        public IORetryer(TimeSpan initialWaitTime, int maxRetries, WaitTimeIncrementDelegate waitTimeIncrement)
            : base(initialWaitTime, maxRetries, waitTimeIncrement)
        {
            this.Options.GetIsBlackListed = IsNotIOException;
            this.Options.ThrowFinalException = true;
        }


        private static bool IsNotIOException(Exception ex)
        {
            // this method will cause the retrier to throw all exceptions except IOExceptions
            // when assigned to the GetIsBlackListed delegate
            return !(ex is System.IO.IOException);
        }


        /// <summary>
        /// Default WaitTimeIncrementDelegate for graduating the wait time with each retry.
        /// </summary>
        /// <param name="currentWaitTime">The current wait time.</param>
        /// <param name="currentRetryCount">The number of tries that have executed.</param>
        private static TimeSpan IncrementWaitTime(TimeSpan currentWaitTime, int currentRetryCount)
        {
            return TimeSpan.FromSeconds(currentWaitTime.TotalSeconds * (currentRetryCount + 1));
        }
    }

}
