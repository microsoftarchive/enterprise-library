//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    internal class AddPolicyInjectionSettingsCommand : AddChildNodeCommand
    {
        public AddPolicyInjectionSettingsCommand(IServiceProvider serviceProvider) 
            : base(serviceProvider, typeof(PolicyInjectionSettingsNode))
        {
        }


        protected override void ExecuteCore(ConfigurationNode node)
        {
            base.ExecuteCore(node);
            if(ChildNode != null)
            {
                ChildNode.AddNode(new PolicyCollectionNode());
            }
        }
    }
}
