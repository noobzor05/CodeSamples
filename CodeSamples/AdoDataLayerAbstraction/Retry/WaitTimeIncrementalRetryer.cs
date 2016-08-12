using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class WaitTimeIncrementalRetryer : IncrementalRetryer
    {
        public WaitTimeIncrementalRetryer(TimeSpan waitTime, int maxRetries)
            : base(maxRetries)
        {
            WaitTime = waitTime;
        }

        /// <summary>
        /// The wait time for retrying an operation.
        /// </summary>
        public TimeSpan WaitTime { get; protected set; }

        public override bool CanRetry
        {
            get
            {
                bool retry = base.CanRetry;
                if (retry)
                {
                    //Wait if the retry count was successful.
                    System.Threading.Thread.Sleep(WaitTime);
                }
                return retry;
            }
        }
    }

}
