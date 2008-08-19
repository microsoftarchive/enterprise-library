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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    /// <summary>
    /// Tests for the Policy class
    /// </summary>
    [TestClass]
    public class PolicyFixture
    {
        [TestMethod]
        public void ShouldInitializeToEmpty()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("Empty");
            Assert.AreEqual("Empty", p.Name);
            Assert.AreEqual(0, p.RuleSet.Count);
            Assert.AreEqual(0, p.Handlers.Count);
        }

        [TestMethod]
        public void ShouldPreserveHandlerOrder()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("OrderedHandlers");

            ICallHandler h1 = new Handler1();
            ICallHandler h2 = new Handler2();
            ICallHandler h3 = new Handler3();

            p.Handlers.Add(h2);
            p.Handlers.Add(h1);
            p.Handlers.Add(h3);

            Assert.AreEqual(3, p.Handlers.Count);
            Assert.AreSame(h2, p.Handlers[0]);
            Assert.AreSame(h1, p.Handlers[1]);
            Assert.AreSame(h3, p.Handlers[2]);
        }

        [TestMethod]
        public void ShouldHaveNoHandlersWhenPolicyDoesntMatch()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("NoRules");
            ICallHandler[] handlers = { new Handler1(), new Handler2(), new Handler3() };
            Array.ForEach(handlers, delegate(ICallHandler handler)
                                    {
                                        p.Handlers.Add(handler);
                                    });

            MethodBase thisMember =
                GetType().GetMethod("ShouldHaveNoHandlersWhenPolicyDoesntMatch");
            List<ICallHandler> memberHandlers = new List<ICallHandler>(p.GetHandlersFor(thisMember));
            Assert.AreEqual(0, memberHandlers.Count);
        }

        [TestMethod]
        public void ShouldGetHandlersInOrderWithGetHandlersFor()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("OrderedHandlers");
            p.RuleSet.Add(new MemberNameMatchingRule("ShouldGetHandlersInOrderWithGetHandlersFor"));

            ICallHandler[] handlers = { new Handler1(), new Handler2(), new Handler3() };
            Array.ForEach(handlers, delegate(ICallHandler handler)
                                    {
                                        p.Handlers.Add(handler);
                                    });

            int i = 0;
            MethodBase member =
                GetType().GetMethod("ShouldGetHandlersInOrderWithGetHandlersFor");
            foreach (ICallHandler h in p.GetHandlersFor(member))
            {
                Assert.AreSame(handlers[i], h);
                ++i;
            }
            Assert.AreEqual(3, i); // Make sure we actually got handlers
        }

        [TestMethod]
        public void ShouldBeAbleToMatchPropertyGet()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy("Property get");
            p.RuleSet.Add(new MemberNameMatchingRule("get_Balance"));
            ICallHandler callHandler = new CallCountHandler();
            p.Handlers.Add(callHandler);

            PropertyInfo balanceProperty = typeof(MockDal).GetProperty("Balance");
            MethodBase getMethod = balanceProperty.GetGetMethod();
            List<ICallHandler> handlers = new List<ICallHandler>(p.GetHandlersFor(getMethod));
            Assert.AreEqual(1, handlers.Count);
            Assert.AreSame(callHandler, handlers[0]);
        }

        [TestMethod]
        public void ShouldOnlyGetHandlersOnceIfPolicyMatchesBothClassAndInterface()
        {
            RuleDrivenPolicy p = new RuleDrivenPolicy();
            ICallHandler callHandler = new CallCountHandler();
            MemberNameMatchingRule nameRule = new MemberNameMatchingRule("MyMethod");
            p.RuleSet.Add(nameRule);
            p.Handlers.Add(callHandler);

            MethodInfo myMethod = typeof(MyFooClass).GetMethod("MyMethod");
            List<ICallHandler> handlers = new List<ICallHandler>(p.GetHandlersFor(myMethod));

            Assert.AreEqual(1, handlers.Count);
            Assert.AreSame(callHandler, handlers[0]);
        }
    }

    class Handler1 : ICallHandler
    {
        int order = 0;

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
            throw new NotImplementedException();
        }
    }

    class Handler2 : ICallHandler
    {
        int order = 0;

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
            throw new NotImplementedException();
        }
    }

    class Handler3 : ICallHandler
    {
        int order = 0;

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
            throw new NotImplementedException();
        }
    }

    public interface IFoo
    {
        void MyMethod();
    }

    public class MyFooClass : IFoo
    {
        public void MyMethod() {}
    }
}