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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.ExceptionHandling.given_an_exception_policy_add_command
{
    [TestClass]
    public class when_adding_an_exception_policy : ContainerContext
    {
        AddExceptionPolicyCommand addExceptionPolicyCommand;
        SectionViewModel exceptionSettingsViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            exceptionSettingsViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, new ExceptionHandlingSettings());
            var exceptionPolicyContainer = exceptionSettingsViewModel.GetDescendentsOfType<NamedElementCollection<ExceptionPolicyData>>().First();
            
            addExceptionPolicyCommand = exceptionPolicyContainer.AddCommands.OfType<AddExceptionPolicyCommand>().First();
        }

        protected override void Act()
        {
            addExceptionPolicyCommand.Execute(null);
        }

        [TestMethod]
        public void then_added_policy_has_exception_type_element()
        {
            var exceptionTypeElement = addExceptionPolicyCommand.AddedElementViewModel.GetDescendentsOfType<ExceptionTypeData>().Single();
            Assert.IsNotNull(exceptionTypeElement);
            Assert.AreEqual(typeof(Exception), Type.GetType((string)exceptionTypeElement.Property("TypeName").Value));
        }
    }
}
