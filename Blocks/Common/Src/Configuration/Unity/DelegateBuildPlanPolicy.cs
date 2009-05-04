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
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// An <see cref="IBuildPlanPolicy"/> implementation configured with a delegate that can be set to 
	/// replace the dynamically generated IL policies.
	/// </summary>
	/// <remarks>
	/// If a <see cref="DelegateBuildPlanPolicy"/> is set, all the creation stages will be overriden, 
	/// not just the constructor call.
	/// </remarks>
	public sealed class DelegateBuildPlanPolicy : IBuildPlanPolicy
	{
		private readonly Func<IBuilderContext, object> factoryDelegate;

		/// <summary>
		/// Initializes a new instance of class <see cref="DelegateBuildPlanPolicy"/> with a 
		/// delegate.
		/// </summary>
		/// <param name="factoryDelegate">The delagate to invoke when building an object.</param>
		public DelegateBuildPlanPolicy(Func<IBuilderContext, object> factoryDelegate)
		{
			Guard.ArgumentNotNull(factoryDelegate, "factoryDelegate");

			this.factoryDelegate = factoryDelegate;
		}

		void IBuildPlanPolicy.BuildUp(IBuilderContext context)
		{
			if (context.Existing == null)
			{
				context.Existing = factoryDelegate(context);
			}
		}
	}
}
