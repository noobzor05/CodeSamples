using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    //This is a very simple interface for taking a single input item and creating
    //one or more corresponding output items.  
    //TIn is the input type.
    //TOut is the output type.
    public interface IProcessor<TIn, TOut>
    {
        IEnumerable<TOut> Process(TIn item);
    }
}
