using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    public class MockReader : IReader<string, string>
    {
        public int ReadTimeMS { get; set; }
        public int RecordCount { get; set; }

        public string Source
        {
            get
            {
                return string.Empty;
            }
        }

        public MockReader()
        {
            ReadTimeMS = 10;
            RecordCount = 1000;
        }

        public void Close()
        {
            //nothing to do
        }

        public void Open()
        {
            //nothing to do
        }

        public bool Read(out string item)
        {
            if (RecordCount == 0)
            {
                item = null;
                return false;
            }
            RecordCount--;
            Thread.Sleep(ReadTimeMS);
            item = string.Empty;
            return true;
        }
    }
}
