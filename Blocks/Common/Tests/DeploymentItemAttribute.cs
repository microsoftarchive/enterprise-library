using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Tests
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class DeploymentItemAttribute : Attribute
	{
		public DeploymentItemAttribute(string ignored)
		{ }
	}
}
