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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration
{
	public class MockValidationSettings : SerializableConfigurationSection
	{
		private const string ValidatorsPropertyName = "";
		[ConfigurationProperty(ValidatorsPropertyName, IsDefaultCollection = true)]
		public ValidatorDataCollection Validators
		{
			get { return (ValidatorDataCollection)this[ValidatorsPropertyName]; }
		}
	}
}
