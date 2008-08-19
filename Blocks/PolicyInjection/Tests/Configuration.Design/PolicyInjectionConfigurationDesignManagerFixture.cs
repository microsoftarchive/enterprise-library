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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class PolicyInjectionConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod, DeploymentItem("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests.dll.config")]
        public void OpenAndSaveConfiguration()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PolicyInjectionSettingsNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(PolicyNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(AssemblyMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CustomAttributeMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MemberNameMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MethodSignatureMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(NamespaceMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PropertyMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TagAttributeMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TypeMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorCollectionNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorNode)).Count);
            Assert.AreSame(
                ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorNode))[0],
                ((InjectorCollectionNode)ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorCollectionNode))[0]).DefaultInjector);

            ApplicationNode.Hierarchy.Save();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PolicyInjectionSettingsNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(PolicyNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(AssemblyMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CustomAttributeMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MemberNameMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MethodSignatureMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(NamespaceMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PropertyMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TagAttributeMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TypeMatchingRuleNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorCollectionNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorNode)).Count);
            Assert.AreSame(
                ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorNode))[0],
                ((InjectorCollectionNode)ApplicationNode.Hierarchy.FindNodesByType(typeof(InjectorCollectionNode))[0]).DefaultInjector);
        }

        [TestMethod, DeploymentItem("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests.dll.config")]
        public void BuildContextTest()
        {
            PolicyInjectionConfigurationDesignManager designManager = new PolicyInjectionConfigurationDesignManager();
            designManager.Register(ServiceProvider);
            designManager.Open(ServiceProvider);

            DictionaryConfigurationSource dictionarySource = new DictionaryConfigurationSource();
            designManager.BuildConfigurationSource(ServiceProvider, dictionarySource);
            Assert.IsTrue(dictionarySource.Contains(PolicyInjectionSettings.SectionName));
        }

        [TestMethod]
        [DeploymentItem("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests.dll.config")]
        public void ShouldBeAbleToMovePolicies()
        {
            ApplicationNode.Hierarchy.Load();
            ApplicationNode.Hierarchy.Open();
            ConfigurationNode policyInjectionNode =
                ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyInjectionSettingsNode));
            Assert.IsNotNull(policyInjectionNode);
            Assert.IsTrue(policyInjectionNode is PolicyInjectionSettingsNode);
            Assert.AreEqual(2, policyInjectionNode.Nodes.Count);

            ConfigurationNode policyCollectionNode =
                policyInjectionNode.Hierarchy.FindNodeByType(typeof(PolicyCollectionNode));
            Assert.IsNotNull(policyCollectionNode);
            PolicyNode policy1Node = (PolicyNode)policyCollectionNode.Nodes[0];
            PolicyNode policy2Node = (PolicyNode)policyCollectionNode.Nodes[1];

            Assert.AreEqual("Policy One", policy1Node.Name);
            Assert.AreEqual("Policy Two", policy2Node.Name);

            policyCollectionNode.MoveAfter(policy1Node, policy2Node);

            Assert.AreSame(policy2Node, policyCollectionNode.Nodes[0]);
            Assert.AreSame(policy1Node, policyCollectionNode.Nodes[1]);

            IConfigurationSource configSource = ApplicationNode.Hierarchy.BuildConfigurationSource();

            Assert.IsNotNull(configSource);
            PolicyInjectionSettings settings =
                (PolicyInjectionSettings)
                configSource.GetSection(PolicyInjectionSettings.SectionName);
            Assert.AreEqual(2, settings.Policies.Count);
            Assert.AreEqual("Policy Two", settings.Policies.Get(0).Name);
            Assert.AreEqual("Policy One", settings.Policies.Get(1).Name);
        }

        [TestMethod]
        [DeploymentItem("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests.dll.config")]
        public void ShouldBeAbleToMoveHandlers()
        {
            ApplicationNode.Hierarchy.Load();
            ApplicationNode.Hierarchy.Open();
            ConfigurationNode policyInjectionNode =
                ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyInjectionSettingsNode));
            ConfigurationNode policyCollectionNode =
                policyInjectionNode.Hierarchy.FindNodeByType(typeof(PolicyCollectionNode));

            PolicyNode policy1Node = (PolicyNode)policyCollectionNode.Nodes[0];

            CallHandlersCollectionNode handlersNode =
                (CallHandlersCollectionNode)
                policy1Node.Hierarchy.FindNodeByType(typeof(CallHandlersCollectionNode));
            ExceptionCallHandlerNode exceptionNode = (ExceptionCallHandlerNode)handlersNode.Nodes[3];
            AuthorizationCallHandlerNode authNode =
                (AuthorizationCallHandlerNode)handlersNode.Nodes[0];

            ConfigurationNode logNode =
                handlersNode.Hierarchy.FindNodeByType(typeof(LogCallHandlerNode));
            ConfigurationNode validationNode =
                handlersNode.Hierarchy.FindNodeByType(typeof(ValidationCallHandlerNode));

            handlersNode.MoveBefore(exceptionNode, authNode);
            handlersNode.MoveAfter(logNode, validationNode);

            IConfigurationSource configSource = ApplicationNode.Hierarchy.BuildConfigurationSource();

            PolicyInjectionSettings settings =
                (PolicyInjectionSettings)
                configSource.GetSection(PolicyInjectionSettings.SectionName);

            PolicyData policy1Data = settings.Policies.Get(0);

            Assert.IsTrue(policy1Data.Handlers.Get(0) is ExceptionCallHandlerData);
            Assert.IsTrue(policy1Data.Handlers.Get(1) is AuthorizationCallHandlerData);
            Assert.IsTrue(policy1Data.Handlers.Get(policy1Data.Handlers.Count - 2) is ValidationCallHandlerData);
            Assert.IsTrue(policy1Data.Handlers.Get(policy1Data.Handlers.Count - 1) is LogCallHandlerData);
        }

        [TestMethod]
        public void LoadsAndSavesRequirePermissionFlag()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            PolicyInjectionSettingsNode originalNode
                = (PolicyInjectionSettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyInjectionSettingsNode));

            bool requirePermission = originalNode.RequirePermission;
            originalNode.RequirePermission = !requirePermission;

            ApplicationNode.Hierarchy.Save();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            PolicyInjectionSettingsNode newNode
                = (PolicyInjectionSettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyInjectionSettingsNode));

            Assert.AreNotSame(originalNode, newNode);
            Assert.AreEqual(originalNode.RequirePermission, newNode.RequirePermission);
        }
    }
}