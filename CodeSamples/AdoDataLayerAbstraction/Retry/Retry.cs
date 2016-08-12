using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    /// <summary>
    /// Provides a base mechansim for implementing retries on an operation.
    /// </summary>
    public abstract class Retry
    {

        /// <summary>
        /// Gets the options for this retrier.
        /// </summary>
        public RetryOptions Options { get; protected set; }

        /// <summary>
        /// Gets a value indicating if the operation can be retried.
        /// </summary>
        public abstract bool CanRetry { get; }

        /// <summary>
        /// Gets or sets a value indcating if this retry class has been utilized before.
        /// </summary>
        public bool HasRun { get; set; }

        /// <summary>
        /// Called when a retry occurs
        /// </summary>
        /// <param name="exception"></param>
        public virtual void OnRetry(Exception exception) { }

    }
}
