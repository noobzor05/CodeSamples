using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public enum DataProviderTypes
    {
        NotSet = -1,
        None = 0,
        SqlServerDataProvider = 1,
        SqlCompactDataProvider = 2,
        MySqlDataProvider = 3,
        SQLiteDataProvider = 4,
    }
}
