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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Console.Wpf.Tests.VSTS.DevTests;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_view_model_commands
{
    public abstract class given_delete_element_command : ExceptionHandlingSettingsContext
    {
        protected SectionViewModel Viewmodel;
        protected CollectionElementViewModel Handler;

        protected override void Arrange()
        {
            base.Arrange();

            Viewmodel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);
            Handler = (CollectionElementViewModel) Viewmodel.DescendentElements(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            
        }
    }

    [TestClass]
    public class when_invoking_delete_command_collection_element_is_removed : given_delete_element_command
    {
        protected override void Act()
        {
            Handler.DeleteCommand.Execute(Handler);
        }

        [TestMethod]
        public void then_collectionElementIsRemoved()
        {
            Assert.IsFalse(Handler.ParentElement.ChildElements.Where(x => Handler == x).Any());
        }
    }
}
