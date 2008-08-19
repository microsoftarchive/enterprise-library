//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
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
    /// <devdoc>
    /// A wrapper to convert data for oracle for the reader.
    /// </devdoc>
    internal class OracleDataReaderWrapper : MarshalByRefObject, IDataReader, IEnumerable
    {
        private OracleDataReader innerReader;

        public static explicit operator OracleDataReader(OracleDataReaderWrapper oracleDataReaderWrapper)
        {
            return oracleDataReaderWrapper.InnerReader;
        }

        public OracleDataReaderWrapper(OracleDataReader reader)
        {
            this.innerReader = reader;
        }

        public object this[int index]
        {
            get { return InnerReader[index]; }
        }

        public object this[string name]
        {
            get { return InnerReader[name]; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerReader).GetEnumerator();
        }

        void IDisposable.Dispose()
        {
            InnerReader.Dispose();
        }

        public void Close()
        {
            InnerReader.Close();
        }

        public DataTable GetSchemaTable()
        {
            return InnerReader.GetSchemaTable();
        }

        public bool NextResult()
        {
            return InnerReader.NextResult();
        }

        public bool Read()
        {
            return InnerReader.Read();
        }

        public int Depth
        {
            get { return InnerReader.Depth; }
        }

        public bool IsClosed
        {
            get { return InnerReader.IsClosed; }
        }

        public int RecordsAffected
        {
            get { return InnerReader.RecordsAffected; }
        }

        public int FieldCount
        {
            get { return InnerReader.FieldCount; }
        }

        /// <devdoc>
        /// Bit data type is mapped to a number in Oracle database. When reading bit data from Oracle database,
        /// it will map to 0 as false and everything else as true.  This method uses System.Convert.ToBoolean() method
        /// for type conversions.
        /// </devdoc>        
        public bool GetBoolean(int index)
        {
            return Convert.ToBoolean(InnerReader[index], CultureInfo.InvariantCulture);
        }

        public byte GetByte(int index)
        {
            return Convert.ToByte(InnerReader[index], CultureInfo.InvariantCulture);
        }

        public long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            return InnerReader.GetBytes(ordinal, dataIndex, buffer, bufferIndex, length);
        }

        public Char GetChar(int index)
        {
            return InnerReader.GetChar(index);
        }

        public long GetChars(int index, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            return InnerReader.GetChars(index, dataIndex, buffer, bufferIndex, length);
        }

        public IDataReader GetData(int index)
        {
            return InnerReader.GetData(index);
        }

        public string GetDataTypeName(int index)
        {
            return InnerReader.GetDataTypeName(index);
        }

        public DateTime GetDateTime(int ordinal_)
        {
            return InnerReader.GetDateTime(ordinal_);
        }

        public decimal GetDecimal(int index)
        {
            return InnerReader.GetDecimal(index);
        }

        public double GetDouble(int index)
        {
            return InnerReader.GetDouble(index);
        }

        public Type GetFieldType(int index)
        {
            return InnerReader.GetFieldType(index);
        }

        /// <devdoc>
        /// When reading number from Oracle, data reader gets it back at decimal regardless of data type in
        /// Oracle database. This will cast the result to float data type.
        /// </devdoc>        
        public float GetFloat(int index)
        {
            return InnerReader.GetFloat(index);
        }

        /// <devdoc>
        /// This method will cast the result data Guid data type. In Oracle you must use that as Raw(16) so
        /// that this method can convert that to Guid properly.
        /// </devdoc>        
        public Guid GetGuid(int index)
        {
            byte[] guidBuffer = (byte[])InnerReader[index];
            return new Guid(guidBuffer);
        }

        public short GetInt16(int index)
        {
            return Convert.ToInt16(InnerReader[index], CultureInfo.InvariantCulture);
        }

        public int GetInt32(int index)
        {
            return InnerReader.GetInt32(index);
        }

        public long GetInt64(int index)
        {
            return InnerReader.GetInt64(index);
        }

        public string GetName(int index)
        {
            return InnerReader.GetName(index);
        }

        public int GetOrdinal(string index)
        {
            return InnerReader.GetOrdinal(index);
        }

        public string GetString(int index)
        {
            return InnerReader.GetString(index);
        }

        public object GetValue(int index)
        {
            return InnerReader.GetValue(index);
        }

        public int GetValues(object[] values)
        {
            return InnerReader.GetValues(values);
        }

        public bool IsDBNull(int index)
        {
            return InnerReader.IsDBNull(index);
        }

        public OracleDataReader InnerReader
        {
            get { return this.innerReader; }
        }
    }
}