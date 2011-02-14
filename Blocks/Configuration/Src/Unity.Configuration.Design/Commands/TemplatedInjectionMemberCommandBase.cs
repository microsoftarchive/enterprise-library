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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Commands
{
    public class TemplatedInjectionMemberCommandBase : DefaultCollectionElementAddCommand
    {
        public TemplatedInjectionMemberCommandBase(ConfigurationElementType configurationElementType, ElementCollectionViewModel collection, IUIServiceWpf uiService)
            : base(configurationElementType, collection, uiService)
        {

        }
    }
}
