using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DataLoaderSample
{
    //A fairly simple wrapper around ConcurrentQueue that allows us
    //to mark it as finished, meaning that we can no longer add
    //any items to it.
    //This is a wrapper instead of inheriting so as to prevent
    //access to the ConcurrentQueue's Enqueue method, which is not virtual
    //and so cannot be overridden, only hidden.
    public class ProcessQueue<T>
    {
        private ConcurrentQueue<T> _Queue;

        //Because reading/writing a bool is an atomic operation,
        //volatile keyword should be sufficient for ensuring thread-safety
        //without requiring locks
        private volatile bool _Finished;
        public bool Finished
        {
            get
            {
                return _Finished;
            }
        }

        public int Count
        {
            get
            {
                return _Queue.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _Queue.IsEmpty;
            }
        }

        public ProcessQueue()
        {
            _Queue = new ConcurrentQueue<T>();
            _Finished = false;
        }

        //Once this method is called, nothing else can be added to the queue.
        //Best to leave this to a master thread that can know these things.
        public void Finish()
        {
            lock(_Queue)
            {
                _Finished = true;
            }
        }

        public bool TryEnqueue(T item)
        {
            //Locking here prevents a race condition where _Finished could be
            //set to true between the check and the Enqueue, guaranteeing
            //that once _Finished is set to true, there will be no other items added
            //by any thread.
            //The downside is that this negates the benefit of the thread-safe Enqueue provided
            //by ConcurrentQueue, but for practical purposes, the difference is negligible.
            lock (_Queue)
            {
                if (Finished)
                {
                    return false;
                }
                _Queue.Enqueue(item);
                return true;
            }
        }

        public bool TryDequeue(out T result)
        {
            return _Queue.TryDequeue(out result);
        }

        public bool TryPeek(out T result)
        {
            return _Queue.TryPeek(out result);
        }
    }
}
