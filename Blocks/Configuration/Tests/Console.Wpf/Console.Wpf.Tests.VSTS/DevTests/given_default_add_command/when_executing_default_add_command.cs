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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;


namespace Console.Wpf.Tests.VSTS.DevTests.given_default_add_command
{
    [TestClass]
    public class when_executing_default_add_command : ContainerContext
    {
        DefaultCollectionElementAddCommand addCommand;
        Mock<IApplicationModel> applicationModelMock;

        protected override void Arrange()
        {
            base.Arrange();

            applicationModelMock = new Mock<IApplicationModel>();

            Container.RegisterInstance<IApplicationModel>(applicationModelMock.Object);

            var section = SectionViewModel.CreateSection(Container, "appSettings", new AppSettingsSection());
            addCommand = section.ChildElements.First().Commands.OfType<DefaultCollectionElementAddCommand>().First();

            applicationModelMock.Setup(x => x.SetDirty()).Verifiable();
        }


        protected override void Act()
        {
            addCommand.Execute(null);
        }

        [TestMethod]
        public void then_application_model_is_set_to_dirty()
        {
            applicationModelMock.Verify();
        }
    }
}
