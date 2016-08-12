using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class IncrementalRetryer : Retry
    {
        public IncrementalRetryer(int maxRetries)
        {
            MaxRetries = maxRetries;
            Options = new RetryOptions();
        }

        /// <summary>
        /// Gets the maximum number of retries.
        /// </summary>
        public int MaxRetries { get; private set; }

        /// <summary>
        /// Gets the current retry count.
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// Gets a value indicating if the operation can be retried.
        /// </summary>
        public override bool CanRetry
        {
            get
            {
                RetryCount++;
                return RetryCount < MaxRetries;
            }
        }
    }

}
