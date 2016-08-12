using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction.Data
{
    public abstract class StuffDataFactory
    {
        public abstract DataContext DataContext
        {
            get;
        }
        public abstract ISomeObjectData SomeObjectData
        {
            get;
        }
        public abstract ISomeChildObjectData SomeChildObjectData
        {
            get;
        }

        public static StuffDataFactory GetDataFactory(DataContext dataContext)
        {
            switch (dataContext.DataProviderType)
            {
                case DataProviderTypes.SqlServerDataProvider:
                    return new DbStuffDataFactory(dataContext);
                    break;
                //TODO: Implement SQLite data access
                //case DataProviderTypes.SQLiteDataProvider:
                //    return new DbStuffDataFactory(dataContext as SQLiteDataContext);
                //    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }
    }
}
