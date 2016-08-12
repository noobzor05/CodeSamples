using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction.Data
{
    public interface ISomeObjectData
    {
        SomeObject GetById(int id);
        int Insert(SomeObject value);
        bool Update(SomeObject value);
        bool Delete(SomeObject value);
    }
}
