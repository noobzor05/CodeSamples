using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public abstract class DataContext
    {
        abstract public DataProviderTypes DataProviderType
        {
            get;
        }
        abstract public string ConnectionString
        {
            get;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DataContext))
            {
                return false;
            }
            DataContext dc = (DataContext)obj;
            return this.DataProviderType.Equals(dc.DataProviderType) && this.ConnectionString.Equals(dc.ConnectionString);
        }

        public override int GetHashCode()
        {
            return this.DataProviderType.GetHashCode() ^ this.ConnectionString.GetHashCode();
        }
    }
}
