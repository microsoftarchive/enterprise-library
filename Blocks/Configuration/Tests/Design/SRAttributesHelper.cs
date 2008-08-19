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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
	public static class SRAttributesHelper
	{
		public static bool AssertSRDescription(Type type, string propertyName, string description)
		{
			PropertyInfo property = type.GetProperty(propertyName);
			SRDescriptionAttribute attribute = (SRDescriptionAttribute)property.GetCustomAttributes(typeof(SRDescriptionAttribute), false)[0];
			return description == attribute.Description;
		}

		public static bool AssertSRCategory(Type type, string propertyName)
		{
			PropertyInfo property = type.GetProperty(propertyName);
			SRCategoryAttribute attribute = (SRCategoryAttribute)property.GetCustomAttributes(typeof(SRCategoryAttribute), false)[0];
			return (Resources.CategoryGeneral == attribute.Category);
		}
	}
}
