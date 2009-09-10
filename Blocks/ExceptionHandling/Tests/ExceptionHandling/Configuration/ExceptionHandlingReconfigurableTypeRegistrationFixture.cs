//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using System.IO;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration
{
    public abstract class UpdatedConfigurationSourceContext : ArrangeActAssert
    {
        protected UnityContainerConfigurator containerConfigurator;
        protected UnityContainer container;
        protected ConfigurationSourceUpdatable updatableConfigurationSource;
        protected ExceptionHandlingSettings ehabSettings;
        protected ExceptionPolicyData exceptionPolicy1;
        protected ExceptionTypeData exceptionTypeData;

        protected override void Arrange()
        {
            updatableConfigurationSource = new ConfigurationSourceUpdatable();

            exceptionPolicy1 = new ExceptionPolicyData("default");
            exceptionTypeData = new ExceptionTypeData("Exception", "System.Exception", PostHandlingAction.ThrowNewException);
            exceptionPolicy1.ExceptionTypes.Add(exceptionTypeData);

            ehabSettings = new ExceptionHandlingSettings();
            ehabSettings.ExceptionPolicies.Add(exceptionPolicy1);

            updatableConfigurationSource.Add(ExceptionHandlingSettings.SectionName, ehabSettings);

            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            EnterpriseLibraryContainer.ConfigureContainer(containerConfigurator, updatableConfigurationSource);
        }

        protected override void Act()
        {
            updatableConfigurationSource.DoSourceChanged(new string[] { ExceptionHandlingSettings.SectionName });
        }
    }

    [TestClass]
    public class WhenExceptionPolicyConfigurationChanges : UpdatedConfigurationSourceContext
    {

        protected override void Act()
        {
            exceptionTypeData.PostHandlingAction = PostHandlingAction.NotifyRethrow;

            base.Act();
        }

        [TestMethod]
        public void ThenNewlyRetrievedPolicyReflectsNewConfiguration()
        {
            ExceptionPolicyImpl exceptionPolicy = container.Resolve<ExceptionPolicyImpl>(exceptionPolicy1.Name);
            bool notifyRetrow = exceptionPolicy.HandleException(new Exception());
            Assert.AreEqual(true, notifyRetrow);
        }
    }

    [TestClass]
    public class WhenInstrumentationConfigurationChanges : UpdatedConfigurationSourceContext
    {

        protected override void Act()
        {
            exceptionTypeData.PostHandlingAction = PostHandlingAction.NotifyRethrow;

            updatableConfigurationSource.DoSourceChanged(new string[] { InstrumentationConfigurationSection.SectionName });
        }

        [TestMethod]
        public void ThenNewlyRetrievedPolicyReflectsNewConfiguration()
        {
            ExceptionPolicyImpl exceptionPolicy = container.Resolve<ExceptionPolicyImpl>(exceptionPolicy1.Name);
            bool notifyRetrow = exceptionPolicy.HandleException(new Exception());
            Assert.AreEqual(true, notifyRetrow);
        }
    }
}
