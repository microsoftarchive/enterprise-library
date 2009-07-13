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
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Represents a call to the database using SQL that will return an enumerable of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The element type that will be used to consume the result set.</typeparam>
    public class SqlStringAccessor<TResult> : CommandAccessor<TResult>
    {
        readonly IParameterMapper parameterMapper;
        readonly string sqlString;

        /// <summary>
        /// Creates a new instance of <see cref="SqlStringAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the SQL.</param>
        /// <param name="sqlString">The SQL that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        public SqlStringAccessor(Database database, string sqlString, IRowMapper<TResult> rowMapper)
            : this(database, sqlString, new DefaultSqlStringParameterMapper(), rowMapper)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlStringAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the SQL.</param>
        /// <param name="sqlString">The SQL that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        public SqlStringAccessor(Database database, string sqlString, IResultSetMapper<TResult> resultSetMapper)
            : this(database, sqlString, new DefaultSqlStringParameterMapper(), resultSetMapper)
        {
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="SqlStringAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the SQL.</param>
        /// <param name="sqlString">The SQL that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        public SqlStringAccessor(Database database, string sqlString, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
            : base(database, rowMapper)
        {
            if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

            this.parameterMapper = parameterMapper;
            this.sqlString = sqlString;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlStringAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the SQL.</param>
        /// <param name="sqlString">The SQL that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        public SqlStringAccessor(Database database, string sqlString, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
            : base(database, resultSetMapper)
        {
            if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

            this.parameterMapper = parameterMapper;
            this.sqlString = sqlString;
        }

        

        /// <summary>
        /// Executes the SQL query and returns an enumerable of <typeparamref name="TResult"/>.
        /// The enumerable returned by this method uses deferred loading to return the results.
        /// </summary>
        /// <param name="parameterValues">Values that will be interpret by an <see cref="IParameterMapper"/> and function as parameters to the Transact SQL query.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public override IEnumerable<TResult> Execute(params object[] parameterValues)
        {
            using (var command = Database.GetSqlStringCommand(sqlString))
            {
                parameterMapper.AssignParameters(command, parameterValues);
                foreach (TResult result in base.Execute(command))
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Begin executing the SQL query asynchronously. Only supported if the underlying
        /// <see cref="Database"/> object supports asynchronous operations.
        /// </summary>
        /// <param name="callback">Asynchronous callback to execute when the result of the query is
        /// available. May be null if no callback is desired.</param>
        /// <param name="state">Extra arbitrary state information to pass to the callback. May be null.</param>
        /// <param name="parameterValues">Parameters to pass to the sql query.</param>
        /// <returns>An <see cref="IAsyncResult"/> object representing the pending request.</returns>
        public override IAsyncResult BeginExecute(AsyncCallback callback, object state, params object[] parameterValues)
        {
            GuardAsyncAllowed();
            using (var command = Database.GetSqlStringCommand(sqlString))
            {
                return BeginExecute(command, parameterMapper, callback, state, parameterValues);
            }
        }

        private class DefaultSqlStringParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                if (parameterValues.Length > 0)
                {
                    throw new InvalidOperationException(Resources.ExceptionSqlStringAccessorCannotDiscoverParameters);
                }
            }
        }
    }
}
