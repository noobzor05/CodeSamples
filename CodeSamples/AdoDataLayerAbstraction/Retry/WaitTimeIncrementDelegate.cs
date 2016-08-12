using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    /// <summary>
    /// A method used to increase a wait time by some interval.
    /// </summary>
    /// <param name="currentWaitTime">The current wait time.</param>
    /// <param name="currentRetryCount">The number of retries that has already occured.</param>
    /// <returns>A new <see cref="TimeSpan"/> comprised of the new wait time.</returns>
    public delegate TimeSpan WaitTimeIncrementDelegate(TimeSpan currentWaitTime, int currentRetryCount);
}
