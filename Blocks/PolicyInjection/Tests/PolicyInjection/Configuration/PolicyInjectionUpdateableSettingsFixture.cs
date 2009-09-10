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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    public abstract class UpdatedConfigurationSourceContext : ArrangeActAssert
    {
        protected UnityContainerConfigurator containerConfigurator;
        protected UnityContainer container;
        protected ConfigurationSourceUpdatable updatableConfigurationSource;
        protected PolicyInjectionSettings settings;
        protected PolicyData policy1;
        protected MatchingRuleData  matchingRuleData1;
        protected CallHandlerData callHandler1;

        protected override void Arrange()
        {
            updatableConfigurationSource = new ConfigurationSourceUpdatable();

            matchingRuleData1 = new MemberNameMatchingRuleData("Member()");
            matchingRuleData1.Name = "matchingRuleData1";

            callHandler1 = new CustomCallHandlerData("callHandler1", typeof(GlobalCountCallHandler));

            policy1 = new PolicyData();
            policy1.Name = "policy1";

            policy1.MatchingRules.Add(matchingRuleData1);
            policy1.Handlers.Add(callHandler1);

            settings = new PolicyInjectionSettings();
            settings.Policies.Add(policy1);

            updatableConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            EnterpriseLibraryContainer.ConfigureContainer(containerConfigurator, updatableConfigurationSource);
        }

        protected override void Act()
        {
            updatableConfigurationSource.DoSourceChanged(new string[] { PolicyInjectionSettings.SectionName });
        }
    }

    [TestClass]
    public class GivenChangeInPolicyName : UpdatedConfigurationSourceContext 
    {
        protected override void Act()
        {
            policy1.Name = "New Name";
            
            base.Act();
        }

        [TestMethod]
        public void ThenContainerCanResolvePolicyUsingNewName()
        {
            var policy = container.Resolve<InjectionPolicy>("New Name");
            Assert.IsNotNull(policy);
        }
    }

    [TestClass]
    public class GivenChangeInMatchingRuleName : UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            matchingRuleData1.Name = "New Name";

            base.Act();
        }

        [TestMethod]
        public void ThenContainerCanResolvePolicyUsingNewName()
        {
            var rule = container.Resolve<IMatchingRule>("New Name" + "-" + policy1.Name);
            Assert.IsNotNull(rule);
        }
    }

    [TestClass]
    public class GivenChangeInCallHandlerName : UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            callHandler1.Name = "New Name";

            base.Act();
        }

        [TestMethod]
        public void ThenContainerCanResolvePolicyUsingNewName()
        {
            var handler = container.Resolve<ICallHandler>("New Name" + "-" + policy1.Name);
            Assert.IsNotNull(handler);
        }
    }
}
