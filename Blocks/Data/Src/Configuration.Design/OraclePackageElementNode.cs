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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="OraclePackageData"/> configuration element.
	/// </summary>
	[Image(typeof(OraclePackageElementNode))]
	public sealed class OraclePackageElementNode : ConfigurationNode
	{
		private string prefix;

		/// <summary>
		/// Initialize a new instance of the <see cref="OraclePackageElementNode"/> class.
		/// </summary>
		public OraclePackageElementNode()
			: this(new OraclePackageData(Resources.OracleConnectionElementNodeName, string.Empty))
		{
		}


		/// <summary>
		/// Initialize a new instance of the <see cref="OraclePackageElementNode"/> class with a <see cref="OraclePackageData"/> instance.
		/// </summary>
		/// <param name="oraclePackageElement">A <see cref="OraclePackageData"/> instance.</param>
		public OraclePackageElementNode(OraclePackageData oraclePackageElement)
			: base(null == oraclePackageElement ? string.Empty : oraclePackageElement.Name)
		{
			if (null == oraclePackageElement) throw new ArgumentNullException("oraclePackageElement");
			this.prefix = oraclePackageElement.Prefix;
		}

		/// <summary>
		/// Gets or sets the prefix for the Oracle package.
		/// </summary>
		/// <value>
		/// The prefix for the Oracle package.
		/// </value>
		[Required]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("PrefixDescription", typeof(Resources))]
		public string Prefix
		{
			get { return prefix; }
			set { prefix = value; }
		}

		/// <summary>
		/// Gets the <see cref="OraclePackageData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="OraclePackageData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public OraclePackageData OraclePackageElement
		{
			get { return new OraclePackageData(Name, prefix); }
		}
	}
}
