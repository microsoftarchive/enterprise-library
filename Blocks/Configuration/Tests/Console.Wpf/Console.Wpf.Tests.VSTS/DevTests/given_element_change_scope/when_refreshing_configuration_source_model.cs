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

using System.Collections.Specialized;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_change_scope
{
    [TestClass]
    public class when_refreshing_configuration_source_model : ExceptionHandlingConfigurationSectionContext
    {
        SectionViewModel exceptionHandlingViewModel;
        IElementChangeScope changeScopeForHandlers;
        NotifyCollectionChangedEventArgs changeScopeChangeNotification;

        protected override void Arrange()
        {
            base.Arrange();

            exceptionHandlingViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, base.ExceptionSettings);

            ElementLookup lookup = Container.Resolve<ElementLookup>();
            changeScopeForHandlers = lookup.CreateChangeScope(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType));

            changeScopeForHandlers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(changeScopeForHandlers_CollectionChanged);
        }

        void changeScopeForHandlers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            changeScopeChangeNotification = e;
        }

        protected override void Act()
        {
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.New();
        }


        [TestMethod]
        [Ignore] //changescopes should have finer grained events
        public void then_change_scope_changed()
        {
            Assert.IsNotNull(changeScopeChangeNotification);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, changeScopeChangeNotification.Action);
        }


        [TestMethod]
        public void then_change_scope_is_empty()
        {
            Assert.AreEqual(0, changeScopeForHandlers.Count());
        }


    }
}
