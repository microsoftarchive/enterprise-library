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
using System.Collections;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <devdoc>
    /// CachingMechanism provides caching support for stored procedure 
    /// parameter discovery and caching
    /// </devdoc>
    internal class CachingMechanism
    {
        private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <devdoc>
        /// Create and return a copy of the IDataParameter array.
        /// </devdoc>        
        public static IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
        {
            IDataParameter[] clonedParameters = new IDataParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (IDataParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        /// <devdoc>
        /// Empties all items from the cache
        /// </devdoc>
        public void Clear()
        {
            this.paramCache.Clear();
        }

        /// <devdoc>
        /// Add a parameter array to the cache for the command.
        /// </devdoc>        
		public void AddParameterSetToCache(string connectionString, IDbCommand command, IDataParameter[] parameters)
        {
            string storedProcedure = command.CommandText;
            string key = CreateHashKey(connectionString, storedProcedure);
            this.paramCache[key] = parameters;
        }

        /// <devdoc>
        /// Gets a parameter array from the cache for the command. Returns null if no parameters are found.
        /// </devdoc>        
		public IDataParameter[] GetCachedParameterSet(string connectionString, IDbCommand command)
        {
            string storedProcedure = command.CommandText;
            string key = CreateHashKey(connectionString, storedProcedure);
            IDataParameter[] cachedParameters = (IDataParameter[])(this.paramCache[key]);
            return CloneParameters(cachedParameters);
        }

        /// <devdoc>
        /// Gets if a given stored procedure on a specific connection string has a cached parameter set
        /// </devdoc>        
		public bool IsParameterSetCached(string connectionString, IDbCommand command)
        {
            string hashKey = CreateHashKey(
				connectionString,
                command.CommandText);
            return this.paramCache[hashKey] != null;
        }

        private static string CreateHashKey(string connectionString, string storedProcedure)
        {
            return connectionString + ":" + storedProcedure;
        }
    }
}
