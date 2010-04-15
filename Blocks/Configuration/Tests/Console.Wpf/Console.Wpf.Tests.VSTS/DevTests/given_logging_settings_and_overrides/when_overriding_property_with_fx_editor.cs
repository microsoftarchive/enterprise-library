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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    [TestClass]
    public class when_overriding_property_with_fx_editor : LoggingSettingsContext
    {
        private Property propertyWithFxEditor;
        private Property overriddenPropertyWithFxEditor;

        protected override void Act()
        {
            var eventLogListener = base.LoggingSectionViewModel.GetDescendentsOfType<FormattedEventLogTraceListenerData>().First();

            eventLogListener.InheritedFromParentConfiguration = false;
            foreach (var ancestor in eventLogListener.AncestorElements()) ancestor.InheritedFromParentConfiguration = false;

            var applicationModel = Container.Resolve<ApplicationViewModel>();
            applicationModel.NewEnvironment();

            propertyWithFxEditor = eventLogListener.Property("TraceOutputOptions");
            var overridesProperty = eventLogListener.Properties.OfType<EnvironmentOverriddenElementProperty>().First();
            overriddenPropertyWithFxEditor = overridesProperty.ChildProperties.Single(x => x.PropertyName == "TraceOutputOptions");
        }

        [TestMethod]
        public void then_overridden_property_has_fx_editor_bindable()
        {
            Assert.IsInstanceOfType(overriddenPropertyWithFxEditor.BindableProperty,
                                    typeof (FrameworkEditorBindableProperty));
        }

        [TestMethod]
        public void then_overridden_property_has_different_bindable_instance()
        {
            FrameworkEditorBindableProperty original = (FrameworkEditorBindableProperty)propertyWithFxEditor.BindableProperty;
            FrameworkEditorBindableProperty overridden = (FrameworkEditorBindableProperty)overriddenPropertyWithFxEditor.BindableProperty;

            Assert.AreNotSame(original, overridden);
        }

        [TestMethod]
        public void then_overridden_property_bindable_was_created_for_property()
        {
            FrameworkEditorBindableProperty overridden = (FrameworkEditorBindableProperty)overriddenPropertyWithFxEditor.BindableProperty;

            Assert.AreEqual(overriddenPropertyWithFxEditor, overridden.Property);
        }
    }
}
