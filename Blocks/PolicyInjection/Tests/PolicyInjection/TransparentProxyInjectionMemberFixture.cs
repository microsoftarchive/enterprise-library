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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class GivenInstanceInterceptionPolicySettingInjectionMemberRegisteredForType
    {
        private TransparentProxyInterceptor interceptor;
        private PolicyExposingInjectionMember assertingInjectionMember;

        [TestInitialize]
        public void Given()
        {
            var container = new UnityContainer();

            interceptor = new TransparentProxyInterceptor();
            var injectionMember = new InstanceInterceptionPolicySettingInjectionMember(interceptor);
            assertingInjectionMember = new PolicyExposingInjectionMember();
            container.RegisterType<Foo>(injectionMember, assertingInjectionMember);
        }

        [TestMethod]
        public void ThenInjectorPolicyAdded()
        {
            var policy = assertingInjectionMember.Policies
                .Get<IInstanceInterceptionPolicy>(typeof(Foo));
            Assert.IsNotNull(policy);
        }

        [TestMethod]
        public void ThenProvidedInterceptorMatchesProvidedInterceptor()
        {
            var policy = assertingInjectionMember.Policies
                .Get<IInstanceInterceptionPolicy>(typeof(Foo));
            Assert.AreSame(interceptor, policy.Interceptor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenInterceptorIsNull_ThenInterceptionMemberThrows()
        {
            new InstanceInterceptionPolicySettingInjectionMember(null);
        }

        private class PolicyExposingInjectionMember : InjectionMember
        {
            public IPolicyList Policies;
            public override void AddPolicies(Type typeToCreate, string name, IPolicyList policies)
            {
                this.Policies = policies;
            }
        }

        class Foo
        {

        }
    }

   
}
