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

using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class GivenAnAssemblyMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData = new AssemblyMatchingRuleData("assembly", typeof(object).Assembly.FullName);
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (AssemblyMatchingRule)container.Resolve<IMatchingRule>("assembly-test");

                Assert.IsTrue(rule.Matches(typeof(object).GetMethod("ToString")));
                Assert.IsFalse(rule.Matches(this.GetType().GetMethods()[0]));
            }
        }
    }

    [TestClass]
    public class GivenACustomAttributeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new CustomAttributeMatchingRuleData(
                    "custom attribute",
                    typeof(InjectionMethodAttribute).AssemblyQualifiedName,
                    true);
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (CustomAttributeMatchingRule)container.Resolve<IMatchingRule>("custom attribute-test");

                Assert.IsTrue(rule.Matches(this.GetType().GetMethod("MemberWithAttribute")));
                Assert.IsFalse(rule.Matches(this.GetType().GetMethod("MemberWithoutAttribute")));
            }
        }

        [InjectionMethod]
        public void MemberWithAttribute()
        {
        }

        public void MemberWithoutAttribute()
        {
        }
    }

    [TestClass]
    public class GivenAMemberNameMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new MemberNameMatchingRuleData(
                    "member",
                    new[] { new MatchData("foo", true), new MatchData("bar", false) });
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (MemberNameMatchingRule)container.Resolve<IMatchingRule>("member-test");

                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAMemberNameMatchingRuleData>(x => x.Foo())));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAMemberNameMatchingRuleData>(x => x.foo())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAMemberNameMatchingRuleData>(x => x.Bar())));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAMemberNameMatchingRuleData>(x => x.bar())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAMemberNameMatchingRuleData>(x => x.other())));
            }
        }

        public void Foo() { }
        public void foo() { }
        public void Bar() { }
        public void bar() { }
        public void other() { }
    }

    [TestClass]
    public class GivenAMethodSignatureMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new MethodSignatureMatchingRuleData("signature", "pattern")
                {
                    IgnoreCase = true,
                    Parameters = { 
                        new ParameterTypeElement("foo", typeof(object).FullName), 
                        new ParameterTypeElement("bar", typeof(int).FullName) }
                };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (MethodSignatureMatchingRule)container.Resolve<IMatchingRule>("signature-test");

                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAMethodSignatureMatchingRuleData>(x => x.pattern(null, 0))));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAMethodSignatureMatchingRuleData>(x => x.pattern(null, 0, "something"))));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAMethodSignatureMatchingRuleData>(x => x.Pattern(this, 100))));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAMethodSignatureMatchingRuleData>(x => x.patternx(null, 0))));
            }
        }

        public void pattern(object foo, int bar) { }
        public void pattern(object foo, int bar, string baz) { }
        public void Pattern(object foo, int bar) { }
        public void patternx(object foo, int bar) { }
    }

    [TestClass]
    public class GivenANamespaceMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new NamespaceMatchingRuleData("namespace")
                {
                    Matches = { 
                        new MatchData("Test1", true), 
                        new MatchData("Test2", false) }
                };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (NamespaceMatchingRule)container.Resolve<IMatchingRule>("namespace-test");

                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<Test1.TestClass>(x => x.TestMethod())));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<test1.TestClass>(x => x.TestMethod())));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<Test2.TestClass>(x => x.TestMethod())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<test2.TestClass>(x => x.TestMethod())));
            }
        }
    }

    [TestClass]
    public class GivenAParameterTypeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new ParameterTypeMatchingRuleData("parameters")
                {
                    Matches = { 
                        new ParameterTypeMatchData(typeof(int).FullName, ParameterKind.Input, true), 
                        new ParameterTypeMatchData(typeof(string).FullName, ParameterKind.ReturnValue,  false) }
                };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (ParameterTypeMatchingRule)container.Resolve<IMatchingRule>("parameters-test");

                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAParameterTypeMatchingRuleData>(x => x.pattern1(null))));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAParameterTypeMatchingRuleData>(x => x.pattern2(null))));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAParameterTypeMatchingRuleData>(x => x.pattern3(10))));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAParameterTypeMatchingRuleData>(x => x.pattern4(null))));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAParameterTypeMatchingRuleData>(x => x.pattern5(10))));
            }
        }

        public int pattern1(object foo) { return 0; }
        public void pattern2(object foo) { }
        public string pattern3(int foo) { return null; }
        public int pattern4(string foo) { return 0; }
        public object pattern5(int foo) { return null; }
    }

    [TestClass]
    public class GivenAPropertyMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new PropertyMatchingRuleData("properties")
                {
                    Matches = { 
                        new PropertyMatchData("foo", PropertyMatchingOption.Get, true), 
                        new PropertyMatchData("bar", PropertyMatchingOption.Set,  false) }
                };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (PropertyMatchingRule)container.Resolve<IMatchingRule>("properties-test");

                Assert.IsTrue(rule.Matches(StaticReflection.GetPropertyGetMethodInfo<GivenAPropertyMatchingRuleData, int>(x => x.foo)));
                Assert.IsTrue(rule.Matches(StaticReflection.GetPropertyGetMethodInfo<GivenAPropertyMatchingRuleData, int>(x => x.Foo)));
                Assert.IsFalse(rule.Matches(StaticReflection.GetPropertySetMethodInfo<GivenAPropertyMatchingRuleData, int>(x => x.foo)));
                Assert.IsTrue(rule.Matches(StaticReflection.GetPropertySetMethodInfo<GivenAPropertyMatchingRuleData, int>(x => x.bar)));
                Assert.IsFalse(rule.Matches(StaticReflection.GetPropertySetMethodInfo<GivenAPropertyMatchingRuleData, int>(x => x.Bar)));
                Assert.IsFalse(rule.Matches(StaticReflection.GetPropertyGetMethodInfo<GivenAPropertyMatchingRuleData, int>(x => x.other)));
            }
        }

        public int foo { get; set; }
        public int Foo { get; set; }
        public int bar { get; set; }
        public int Bar { get; set; }
        public int other { get; set; }
    }

    [TestClass]
    public class GivenAReturnTypeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new ReturnTypeMatchingRuleData("returnType", typeof(object).FullName) { IgnoreCase = true };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (ReturnTypeMatchingRule)container.Resolve<IMatchingRule>("returnType-test");

                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAReturnTypeMatchingRuleData>(x => x.pattern1())));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenAReturnTypeMatchingRuleData>(x => x.pattern2())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAReturnTypeMatchingRuleData>(x => x.pattern3())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenAReturnTypeMatchingRuleData>(x => x.pattern4())));
            }
        }

        public void pattern1() { }
        public object pattern2() { return null; }
        public int pattern3() { return 0; }
        public string pattern4() { return null; }
    }

    [TestClass]
    public class GivenATagAttributeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new TagAttributeMatchingRuleData("tag", "match") { IgnoreCase = true };
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (TagAttributeMatchingRule)container.Resolve<IMatchingRule>("tag-test");

                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<GivenATagAttributeMatchingRuleData>(x => x.Tagged())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenATagAttributeMatchingRuleData>(x => x.TaggedWithOther())));
                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenATagAttributeMatchingRuleData>(x => x.NonTagged())));
            }
        }

        [Tag("match")]
        public void Tagged() { }
        [Tag("other")]
        public void TaggedWithOther() { }
        public void NonTagged() { }
    }

    [TestClass]
    public class GivenATypeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData = new TypeMatchingRuleData("type", typeof(object).FullName);
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (TypeMatchingRule)container.Resolve<IMatchingRule>("type-test");

                Assert.IsFalse(rule.Matches(StaticReflection.GetMethodInfo<GivenATypeMatchingRuleData>(x => x.Setup())));
                Assert.IsTrue(rule.Matches(StaticReflection.GetMethodInfo<object>(x => x.ToString())));
            }
        }
    }

    [TestClass]
    public class GivenACustomMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData =
                new CustomMatchingRuleData("custom", typeof(AlwaysMatchingRule).AssemblyQualifiedName)
                {
                    Attributes = { { "foo", "bar" }, { "bar", "baz" } }
                };
        }


        [TestMethod]
        public void WhenConfiguredContainer_ThenResolvedInstanceIsExpectedRule()
        {
            using (var container = new UnityContainer())
            {
                this.matchingRuleData.ConfigureContainer(container, "-test");

                var rule = (AlwaysMatchingRule)container.Resolve<IMatchingRule>("custom-test");

                Assert.AreEqual(2, rule.configuration.Count);
                Assert.AreEqual("bar", rule.configuration["foo"]);
                Assert.AreEqual("baz", rule.configuration["bar"]);
            }
        }
    }
}


namespace Test1
{
    public class TestClass
    {
        public void TestMethod() { }
    }
}

namespace test1
{
    public class TestClass
    {
        public void TestMethod() { }
    }
}

namespace Test2
{
    public class TestClass
    {
        public void TestMethod() { }
    }
}

namespace test2
{
    public class TestClass
    {
        public void TestMethod() { }
    }
}