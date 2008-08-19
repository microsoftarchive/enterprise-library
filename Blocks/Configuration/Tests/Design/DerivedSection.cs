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
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests 
{
    public class DerivedSection : InstrumentationConfigurationSection
    {
		private const string newProperty = "newProperty";

		[ConfigurationProperty(newProperty)]
		public string NewProperty
		{
			get { return (string)base[newProperty];  }
			set { base[newProperty] = value;  }
		}        
    }
}
