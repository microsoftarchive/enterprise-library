//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a custom trace listener.
	/// </summary>
	public class BaseCustomTraceListenerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			// this policy creator will not create policies to support the build plan
			// dynamic method generation.
			// instead, a fixed build plan policy is set

			BasicCustomTraceListenerData castConfigurationObject = (BasicCustomTraceListenerData)configurationObject;

			// local vars to avoid getting the configuration object in the delegate's closure
			string listenerName = castConfigurationObject.Name;
			Type listenerType = castConfigurationObject.Type;
			string initData = castConfigurationObject.InitData;
			TraceOptions traceOutputOptions = castConfigurationObject.TraceOutputOptions;
			NameValueCollection attributes = castConfigurationObject.Attributes;	// should this be cloned?
			TraceFilter filter = castConfigurationObject.Filter != SourceLevels.All ? new EventTypeFilter(castConfigurationObject.Filter) : null;
			string formatterName = castConfigurationObject is CustomTraceListenerData
				? ((CustomTraceListenerData)castConfigurationObject).Formatter
				: null;

			policyList.Set<IBuildPlanPolicy>(
				new DelegateBuildPlanPolicy(
					context =>
					{
						TraceListener traceListener
							= SystemDiagnosticsTraceListenerCreationHelper.CreateSystemDiagnosticsTraceListener(
								listenerName,
								listenerType,
								initData,
								attributes);

						// must set up shared properties here as the whole build plan policy will be overriden
						traceListener.Name = listenerName;
						traceListener.TraceOutputOptions = traceOutputOptions;
						traceListener.Filter = filter;

						CustomTraceListener customTraceListener = traceListener as CustomTraceListener;
						if (customTraceListener != null && !string.IsNullOrEmpty(formatterName))
						{
							IBuilderContext formatterContext = context.CloneForNewBuild(NamedTypeBuildKey.Make<ILogFormatter>(formatterName), null);
							customTraceListener.Formatter = (ILogFormatter)formatterContext.Strategies.ExecuteBuildUp(formatterContext);
						}

						return traceListener;
					}),
				new NamedTypeBuildKey(listenerType, instanceName));
		}
	}
}
