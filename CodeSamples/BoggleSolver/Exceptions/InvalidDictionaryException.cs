using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleSolver
{
    public class InvalidDictionaryException : System.Exception
    {
        public InvalidDictionaryException()
            : base()
        {
        }

        public InvalidDictionaryException(string message)
            : base(message)
        {
        }

        public InvalidDictionaryException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public InvalidDictionaryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
