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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    class ProtectedConfigurationSource : DesignDictionaryConfigurationSource
    {
        public string protectionProviderNameOnLastCall;
        public int ProtectedAddCallCount;

        public override void Add(string sectionName, System.Configuration.ConfigurationSection configurationSection, string protectionProviderName)
        {
            ProtectedAddCallCount++;
            protectionProviderNameOnLastCall = protectionProviderName;   
        }

    }
}
