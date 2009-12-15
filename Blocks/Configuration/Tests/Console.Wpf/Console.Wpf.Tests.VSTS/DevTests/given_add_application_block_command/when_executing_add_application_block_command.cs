using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_add_application_block_command
{
    [TestClass]
    public class when_executing_add_application_block_command : ContainerContext
    {
        Mock<IApplicationModel> applicationModelMock;
        AddApplicationBlockCommand addApplicationBlockCommand;

        protected override void Arrange()
        {
            base.Arrange();

            applicationModelMock = new Mock<IApplicationModel>();

            Container.RegisterInstance<IApplicationModel>(applicationModelMock.Object);

            addApplicationBlockCommand = Container.Resolve<AddApplicationBlockCommand>(new DependencyOverride<AddApplicationBlockCommandAttribute>(new AddApplicationBlockCommandAttribute(LoggingSettings.SectionName, typeof(LoggingSettings))));
        }


        protected override void Act()
        {
            addApplicationBlockCommand.Execute(null);
        }

        [TestMethod]
        public void then_application_model_is_set_to_dirty()
        {
            applicationModelMock.Verify();
        }
    }
}
