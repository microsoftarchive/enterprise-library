//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    /// <summary>
    ///	This class tracks a file cache dependency.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    public class FileDependency : ICacheItemExpiration
    {
        private readonly string dependencyFileName;

        private DateTime lastModifiedTime;

        /// <summary>
        ///	Constructor with one argument.
        /// </summary>
        /// <param name="fullFileName">
        ///	Indicates the name of the file
        /// </param>
        public FileDependency(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName))
            {
                throw new ArgumentException("fullFileName", Resources.ExceptionNullFileName);
            }           

            dependencyFileName = Path.GetFullPath(fullFileName);
            EnsureTargetFileAccessible();

            if (!File.Exists(dependencyFileName))
            {
                throw new ArgumentException(Resources.ExceptionInvalidFileName, "fullFileName");
            }

            this.lastModifiedTime = File.GetLastWriteTime(fullFileName);
        }

		/// <summary>
		/// Gets the name of the dependent file.
		/// </summary>
		/// <value>
		/// The name of the dependent file.
		/// </value>
		public string FileName
		{
			get { return dependencyFileName; }
		}

		/// <summary>
		/// Gets the last modifed time of the file.
		/// </summary>
		/// <value>
		/// The last modifed time of the file
		/// </value>
		public DateTime LastModifiedTime
		{
			get { return lastModifiedTime; }
		}

        /// <summary>
        ///	Specifies if the item has expired or not.
        /// </summary>
        /// <returns>Returns true if the item has expired, otherwise false.</returns>
        public bool HasExpired()
        {
            EnsureTargetFileAccessible();

            if (File.Exists(this.dependencyFileName) == false)
            {
                return true;
            }

            DateTime currentModifiedTime = File.GetLastWriteTime(dependencyFileName);
            if (DateTime.Compare(lastModifiedTime, currentModifiedTime) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="owningCacheItem">Not used</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        }

        private void EnsureTargetFileAccessible()
        {
			// keep from changing during demand
			string file = dependencyFileName;
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, file);
            permission.Demand();
        }
    }
}