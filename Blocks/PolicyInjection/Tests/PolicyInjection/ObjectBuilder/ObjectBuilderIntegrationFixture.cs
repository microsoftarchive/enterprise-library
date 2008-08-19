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
using System.Configuration;
using System.Runtime.Remoting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectBuilder
{
    [TestClass]
    public class ObjectBuilderIntegrationFixture
    {
		private StagedStrategyChain<BuilderStage> baseStrategyChain;
        private StagedStrategyChain<BuilderStage> buildPlanStrategyChain;

		[TestInitialize]
		public void SetUp()
		{
			baseStrategyChain = new StagedStrategyChain<BuilderStage>();

			baseStrategyChain.AddNew<PolicyInjectionStrategy>(BuilderStage.PreCreation);
			baseStrategyChain.AddNew<BuildKeyMappingStrategy>(BuilderStage.PreCreation);
			baseStrategyChain.AddNew<BuildPlanStrategy>(BuilderStage.Creation);

            buildPlanStrategyChain = new StagedStrategyChain<BuilderStage>();
            buildPlanStrategyChain.AddNew<DynamicMethodConstructorStrategy>(BuilderStage.Creation);
            buildPlanStrategyChain.AddNew<DynamicMethodPropertySetterStrategy>(BuilderStage.Initialization);
		}

        [TestMethod]
        public void CanCreateInterceptedObjectInOBChain()
        {
			Builder builder = new Builder();

			InterceptableClass instance
				= (InterceptableClass)builder.BuildUp(null,
													   null,
													   CreatePolicyList(GetInjectionSettings(), true),
													   baseStrategyChain.MakeStrategyChain(),
													   NamedTypeBuildKey.Make<InterceptableClass>(string.Empty),
													   null);

            Assert.IsNotNull(instance);
            Assert.IsTrue(RemotingServices.IsTransparentProxy(instance));
        }

        [TestMethod]
        public void RegularInstanceIsReturnedWhenInterceptionIsSwitchedOf()
        {
			Builder builder = new Builder();

			InterceptableClass instance
				= (InterceptableClass)builder.BuildUp(null,
													   null,
													   CreatePolicyList(new PolicyInjectionSettings(), true),
													   baseStrategyChain.MakeStrategyChain(),
													   NamedTypeBuildKey.Make<InterceptableClass>(string.Empty),
													   null);

            Assert.IsNotNull(instance);
            Assert.IsFalse(RemotingServices.IsTransparentProxy(instance));
        }

        [TestMethod]
        public void TypeMappingStrategyAllowsToInterceptNonMBROs()
        {
			Builder builder = new Builder();
			PolicyList policyList = CreatePolicyList(GetInjectionSettings(), true);
			policyList.Set<IBuildKeyMappingPolicy>(
				new BuildKeyMappingPolicy(NamedTypeBuildKey.Make<ClassThatImplementsInterface>(string.Empty)),
				NamedTypeBuildKey.Make<IInterface>(string.Empty));

			IInterface instance
				= (IInterface)builder.BuildUp(null,
											   null,
											   policyList,
											   baseStrategyChain.MakeStrategyChain(),
											   NamedTypeBuildKey.Make<IInterface>(string.Empty),
											   null);

            Assert.IsNotNull(instance);
            Assert.IsTrue(RemotingServices.IsTransparentProxy(instance));
        }

        [TestMethod]
        public void InterceptedInstanceCanBeInjected()
        {
			Builder builder = new Builder();
			PolicyList policyList = CreatePolicyList(GetInjectionSettings(), true);
			// NOTE: this key is a NamedTypeBuildKey just because we are using an enhanced version of the
			// default creation policy that knows about these keys
			policyList.Set<IBuildKeyMappingPolicy>(
				new BuildKeyMappingPolicy(typeof(ClassThatImplementsInterface)),
				typeof(IInterface));

			ClassThatDependsOnIInterface instance
				= (ClassThatDependsOnIInterface)builder.BuildUp(null,
																 null,
																 policyList,
																 baseStrategyChain.MakeStrategyChain(),
																 typeof(ClassThatDependsOnIInterface),
																 null);
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Interface);
            Assert.IsTrue(RemotingServices.IsTransparentProxy(instance.Interface));
        }

		private PolicyList CreatePolicyList(ConfigurationSection settings, bool applyInjectionPolicies)
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(PolicyInjectionSettings.SectionName, settings);

			PolicyList policyList = new PolicyList();
			policyList.SetDefault<IPolicyInjectionPolicy>(new PolicyInjectionPolicy(applyInjectionPolicies));
            //policyList.Set<IPolicyInjectionPolicy>(new PolicyInjectionPolicy(false),
            //                                       typeof(ClassThatDependsOnIInterface));
			policyList.Set<IConfigurationObjectPolicy>(new ConfigurationObjectPolicy(configurationSource),
													   typeof(IConfigurationSource));

            policyList.SetDefault<IBuildPlanCreatorPolicy>(new DynamicMethodBuildPlanCreatorPolicy(buildPlanStrategyChain));

            policyList.SetDefault<IConstructorSelectorPolicy>(new ConstructorSelectorPolicy<Attribute>());
            policyList.SetDefault<IPropertySelectorPolicy>(new PropertySelectorPolicy<Attribute>());

			return policyList;
        }

        static PolicyInjectionSettings GetInjectionSettings()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policy = new PolicyData("Intercept DoSomething");
            policy.MatchingRules.Add(new MemberNameMatchingRuleData("Match DoSomething", "DoSomething"));
            PerformanceCounterCallHandlerData handlerData =
                new PerformanceCounterCallHandlerData("Perfcounters");
            handlerData.CategoryName = "Category";
            handlerData.InstanceName = "Instance";
            policy.Handlers.Add(handlerData);

            settings.Policies.Add(policy);
            return settings;
        }

        private class CreationStrategy : BuilderStrategy
        {
            public override void PreBuildUp(IBuilderContext context)
            {
                if (context.Existing == null)
                {
                    Type typeToBuild = BuildKey.GetType(context.BuildKey);
                    context.Existing = Activator.CreateInstance(typeToBuild);
                }
            }
        }

        class ClassThatDependsOnIInterface
        {
            IInterface @interface;

            public ClassThatDependsOnIInterface(IInterface @interface)
            {
                this.@interface = @interface;
            }

            public IInterface Interface
            {
                get { return @interface; }
            }
        }

        class ClassThatImplementsInterface : IInterface
        {
            void IInterface.DoSomething() {}
        }

        interface IInterface
        {
            void DoSomething();
        }

        class InterceptableClass : MarshalByRefObject
        {
            public InterceptableClass() {}

            public void DoSomething()
            {
                // No need to actually do anything
            }
        }
	}
}