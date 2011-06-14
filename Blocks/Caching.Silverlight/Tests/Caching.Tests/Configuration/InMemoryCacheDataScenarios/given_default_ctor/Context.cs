//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.InMemoryCacheDataScenarios.given_default_ctor
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCacheData Data;

        protected override void Arrange()
        {
            base.Arrange();

        }
    }
}
