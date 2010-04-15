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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Base class for Data Accessors that execute a <see cref="DbCommand"/>.
    /// </summary>
    /// <typeparam name="TResult">The element type this accessor will return.</typeparam>
    public abstract class CommandAccessor<TResult> : DataAccessor<TResult>
    {
        readonly IResultSetMapper<TResult> resultSetMapper;
        readonly Database database;

        /// <summary>
        /// Initialized the <see cref="CommandAccessor{TResult}"/> with a database instance and a Row Mapper.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the <see cref="DbCommand"/>.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper{TResult}"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        protected CommandAccessor(Database database, IRowMapper<TResult> rowMapper)
            : this(database, new DefaultResultSetMapper(rowMapper))
        {
            if (rowMapper == null) throw new ArgumentNullException("rowMapper");
        }

        /// <summary>
        /// Initialized the <see cref="CommandAccessor{TResult}"/> with a database instance and a Row Mapper.
        /// </summary>
        /// <param name="database">The <see cref="Database"/> used to execute the <see cref="DbCommand"/>.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper{TResult}"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        protected CommandAccessor(Database database, IResultSetMapper<TResult> resultSetMapper)
        {
            if (database == null) throw new ArgumentNullException("database");
            if (resultSetMapper == null) throw new ArgumentNullException("resultSetMapper");

            this.database = database;
            this.resultSetMapper = resultSetMapper;
        }

        /// <summary>
        /// The database object this accessor is wrapped around.
        /// </summary>
        protected Database Database { get { return database; } }

        /// <summary>
        /// Executes the <paramref name="command"/> and returns an enumerable of <typeparamref name="TResult"/>.
        /// The enumerable returned by this method uses deferred loading to return the results.
        /// </summary>
        /// <param name="command">The command that will be executed.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        protected IEnumerable<TResult> Execute(DbCommand command)
        {
            IDataReader reader = database.ExecuteReader(command);

            foreach (TResult result in resultSetMapper.MapSet(reader))
            {
                yield return result;
            }
        }

        /// <summary>
        /// Helper method to kick off execution of an asynchronous database operation.
        /// This method handles the boilerplate of setting up the parameters and invoking
        /// the operation on the database with the right options.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> to execute.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> to use to set the parameter
        /// values.</param>
        /// <param name="callback">Callback to execute when the operation's result is available.</param>
        /// <param name="state">State to pass to the callback.</param>
        /// <param name="parameterValues">Input parameter values.</param>
        /// <returns>An <see cref='IAsyncResult'/> object representing the outstanding async request.</returns>
        protected IAsyncResult BeginExecute(DbCommand command, IParameterMapper parameterMapper,
            AsyncCallback callback, object state, object[] parameterValues)
        {
            parameterMapper.AssignParameters(command, parameterValues);
            return database.BeginExecuteReader(command, callback, state);
        }

        /// <summary>Complete an operation started by <see cref="DataAccessor{TResult}.BeginExecute"/>.</summary>
        /// <returns>The result sequence.</returns>
        public override IEnumerable<TResult> EndExecute(IAsyncResult asyncResult)
        {
            GuardAsyncAllowed();

            IDataReader reader = database.EndExecuteReader(asyncResult);
            return resultSetMapper.MapSet(reader);
        }

        /// <summary>
        /// Checks if the current <see cref="Database"/> object supports asynchronous operations,
        /// and throws <see cref="InvalidOperationException"/> if not.
        /// </summary>
        /// <exception cref="InvalidOperationException">The database does not support asynchronous operations.</exception>
        protected void GuardAsyncAllowed()
        {
            if(!database.SupportsAsync)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                                  Resources.AsyncOperationsNotSupported,
                                  database.GetType().FullName));
            }
        }

        private class DefaultResultSetMapper : IResultSetMapper<TResult>
        {
            readonly IRowMapper<TResult> rowMapper;

            public DefaultResultSetMapper(IRowMapper<TResult> rowMapper)
            {
                this.rowMapper = rowMapper;
            }

            public IEnumerable<TResult> MapSet(IDataReader reader)
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        yield return rowMapper.MapRow(reader);
                    }
                }
            }
        }

    }
}
