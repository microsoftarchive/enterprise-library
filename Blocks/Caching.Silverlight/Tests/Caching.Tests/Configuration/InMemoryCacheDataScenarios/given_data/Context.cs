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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.InMemoryCacheDataScenarios.given_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCacheData data;

        protected override void Arrange()
        {
            base.Arrange();

            this.data =
                new InMemoryCacheData
                {
                    Name = "test name",
                    MaxItemsBeforeScavenging = 500,
                    ItemsLeftAfterScavenging = 300,
                    ExpirationPollingInterval = TimeSpan.FromSeconds(45)
                };
        }
    }
}
