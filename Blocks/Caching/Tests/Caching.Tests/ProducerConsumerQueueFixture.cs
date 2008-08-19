//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class ProducerConsumerQueueFixture
    {
        static ProducerConsumerQueue queue;
        static string dequeuedMsg = null;
        static ArrayList accumulatedResults;

        static object staticLock = new object();
        static int staticCounter = 0;

        [TestInitialize]
        public void TestInitialize()
        {
            dequeuedMsg = null;
            accumulatedResults = new ArrayList();
            staticCounter = 0;
        }

        [TestMethod]
        public void CanStopConsumerSide()
        {
            queue = new ProducerConsumerQueue();
            dequeuedMsg = "This will be null if we break out of dequeue and return a null msg";
            ThreadStart consumerMethod = new ThreadStart(runner);
            Thread consumerThread = new Thread(consumerMethod);
            consumerThread.Start();
            Thread.Sleep(500);
            consumerThread.Interrupt();
            consumerThread.Join();
            Thread.Sleep(500);
            Assert.IsNull(dequeuedMsg);
        }

        [TestMethod]
        public void CanAddElementAsProducerAndRemoveAsConsumer()
        {
            queue = new ProducerConsumerQueue();
            ThreadStart consumerMethod = new ThreadStart(runner);
            Thread consumerThread = new Thread(consumerMethod);
            consumerThread.Start();
            Thread.Sleep(500);

            queue.Enqueue("this is a string");

            Thread.Sleep(500);
            consumerThread.Join();

            Assert.AreEqual("this is a string", dequeuedMsg);
        }

        [TestMethod]
        public void StressTest()
        {
            queue = new ProducerConsumerQueue();

            ThreadStart consumerMethod = new ThreadStart(accumulatingRunner);
            Thread consumerThread = new Thread(consumerMethod);
            consumerThread.Start();
            Thread.Sleep(500);

            ArrayList threads = new ArrayList();
            for (int i = 0; i < 100; i++)
            {
                ThreadStart producerMethod = new ThreadStart(producingThread);
                threads.Add(new Thread(producerMethod));
            }

            for (int i = 0; i < 100; i++)
            {
                ((Thread)threads[i]).Start();
            }

            for (int i = 0; i < 100; i++)
            {
                ((Thread)threads[i]).Join();
            }

            consumerThread.Join();
            Assert.AreEqual(0, queue.Count);
            Assert.AreEqual(100000, staticCounter);

            for (int i = 0; i < 100000; i++)
            {
                Assert.AreEqual(i, (int)accumulatedResults[i], "Failed at index " + i);
            }
        }

        void producingThread()
        {
            Random generator = new Random(12345);
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(generator.Next(5));
                int valueToInsert = -1;
                lock (staticLock)
                {
                    staticCounter++;
                    valueToInsert = staticCounter;
                }
                queue.Enqueue(valueToInsert);
            }
        }

        void accumulatingRunner()
        {
            for (int i = 0; i < 100000; i++)
            {
                object dequeuedObject = queue.Dequeue();
                accumulatedResults.Add(i);
            }
        }

        void runner()
        {
            object dequeuedObject = queue.Dequeue();
            dequeuedMsg = dequeuedObject as string;
        }
    }
}