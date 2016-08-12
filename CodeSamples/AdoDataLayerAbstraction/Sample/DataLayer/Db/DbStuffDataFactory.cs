using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction.Data
{
    public class DbStuffDataFactory : StuffDataFactory
    {

        private DataContext _DataContext;
        public override DataContext DataContext
        {
            get
            {
                return _DataContext;
            }
        }

        private Lazy<DbSomeObjectData> _SomeObjectData;
        public override ISomeObjectData SomeObjectData
        {
            get
            {
                return _SomeObjectData.Value;
            }
        }

        private Lazy<DbSomeChildObjectData> _SomeChildObjectData;
        public override ISomeChildObjectData SomeChildObjectData
        {
            get
            {
                return _SomeChildObjectData.Value;
            }
        }

        internal DbStuffDataFactory(DataContext dataContext)
        {
            _DataContext = dataContext;
            _SomeObjectData = new Lazy<DbSomeObjectData>(() => new DbSomeObjectData(this));
            _SomeChildObjectData = new Lazy<DbSomeChildObjectData>(() => new DbSomeChildObjectData(this));
        }

        private DbStuffDataFactory()
        {
            //no default constructor
        }

    }
}
