using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLoaderSample
{
    public class DataLoader<TSource, TIn, TOut, TDest>
    {
        private IReader<TSource, TIn> _Reader;
        private IProcessor<TIn, TOut> _Processor;
        private IWriter<TOut, TDest> _Writer;

        private ProcessQueue<TIn> _InputQueue;
        private ProcessQueue<TOut> _OutputQueue;

        public int MaxMapperThreads { get; set; }
        public int InputBufferSize { get; set; }
        public int OutputBufferSize { get; set; }
        public int SleepTimeMS { get; set; }

        public DataLoader(IReader<TSource, TIn> reader, IProcessor<TIn, TOut> processor, IWriter<TOut, TDest> writer)
        {
            _InputQueue = new ProcessQueue<TIn>();
            _OutputQueue = new ProcessQueue<TOut>();
            MaxMapperThreads = 10;
            InputBufferSize = 1000;
            OutputBufferSize = 1000;
            SleepTimeMS = 1000;
            _Reader = reader;
            _Processor = processor;
            _Writer = writer;
        }

        private DataLoader()
        {
            //no public default constructor
        }

        public void Load()
        {
            //create one thread for reading data into the input queue
            Task inputTask = Task.Factory.StartNew(() => ReadInput())
                //when the input thread is done, mark the _InputQueue as finished
                .ContinueWith(t => _InputQueue.Finish());

            //create many threads for processing the data
            Task[] processTasks = new Task[MaxMapperThreads];
            for(int i = 0; i < MaxMapperThreads; i++)
            {
                processTasks[i] = Task.Factory.StartNew(() => ProcessData());
            }

            //when all process threads are done, mark the output queue as finished
            Task.Factory.ContinueWhenAll(processTasks, tasks => _OutputQueue.Finish());

            //create one thread for writing output queue to destination
            Task outputTask = Task.Factory.StartNew(() => WriteOutput());
        }

        private void ReadInput()
        {
            //Just read until there is nothing left to read and push
            //items to the InputQueue.
            //If the InputQueue is full, wait a bit to let it clear out.
            _Reader.Open();
            TIn item;
            while(_Reader.Read(out item))
            {
                while(_InputQueue.Count >= InputBufferSize)
                {
                    Thread.Sleep(SleepTimeMS);
                }
                if(!_InputQueue.TryEnqueue(item))
                {
                    throw new InvalidOperationException("Tried to add an item to InputQueue when it was already marked as finished.");
                }
            }
            _Reader.Close();
        }

        private void ProcessData()
        {
            //Until the InputQueue is marked as Finished and is empty,
            //keep processing items and queueing the results up for output
            //if the InputQueue is empty but not finished, wait a bit before checking again
            while(!_InputQueue.Finished || _InputQueue.Count > 0)
            {
                while(_InputQueue.Count < 1)
                {
                    Thread.Sleep(SleepTimeMS);
                }
                TIn item;
                if(_InputQueue.TryDequeue(out item))
                {
                    IEnumerable<TOut> results = _Processor.Process(item);
                    foreach (TOut result in results)
                    {
                        if (!_OutputQueue.TryEnqueue(result))
                        {
                            throw new InvalidOperationException("Tried to enqueue item for output when output queue was already finished.");
                        }
                    }
                }
            }
        }

        private void WriteOutput()
        {
            //Whenever the output buffer is full or is marked Finished,
            //write everything to the destination.
            //Open and close the writer as needed.
            while(!_OutputQueue.Finished || _OutputQueue.Count > 0)
            {
                while(!_OutputQueue.Finished && _OutputQueue.Count < OutputBufferSize)
                {
                    Thread.Sleep(SleepTimeMS);
                }
                List<TOut> outItems = new List<TOut>();
                TOut item;
                while(_OutputQueue.TryDequeue(out item))
                {
                    outItems.Add(item);
                }
                if(outItems.Any())
                {
                    _Writer.Open();
                    _Writer.Write(outItems);
                    _Writer.Close();
                }
            }
        }
    }
}
