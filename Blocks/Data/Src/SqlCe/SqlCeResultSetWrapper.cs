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
using System.Data.SqlServerCe;
using System.Data.SqlTypes;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe
{
    /// <summary>
    /// A wrapper around <see cref="SqlCeResultSet"/> which also manages
    /// reference counting on the corresponding connection.
    /// </summary>
    internal class SqlCeResultSetWrapper : SqlCeResultSet
    {
        private readonly DatabaseConnectionWrapper connection;

        public SqlCeResultSetWrapper(DatabaseConnectionWrapper connection, SqlCeResultSet innerResultSet)
        {
            this.connection = connection;
            this.connection.AddRef();
            this.InnerResultSet = innerResultSet;
        }

        #region connection management

        public override void Close()
        {
            if (InnerResultSet != null)
            {
                InnerResultSet.Close();
                connection.Dispose();
                InnerResultSet = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (InnerResultSet != null)
                {
                    InnerResultSet.Dispose();
                    connection.Dispose();
                    InnerResultSet = null;
                }
            }
        }

        public bool IsDisposed { get { return InnerResultSet != null; } }

        public SqlCeResultSet InnerResultSet { get; private set; }

        #endregion

        #region Delegating implementation

        public override object this[int index]
        {
            get { return InnerResultSet[index]; }
        }

        public override object this[string name]
        {
            get { return InnerResultSet[name]; }
        }

        public override int Depth
        {
            get { return InnerResultSet.Depth; }
        }

        public override int FieldCount
        {
            get { return InnerResultSet.FieldCount; }
        }

        public override bool IsClosed
        {
            get { return InnerResultSet.IsClosed; }
        }

        public override int RecordsAffected
        {
            get { return InnerResultSet.RecordsAffected; }
        }

        public override bool HasRows
        {
            get { return InnerResultSet.HasRows; }
        }

        public override int VisibleFieldCount
        {
            get { return InnerResultSet.VisibleFieldCount; }
        }

        public override IEnumerator GetEnumerator()
        {
            return InnerResultSet.GetEnumerator();
        }

        public override bool IsDBNull(int ordinal)
        {
            return InnerResultSet.IsDBNull(ordinal);
        }

        public override bool GetBoolean(int ordinal)
        {
            return InnerResultSet.GetBoolean(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return InnerResultSet.GetDecimal(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return InnerResultSet.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            return InnerResultSet.GetBytes(ordinal, dataIndex, buffer, bufferIndex, length);
        }

        public override long GetChars(int ordinal, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            return InnerResultSet.GetChars(ordinal, dataIndex, buffer, bufferIndex, length);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return InnerResultSet.GetDateTime(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return InnerResultSet.GetDouble(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return InnerResultSet.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return InnerResultSet.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return InnerResultSet.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return InnerResultSet.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return InnerResultSet.GetInt64(ordinal);
        }

        public override string GetString(int ordinal)
        {
            return InnerResultSet.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return InnerResultSet.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return InnerResultSet.GetValues(values);
        }

        public override SqlBoolean GetSqlBoolean(int ordinal)
        {
            return InnerResultSet.GetSqlBoolean(ordinal);
        }

        public override SqlMoney GetSqlMoney(int ordinal)
        {
            return InnerResultSet.GetSqlMoney(ordinal);
        }

        public override SqlDecimal GetSqlDecimal(int ordinal)
        {
            return InnerResultSet.GetSqlDecimal(ordinal);
        }

        public override SqlByte GetSqlByte(int ordinal)
        {
            return InnerResultSet.GetSqlByte(ordinal);
        }

        public override SqlBinary GetSqlBinary(int ordinal)
        {
            return InnerResultSet.GetSqlBinary(ordinal);
        }

        public override SqlDateTime GetSqlDateTime(int ordinal)
        {
            return InnerResultSet.GetSqlDateTime(ordinal);
        }

        public override SqlDouble GetSqlDouble(int ordinal)
        {
            return InnerResultSet.GetSqlDouble(ordinal);
        }

        public override SqlSingle GetSqlSingle(int ordinal)
        {
            return InnerResultSet.GetSqlSingle(ordinal);
        }

        public override SqlGuid GetSqlGuid(int ordinal)
        {
            return InnerResultSet.GetSqlGuid(ordinal);
        }

        public override SqlInt16 GetSqlInt16(int ordinal)
        {
            return InnerResultSet.GetSqlInt16(ordinal);
        }

        public override SqlInt32 GetSqlInt32(int ordinal)
        {
            return InnerResultSet.GetSqlInt32(ordinal);
        }

        public override SqlInt64 GetSqlInt64(int ordinal)
        {
            return InnerResultSet.GetSqlInt64(ordinal);
        }

        public override SqlString GetSqlString(int ordinal)
        {
            return InnerResultSet.GetSqlString(ordinal);
        }

        public override DataTable GetSchemaTable()
        {
            return InnerResultSet.GetSchemaTable();
        }

        public override bool NextResult()
        {
            return InnerResultSet.NextResult();
        }

        public override bool Read()
        {
            return InnerResultSet.Read();
        }

        public override string GetDataTypeName(int index)
        {
            return InnerResultSet.GetDataTypeName(index);
        }

        public override Type GetFieldType(int ordinal)
        {
            return InnerResultSet.GetFieldType(ordinal);
        }

        public override Type GetProviderSpecificFieldType(int ordinal)
        {
            return InnerResultSet.GetProviderSpecificFieldType(ordinal);
        }

        public override string GetName(int index)
        {
            return InnerResultSet.GetName(index);
        }

        public override int GetOrdinal(string name)
        {
            return InnerResultSet.GetOrdinal(name);
        }

        public override char GetChar(int ordinal)
        {
            return InnerResultSet.GetChar(ordinal);
        }

        public override object GetProviderSpecificValue(int ordinal)
        {
            return InnerResultSet.GetProviderSpecificValue(ordinal);
        }

        public override int GetProviderSpecificValues(object[] values)
        {
            return InnerResultSet.GetProviderSpecificValues(values);
        }

        #endregion
    }
}
