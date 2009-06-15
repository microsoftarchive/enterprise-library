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
using System.Data.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary/>
    public abstract class CommandAccessor<TResult>
    {
        IResultSetMapper<TResult> resultSetMapper;
        Database database;

        /// <summary/>
        protected CommandAccessor(Database database, IRowMapper<TResult> rowMapper)
            : this(database, new DefaultResultSetMapper(rowMapper))
        {
            if (rowMapper == null) throw new ArgumentNullException("rowMapper");
        }

        /// <summary/>
        protected CommandAccessor(Database database, IResultSetMapper<TResult> resultSetMapper)
        {
            if (database == null) throw new ArgumentNullException("database");
            if (resultSetMapper == null) throw new ArgumentNullException("resultSetMapper");

            this.database = database;
            this.resultSetMapper = resultSetMapper;
        }

        /// <summary/>
        protected IEnumerable<TResult> Execute(DbCommand command)
        {
            IDataReader reader = database.ExecuteReader(command);

            return resultSetMapper.MapSet(reader);
        }


        private class DefaultResultSetMapper : IResultSetMapper<TResult>
        {
            IRowMapper<TResult> rowMapper;

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
