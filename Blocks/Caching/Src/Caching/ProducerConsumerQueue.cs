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

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Represents a thread safe queue.
	/// </summary>	
	public class ProducerConsumerQueue
    {
        private object lockObject = new Object();
        private Queue queue = new Queue();

		/// <summary>
		/// Gets the number of items in the queue.
		/// </summary>
		/// <value>
		/// The number of items in the queue.
		/// </value>
        public int Count
        {
            get { return queue.Count; }
        }

		/// <summary>
		/// Removes and returns the object at the beginning of the queue. 
		/// </summary>
		/// <returns>
		/// The object at the beginning of the queue.
		/// </returns>
        public object Dequeue()
        {
            lock (lockObject)
            {
                while (queue.Count == 0)
                {
                    if (WaitUntilInterrupted())
                    {
                        return null;
                    }
                }

                return queue.Dequeue();
            }
        }

		/// <summary>
		/// Adds the object at the end of the queue. 
		/// </summary>		
        public void Enqueue(object queueItem)
        {
            lock (lockObject)
            {
                queue.Enqueue(queueItem);
                Monitor.Pulse(lockObject);
            }
        }

        private bool WaitUntilInterrupted()
        {
            try
            {
                Monitor.Wait(lockObject);
            }
            catch (ThreadInterruptedException)
            {
                return true;
            }

            return false;
        }
    }
}