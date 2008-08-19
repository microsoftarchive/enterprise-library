//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class AuthorizationCallHandlerFixture
    {
        GenericPrincipal FredPrincipal = new GenericPrincipal(new GenericIdentity("Fred"), new string[0]);
        GenericPrincipal JackPrincipal = new GenericPrincipal(new GenericIdentity("Jack"), new string[0]);

        PolicySet AllowFredPolicySet;
        PolicySet AllowJackPolicySet;
        PolicySet AllowBasedOnTokensPolicySet;

        [TestInitialize]
        public void TestInitialize()
        {
            IConfigurationSource authorizationConfiguration = new FileConfigurationSource("Authorization.config");

            RuleDrivenPolicy allowFredPolicy = new RuleDrivenPolicy("allowFred");
            allowFredPolicy.Handlers.Add(new AuthorizationCallHandler("RuleProvider", "OnlyFredHasAccess", authorizationConfiguration));
            allowFredPolicy.RuleSet.Add(new AlwaysMatchingRule());
            AllowFredPolicySet = new PolicySet(allowFredPolicy);

            RuleDrivenPolicy allowJackPolicy = new RuleDrivenPolicy("allowJack");
            allowJackPolicy.Handlers.Add(new AuthorizationCallHandler(string.Empty, "OnlyJackHasAccess", authorizationConfiguration));
            allowJackPolicy.RuleSet.Add(new AlwaysMatchingRule());
            AllowJackPolicySet = new PolicySet(allowJackPolicy);

            RuleDrivenPolicy tokenBasedPolicy = new RuleDrivenPolicy("tokens");
            tokenBasedPolicy.RuleSet.Add(new AlwaysMatchingRule());
            tokenBasedPolicy.Handlers.Add(
                new AuthorizationCallHandler(string.Empty, "{type}-{method}",
                                             authorizationConfiguration));
            AllowBasedOnTokensPolicySet = new PolicySet(tokenBasedPolicy);
        }

        [TestMethod, DeploymentItem("Authorization.config"), ExpectedException(typeof(UnauthorizedAccessException))]
        public void AuthorizationCanBeDeniedByCallHandler()
        {
            using (new PrincipalSwitcher(JackPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowFredPolicySet);
                testTarget.GetCurrentPrincipalName();
            }
        }

        [TestMethod, DeploymentItem("Authorization.config")]
        public void CallHandlerDoesNothingWhenUserIsAuthorized()
        {
            using (new PrincipalSwitcher(FredPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowFredPolicySet);
                string currentPrincipalName = testTarget.GetCurrentPrincipalName();
                Assert.AreEqual("Fred", currentPrincipalName);
            }
        }

        [TestMethod, DeploymentItem("Authorization.config"), ExpectedException(typeof(UnauthorizedAccessException))]
        public void CallHandlerWithoutProviderUsesDefaultProvider()
        {
            using (new PrincipalSwitcher(FredPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowJackPolicySet);
                string currentPrincipalName = testTarget.GetCurrentPrincipalName();
            }
        }

        [TestMethod]
        [DeploymentItem("Authorization.config")]
        public void ShouldAllowFredToCallGetCurrentThreadId()
        {
            using (new PrincipalSwitcher(FredPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowBasedOnTokensPolicySet);
                int threadId = testTarget.GetCurrentThreadId();
            }
        }

        [TestMethod]
        [DeploymentItem("Authorization.config")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ShouldNotAllowFredToCallGetCurrentPrincipalName()
        {
            using (new PrincipalSwitcher(FredPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowBasedOnTokensPolicySet);
                string currentPrincipalName = testTarget.GetCurrentPrincipalName();
            }
        }

        [TestMethod]
        [DeploymentItem("Authorization.config")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ShouldNotAllowJackToCallGetCurrentThreadId()
        {
            using (new PrincipalSwitcher(JackPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowBasedOnTokensPolicySet);
                int threadId = testTarget.GetCurrentThreadId();
            }
        }

        [TestMethod]
        public void ShouldAllowJackToCallGetCurrentPrincipalName()
        {
            using (new PrincipalSwitcher(JackPrincipal))
            {
                AuthorizationTestTarget testTarget = GetTarget(AllowBasedOnTokensPolicySet);
                string currentPrincipalName = testTarget.GetCurrentPrincipalName();
            }
        }

        [TestMethod]
        public void CreateCachingCallHandlerFromConfiguration()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policyData = new PolicyData("policy");
            AuthorizationCallHandlerData data = new AuthorizationCallHandlerData("foo", 2);
            policyData.Handlers.Add(data);
            settings.Policies.Add(policyData);
            
            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            ICallHandler handler = CallHandlerCustomFactory.Instance.Create(null, data, dictConfigurationSource, null);
            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        [TestMethod]
        public void ShouldCreateCacheUsingDefaultConfiguration()
        {
            AuthorizationCallHandler handler = new AuthorizationCallHandler("providerName", "operationName", null);

            Assert.AreEqual(0, handler.Order);
        }

        [TestMethod]
        public void CreatesHandlerProperlyFromAttributes()
        {
            MethodInfo method = typeof(AuthorizationTestTarget).GetMethod("GetName");

            Assert.IsNotNull(method);

            object[] attributes = method.GetCustomAttributes(typeof(AuthorizationCallHandlerAttribute), false);

            Assert.AreEqual(1, attributes.Length);

            AuthorizationCallHandlerAttribute att = attributes[0] as AuthorizationCallHandlerAttribute;
            ICallHandler callHandler = att.CreateHandler();

            Assert.IsNotNull(callHandler);
            Assert.AreEqual(3, callHandler.Order);
        }

        AuthorizationTestTarget GetTarget(PolicySet policySet)
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector(policySet);
            return factory.Create<AuthorizationTestTarget>();
        }

        class AuthorizationTestTarget : MarshalByRefObject
        {
            public string GetCurrentPrincipalName()
            {
                return Thread.CurrentPrincipal.Identity.Name;
            }

            public int GetCurrentThreadId()
            {
                return Thread.CurrentThread.ManagedThreadId;
            }

            [AuthorizationCallHandler("OperationName", Order=3)]
            public string GetName()
            {
                return "Name";
            }
        }
    }

    class PrincipalSwitcher : IDisposable
    {
        IPrincipal previousPrincipal;

        public PrincipalSwitcher(IPrincipal newPrincipal)
        {
            previousPrincipal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = newPrincipal;
        }

        public void Dispose()
        {
            if (previousPrincipal != null)
            {
                Thread.CurrentPrincipal = previousPrincipal;
                previousPrincipal = null;
            }
        }
    }
}