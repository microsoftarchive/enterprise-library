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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Overrides
{
    public abstract class given_two_overrides_sections : ContainerContext
    {
        ApplicationViewModel applicationModel;
        protected ElementViewModel environment1;
        protected ElementViewModel environment2;

        protected override void Arrange()
        {
            base.Arrange();
            applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.NewEnvironment();
            applicationModel.NewEnvironment();

            environment1 = applicationModel.Environments.First();
            environment2 = applicationModel.Environments.Skip(1).First();
            
        }

    }
}
