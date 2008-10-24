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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	partial class ConfigurationSetting
	{
		/// <summary>
		/// 
		/// </summary>
		public void Publish()
		{
			System.Management.Instrumentation.Instrumentation.Publish(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Revoke()
		{
			System.Management.Instrumentation.Instrumentation.Revoke(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<EventArgs> Changed
		{
			// no need to implement this event for RO WMI objects
			add { ;}
			remove { ;}
		}
	}
}
