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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Adapter used to inject instrumentation attachment process into ObjectBuilder creation process
	/// for objects.
	/// </summary>
	/// <seealso cref="InstrumentationAttachmentStrategy"/>
	public class InstrumentationStrategy : EnterpriseLibraryBuilderStrategy
	{
		/// <summary>
		/// Implementation of <see cref="IBuilderStrategy.PreBuildUp"/>.
		/// </summary>
		/// <remarks>
		/// This implementation will attach instrumentation to the created objects. 
		/// </remarks>
		/// <param name="context">The build context.</param>
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);

			if (context.Existing != null && context.Existing is IInstrumentationEventProvider)
			{
				IConfigurationSource configurationSource = GetConfigurationSource(context);
				ConfigurationReflectionCache reflectionCache = GetReflectionCache(context);

				NamedTypeBuildKey key = (NamedTypeBuildKey)context.BuildKey;
				string id = key.Name;

				InstrumentationAttachmentStrategy instrumentation = new InstrumentationAttachmentStrategy();

				if (ConfigurationNameProvider.IsMadeUpName(id))
				{
					instrumentation.AttachInstrumentation(context.Existing, configurationSource, reflectionCache);
				}
				else
				{
					instrumentation.AttachInstrumentation(id, context.Existing, configurationSource, reflectionCache);
				}
			}
		}
	}
}
