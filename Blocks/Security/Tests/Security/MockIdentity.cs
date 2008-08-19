//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Principal;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    public class MockIdentity : IIdentity
    {
        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Mock"; }
        }

        public string AuthenticationType
        {
            get { return "Mock"; }
        }
    }
}

