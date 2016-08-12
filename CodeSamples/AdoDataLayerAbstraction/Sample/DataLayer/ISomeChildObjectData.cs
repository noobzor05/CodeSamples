using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction.Data
{
    public interface ISomeChildObjectData
    {
        SomeChildObject GetById(int id);
        List<SomeChildObject> GetAllByParentId(int parentId);
        int Insert(SomeChildObject value);
        bool Update(SomeChildObject value);
        bool Delete(SomeChildObject value);
        bool DeleteAllByParentId(int parentId);

    }
}
