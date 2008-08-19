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
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	/// <summary>
	/// Extends <see cref="ConstructorArgumentMatchingPolicyCreator"/> to handle the resolution of <see cref="ILogFormatter"/> 
	/// references in trace listener configuration objects.
	/// </summary>
	public class TraceListenerConstructorArgumentMatchingPolicyCreator : ConstructorArgumentMatchingPolicyCreator
	{
		/// <summary>
		/// Initializes a new instance of class <see cref="TraceListenerConstructorArgumentMatchingPolicyCreator"/> for a given type.
		/// </summary>
		/// <param name="targetType">The <see cref="Type"/> for which policies will be created.</param>
		public TraceListenerConstructorArgumentMatchingPolicyCreator(Type targetType)
			: base(CheckTraceListenerType(targetType))
		{ }

		private static Type CheckTraceListenerType(Type targetType)
		{
			Guard.ArgumentNotNull(targetType, "targetType");
			
			return targetType;
		}

		/// <summary>
		/// Checks <paramref name="parameterInfo"/> and <paramref name="propertyInfo"/> for type matching.
		/// </summary>
		/// <param name="parameterInfo">The constructor parameter to match</param>
		/// <param name="propertyInfo">The property to match.</param>
		/// <returns><see langword="true"/> if types are compatible, otherwise <see langword="false"/>.</returns>.
		/// <remarks>
		/// Overrides the inherited behavior to allow matching <see cref="ILogFormatter"/> parameters with <see cref="string"/>
		/// properties.
		/// </remarks>
		protected override bool MatchArgumentAndPropertyTypes(ParameterInfo parameterInfo, PropertyInfo propertyInfo)
		{
			return base.MatchArgumentAndPropertyTypes(parameterInfo, propertyInfo)
				|| (parameterInfo.ParameterType == typeof(ILogFormatter)
					&& propertyInfo.PropertyType == typeof(string));
		}

		/// <summary>
		/// Returns a <see cref="IDependencyResolverPolicy"/> suitable to resolve <paramref name="value"/>
		/// for <paramref name="parameterInfo"/>.
		/// </summary>
		/// <param name="parameterInfo">The constructor parameter.</param>
		/// <param name="value">The raw value from the source object.</param>
		/// <returns>
		/// An instance of <see cref="ReferenceResolverPolicy"/> if the parameter has type 
		/// <see cref="ILogFormatter"/> and <paramref name="value"/> is a <see cref="string"/>,
		/// otherwise a <see cref="ConstantResolverPolicy"/> that returns <paramref name="value"/>.
		/// </returns>
		protected override IDependencyResolverPolicy CreateDependencyResolverPolicy(ParameterInfo parameterInfo, object value)
		{
			if (parameterInfo.ParameterType == typeof(ILogFormatter))
			{
				// try to do magic with the formatter parameter
				if (value != null)
				{
					if (value is string)
					{
						string stringValue = (string)value;
						if (!string.IsNullOrEmpty(stringValue))
						{
							return new ReferenceResolverPolicy(NamedTypeBuildKey.Make<ILogFormatter>(stringValue));
						}
						else
						{
							// override the value to set null as the formatter
							value = null;
						}
					}
					else
					{
						Debug.Fail("This shouldn't happen. When matching the parameter and the property type compatibility should have been ensured.");
					}
				}
				else
				{
					// value is null, just let it be set.
				}
			}
			return base.CreateDependencyResolverPolicy(parameterInfo, value);
		}
	}
}
