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
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Data.SqlClient;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class SqlStringAccessor<TResult> : CommandAccessor<TResult>
    {
        Database database;
        string sqlString;

        /// <summary/>
        public SqlStringAccessor(Database database, string sqlString, IRowMapper<TResult> rowMapper)
            : base(database, rowMapper)
        {
            if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            this.database = database;
            this.sqlString = sqlString;
        }

        /// <summary/>
        public SqlStringAccessor(Database database, string sqlString, IResultSetMapper<TResult> resultSetMapper)
            : base(database, resultSetMapper)
        {
            if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            this.database = database;
            this.sqlString = sqlString;
        }

        /// <summary/>
        public IEnumerable<TResult> Execute(params object[] parameterValues)
        {
            var parameters = parameterValues.OfType<DbParameter>().ToArray();
            using (DbCommand command = database.GetSqlStringCommand(sqlString))
            {
                command.Parameters.AddRange(parameters);
                return base.Execute(command);
            }
        }
    }
}
