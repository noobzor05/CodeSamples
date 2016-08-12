using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AdoDataLayerAbstraction
{
    public class ExecuteReaderRetryer : DbRetryer
    {
        public ExecuteReaderRetryer() : base(DbRetryer.DefaultRetryCount) { }
        public ExecuteReaderRetryer(int retryCount) : base(retryCount) { }

        private const int SqlErrorDuringLoginProcess = 233;
        private const int ConnectionForciblyClosed = 10054;
        private static readonly List<int> SqlServerErrorCodes = new List<int>
        {
            SqlErrorDuringLoginProcess,
            ConnectionForciblyClosed,
        };

        /// <summary>
        ///Certain sql exceptions can be retried, others should not.
        ///Sometimes an ExecuteReader throws an InvalidOperationException if the connection is closed before the read is attempted and should be retried
        ///All other exception types are BlackListed and will not be retried.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected override bool GetIsBlackListed(Exception exception)
        {

            SqlException sqlEx = exception as SqlException;

            if (sqlEx == null)
            {
                return (exception as InvalidOperationException) == null;
            }
            return !SqlServerErrorCodes.Contains(sqlEx.Number);
        }
    }

}
