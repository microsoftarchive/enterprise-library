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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class MethodSignatureFixture
    {
        [TestMethod]
        public void ShouldPassWithNoParameters()
        {
            SignatureTestTarget target = GetTarget();
            target.MethodWithNoParameters();
        }

        [TestMethod]
        public void ShouldPassWithSimpleInputs()
        {
            SignatureTestTarget target = GetTarget();
            target.MethodWithSimpleInputs(1, "two");
        }

        [TestMethod]
        public void ShouldPassWithOutParams()
        {
            SignatureTestTarget target = GetTarget();
            int first;
            string second;
            target.MethodWithOutParams(out first, out second);
            Assert.AreEqual(1, first);
            Assert.AreEqual("two", second);
        }

        [TestMethod]
        public void ShouldPassWithInOutRef()
        {
            SignatureTestTarget target = GetTarget();
            string two;
            float three = 1.0f;

            target.MethodWithInOutByrefParams(1, out two, ref three, 5.0M);
            Assert.AreEqual("owt", two);
            Assert.AreEqual(3.0f, three);
        }

        [TestMethod]
        public void ShouldPassWithVarArgs()
        {
            SignatureTestTarget target = GetTarget();
            target.MethodWithVarArgs(1, "two", "two and a half", "two and three quarters");
        }

        SignatureTestTarget GetTarget()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPolicies());
            return factory.Create<SignatureTestTarget>();
        }

        PolicySet GetPolicies()
        {
            PolicySet policies = new PolicySet();

            RuleDrivenPolicy noParamsPolicy = new RuleDrivenPolicy("noParamsPolicy");
            noParamsPolicy.RuleSet.Add(new MatchByNameRule("MethodWithNoParameters"));
            noParamsPolicy.Handlers.Add(new SignatureCheckingHandler(new Type[] { }));
            policies.Add(noParamsPolicy);

            RuleDrivenPolicy simpleInputsPolicy = new RuleDrivenPolicy("simpleInputsPolicy");
            simpleInputsPolicy.RuleSet.Add(new MatchByNameRule("MethodWithSimpleInputs"));
            simpleInputsPolicy.Handlers.Add(new SignatureCheckingHandler(new Type[] { typeof(int), typeof(string) }));
            policies.Add(simpleInputsPolicy);

            RuleDrivenPolicy outParamsPolicy = new RuleDrivenPolicy("outParamsPolicy");
            outParamsPolicy.RuleSet.Add(new MatchByNameRule("MethodWithOutParams"));
            outParamsPolicy.Handlers.Add(new SignatureCheckingHandler(new Type[] { typeof(int).MakeByRefType(), typeof(string).MakeByRefType() }));
            policies.Add(outParamsPolicy);

            RuleDrivenPolicy mixedParamsPolicy = new RuleDrivenPolicy("mixedParamsPolicy");
            mixedParamsPolicy.RuleSet.Add(new MatchByNameRule("MethodWithInOutByrefParams"));
            mixedParamsPolicy.Handlers.Add(new SignatureCheckingHandler(
                                               new Type[]
                                                   {
                                                       typeof(int),
                                                       typeof(string).MakeByRefType(),
                                                       typeof(float).MakeByRefType(),
                                                       typeof(decimal)
                                                   }));
            policies.Add(mixedParamsPolicy);

            RuleDrivenPolicy varargsParamsPolicy = new RuleDrivenPolicy("varargsParamsPolicy");
            varargsParamsPolicy.RuleSet.Add(new MatchByNameRule("MethodWithVarArgs"));
            varargsParamsPolicy.Handlers.Add(new SignatureCheckingHandler(
                                                 new Type[] { typeof(int), typeof(string).MakeArrayType() }));
            policies.Add(varargsParamsPolicy);

            return policies;
        }
    }

    class SignatureTestTarget : MarshalByRefObject
    {
        public void MethodWithInOutByrefParams(int one,
                                               out string two,
                                               ref float three,
                                               decimal four)
        {
            two = "owt";
            three = 3.0f;
        }

        public void MethodWithNoParameters() {}

        public void MethodWithOutParams(out int first,
                                        out string second)
        {
            first = 1;
            second = "two";
        }

        public void MethodWithSimpleInputs(int one,
                                           string two) {}

        public void MethodWithVarArgs(int one,
                                      params string[] two) {}
    }

    class MatchAllRule : IMatchingRule
    {
        public bool Matches(MethodBase member)
        {
            return true;
        }
    }

    class MatchByNameRule : IMatchingRule
    {
        string name;

        public MatchByNameRule(string name)
        {
            this.name = name;
        }

        public bool Matches(MethodBase member)
        {
            return member.Name == name;
        }
    }

    class SignatureCheckingHandler : ICallHandler
    {
        Type[] expectedSignature;
        int order = 0;

        public SignatureCheckingHandler(Type[] expectedSignature)
        {
            this.expectedSignature = expectedSignature;
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public IMethodReturn Invoke(IMethodInvocation input,
                                    GetNextHandlerDelegate getNext)
        {
            Assert.IsNotNull(expectedSignature);
            Type[] messageSignature =
                Array.ConvertAll<ParameterInfo, Type>(input.MethodBase.GetParameters(),
                                                      delegate(ParameterInfo info)
                                                      {
                                                          return info.ParameterType;
                                                      });
            Assert.AreEqual(expectedSignature.Length, messageSignature.Length);
            for (int i = 0; i < messageSignature.Length; ++i)
            {
                Assert.AreSame(expectedSignature[i], messageSignature[i],
                               "Signature for method {0} failed to match for parameter {1}",
                               input.MethodBase.Name, i);
            }

            return getNext()(input, getNext);
        }
    }
}