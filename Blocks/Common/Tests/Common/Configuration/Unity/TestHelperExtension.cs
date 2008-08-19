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

using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	public class TestHelperExtension : UnityContainerExtension
	{
		public TestHelperExtension()
			: this(context => { })
		{ }

		public TestHelperExtension(InitializeDelegate initialize)
		{
			this.initialize = initialize;
		}

		public delegate void InitializeDelegate(ExtensionContext context);

		internal InitializeDelegate initialize;

		protected override void Initialize()
		{
			this.initialize(this.Context);
		}
	}
}
