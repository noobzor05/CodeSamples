using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    public class MockWriter : IWriter<string, string>
    {
        public int WriteTimeMS { get; set; }

        public string Destination
        {
            get
            {
                return string.Empty;
            }
        }

        public MockWriter()
        {
            WriteTimeMS = 10;
        }

        public void Close()
        {
            //nothing to do
        }

        public void Open()
        {
            //nothing to do
        }

        public void Write(IEnumerable<string> items)
        {
            foreach(string item in items)
            {
                Thread.Sleep(WriteTimeMS);
            }
        }
    }
}
