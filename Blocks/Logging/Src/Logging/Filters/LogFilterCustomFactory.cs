//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the general process to build an <see cref="ILogFilter"/> object given a concrete sublcass of <see cref="LogFilterData"/>.
	/// </summary>
	/// <remarks>
	/// This type leverages the generic implementation from AssemblerBasedObjectFactory.
	/// </remarks>
	public class LogFilterCustomFactory : AssemblerBasedObjectFactory<ILogFilter, LogFilterData>
	{
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public static LogFilterCustomFactory Instance = new LogFilterCustomFactory();
	}
}
