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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_change_scope
{
    [TestClass]
    public class when_adding_elements_that_match_scope : ExceptionHandlingConfigurationSectionContext
    {
        private SectionViewModel exceptionHandlingViewModel;
        private IElementChangeScope changeScopeForHandlers;
        private NotifyCollectionChangedEventArgs lastHandlerChangeArgs;
        private IElementChangeScope neverFiringChangeScope;
        private bool neverFiringChangeScopeFired;
        private ElementViewModel addedHandlerViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            exceptionHandlingViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, base.ExceptionSettings);

            ElementLookup lookup = Container.Resolve<ElementLookup>();
            lookup.AddSection(exceptionHandlingViewModel);

            changeScopeForHandlers = lookup.CreateChangeScope(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType));
            changeScopeForHandlers.CollectionChanged += changeScopeForHandler;

            neverFiringChangeScope = lookup.CreateChangeScope(x => true == false);
            neverFiringChangeScope.CollectionChanged += shouldNeverFireCollectionChanged;
        }

        protected override void Act()
        {
            var handlersCollection = exceptionHandlingViewModel.DescendentConfigurationsOfType<NamedElementCollection<ExceptionHandlerData>>()
                .OfType<ElementCollectionViewModel>().First();
            addedHandlerViewModel = handlersCollection.AddNewCollectionElement(typeof (WrapHandlerData));
        }

        private void shouldNeverFireCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            neverFiringChangeScopeFired = true;
        }

        private void changeScopeForHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            lastHandlerChangeArgs = e;
        }

        [TestMethod]
        public void then_should_notify_add_change_for_added_handler()
        {
            Assert.AreEqual(NotifyCollectionChangedAction.Add, lastHandlerChangeArgs.Action);
        }

        [TestMethod]
        public void then_should_only_include_added_element()
        {
            Assert.AreEqual(1, lastHandlerChangeArgs.NewItems.Count);
            Assert.AreSame(addedHandlerViewModel, lastHandlerChangeArgs.NewItems[0]);
        }

        [TestMethod]
        public void then_should_not_fire_for_nonmatching_predicate()
        {
            Assert.IsFalse(neverFiringChangeScopeFired);
        }
    }

    [TestClass]
    public class when_removing_elements_that_match_scope : ExceptionHandlingConfigurationSectionContext
    {
        private SectionViewModel exceptionHandlingViewModel;
        private IElementChangeScope changeScopeForHandlers;
        private NotifyCollectionChangedEventArgs lastHandlerChangeArgs;
        private IElementChangeScope neverFiringChangeScope;
        private bool neverFiringChangeScopeFired;
        private ElementViewModel removedHandlerViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            exceptionHandlingViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, base.ExceptionSettings);

            ElementLookup lookup = Container.Resolve<ElementLookup>();
            lookup.AddSection(exceptionHandlingViewModel);

            changeScopeForHandlers = lookup.CreateChangeScope(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType));
            changeScopeForHandlers.CollectionChanged += changeScopeForHandler;

            neverFiringChangeScope = lookup.CreateChangeScope(x => true == false);
            neverFiringChangeScope.CollectionChanged += shouldNeverFireCollectionChanged;
        }

        protected override void Act()
        {
            removedHandlerViewModel = exceptionHandlingViewModel.DescendentConfigurationsOfType<ExceptionHandlerData>().First();
            removedHandlerViewModel.Delete();
        }

        private void shouldNeverFireCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            neverFiringChangeScopeFired = true;
        }

        private void changeScopeForHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            lastHandlerChangeArgs = e;
        }

        [TestMethod]
        public void then_should_notify_remove_change_for_added_handler()
        {
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, lastHandlerChangeArgs.Action);
        }

        [TestMethod]
        public void then_should_only_include_added_element()
        {
            Assert.AreEqual(1, lastHandlerChangeArgs.OldItems.Count);
            Assert.AreSame(removedHandlerViewModel, lastHandlerChangeArgs.OldItems[0]);
        }

        [TestMethod]
        public void then_should_not_fire_for_nonmatching_predicate()
        {
            Assert.IsFalse(neverFiringChangeScopeFired);
        }
    }

}


