using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class SqlDataContext : DataContext
    {
        private string _ConnectionString;

        public override DataProviderTypes DataProviderType
        {
            get
            {
                return DataProviderTypes.SqlServerDataProvider;
            }
        }

        public override string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        public SqlDataContext(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

    }
}
