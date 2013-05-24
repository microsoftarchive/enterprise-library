using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents a collection of <see cref="LogSourceData"/> instances.
    /// </summary>
    public class LogSourceDataCollection : ICollection<LogSourceData>
    {
        private Dictionary<string, LogSourceData> dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSourceDataCollection"/> class.
        /// </summary>
        public LogSourceDataCollection()
        {
            this.dict = new Dictionary<string, LogSourceData>();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSourceDataCollection"/> class with the specified collection.
        /// </summary>
        /// <param name="collection">The data collection for the new instance.</param>
        public LogSourceDataCollection(IEnumerable<LogSourceData> collection)
        {
            this.dict = collection.ToDictionary(k => k.Name);
        }

        /// <summary>
        /// Gets the specified <see cref="LogSourceData"/> object.
        /// </summary>
        /// <param name="name">The name of the log source.</param>
        /// <returns>The <see cref="LogSourceData"/> object.</returns>
        public LogSourceData this[string name]
        {
            get
            {
                return this.dict[name];
            }
        }

        /// <summary>
        /// Adds a new <see cref="LogSourceData"/> object to the collection.
        /// </summary>
        /// <param name="item">The <see cref="LogSourceData"/> object.</param>
        public void Add(LogSourceData item)
        {
            // TODO: Validate not adding special log sources
            this.dict.Add(item.Name, item);
        }

        /// <summary>
        /// Removes all <see cref="LogSourceData"/> from the <see cref="LogSourceDataCollection"/>.
        /// </summary>
        public void Clear()
        {
            this.dict.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="LogSourceDataCollection"/> contains the specified <see cref="LogSourceData"/> item.
        /// </summary>
        /// <param name="item">The <see cref="LogSourceData"/> object to check for.</param>
        /// <returns><see langword="true"/> if the <see cref="LogSourceDataCollection"/> contains the specified element; otherwise, <see langword="false"/>.</returns>
        public bool Contains(LogSourceData item)
        {
            return this.dict.ContainsKey(item.Name);
        }

        /// <summary>
        /// Copies the <see cref="LogSourceDataCollection"/> elements to an existing one-dimensional System.Array, starting at the specified array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional LogSourceData[] array that is the destination of the elements copied from <see cref="LogSourceDataCollection"/>.
        /// The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(LogSourceData[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of <see cref="LogSourceData"/> objects in the <see cref="LogSourceDataCollection"/>.
        /// </summary>
        public int Count
        {
            get { return this.dict.Count; }
        }

        /// <summary>
        /// Gets a value that indicates whether this collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes all <see cref="LogSourceData"/> objects from the collection.
        /// </summary>
        public bool Remove(LogSourceData item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="LogSourceDataCollection"/>.
        /// </summary>
        /// <returns>An enumerator that interates through the collection.</returns>
        public IEnumerator<LogSourceData> GetEnumerator()
        {
            return this.dict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dict.Values.GetEnumerator();
        }
    }
}
