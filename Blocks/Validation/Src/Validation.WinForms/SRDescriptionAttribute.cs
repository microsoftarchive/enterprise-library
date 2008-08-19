//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		public SRDescriptionAttribute(string resourceName)
			: base(GetResource(resourceName))
		{ }

		private static string GetResource(string resourceName)
		{
			string resource = Resources.ResourceManager.GetString(resourceName);
			return resource != null ? resource : resourceName;
		}
	}
}