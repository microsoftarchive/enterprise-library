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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Configuration;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_delete_element_command
{
    [TestClass]
    public class when_executing_default_delete_command : ContainerContext
    {
        DefaultDeleteCommandModel deleteCommand;
        Mock<IApplicationModel> applicationModelMock;

        protected override void Arrange()
        {
            base.Arrange();

            applicationModelMock = new Mock<IApplicationModel>();

            var section = SectionViewModel.CreateSection(Container, "appSettings", new AppSettingsSection());
            applicationModelMock.Setup(x => x.SetDirty()).Verifiable();

            Container.RegisterInstance<IApplicationModel>(applicationModelMock.Object);
            deleteCommand = Container.Resolve<DefaultDeleteCommandModel>(new DependencyOverride<ElementViewModel>(section));
        }


        protected override void Act()
        {
            deleteCommand.Execute(null);
        }

        [TestMethod]
        public void then_application_model_is_set_to_dirty()
        {
            applicationModelMock.Verify();
        }
    }
}
