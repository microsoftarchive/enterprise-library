//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	sealed class ExceptionHandlingSettingsBuilder 
	{
		private ExceptionHandlingSettingsNode exceptionHandlingSettingsNode;
		private IConfigurationUIHierarchy hierarchy;
		private ExceptionHandlingSettings exceptionHandlingSettings;

		public ExceptionHandlingSettingsBuilder(IServiceProvider serviceProvider, ExceptionHandlingSettingsNode exceptionHandlingSettingsNode) 
		{
			this.exceptionHandlingSettingsNode = exceptionHandlingSettingsNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}

		
		public ExceptionHandlingSettings Build()
		{
			exceptionHandlingSettings = new ExceptionHandlingSettings();
			if (!this.exceptionHandlingSettingsNode.RequirePermission)	// don't set if false
				exceptionHandlingSettings.SectionInformation.RequirePermission = this.exceptionHandlingSettingsNode.RequirePermission;
			BuildPolicies();
			return exceptionHandlingSettings;
		}

		private void BuildPolicies()
		{
			IList<ConfigurationNode> policies = hierarchy.FindNodesByType(exceptionHandlingSettingsNode, typeof(ExceptionPolicyNode));
			foreach (ConfigurationNode policyNode in policies)
			{
				exceptionHandlingSettings.ExceptionPolicies.Add(CreateExceptionPolicyData((ExceptionPolicyNode)policyNode));
			}
		}		

		private ExceptionPolicyData CreateExceptionPolicyData(ExceptionPolicyNode policyNode)
		{
			ExceptionPolicyData policyData = new ExceptionPolicyData(policyNode.Name);						
			IList<ConfigurationNode> exceptionTypes = hierarchy.FindNodesByType(policyNode, typeof(ExceptionTypeNode));
			foreach (ConfigurationNode exceptionTypeNode in exceptionTypes)
			{
				policyData.ExceptionTypes.Add(CreateExceptionTypeData((ExceptionTypeNode)exceptionTypeNode));
			}
			return policyData;
		}
		
		private static ExceptionTypeData CreateExceptionTypeData(ExceptionTypeNode exceptionTypeNode)
		{
			ExceptionTypeData exceptionTypeData = new ExceptionTypeData(exceptionTypeNode.Name, exceptionTypeNode.Type, exceptionTypeNode.PostHandlingAction);			
			foreach (ConfigurationNode exceptionHandlerNode in exceptionTypeNode.Nodes)
			{
				exceptionTypeData.ExceptionHandlers.Add(((ExceptionHandlerNode)exceptionHandlerNode).ExceptionHandlerData);
			}
			return exceptionTypeData;
		}
	}
}
