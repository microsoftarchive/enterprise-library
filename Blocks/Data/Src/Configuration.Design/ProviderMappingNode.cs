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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="DbProviderMapping"/> configuration elements.
	/// </summary>
    [Image(typeof(ProviderMappingNode))]
	public sealed class ProviderMappingNode : ConfigurationNode, IDatabaseProviderName
    {
        private string databaseTypeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ProviderMappingNode"/> class.
        /// </summary>
        public ProviderMappingNode() : this(new DbProviderMapping("System.Data.SqlClient", typeof(SqlDatabase)))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="ProviderMappingNode"/> class with a <see cref="DbProviderMapping"/> instance.
        /// </summary>
        /// <param name="dbProviderMapping">A <see cref="DbProviderMapping"/> instance.</param>
		public ProviderMappingNode(DbProviderMapping dbProviderMapping)
			: base(dbProviderMapping == null ? "System.Data.SqlClient" : dbProviderMapping.Name)
        {
			if (dbProviderMapping == null)
            {
				throw new ArgumentNullException("dbProviderMapping");
            }
            this.databaseTypeName = dbProviderMapping.DatabaseTypeName;
        }

		/// <summary>
		/// Gets or sets the name of the provider for this mapping.
		/// </summary>
		/// <value>
		/// The name of the provider for this mapping.
		/// </value>
		[Editor(typeof(ProviderEditor), typeof(UITypeEditor))]
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

        /// <summary>
        /// Gets or sets the type of <see cref="Database"/> for this mapping.
        /// </summary>
		/// <value>
		/// The type of <see cref="Database"/> for this mapping.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Database))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("DatabaseTypeFullyQualifedNameDescription", typeof(Resources))]
        public string TypeName
        {
            get { return databaseTypeName; }
            set { databaseTypeName = value; }
        }

        /// <summary>
        /// Gets the <see cref="DbProviderMapping"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="DbProviderMapping"/> this node represents.
		/// </value>
        [Browsable(false)]
        public DbProviderMapping ProviderMapping
        {
            get { return new DbProviderMapping(Name, databaseTypeName); }
        }
		
		[Browsable(false)]
		string IDatabaseProviderName.DatabaseProviderName
		{
			get { return Name; }			
		}
    }
}