//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design
{
	/// <summary>
	/// 
	/// </summary>
	public class SqlConfigurationSourceElementNode : ConfigurationSourceElementNode
	{
        private string connectionString;
        private string getStoredProc;
        private string setStoredProc;
        private string refreshStoredProc;
        private string removeStoredProc;
        private Type type;

		/// <summary>
		/// 
		/// </summary>
		public SqlConfigurationSourceElementNode()
			: this(new SqlConfigurationSourceElement())
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		public SqlConfigurationSourceElementNode(SqlConfigurationSourceElement element)
			: base(null == element ? string.Empty : element.Name)
		{
			if (null == element) throw new ArgumentNullException("element");

			this.connectionString = element.ConnectionString;
		    this.getStoredProc = element.GetStoredProcedure;
		    this.setStoredProc = element.SetStoredProcedure;
            this.refreshStoredProc = element.RefreshStoredProcedure;
            this.removeStoredProc = element.RemoveStoredProcedure;
		    
			this.type = element.Type;
		}

        /// <summary>
        /// 
        /// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TypeNameDescription", typeof(Resources))]
        [ReadOnly(true)]
        public Type Type
        {
            get { return type; }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        [SRCategory("CategorySql", typeof(Resources))]
		[SRDescription("SqlConnectionStringDescription", typeof(Resources))]
        [Editor(typeof(SqlConnectionStringEditor), typeof(UITypeEditor))]
        [Required]
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
		[SRCategory("CategorySql", typeof(Resources))]
		[SRDescription("SqlGetSProcDescription", typeof(Resources))]
        [Required]
        public string GetStoredProcedure
        {
            get
            {
                return getStoredProc;
            }
            set
            {
                getStoredProc = value;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
		[SRCategory("CategorySql", typeof(Resources))]
		[SRDescription("SqlSetSProcDescription", typeof(Resources))]
        [Required]
        public string SetStoredProcedure
        {
            get
            {
                return setStoredProc;
            }
            set
            {
                setStoredProc = value;
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
		[SRCategory("CategorySql", typeof(Resources))]
		[SRDescription("SqlRefreshSProcDescription", typeof(Resources))]
        [Required]
        public string RefreshStoredProcedure
        {
            get
            {
                return refreshStoredProc;
            }
            set
            {
                refreshStoredProc = value;
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
		[SRCategory("CategorySql", typeof(Resources))]
		[SRDescription("SqlRemoveSProcDescription", typeof(Resources))]
        [Required]
        public string RemoveStoredProcedure
        {
            get
            {
                return removeStoredProc;
            }
            set
            {
                removeStoredProc = value;
            }
        }
        
	    
	    ///// <summary>
        ///// 
        ///// </summary>
        public override ConfigurationSourceElement ConfigurationSourceElement
        {
            get { return new SqlConfigurationSourceElement(Name, ConnectionString, GetStoredProcedure, SetStoredProcedure, RefreshStoredProcedure, RemoveStoredProcedure); }
        }

        /// <summary>
        /// 
        /// </summary>
	    public override IConfigurationParameter ConfigurationParameter
        {
            get
            {
                return new SqlConfigurationParameter(ConnectionString, GetStoredProcedure, SetStoredProcedure, RefreshStoredProcedure, RemoveStoredProcedure);
            }
        }

        /// <summary>
        /// 
        /// </summary>
	    public override IConfigurationSource ConfigurationSource
        {
            get
            {
                return new SqlConfigurationSource(ConnectionString, GetStoredProcedure, SetStoredProcedure, RefreshStoredProcedure, RemoveStoredProcedure);
            }
        }
    }
}

