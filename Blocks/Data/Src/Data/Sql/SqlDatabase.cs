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
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql
{
    /// <summary>
    /// <para>Represents a SQL Server database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses SQL Server .NET Managed Provider from Microsoft (System.Data.SqlClient) to connect to the database.
    /// </para>  
    /// </remarks>
    [SqlClientPermission(SecurityAction.Demand)]
    [ConfigurationElementType(typeof(SqlDatabaseData))]
    public class SqlDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlDatabase(string connectionString)
            : base(connectionString, SqlClientFactory.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a
        /// connection string and instrumentation provider.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="instrumentationProvider">The instrumentation provider.</param>
        public SqlDatabase(string connectionString, IDataInstrumentationProvider instrumentationProvider)
            : base(connectionString, SqlClientFactory.Instance, instrumentationProvider)
        {

        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
        /// </summary>
        /// <value>
        /// <para>The '@' symbol.</para>
        /// </value>
        protected char ParameterToken
        {
            get { return '@'; }
        }

        /// <summary>
        /// Does this <see cref='Database'/> object support asynchronous execution?
        /// </summary>
        /// <value>true.</value>
        public override bool SupportsAsync
        {
            get { return true; }
        }

        /// <summary>
        /// <para>Executes the <see cref="SqlCommand"/> and returns a new <see cref="XmlReader"/>.</para>
        /// </summary>
        /// <remarks>
        ///	When the returned reader is closed, the underlying connection will be closed
        /// (with appropriate handling of connections in the case of an ambient transaction).
        /// This is a behavior change from Enterprise Library versions prior to v5.
        /// </remarks>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="XmlReader"/> object.</para>
        /// </returns>
        public XmlReader ExecuteXmlReader(DbCommand command)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return new RefCountingXmlReader(wrapper, DoExecuteXmlReader(sqlCommand));
            }
        }

        /// <summary>
        /// <para>Executes the <see cref="SqlCommand"/> in a transaction and returns a new <see cref="XmlReader"/>.</para>
        /// </summary>
        /// <remarks>
        ///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
        ///		does not set the command behavior to close the connection when you close the reader.
        ///		That means you'll need to close the connection yourself, by calling the
        ///		command.Connection.Close() method.
        /// </remarks>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="XmlReader"/> object.</para>
        /// </returns>
        public XmlReader ExecuteXmlReader(DbCommand command, DbTransaction transaction)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            PrepareCommand(sqlCommand, transaction);
            return DoExecuteXmlReader(sqlCommand);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <see cref="SqlCommand"/> which will result in a <see cref="XmlReader"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <seealso cref="ExecuteXmlReader(DbCommand)"/>
        /// <seealso cref="EndExecuteXmlReader"/>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteXmlReader"/>, 
        /// which returns the <see cref="XmlReader"/> object.</para>
        /// </returns>
        public IAsyncResult BeginExecuteXmlReader(DbCommand command, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(command, connection);
                return DoBeginExecuteXmlReader(sqlCommand, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <see cref="SqlCommand"/> inside a transaction which will result in a <see cref="XmlReader"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <seealso cref="ExecuteXmlReader(DbCommand, DbTransaction)"/>
        /// <seealso cref="EndExecuteXmlReader"/>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteXmlReader"/>, 
        /// which returns the <see cref="XmlReader"/> object.</para>
        /// </returns>
        public IAsyncResult BeginExecuteXmlReader(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteXmlReader(sqlCommand, callback, state);
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement, returning the requested data as XML.
        /// </summary>
        /// <param name="asyncResult">
        /// <para>The <see cref="IAsyncResult"/> returned by a call to any overload of <see cref="BeginExecuteXmlReader(DbCommand, AsyncCallback, object)"/>.</para>
        /// </param>
        /// <seealso cref="ExecuteXmlReader(DbCommand)"/>
        /// <seealso cref="BeginExecuteXmlReader(DbCommand, AsyncCallback, object)"/>
        /// <seealso cref="BeginExecuteXmlReader(DbCommand, DbTransaction, AsyncCallback, object)"/>
        /// <returns>
        /// <para>An <see cref="XmlReader"/> object that can be used to fetch the resulting XML data.</para>
        /// </returns>
        public XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
        {
            var daabAsyncResult = (DaabAsyncResult)asyncResult;
            var command = (SqlCommand)daabAsyncResult.Command;
            try
            {
                XmlReader reader = command.EndExecuteXmlReader(daabAsyncResult.InnerAsyncResult);
                instrumentationProvider.FireCommandExecutedEvent(daabAsyncResult.StartTime);

                if(command.Transaction == null)
                {
                    using (var wrapper = new DatabaseConnectionWrapper(command.Connection))
                    {
                        return new RefCountingXmlReader(wrapper, reader);
                    }
                }
                return reader;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                if (command.Transaction == null)
                {
                    // for a reader, the standard cleanup will not close the connection, so it needs to be closed
                    // in the catch block if necessary
                    command.Connection.Close();
                }
                throw;
            }
            finally
            {
                CleanupConnectionFromAsyncOperation(daabAsyncResult);
            }
        }

        private IAsyncResult DoBeginExecuteXmlReader(SqlCommand command, AsyncCallback callback, object state)
        {
            try
            {
                return WrappedAsyncOperation.BeginAsyncOperation(
                    callback,
                    cb => command.BeginExecuteXmlReader(cb, state),
                    ar => new DaabAsyncResult(ar, command, false, false, DateTime.Now));
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        /// <devdoc>
        /// Execute the actual XML Reader call.
        /// </devdoc>        
        private XmlReader DoExecuteXmlReader(SqlCommand sqlCommand)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                XmlReader reader = sqlCommand.ExecuteXmlReader();
                instrumentationProvider.FireCommandExecutedEvent(startTime);
                return reader;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(sqlCommand.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        private static SqlCommand CheckIfSqlCommand(DbCommand command)
        {
            SqlCommand sqlCommand = command as SqlCommand;
            if (sqlCommand == null) throw new ArgumentException(Resources.ExceptionCommandNotSqlCommand, "command");
            return sqlCommand;
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnSqlRowUpdated(object sender, SqlRowUpdatedEventArgs rowThatCouldNotBeWritten)
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

        /// <summary>
        /// Does this <see cref='Database'/> object support parameter discovery?
        /// </summary>
        /// <value>true.</value>
        public override bool SupportsParemeterDiscovery
        {
            get { return true; }
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>The <see cref="DbCommand"/> must be a <see cref="SqlCommand"/> instance.</remarks>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)discoveryCommand);
        }

        /// <summary>
        /// Returns the starting index for parameters in a command.
        /// </summary>
        /// <returns>The starting index for parameters in a command.</returns>
        protected override int UserParametersStartIndex()
        {
            return 1;
        }

        /// <summary>
        /// Builds a value parameter name for the current database.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formated parameter name.</returns>
        public override string BuildParameterName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (name[0] != ParameterToken)
            {
                return name.Insert(0, new string(ParameterToken, 1));
            }
            return name;
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((SqlDataAdapter)adapter).RowUpdated += OnSqlRowUpdated;
        }

        /// <summary>
        /// Determines if the number of parameters in the command matches the array of parameter values.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
        /// <param name="values">The array of parameter values.</param>
        /// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
        protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            int returnParameterCount = 1;
            int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>       
        public virtual void AddParameter(DbCommand command, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            if (command == null) throw new ArgumentNullException("command");

            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public void AddParameter(DbCommand command, string name, SqlDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the out parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public void AddOutParameter(DbCommand command, string name, SqlDbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public void AddInParameter(DbCommand command, string name, SqlDbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, SqlDbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public void AddInParameter(DbCommand command, string name, SqlDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected DbParameter CreateParameter(string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            SqlParameter param = CreateParameter(name) as SqlParameter;
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// Configures a given <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="param">The <see cref="DbParameter"/> to configure.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected virtual void ConfigureParameter(SqlParameter param, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.SqlDbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        private static SqlCommand CreateSqlCommandByCommandType(CommandType commandType, string commandText)
        {
            return new SqlCommand(commandText)
            {
                CommandType = commandType
            };
        }

        private IAsyncResult DoBeginExecuteNonQuery(SqlCommand command, bool disposeCommand, AsyncCallback callback, object state)
        {
            bool closeConnection = command.Transaction == null;

            try
            {
                return WrappedAsyncOperation.BeginAsyncOperation(
                    callback,
                    cb => command.BeginExecuteNonQuery(cb, state),
                    ar => new DaabAsyncResult(ar, command, disposeCommand, closeConnection, DateTime.Now));
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <see cref="SqlCommand"/> which will return the number of affected records.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <seealso cref="Database.ExecuteNonQuery(DbCommand)"/>
        /// <seealso cref="EndExecuteNonQuery(IAsyncResult)"/>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected records.</para>
        /// </returns>
        public override IAsyncResult BeginExecuteNonQuery(DbCommand command, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteNonQuery(sqlCommand, false, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <see cref="DbCommand"/> inside a transaction which will return the number of affected records.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <seealso cref="Database.ExecuteNonQuery(DbCommand)"/>
        /// <seealso cref="EndExecuteNonQuery(IAsyncResult)"/>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected records.</para>
        /// </returns>
        public override IAsyncResult BeginExecuteNonQuery(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteNonQuery(sqlCommand, false, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> which will return the number of rows affected.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected records.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteNonQuery(string,object[])"/>
        /// <seealso cref="EndExecuteNonQuery(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteNonQuery(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> inside a transaction which will return the number of rows affected.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected records.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteNonQuery(string,object[])"/>
        /// <seealso cref="EndExecuteNonQuery(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, string storedProcedureName,
            AsyncCallback callback, object state,
            params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> which will return the number of rows affected.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected records.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteNonQuery(CommandType,string)"/>
        /// <seealso cref="EndExecuteNonQuery(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteNonQuery(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> inside a tranasaction which will return the number of rows affected.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected records.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteNonQuery(CommandType,string)"/>
        /// <seealso cref="EndExecuteNonQuery(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText,
            AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement, returning the number of affected records.
        /// </summary>
        /// <param name="asyncResult">
        /// <para>The <see cref="IAsyncResult"/> returned by a call to any overload of <see cref="BeginExecuteNonQuery(DbCommand, AsyncCallback, object)"/>.</para>
        /// </param>
        /// <seealso cref="Database.ExecuteNonQuery(DbCommand)"/>
        /// <seealso cref="BeginExecuteNonQuery(DbCommand, AsyncCallback, object)"/>
        /// <seealso cref="BeginExecuteNonQuery(DbCommand, DbTransaction, AsyncCallback, object)"/>
        /// <returns>
        /// <para>The number of affected records.</para>
        /// </returns>
        public override int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            DaabAsyncResult daabAsyncResult = (DaabAsyncResult)asyncResult;
            SqlCommand command = (SqlCommand)daabAsyncResult.Command;
            try
            {
                int affected = command.EndExecuteNonQuery(daabAsyncResult.InnerAsyncResult);
                instrumentationProvider.FireCommandExecutedEvent(daabAsyncResult.StartTime);

                return affected;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
            finally
            {
                CleanupConnectionFromAsyncOperation(daabAsyncResult);
            }
        }

        private IAsyncResult DoBeginExecuteReader(SqlCommand command, bool disposeCommand, AsyncCallback callback, object state)
        {
            CommandBehavior commandBehavior =
                command.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;

            try
            {
                return WrappedAsyncOperation.BeginAsyncOperation(
                    callback,
                    cb => command.BeginExecuteReader(cb, state, commandBehavior),
                    ar => new DaabAsyncResult(ar, command, disposeCommand, false, DateTime.Now));
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of a <paramref name="command"/> which will return a <see cref="IDataReader"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteReader"/>, 
        /// which returns the <see cref="IDataReader"/>.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteReader(DbCommand)"/>
        /// <seealso cref="EndExecuteReader(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteReader(DbCommand command, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteReader(sqlCommand, false, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of a <paramref name="command"/> inside a transaction which will return a <see cref="IDataReader"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteReader"/>, 
        /// which returns the <see cref="IDataReader"/>.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteReader(DbCommand)"/>
        /// <seealso cref="EndExecuteReader(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteReader(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteReader(sqlCommand, false, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> which will return a <see cref="IDataReader"/>.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <param name="parameterValues">
        /// <para>An array of parameters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteReader"/>, 
        /// which returns the <see cref="IDataReader"/>.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteReader(string, object[])"/>
        /// <seealso cref="EndExecuteReader(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteReader(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteReader(sqlCommand, true, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> inside a transaction which will return a <see cref="IDataReader"/>.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <param name="parameterValues">
        /// <para>An array of parameters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteReader"/>, 
        /// which returns the <see cref="IDataReader"/>.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteReader(DbTransaction, string, object[])"/>
        /// <seealso cref="EndExecuteReader(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteReader(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteReader(sqlCommand, true, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="commandText"/> 
        /// interpreted as specified by the <paramref name="commandType" /> which will return 
        /// a <see cref="IDataReader"/>. When the async operation completes, the
        /// <paramref name="callback"/> will be invoked on another thread to process the
        /// result.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="callback"><see cref="AsyncCallback"/> to execute when the async operation
        /// completes.</param>
        /// <param name="state">State object passed to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteReader"/>, 
        /// which returns the <see cref="IDataReader"/>.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteReader(CommandType, string)"/>
        /// <seealso cref="EndExecuteReader(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteReader(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);

            DbConnection connection = this.GetNewOpenConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteReader(sqlCommand, true, callback, state);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> inside an transaction which will return a <see cref="IDataReader"/>.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteReader"/>, 
        /// which returns the <see cref="IDataReader"/>.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteReader(CommandType, string)"/>
        /// <seealso cref="EndExecuteReader(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteReader(DbTransaction transaction, CommandType commandType, string commandText,
            AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);

            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteReader(sqlCommand, true, callback, state);
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement, returning an <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="asyncResult">
        /// <para>The <see cref="IAsyncResult"/> returned by a call to any overload of BeginExecuteReader.</para>
        /// </param>
        /// <seealso cref="Database.ExecuteReader(DbCommand)"/>
        /// <seealso cref="BeginExecuteReader(DbCommand,AsyncCallback,object)"/>
        /// <seealso cref="BeginExecuteReader(DbCommand, DbTransaction,AsyncCallback,object)"/>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object that can be used to consume the queried information.</para>
        /// </returns>     
        public override IDataReader EndExecuteReader(IAsyncResult asyncResult)
        {
            DaabAsyncResult daabAsyncResult = (DaabAsyncResult)asyncResult;
            SqlCommand command = (SqlCommand)daabAsyncResult.Command;
            try
            {
                IDataReader reader = command.EndExecuteReader(daabAsyncResult.InnerAsyncResult);
                instrumentationProvider.FireCommandExecutedEvent(daabAsyncResult.StartTime);

                return reader;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                if (command.Transaction == null)
                {
                    // for a reader, the standard cleanup will not close the connection, so it needs to be closed
                    // in the catch block if necessary
                    command.Connection.Close();
                }
                throw;
            }
            finally
            {
                CleanupConnectionFromAsyncOperation(daabAsyncResult);
            }
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of a <paramref name="command"/> which will return a single value.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteScalar"/>, 
        /// which returns the actual result.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteScalar(DbCommand)"/>
        /// <seealso cref="EndExecuteScalar(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteScalar(DbCommand command, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(command, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of a <paramref name="command"/> inside a transaction which will return a single value.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteScalar"/>, 
        /// which returns the actual result.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteScalar(DbCommand, DbTransaction)"/>
        /// <seealso cref="EndExecuteScalar(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteScalar(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(command, transaction, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> which will return a single value.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <param name="parameterValues">
        /// <para>An array of parameters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteScalar"/>, 
        /// which returns the actual result.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteScalar(string, object[])"/>
        /// <seealso cref="EndExecuteScalar(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteScalar(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return BeginExecuteReader(storedProcedureName, callback, state, parameterValues);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> inside a transaction which will return a single value.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <param name="parameterValues">
        /// <para>An array of parameters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteScalar"/>, 
        /// which returns the actual result.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteScalar(DbTransaction, string, object[])"/>
        /// <seealso cref="EndExecuteScalar(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteScalar(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return BeginExecuteReader(transaction, storedProcedureName, callback, state, parameterValues);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> which will return a single value.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteScalar"/>, 
        /// which returns the actual result.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteScalar(CommandType, string)"/>
        /// <seealso cref="EndExecuteScalar(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteScalar(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(commandType, commandText, callback, state);
        }

        /// <summary>
        /// <para>Initiates the asynchronous execution of the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> inside an transaction which will return a single value.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="DbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="callback">The async callback to execute when the result of the operation is available. Pass <langword>null</langword>
        /// if you don't want to use a callback.</param>
        /// <param name="state">Additional state object to pass to the callback.</param>
        /// <returns>
        /// <para>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking <see cref="EndExecuteScalar"/>, 
        /// which returns the actual result.</para>
        /// </returns>
        /// <seealso cref="Database.ExecuteScalar(DbTransaction, CommandType, string)"/>
        /// <seealso cref="EndExecuteScalar(IAsyncResult)"/>
        public override IAsyncResult BeginExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText,
            AsyncCallback callback, object state)
        {
            return BeginExecuteReader(transaction, commandType, commandText, callback, state);
        }

        /// <summary>
        /// <para>Finishes asynchronous execution of a Transact-SQL statement, returning the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="asyncResult">
        /// <para>The <see cref="IAsyncResult"/> returned by a call to any overload of BeginExecuteScalar.</para>
        /// </param>
        /// <seealso cref="Database.ExecuteScalar(DbCommand)"/>
        /// <seealso cref="BeginExecuteScalar(DbCommand,AsyncCallback,object)"/>
        /// <seealso cref="BeginExecuteScalar(DbCommand,DbTransaction,AsyncCallback,object)"/>
        /// <returns>
        /// <para>The value of the first column of the first row in the result set returned by the query.
        /// If the result didn't have any columns or rows <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// </returns>     
        public override object EndExecuteScalar(IAsyncResult asyncResult)
        {
            using (IDataReader reader = EndExecuteReader(asyncResult))
            {
                if (!reader.Read() || reader.FieldCount == 0)
                {
                    return null;
                }
                return reader.GetValue(0);
            }
        }

        private static void CleanupConnectionFromAsyncOperation(DaabAsyncResult daabAsyncResult)
        {
            if (daabAsyncResult.DisposeCommand)
            {
                if (daabAsyncResult.Command != null)
                {
                    daabAsyncResult.Command.Dispose();
                }
            }
            if (daabAsyncResult.CloseConnection)
            {
                if (daabAsyncResult.Connection != null)
                {
                    daabAsyncResult.Connection.Close();
                }
            }
        }
    }
}
