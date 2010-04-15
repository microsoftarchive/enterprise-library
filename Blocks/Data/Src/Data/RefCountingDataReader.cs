//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Data;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// An implementation of <see cref="IDataReader"/> which also properly
    /// cleans up the reference count on the given inner <see cref="DatabaseConnectionWrapper"/>
    /// when the reader is closed or disposed.
    /// </summary>
    public class RefCountingDataReader : DataReaderWrapper
    {
        private readonly DatabaseConnectionWrapper connectionWrapper;

        /// <summary>
        /// Create a new <see cref='RefCountingDataReader'/> that wraps
        /// the given <paramref name="innerReader"/> and properly
        /// cleans the refcount on the given <paramref name="connection"/>
        /// when done.
        /// </summary>
        /// <param name="connection">Connection to close.</param>
        /// <param name="innerReader">Reader to do the actual work.</param>
        public RefCountingDataReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
            : base(innerReader)
        {
            Guard.ArgumentNotNull(connection, "connection");
            Guard.ArgumentNotNull(innerReader, "innerReader");

            connectionWrapper = connection;
            connectionWrapper.AddRef();
        }

        /// <summary>
        /// Closes the <see cref="T:System.Data.IDataReader"/> Object.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Close()
        {
            if(!IsClosed)
            {
                base.Close();
                connectionWrapper.Dispose();
            }
        }

        /// <summary>
        /// Clean up resources.
        /// </summary>
        /// <param name="disposing">True if called from dispose, false if called from finalizer. We have no finalizer,
        /// so this will never be false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!IsClosed)
                {
                    base.Dispose(true);
                    connectionWrapper.Dispose();
                }
            }
        }
    }
}
