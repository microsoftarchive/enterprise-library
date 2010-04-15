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

using System;
using System.Collections;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
    /// <summary>
    /// A wrapper to convert data for Oracle for the reader.
    /// </summary>
    /// <remarks>
    /// The wrapper performs type conversions to enable retrieving values for types not supported natively by the
    /// <see cref="OracleDataReader"/>.
    /// <para/>
    /// The wrapped data reader can be accessed through the <see cref="OracleDataReaderWrapper.InnerReader"/>
    /// property.
    /// </remarks>
    /// <seealso cref="IDataReader"/>
    /// <seealso cref="OracleDataReader"/>
    public class OracleDataReaderWrapper : DataReaderWrapper, IEnumerable
    {
        internal OracleDataReaderWrapper(OracleDataReader innerReader)
            : base(innerReader)
        {
        }

        /// <summary>
        /// Gets the wrapped <see cref="OracleDataReader"/>.
        /// </summary>
        public new OracleDataReader InnerReader
        {
            get { return (OracleDataReader)base.InnerReader; }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The converted value of the column.</returns>
        /// <remarks>
        /// Bit data type is mapped to a number in Oracle database. When reading bit data from Oracle database,
        /// it will map to 0 as false and everything else as true.  This method uses System.Convert.ToBoolean() method
        /// for type conversions.
        /// </remarks>        
        public override bool GetBoolean(int index)
        {
            return Convert.ToBoolean(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value of the specified column converted to an 8-bit unsigned integer. 
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        /// <remarks> This method uses System.Convert.ToByte() method
        /// for type conversions.</remarks>
        public override byte GetByte(int index)
        {
            return Convert.ToByte(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value of the specified field converted to a GUID.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The GUID of the specified field.</returns>
        /// <remarks>
        /// This method will cast the result data Guid data type. In Oracle you must use that as Raw(16) so
        /// that this method can convert that to Guid properly.
        /// </remarks>        
        public override Guid GetGuid(int index)
        {
            var guidBuffer = (byte[]) InnerReader[index];
            return new Guid(guidBuffer);
        }

        /// <summary>
        /// Gets the value of the specified field converted to a 16-bit signed integer.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public override short GetInt16(int index)
        {
            return Convert.ToInt16(InnerReader[index], CultureInfo.InvariantCulture);
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) InnerReader).GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Another wrapper for <see cref="OracleDataReader"/> that adds connection
    /// reference counting.
    /// </summary>
    public class RefCountingOracleDataReaderWrapper : OracleDataReaderWrapper
    {
        private readonly DatabaseConnectionWrapper connection;

        internal RefCountingOracleDataReaderWrapper(DatabaseConnectionWrapper connection, OracleDataReader innerReader)
            : base(innerReader)
        {
            this.connection = connection;
            this.connection.AddRef();
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
                connection.Dispose();
            }
        }

        /// <summary>
        /// Close the contained data reader when disposing and releases the connection
        /// if it's not used anymore.
        /// </summary>
        /// <param name="disposing">True if called from Dispose method, false if called from finalizer. Since
        /// this class doesn't have a finalizer, this will always be true.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!IsClosed)
                {
                    base.Dispose(true);
                    connection.Dispose();
                }
            }
        }
    }
}
