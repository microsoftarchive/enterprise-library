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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Class that contains extension methods that apply on <see cref="Database"/>.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor<TResult>(database, procedureName).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor<TResult>(database, procedureName, parameterMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, rowMapper).Execute(parameterValues);
        }
        
        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, parameterMapper, rowMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateSprocAccessor(database, procedureName, parameterMapper, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, procedureName, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, procedureName, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            return new SprocAccessor<TResult>(database, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, parameterMapper, resultSetMapper);
        }

        /// <summary>
        /// Executes a Transact-SQL query and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSqlStringAccessor<TResult>(this Database database, string sqlString)
            where TResult : new()
        {
            return CreateSqlStringAccessor<TResult>(database, sqlString).Execute();   
        }


        /// <summary>
        /// Executes a Transact-SQL query and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSqlStringAccessor<TResult>(this Database database, string sqlString, IResultSetMapper<TResult> resultSetMapper)
        {
            return CreateSqlStringAccessor(database, sqlString, resultSetMapper).Execute();
        }


        /// <summary>
        /// Executes a Transact-SQL query and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSqlStringAccessor<TResult>(this Database database, string sqlString, IRowMapper<TResult> rowMapper)
        {
            return CreateSqlStringAccessor(database, sqlString, rowMapper).Execute();
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return new SqlStringAccessor<TResult>(database, sqlString, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return new SqlStringAccessor<TResult>(database, sqlString, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IRowMapper<TResult> rowMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IResultSetMapper<TResult> resultSetMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SqlStringAccessor&lt;TResult&gt;"/> for the given Transact-SQL query.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="sqlString">The Transact-SQL query that will be executed by the <see cref="SqlStringAccessor&lt;TResult&gt;"/>.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public static DataAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string sqlString, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            return new SqlStringAccessor<TResult>(database, sqlString, parameterMapper, resultSetMapper);
        }
    }
}
