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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks
{
    public class MockRegistryAccessor : IRegistryAccessor
    {
        private MockRegistryKey currentUser;
        private MockRegistryKey localMachine;

        public MockRegistryAccessor(MockRegistryKey currentUser, MockRegistryKey localMachine)
        {
            this.currentUser = currentUser;
            this.localMachine = localMachine;
        }

        public IRegistryKey CurrentUser
        {
            get { return currentUser; }
        }

        public IRegistryKey LocalMachine
        {
            get { return localMachine; }
        }
    }
}
