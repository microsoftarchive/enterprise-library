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
using System.Data;
using System.Reflection;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary/>
    public class SprocAccessor<TResult> : CommandAccessor<TResult>, IDataAccessor<TResult>
    {
        Database database;
        string procedureName;

        /// <summary/>
        public SprocAccessor(Database database, string procedureName, IRowMapper<TResult> rowMapper)
            : base(database, rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            this.database = database;
            this.procedureName = procedureName;
        }

        /// <summary/>
        public SprocAccessor(Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
            : base(database, resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            this.database = database;
            this.procedureName = procedureName;
        }

        /// <summary/>
        public IEnumerable<TResult> Execute(params object[] parameterValues)
        {
            using(DbCommand command = database.GetStoredProcCommand(procedureName, parameterValues))
            {
                return base.Execute(command);
            }
        }
    }
}
