using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleSolver
{
    public class InvalidBoardException : System.Exception
    {
        public InvalidBoardException()
            : base()
        {
        }

        public InvalidBoardException(string message)
            : base(message)
        {
        }

        public InvalidBoardException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public InvalidBoardException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
