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

using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.FakeObjects;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    /// <summary>
    /// Tests for the HandlerPipeline class
    /// </summary>
    [TestClass]
    public class PipelineFixture
    {
        CallCountHandler callCountHandler;
        StringReturnRewriteHandler returnHandler;

        [TestMethod]
        public void ShouldBeCreatable()
        {
            HandlerPipeline pipeline = new HandlerPipeline();
        }

        [TestMethod]
        public void ShouldBeCreateableWithHandlers()
        {
            PolicySet policies = GetPolicies();
            HandlerPipeline pipeline =
                new HandlerPipeline(policies.GetHandlersFor(GetTargetMemberInfo()));
        }

        [TestMethod]
        public void ShouldBeInvokable()
        {
            PolicySet policies = GetPolicies();
            HandlerPipeline pipeline =
                new HandlerPipeline(policies.GetHandlersFor(GetTargetMemberInfo()));

            IMethodReturn result = pipeline.Invoke(
                MakeCallMessage(),
                delegate(IMethodInvocation message,
                         GetNextHandlerDelegate getNext)
                {
                    return MakeReturnMessage(message);
                });
            Assert.IsNotNull(result);
            Assert.AreEqual(1, callCountHandler.CallCount);
            Assert.AreEqual(returnHandler.ValueToRewriteTo, (string)result.ReturnValue);
        }

        public string MyTargetMethod(int i)
        {
            return string.Format("i = {0}", i);
        }

        PolicySet GetPolicies()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("PipelineTestPolicy");
            p.RuleSet.Add(new AlwaysMatchingRule());
            callCountHandler = new CallCountHandler();
            returnHandler = new StringReturnRewriteHandler("REWRITE");
            p.Handlers.Add(callCountHandler);
            p.Handlers.Add(returnHandler);
            return new PolicySet(p);
        }

        MethodInfo GetTargetMemberInfo()
        {
            return (MethodInfo)(GetType().GetMember("MyTargetMethod")[0]);
        }

        IMethodInvocation MakeCallMessage()
        {
            FakeMethodCallMessage msg = new FakeMethodCallMessage(GetTargetMemberInfo(), 15);
            IMethodInvocation invocation = new RemotingMethodInvocation(msg, null);
            return invocation;
        }

        IMethodReturn MakeReturnMessage(IMethodInvocation input)
        {
            IMethodReturn result = input.CreateMethodReturn(MyTargetMethod((int)input.Inputs[0]));
            return result;
        }
    }

    class StringReturnRewriteHandler : ICallHandler
    {
        int order = 0;
        string valueToRewriteTo;

        public StringReturnRewriteHandler(string valueToRewriteTo)
        {
            this.valueToRewriteTo = valueToRewriteTo;
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public string ValueToRewriteTo
        {
            get { return valueToRewriteTo; }
        }

        public IMethodReturn Invoke(IMethodInvocation input,
                                    GetNextHandlerDelegate getNext)
        {
            IMethodReturn retval = getNext()(input, getNext);
            retval.ReturnValue = valueToRewriteTo;
            return retval;
        }
    }
}