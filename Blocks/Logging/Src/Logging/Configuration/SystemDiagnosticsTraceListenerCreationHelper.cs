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
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	internal static class SystemDiagnosticsTraceListenerCreationHelper
	{
		private static readonly object[] emptyParameters = new object[0];

		public static TraceListener CreateSystemDiagnosticsTraceListener(string name,
			Type traceListenerType,
			string initData,
			NameValueCollection attributes)
		{
			TraceListener traceListener = null;
			if (String.IsNullOrEmpty(initData))
			{
				ConstructorInfo defaultConstructor = traceListenerType.GetConstructor(Type.EmptyTypes);
				if (defaultConstructor != null)
				{
					traceListener = (TraceListener)defaultConstructor.Invoke(emptyParameters);
				}
				else
				{
					throw new InvalidOperationException(
						string.Format(Resources.Culture,
						Resources.ExceptionCustomListenerTypeDoesNotHaveDefaultConstructor,
						name,
						traceListenerType.FullName));
				}
			}
			else
			{
				try
				{
					traceListener = (TraceListener)Activator.CreateInstance(traceListenerType, initData);
				}
				catch (MissingMethodException exception)
				{
					throw new InvalidOperationException(
						string.Format(
							Resources.Culture,
							Resources.ExceptionCustomTraceListenerTypeDoesNotHaveRequiredConstructor,
							name,
							traceListenerType.FullName),
						exception);
				}
			}

			foreach (string attribute in attributes.Keys)
			{
				traceListener.Attributes.Add(attribute, attributes.Get(attribute));
			}

			return traceListener;
		}
	}
}
