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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
	public static class ValidationAttributeHelper
	{
		public static int GetValidationErrorsCount(IServiceProvider serviceProvider)
		{
			MockUIService uiService = (MockUIService)serviceProvider.GetService(typeof(IUIService));
			return uiService.ValidationErrorsCount;
		}

		public static int GetConfigurationErrorsCount(IServiceProvider serviceProvider)
		{
			MockUIService uiService = (MockUIService)serviceProvider.GetService(typeof(IUIService));
			return uiService.ConfigurationErrorsCount;
		}
	}
}
