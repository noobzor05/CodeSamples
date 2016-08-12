using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    /// <summary>
    /// Provides a set of options for retrying an operation.
    /// </summary>
    public class RetryOptions
    {
        public RetryOptions()
        {
            OnBlackListExceptionThrow = true;
            ThrowFinalException = true;
            GetIsBlackListed = ex => false;
            GetIsIgnoredException = ex => false;
        }

        /// <summary>
        /// Gets or sets a value indicating if a Black-listed exception will be thrown instead of aggregating and returning as a result.
        /// </summary>
        public bool OnBlackListExceptionThrow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the final exception should be thrown if the entire retry operation was unsucessful.
        /// </summary>
        public bool ThrowFinalException { get; set; }

        /// <summary>
        /// Gets or sets the method used to determine if an exception is considered "black-listed."
        /// </summary>
        public Func<Exception, bool> GetIsBlackListed { get; set; }

        /// <summary>
        /// Gets or sets the method used to determine if an exception is considered "white-listed."
        /// </summary>
        public Func<Exception, bool> GetIsIgnoredException { get; set; }
    }
}
