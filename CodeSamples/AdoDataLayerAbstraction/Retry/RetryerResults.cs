using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public sealed class RetryerResults
    {
        public RetryerResults(Retry retryDetails)
        {
            RetryDetails = retryDetails;
            Exceptions = new List<Exception>();
        }

        /// <summary>
        /// Creates a new default result set.
        /// </summary>
        /// <param name="retryDetails"> </param>
        /// <param name="exceptions">The exceptions that resulted from the retry operation.</param>
        public RetryerResults(Retry retryDetails, List<Exception> exceptions)
        {
            RetryDetails = retryDetails;
            Exceptions = exceptions;
        }

        /// <summary>
        /// Gets the number of retries that occured prior to a successful result.
        /// </summary>
        public Retry RetryDetails { get; private set; }

        /// <summary>
        /// Gets a value indicating the success or failure of the operation.
        /// </summary>
        public bool Success
        {
            get { return Exceptions.Count == 0; }
        }

        /// <summary>
        /// Gets a collection of exceptions that occured during the retry operation.
        /// </summary>
        public List<Exception> Exceptions { get; private set; }
    }

}
