using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class GraduatedWaitTimeIncrementalRetryer : WaitTimeIncrementalRetryer
    {
        private readonly WaitTimeIncrementDelegate _waitTimeIncrement;

        /// <summary>
        /// Creates a new default instance with an initial wait time, maximum retries and a wait time increment function.
        /// </summary>
        /// <param name="initialWaitTime">The initial wait time for the first retry.</param>
        /// <param name="maxRetries">The maximum number of retries allowed.</param>
        /// <param name="waitTimeIncrement">A method that returns a new wait time.</param>
        public GraduatedWaitTimeIncrementalRetryer(TimeSpan initialWaitTime, int maxRetries, WaitTimeIncrementDelegate waitTimeIncrement)
            : base(initialWaitTime, maxRetries)
        {
            _waitTimeIncrement = waitTimeIncrement;
        }

        public override bool CanRetry
        {
            get
            {
                bool retry = base.CanRetry;
                if (retry)
                {
                    //set the wait time to the new increment value.
                    WaitTime = _waitTimeIncrement(WaitTime, RetryCount);
                }
                return retry;
            }
        }
    }

}
