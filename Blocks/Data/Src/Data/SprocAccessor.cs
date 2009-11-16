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
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Represents a stored procedure call to the database that will return an enumerable of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The element type that will be used to consume the result set.</typeparam>
    public class SprocAccessor<TResult> : CommandAccessor<TResult>
    {
        readonly IParameterMapper parameterMapper;
        readonly string procedureName;

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        public SprocAccessor(Database database, string procedureName, IRowMapper<TResult> rowMapper)
            : this(database, procedureName, new DefaultParameterMapper(database), rowMapper)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        public SprocAccessor(Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
            : this(database, procedureName, new DefaultParameterMapper(database), resultSetMapper)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        public SprocAccessor(Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
            : base(database, rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

            this.procedureName = procedureName;
            this.parameterMapper = parameterMapper;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
        /// <param name="procedureName">The stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        public SprocAccessor(Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
            : base(database, resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

            this.procedureName = procedureName;
            this.parameterMapper = parameterMapper;
        }

        /// <summary>
        /// Executes the stored procedure and returns an enumerable of <typeparamref name="TResult"/>.
        /// The enumerable returned by this method uses deferred loading to return the results.
        /// </summary>
        /// <param name="parameterValues">Values that will be interpret by an <see cref="IParameterMapper"/> and function as parameters to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public override IEnumerable<TResult> Execute(params object[] parameterValues)
        {
            using (DbCommand command = Database.GetStoredProcCommand(procedureName))
            {
                parameterMapper.AssignParameters(command, parameterValues);
                return base.Execute(command);
            }
        }

        /// <summary>Begin executing the database object asynchronously, returning
        /// a <see cref="IAsyncResult"/> object that can be used to retrieve
        /// the result set after the operation completes.</summary>
        /// <param name="callback">Callback to execute when the operation's results are available. May
        /// be null if you don't wish to use a callback.</param>
        /// <param name="state">Extra information that will be passed to the callback. May be null.</param>
        /// <param name="parameterValues">Parameters to pass to the database.</param>
        /// <remarks>This operation will throw if the underlying <see cref="Database"/> object does not
        /// support asynchronous operation.</remarks>
        /// <exception cref="InvalidOperationException">The underlying database does not support asynchronous operation.</exception>
        /// <returns>The <see cref="IAsyncResult"/> representing this operation.</returns>
        public override IAsyncResult BeginExecute(AsyncCallback callback, object state, params object[] parameterValues)
        {
            GuardAsyncAllowed();

            using (DbCommand command = Database.GetStoredProcCommand(procedureName))
            {
                return BeginExecute(command, parameterMapper, callback, state, parameterValues);
            }
        }

        private class DefaultParameterMapper : IParameterMapper
        {
            readonly Database database;
            public DefaultParameterMapper(Database database)
            {
                this.database = database;
            }

            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                if (parameterValues.Length > 0)
                {
                    GuardParameterDiscoverySupported();
                    database.AssignParameters(command, parameterValues);
                }
            }

            private void GuardParameterDiscoverySupported()
            {
                if (!database.SupportsParemeterDiscovery)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentCulture,
                                      Resources.ExceptionParameterDiscoveryNotSupported,
                                      database.GetType().FullName));
                }
            }
        }
    }
}
