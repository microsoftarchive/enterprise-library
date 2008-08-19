//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents the <see cref="SecuritySettings"/> configuration section.
    /// </summary>
    [Image(typeof(SecuritySettingsNode))]
    [SelectedImage(typeof(SecuritySettingsNode))]    
    public sealed class SecuritySettingsNode : ConfigurationSectionNode
    {		
        private AuthorizationProviderNode defaultAuthorizationProviderNode;
        private SecurityCacheProviderNode defaultSecurityCacheProviderNode;

		private EventHandler<ConfigurationNodeChangedEventArgs> onAuthorizationProviderRemoved;
		private EventHandler<ConfigurationNodeChangedEventArgs> onSecurityCacheProviderRemoved;

        /// <summary>
        /// Initialize a new instance of the <see cref="SecuritySettingsNode"/> class.
        /// </summary>
        public SecuritySettingsNode() 
        {
			this.onAuthorizationProviderRemoved += new EventHandler<ConfigurationNodeChangedEventArgs>(OnAuthorizationDefaultProviderRemoved);
			this.onSecurityCacheProviderRemoved += new EventHandler<ConfigurationNodeChangedEventArgs>(OnSecurityCacheDefaultProviderRemoved);

			Name = Resources.SecuritySettingsNodeName;
        }		

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="SecuritySettingsNode"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{			
			if (disposing)
			{
				if (defaultAuthorizationProviderNode != null)
				{
					defaultAuthorizationProviderNode.Removed -= onAuthorizationProviderRemoved;
				}

				if (defaultSecurityCacheProviderNode != null)
				{
					defaultSecurityCacheProviderNode.Removed -= onSecurityCacheProviderRemoved;
				}

			}
			base.Dispose(disposing);
		}


        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }

		/// <summary>
		/// Gets or sets the default authorization provider.
		/// </summary>
		/// <value>
		/// The default authorization provider.
		/// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(AuthorizationProviderNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("DefaultProviderDescription", typeof(Resources))]
        public AuthorizationProviderNode DefaultAuthorizationInstance
        {
            get { return defaultAuthorizationProviderNode; }
            set
            {
                defaultAuthorizationProviderNode = LinkNodeHelper.CreateReference<AuthorizationProviderNode>(defaultAuthorizationProviderNode,
                                                                                 value,
                                                                                 onAuthorizationProviderRemoved,
                                                                                 null);
     
            }
        }

		/// <summary>
		/// Gets or sets the default security cache provider.
		/// </summary>
		/// <value>
		/// The default security cache provider.
		/// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(SecurityCacheProviderNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public SecurityCacheProviderNode DefaultSecurityCacheInstance
        {
            get { return defaultSecurityCacheProviderNode; }
            set
            {
                defaultSecurityCacheProviderNode = LinkNodeHelper.CreateReference<SecurityCacheProviderNode>(defaultSecurityCacheProviderNode,
                                                                                 value,
                                                                                 onSecurityCacheProviderRemoved,
                                                                                 null);
                
            }
        }
      
        private void OnAuthorizationDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultAuthorizationProviderNode = null;
        }

        private void OnSecurityCacheDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultSecurityCacheProviderNode = null;
        }
    }
}