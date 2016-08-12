using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    public class MockProcessor : IProcessor<string, string>
    {
        public int ProcessTimeMS { get; set; }

        public MockProcessor()
        {
            ProcessTimeMS = 100;
        }

        public IEnumerable<string> Process(string item)
        {
            int outCount = ThreadSafeRandom.Next(10);
            for (int i = 0; i < outCount; i++)
            {
                Thread.Sleep(ProcessTimeMS);
                yield return string.Empty;
            }
        }
    }
}
