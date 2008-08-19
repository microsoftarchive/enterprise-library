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