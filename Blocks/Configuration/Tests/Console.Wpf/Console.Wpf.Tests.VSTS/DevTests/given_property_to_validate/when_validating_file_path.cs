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
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    public abstract class PathedPropertyContext : ArrangeActAssert
    {
        protected Property PathProperty { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            var serviceProvider = new Mock<IServiceProvider>();


            PathProperty = new CustomProperty(serviceProvider.Object, "testProp");

        }

        class CustomProperty : CustomProperty<string>
        {
            public CustomProperty(IServiceProvider serviceProvider, string propertyName)
                : base(serviceProvider, propertyName)
            {

            }

            public override object Value { get; set; }
        }

    }

    [TestClass]
    public class when_validating_valid_file_path : PathedPropertyContext
    {
       
        private FilePathValidator validator;
        private List<ValidationResult> errorResults;

        protected override void Arrange()
        {
            base.Arrange();

            var applicationModel = new Mock<IApplicationModel>();
            applicationModel.Setup(x => x.ConfigurationFilePath).Returns(Path.Combine(Environment.CurrentDirectory,
                                                                                      "test.config"));
            validator = new FilePathValidator() {ApplicationModel = applicationModel.Object};

            CreateTestFile("somefile.txt");

            PathProperty.Value = "somefile.txt";
        }

        private void CreateTestFile(string fileName)
        {
            var testFile = Path.Combine(Environment.CurrentDirectory, fileName);
            if (File.Exists(testFile)) File.Delete(testFile);
            var fileStream = File.CreateText(testFile);
            fileStream.WriteLine("test");
            fileStream.Close();
            fileStream.Dispose();
        }

        protected override void Act()
        {
            errorResults = new List<ValidationResult>();
            validator.Validate(PathProperty, PathProperty.Value.ToString(), errorResults);
        }

        [TestMethod]
        public void then_should_return_no_errors()
        {
            Assert.AreEqual(0, errorResults.Count);
        }
    }

    [TestClass]
    public class when_validating_unavailable_path_for_file : PathedPropertyContext
    {

        private FilePathValidator validator;
        private List<ValidationResult> errorResults;
        protected override void Arrange()
        {
            base.Arrange();

            var applicationModel = new Mock<IApplicationModel>();
            applicationModel.Setup(x => x.ConfigurationFilePath).Returns(Path.Combine(Environment.CurrentDirectory,
                                                                                      "test.config"));

            PathProperty.Value = "some/invalid/path/somefile.txt";
            validator = new FilePathValidator() {ApplicationModel = applicationModel.Object};
        }

        protected override void Act()
        {
            errorResults = new List<ValidationResult>();
            validator.Validate(PathProperty, PathProperty.Value.ToString(), errorResults);
        }

        [TestMethod]
        public void then_results_in_errors()
        {
            Assert.AreEqual(1, errorResults.Count);
        }
    }

    [TestClass]
    public class when_validating_invalid_path_for_file : PathedPropertyContext
    {

        private FilePathValidator validator;
        private List<ValidationResult> errorResults;
        protected override void Arrange()
        {
            base.Arrange();

            var applicationModel = new Mock<IApplicationModel>();
            applicationModel.Setup(x => x.ConfigurationFilePath).Returns(Path.Combine(Environment.CurrentDirectory,
                                                                                      "test.config"));

            PathProperty.Value = ">??<";
            validator = new FilePathValidator() { ApplicationModel = applicationModel.Object };
        }

        protected override void Act()
        {
            errorResults = new List<ValidationResult>();
            validator.Validate(PathProperty, PathProperty.Value.ToString(), errorResults);
        }

        [TestMethod]
        public void then_results_in_errors()
        {
            Assert.AreEqual(1, errorResults.Count);
        }
    }

    [TestClass]
    public class when_validating_unavailable_path_existsnce : PathedPropertyContext
    {
        private FilePathExistsValidator validator;
        private List<ValidationResult> errorResults;
        protected override void Arrange()
        {
            base.Arrange();

            var applicationModel = new Mock<IApplicationModel>();
            applicationModel.Setup(x => x.ConfigurationFilePath).Returns(Path.Combine(Environment.CurrentDirectory,
                                                                                      "test.config"));

            PathProperty.Value = "some/invalid/path/somefile.txt";
            validator = new FilePathExistsValidator() { ApplicationModel = applicationModel.Object };
        }

        protected override void Act()
        {
            errorResults = new List<ValidationResult>();
            validator.Validate(PathProperty, PathProperty.Value.ToString(), errorResults);
        }

        [TestMethod]
        public void then_results_in_errors()
        {
            Assert.AreEqual(1, errorResults.Count);
        }
    }

    [TestClass]
    public class when_validating_unavailable_path_over_unc_existsnce : PathedPropertyContext
    {
        private FilePathExistsValidator validator;
        private List<ValidationResult> errorResults;
        protected override void Arrange()
        {
            base.Arrange();

            var applicationModel = new Mock<IApplicationModel>();
            applicationModel.Setup(x => x.ConfigurationFilePath).Returns(Path.Combine(Environment.CurrentDirectory,
                                                                                      "test.config"));

            PathProperty.Value = "\\\\server\\file\\somefile.txt";
            validator = new FilePathExistsValidator() { ApplicationModel = applicationModel.Object };
        }

        protected override void Act()
        {
            errorResults = new List<ValidationResult>();
            validator.Validate(PathProperty, PathProperty.Value.ToString(), errorResults);
        }

        [TestMethod]
        public void then_results_doesnt_contain_errors()
        {
            Assert.AreEqual(0, errorResults.Count);
        }
    }
   
}
