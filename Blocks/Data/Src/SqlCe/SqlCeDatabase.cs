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
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe
{
    /// <summary>
    ///		Provides helper methods to make working with a Sql Server Compact Edition database
    ///		easier.
    /// </summary>
    /// <remarks>
    ///		<para>
    ///			Because SQL Server CE has no connection pooling and the cost of opening the initial
    ///			connection is high, this class implements a simple connection pool.
    ///		</para>
    ///		<para>
    ///			SQL Server CE requires full trust to run, so it cannot be used in partial-trust
    ///			environments.
    ///		</para>
    /// </remarks>
    [ConfigurationElementType(typeof(SqlCeDatabaseData))]
    public class SqlCeDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeDatabase"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlCeDatabase(string connectionString)
            : base(connectionString, SqlCeProviderFactory.Instance)
        {
        }

        /// <summary>
        ///		This will close the "keep alive" connection that is kept open after you first access
        ///		the database through this class. Calling this method will close the "keep alive"
        ///		connection for all instances. The next database access will open a new "keep alive"
        ///		connection.
        /// </summary>
        public void CloseSharedConnection()
        {
            SqlCeConnectionPool.CloseSharedConnection(this);
        }

        /// <summary>
        ///		<para>Creates a connection for this database.</para>
        /// </summary>
        /// <remarks>
        ///		This method has been overridden to support keeping a single connection open until you
        ///		explicitly close it with a call to <see cref="CloseSharedConnection"/>.
        /// </remarks>
        /// <returns>
        ///		<para>The <see cref="DbConnection"/> for this database.</para>
        /// </returns>
        /// <seealso cref="DbConnection"/>        
        public override DbConnection CreateConnection()
        {
            using (DatabaseConnectionWrapper wrapper = SqlCeConnectionPool.CreateConnection(this))
            {
                wrapper.AddRef();
                wrapper.Connection.ConnectionString = ConnectionString;
                return wrapper.Connection;
            }
        }

        /// <summary>
        /// Gets a "wrapped" connection for use outside a transaction.
        /// </summary>
        /// <returns>The wrapped connection.</returns>
        protected override DatabaseConnectionWrapper GetWrappedConnection()
        {
            return SqlCeConnectionPool.CreateConnection(this, true);
        }

        /// <summary>
        ///		Don't need an implementation for SQL Server CE.
        /// </summary>
        /// <param name="discoveryCommand"></param>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((SqlCeDataAdapter)adapter).RowUpdated += OnSqlRowUpdated;
        }

        internal void SetConnectionString(DbConnection connection)
        {
            connection.ConnectionString = ConnectionString;
        }

        #region Not Implemented Overrides

        /// <summary>
        ///		Stored procedures are not supported in SQL Server CE.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        public override DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///		Stored procedures are not supported in SQL Server CE.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        public override int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///		Stored procedures are not supported in SQL Server CE.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        public override object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///		Stored procedures are not supported in SQL Server CE.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="parameterValues"></param>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        public override void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///		Because SQL Server CE doesn't support stored procedures, we've changed queries
        ///		that take a stored procedure name to take a SQL statement instead.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        public override DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///		Because SQL Server CE doesn't support stored procedures, we've changed queries
        ///		that take a stored procedure name to take a SQL statement instead.
        /// </summary>
        /// <param name="storedProcedureName">The SQL statement to execute.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        public override DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Builds a value parameter name for the current database.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formatted parameter name.</returns>
        public override string BuildParameterName(string name)
        {
            if (name[0] != ParameterToken)
            {
                return name.Insert(0, new string(ParameterToken, 1));
            }
            return name;
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
        /// </summary>
        /// <value>
        /// <para>The "@" symbol.</para>
        /// </value>
        protected char ParameterToken
        {
            get { return '@'; }
        }

        #region SqlCeExtensions

        /// <summary>
        ///		Creates a new, empty database file using the file name provided in the Data Source attribute
        ///		of the connection string.
        /// </summary>
        public void CreateFile()
        {
            SqlCeEngine engine = new SqlCeEngine(ConnectionString);
            engine.CreateDatabase();
        }

        /// <summary>
        ///		Creates a new parameter and sets the name of the parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <param name="value">
        ///		The value you want to assign to the parameter. A null value will be converted to
        ///		a <see cref="DBNull"/> value in the parameter.
        /// </param>
        /// <returns>
        ///		A new <see cref="DbParameter"/> instance of the correct type for this database.</returns>
        /// <remarks>
        ///		The database will automatically add the correct prefix (for example, "@" for SQL Server) to the
        ///		parameter name. You can just supply the name without a prefix.
        /// </remarks>
        public virtual DbParameter CreateParameter(string name, DbType type, int size, object value)
        {
            DbParameter param = CreateParameter(name);
            param.DbType = type;
            param.Size = size;
            param.Value = (value == null) ? DBNull.Value : value;
            return param;
        }

        /// <summary>
        ///		Executes a SQL SELECT statement and returns a dataset.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSetSql(string sqlCommand, params DbParameter[] parameters)
        {
            using (DbCommand command = GetSqlStringCommand(sqlCommand))
            {
                AddParameters(command, parameters);
                return ExecuteDataSet(command);
            }
        }

        /// <summary>
        ///		Executes a SQL statement directly, because SQL Server CE
        ///		doesn't support stored procedures.
        /// </summary>
        /// <param name="sqlCommand">The SQL statement to execute.</param>
        /// <param name="parameters">An optional set of parameters and values.</param>
        /// <returns>Number of rows affected.</returns>
        public virtual int ExecuteNonQuerySql(string sqlCommand, params DbParameter[] parameters)
        {
            using (DbCommand command = GetSqlStringCommand(sqlCommand))
            {
                AddParameters(command, parameters);
                return ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///		Executes an INSERT statement and given the identity of
        ///		the row that was inserted for identity tables.
        /// </summary>
        /// <param name="sqlCommand">The SQL statement to execute.</param>
        /// <param name="lastAddedId">The identity value for the last row added, or <see cref="DBNull"/>.</param>
        /// <param name="parameters">Zero or more parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int ExecuteNonQuerySql(string sqlCommand, out int lastAddedId, params DbParameter[] parameters)
        {
            using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
            {
                using (DbCommand command = GetSqlStringCommand(sqlCommand))
                {
                    AddParameters(command, parameters);
                    PrepareCommand(command, wrapper.Connection);
                    int result = DoExecuteNonQuery(command);
                    lastAddedId = GetLastId(wrapper.Connection);
                    return result;
                }
            }
        }

        /// <summary>
        ///		Executes a command and returns a <see cref="DbDataReader"/> that contains the rows
        ///		returned.
        /// </summary>
        /// <param name="sqlCommand">The SQL query to execute.</param>
        /// <param name="parameters">Zero or more parameters for the query.</param>
        /// <returns>A <see cref="DbDataReader"/> that contains the rows returned by the query.</returns>
        public virtual IDataReader ExecuteReaderSql(string sqlCommand, params DbParameter[] parameters)
        {
            using (DbCommand command = GetSqlStringCommand(sqlCommand))
            {
                AddParameters(command, parameters);
                return ExecuteReader(command);
            }
        }

        /// <summary>
        ///		SQL Server CE provides a new type of data reader, the <see cref="SqlCeResultSet"/>, that provides
        ///		new abilities and better performance over a standard reader. This method provides access to
        ///		this reader.
        /// </summary>
        /// <remarks>
        ///		The <see cref="SqlCeResultSet"/> returned from this method will close the connection on dispose.
        /// </remarks>
        /// <param name="command">
        ///		The command that contains the SQL SELECT statement to execute.
        /// </param>
        /// <param name="options">Controls how the <see cref="SqlCeResultSet"/> behaves.</param>
        /// <param name="parameters">An option set of <see cref="DbParameter"/> parameters.</param>
        /// <returns>The reader in the form of a <see cref="SqlCeResultSet"/>.</returns>
        public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, ResultSetOptions options, params DbParameter[] parameters)
        {
            using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
            {
                AddParameters(command, parameters);
                PrepareCommand(command, wrapper.Connection);
                return new SqlCeResultSetWrapper(wrapper, DoExecuteResultSet((SqlCeCommand)command, options));
            }
        }

        /// <summary>
        ///		SQL Server CE provides a new type of data reader, the <see cref="SqlCeResultSet"/>, that provides
        ///		new abilities and better performance over a standard reader. This method provides access to
        ///		this reader.
        /// </summary>
        /// <remarks>
        ///		The <see cref="SqlCeResultSet"/> returned from this method will close the connection on dispose.
        /// </remarks>
        /// <param name="command">The command that contains the SQL SELECT statement to execute.</param>
        /// <param name="parameters">An option set of <see cref="DbParameter"/> parameters.</param>
        /// <returns>The reader in the form of a <see cref="SqlCeResultSet"/>.</returns>
        public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, params DbParameter[] parameters)
        {
            return ExecuteResultSet(command, ResultSetOptions.None, parameters);
        }

        /// <summary>
        ///		SQL Server CE provides a new type of data reader, the <see cref="SqlCeResultSet"/>, that provides
        ///		new abilities and better performance over a standard reader. This method provides access to
        ///		this reader.
        /// </summary>
        ///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
        ///		does not set the command behavior to close the connection when you close the reader.
        ///		That means you'll need to close the connection yourself, by calling the
        ///		command.Connection.Close() method after you're finished using the reader.
        /// <param name="command">The command that contains the SQL SELECT statement to execute.</param>
        /// <param name="transaction">Transaction to execute the command under.</param>
        /// <param name="options">Controls how the <see cref="SqlCeResultSet"/> behaves.</param>
        /// <param name="parameters">An option set of <see cref="DbParameter"/> parameters.</param>
        /// <returns>The reader in the form of a <see cref="SqlCeResultSet"/>.</returns>
        public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, DbTransaction transaction, ResultSetOptions options, params DbParameter[] parameters)
        {
            AddParameters(command, parameters);
            PrepareCommand(command, transaction);
            return DoExecuteResultSet((SqlCeCommand)command, options);
        }

        /// <summary>
        ///		SQL Server CE provides a new type of data reader, the <see cref="SqlCeResultSet"/>, that provides
        ///		new abilities and better performance over a standard reader. This method provides access to
        ///		this reader.
        /// </summary>
        ///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
        ///		does not set the command behavior to close the connection when you close the reader.
        ///		That means you'll need to close the connection yourself, by calling the
        ///		command.Connection.Close() method after you're finished using the reader.
        /// <param name="command">The command that contains the SQL SELECT statement to execute.</param>
        /// <param name="transaction">Transaction to execute the command under.</param>
        /// <param name="parameters">An option set of <see cref="DbParameter"/> parameters.</param>
        /// <returns>The reader in the form of a <see cref="SqlCeResultSet"/>.</returns>
        public virtual SqlCeResultSet ExecuteResultSet(DbCommand command, DbTransaction transaction, params DbParameter[] parameters)
        {
            return ExecuteResultSet(command, transaction, ResultSetOptions.None, parameters);
        }

        private SqlCeResultSet DoExecuteResultSet(SqlCeCommand command, ResultSetOptions options)
        {
            DateTime startTime = DateTime.Now;
            SqlCeResultSet reader = command.ExecuteResultSet(options);
            return reader;
        }

        /// <summary>
        ///		Executes the <paramref name="sqlCommand"/> and returns the first column of the first
        ///		row in the result set returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="sqlCommand">The SQL statement to execute.</param>
        /// <param name="parameters">Zero or more parameters for the query.</param>
        /// <returns>
        /// <para>
        ///		The first column of the first row in the result set.
        /// </para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalarSql(string sqlCommand, params DbParameter[] parameters)
        {
            using (DbCommand command = GetSqlStringCommand(sqlCommand))
            {
                AddParameters(command, parameters);
                return ExecuteScalar(command);
            }
        }

        /// <summary>
        ///		Returns the ID of the most recently added row.
        /// </summary>
        /// <returns>
        ///		The ID of the row added, or -1 if no row was added or if the table doesn't have an identity column.
        ///	</returns>
        private int GetLastId(DbConnection connection)
        {
            using (DbCommand command = GetSqlStringCommand("SELECT @@IDENTITY"))
            {
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return -1;
                    if (reader[0] is DBNull)
                        return -1;
                    return Convert.ToInt32(reader[0], CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        ///		Adds any parameters to the command.
        /// </summary>
        /// <param name="command">The command object you want prepared.</param>
        /// <param name="parameters">Zero or more parameters to add to the command.</param>
        protected void AddParameters(DbCommand command, params DbParameter[] parameters)
        {
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                    command.Parameters.Add(parameters[i]);
            }
        }

        /// <summary>
        ///		Checks to see if a table exists in the open database.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>><b>true</b> if the table exists; otherwise, <b>false</b>.</returns>
        public virtual bool TableExists(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "tableName");

            string sql = @"
					SELECT	COUNT(*) 
					FROM	[INFORMATION_SCHEMA].[TABLES] 
					WHERE	[TABLE_NAME] = " + BuildParameterName("tableName");

            int count = (int)ExecuteScalarSql(sql, CreateParameter("tableName", DbType.String, 0, tableName));
            return (count != 0);
        }

        #endregion

        /// <devdoc>
        /// Listens for the RowUpdate event on a dataa dapter to support UpdateBehavior.Continue.
        /// </devdoc>
        private void OnSqlRowUpdated(object sender, SqlCeRowUpdatedEventArgs rowThatCouldNotBeWritten)
        {
            if (rowThatCouldNotBeWritten.RecordsAffected == 0)
            {
                if (rowThatCouldNotBeWritten.Errors != null)
                {
                    rowThatCouldNotBeWritten.Row.RowError = Resources.ExceptionMessageUpdateDataSetRowFailure;
                    rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }
    }
}
