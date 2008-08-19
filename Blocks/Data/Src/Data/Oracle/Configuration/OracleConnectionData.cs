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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
	/// <summary>
	/// Oracle-specific connection information.
	/// </summary>
	public class OracleConnectionData : NamedConfigurationElement
	{
		private const string packagesProperty = "packages";

		/// <summary>
		/// Initializes a new instance of the <see cref="OracleConnectionData"/> class with default values.
		/// </summary>
		public OracleConnectionData()
		{
		}

		/// <summary>
		/// Gets a collection of <see cref="OraclePackageData"/> objects.
		/// </summary>
		/// <value>
		/// A collection of <see cref="OraclePackageData"/> objects.
		/// </value>
		[ConfigurationProperty(packagesProperty, IsRequired = true)]
		public NamedElementCollection<OraclePackageData> Packages
		{
			get
			{
				return (NamedElementCollection<OraclePackageData>)base[packagesProperty];
			}
		}
	}
}
