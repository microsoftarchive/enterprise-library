//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Configuration parameter for file-based configuration sources.
    /// </summary>
    public class FileConfigurationParameter : IConfigurationParameter
    {
        private readonly string fileName;

        /// <summary>
		/// Initializes a new instance of the <see cref="FileConfigurationParameter"/> class with a file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public FileConfigurationParameter(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }
    }
}
