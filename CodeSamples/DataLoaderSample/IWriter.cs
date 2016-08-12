using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    //This is a very simple interface for writing a set of objects to some destination.
    //TOut is the type of the objects to be persisted
    //TDest is a type representing the data storage to which the objects are being persisted
    public interface IWriter<TOut, TDest>
    {
        TDest Destination { get; }
        void Open();
        void Write(IEnumerable<TOut> items);
        void Close();
    }
}
