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

using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    public class MockContextUtils : IContextUtils
    {
        public string GetActivityId()
        {
            throw new COMException();
        }

        public string GetApplicationId()
        {
            throw new COMException();
        }

        public string GetTransactionId()
        {
            throw new COMException();
        }

        public string GetDirectCallerAccountName()
        {
            throw new COMException();
        }

        public string GetOriginalCallerAccountName()
        {
            throw new COMException();
        }
    }
}

