using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{
    [TestClass]
    public class when_setting_application_file_path : given_dirty_application_model
    {
        PropertyChangedListener applicationModelChangedListener;

        protected override void Arrange()
        {
            base.Arrange();

            applicationModelChangedListener = new PropertyChangedListener(base.ApplicationModel);
        }

        protected override void Act()
        {
            base.ApplicationModel.ConfigurationFilePath = "new path";
        }

        [TestMethod]
        public void then_application_title_changed()
        {
            Assert.IsTrue(applicationModelChangedListener.ChangedProperties.Contains("ApplicationTitle"));
        }
    }
}
