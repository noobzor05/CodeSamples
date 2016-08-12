using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    //The Random class is not inherently thread safe...
    //if you call it simultaneously on multiple threads, you could get the same return value
    public static class ThreadSafeRandom
    {
        private static Random _Random = new Random();

        public static int Next()
        {
            lock (_Random)
            {
                return _Random.Next();
            }
        }

        public static int Next(int maxValue)
        {
            lock (_Random)
            {
                return _Random.Next(maxValue);
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            lock (_Random)
            {
                return _Random.Next(minValue, maxValue);
            }
        }

        public static void NextBytes(byte[] buffer)
        {
            lock (_Random)
            {
                _Random.NextBytes(buffer);
            }
        }

        public static double NextDouble()
        {
            lock (_Random)
            {
                return _Random.NextDouble();
            }
        }
    }
}
