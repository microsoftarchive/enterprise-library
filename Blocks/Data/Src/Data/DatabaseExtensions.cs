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
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public static SprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = new MapBuilder<TResult>().CreateDefault().BuildMapper();

            return CreateSprocAccessor<TResult>(database, procedureName, defaultRowMapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <param name="rowMapper"></param>
        /// <returns></returns>
        public static SprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);

            return new SprocAccessor<TResult>(database, procedureName, rowMapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <param name="resultSetMapper"></param>
        /// <returns></returns>
        public static SprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString);
            
            return new SprocAccessor<TResult>(database, procedureName, resultSetMapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public static SqlStringAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = new MapBuilder<TResult>().CreateDefault().BuildMapper();

            return CreateSqlStringAccessor<TResult>(database, procedureName, defaultRowMapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <param name="rowMapper"></param>
        /// <returns></returns>
        public static SqlStringAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string procedureName, IRowMapper<TResult> rowMapper)
        {
            return new SqlStringAccessor<TResult>(database, procedureName, rowMapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <param name="resultSetMapper"></param>
        /// <returns></returns>
        public static SqlStringAccessor<TResult> CreateSqlStringAccessor<TResult>(this Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            return new SqlStringAccessor<TResult>(database, procedureName, resultSetMapper);
        }
        
    }
}
