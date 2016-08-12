using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class SomeObject
    {
        private Data.StuffDataFactory _DataFactory;

        public int Id { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }

        Lazy<List<SomeChildObject>> _Children;
        public List<SomeChildObject> Children
        {
            get
            {
                return _Children.Value;
            }
        }

        internal SomeObject(Data.StuffDataFactory dataFactory)
        {
            //if this was created from a data factory, set the lazy load to get the children from the same data factory
            _DataFactory = dataFactory;
            _Children = new Lazy<List<SomeChildObject>>(() => _DataFactory == null ? new List<SomeChildObject>() : _DataFactory.SomeChildObjectData.GetAllByParentId(Id));
        }

        public SomeObject()
        {
            //if this was created without a data factory, children defaults to an empty list
            _Children = new Lazy<List<SomeChildObject>>(() => new List<SomeChildObject>());
        }
            
    }
}
