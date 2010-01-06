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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationElementManageabilityProviderBaseFixture
        : ConfigurationElementManageabilityProviderBase<NamedConfigurationElement>
    {
        NamedConfigurationElement configurationObject, configurationObjectParameter;
        IRegistryKey policyKeyParameter, machineKey, userKey;
        bool overrideCalled;
        bool throwOnOverride;
        List<Exception> loggedExceptions;

        public ConfigurationElementManageabilityProviderBaseFixture() { }

        [TestInitialize]
        public void SetUp()
        {
            overrideCalled = false;
            throwOnOverride = false;
            configurationObject = new NamedConfigurationElement("original");
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            loggedExceptions = new List<Exception>();
        }

        [TestMethod]
        public void OverrideIsNotInvokedIfGroupPolicyIsDisabled()
        {
            OverrideWithGroupPolicies(configurationObject,
                                                           false, machineKey, userKey);

            Assert.IsFalse(overrideCalled);
        }

        [TestMethod]
        public void OverrideIsNotInvokedIfRegistryKeysAreNull()
        {
            OverrideWithGroupPolicies(configurationObject,
                                                           true, null, null);

            Assert.IsFalse(overrideCalled);
        }

        [TestMethod]
        public void OverrideIsInvokedWithMachineKeyIfOnlyKey()
        {
            OverrideWithGroupPolicies(configurationObject,
                                                           true, machineKey, null);

            Assert.IsTrue(overrideCalled);
            Assert.AreSame(machineKey, policyKeyParameter);
            Assert.AreSame(configurationObject, configurationObjectParameter);
        }

        [TestMethod]
        public void OverrideIsInvokedWithUserKeyIfOnlyKey()
        {
            OverrideWithGroupPolicies(configurationObject,
                                                           true, null, userKey);

            Assert.IsTrue(overrideCalled);
            Assert.AreSame(userKey, userKey);
            Assert.AreSame(configurationObject, configurationObjectParameter);
        }

        [TestMethod]
        public void OverrideIsInvokedWithMachineKeyIfBothKeys()
        {
            OverrideWithGroupPolicies(configurationObject,
                                                           true, machineKey, userKey);

            Assert.IsTrue(overrideCalled);
            Assert.AreSame(machineKey, policyKeyParameter);
            Assert.AreSame(configurationObject, configurationObjectParameter);
        }

        [TestMethod]
        public void ExceptionIsLoggedIfExceptionIsThrownOnOverride()
        {
            throwOnOverride = true;

            OverrideWithGroupPolicies(configurationObject,
                                                           true, machineKey, userKey);

            Assert.IsTrue(overrideCalled);
            Assert.AreSame(machineKey, policyKeyParameter);
            Assert.AreSame(configurationObject, configurationObjectParameter);

            Assert.AreEqual(1, loggedExceptions.Count);
            Assert.AreEqual("override", loggedExceptions[0].Message);
        }

        protected override void OverrideWithGroupPolicies(NamedConfigurationElement configurationObject,
                                                          IRegistryKey policyKey)
        {
            overrideCalled = true;
            configurationObjectParameter = configurationObject;
            policyKeyParameter = policyKey;

            if (throwOnOverride)
            {
                throw new Exception("override");
            }
            configurationObject.Name = "overriden";
        }

        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    NamedConfigurationElement configurationObject,
                                                                    IConfigurationSource configurationSource,
                                                                    string elementPolicyKeyName)
        {
            // ignored
        }

        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      NamedConfigurationElement configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      string elementPolicyKeyName)
        {
            // ignored
        }

        protected override string ElementPolicyNameTemplate
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        protected override void LogExceptionWhileOverriding(Exception exception)
        {
            loggedExceptions.Add(exception);
        }
    }
}
