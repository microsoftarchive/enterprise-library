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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    public abstract class given_host_adapter_and_type_descriptor : given_host_adapter
    {
        protected ICustomTypeDescriptor TraceListenerTypeDescriptor;

        protected override void Arrange()
        {
            base.Arrange();

            TraceListenerTypeDescriptor = new ComponentModelElement(base.TraceListener, HostAdapter);
        }
    }
}
