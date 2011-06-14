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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.IsolatedStorageCacheDataScenarios.given_empty_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected IsolatedStorageCacheData data;

        protected override void Arrange()
        {
            base.Arrange();

            this.data = new IsolatedStorageCacheData 
                { 
                    Name = "test name", 
                };
        }
    }
}
