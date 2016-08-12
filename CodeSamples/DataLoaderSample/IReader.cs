using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    //This is a very simple interface for reading objects from some source.
    //TSource is a type representing the data storage from which the objects are being read.
    //TIn is the type of each object being read.
    public interface IReader<TSource, TIn>
    {
        TSource Source { get; }
        void Open();
        bool Read(out TIn item);
        void Close();
    }
}
