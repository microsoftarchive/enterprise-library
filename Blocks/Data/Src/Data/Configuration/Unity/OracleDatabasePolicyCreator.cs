//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a <see cref="OracleDatabase"/>.
	/// </summary>
	public class OracleDatabasePolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			ConnectionStringSettings castConfigurationObject = (ConnectionStringSettings)configurationObject;

			IList<IOraclePackage> packages = new IOraclePackage[0];

			// set up the translating packages, if necessary
			OracleConnectionSettings oracleConnectionSettings = OracleConnectionSettings.GetSettings(configurationSource);
			if (oracleConnectionSettings != null)
			{
				OracleConnectionData oracleConnectionData
					= oracleConnectionSettings.OracleConnectionsData.Get(castConfigurationObject.Name);
				if (oracleConnectionData != null)
				{
					packages = new List<IOraclePackage>(from op in oracleConnectionData.Packages select (IOraclePackage)op);
				}
			}

			new PolicyBuilder<OracleDatabase, ConnectionStringSettings>(
				instanceName,
				castConfigurationObject,
				c => new OracleDatabase(c.ConnectionString, packages))

				.AddPoliciesToPolicyList(policyList);
		}
	}
}