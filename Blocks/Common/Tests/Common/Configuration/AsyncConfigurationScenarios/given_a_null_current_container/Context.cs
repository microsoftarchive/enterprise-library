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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.AsyncConfigurationScenarios.given_a_null_current_container
{
    public abstract class Context : ArrangeActAssert
    {
        protected EnterpriseLibraryConfigurationCompletedEventArgs EventArgs;

        protected override void Arrange()
        {
            base.Arrange();

            Common.Configuration.EnterpriseLibraryContainer.Current = null;
            this.EventArgs = null;

            Common.Configuration.EnterpriseLibraryContainer.EnterpriseLibraryConfigurationCompleted +=
              (o, e) => this.EventArgs = e;
        }

        protected override void Teardown()
        {
            Common.Configuration.EnterpriseLibraryContainer.Current = null;

            base.Teardown();
        }
    }
}
