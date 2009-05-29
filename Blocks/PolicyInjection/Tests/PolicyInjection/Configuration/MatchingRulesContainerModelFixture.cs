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
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("assembly-suffix")
                .ForImplementationType(typeof(AssemblyMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(typeof(object).Assembly.FullName)
                .VerifyConstructorParameters();
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
                    typeof(CLSCompliantAttribute).AssemblyQualifiedName,
                    true);
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("custom attribute-suffix")
                .ForImplementationType(typeof(CustomAttributeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(typeof(CLSCompliantAttribute))
                .WithValueConstructorParameter(true)
                .VerifyConstructorParameters();
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("member-suffix")
                .ForImplementationType(typeof(MemberNameMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<MatchingInfo> matches;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out matches)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, matches.Count());
            Assert.AreEqual("foo", matches.ElementAt(0).Match);
            Assert.AreEqual(true, matches.ElementAt(0).IgnoreCase);
            Assert.AreEqual("bar", matches.ElementAt(1).Match);
            Assert.AreEqual(false, matches.ElementAt(1).IgnoreCase);
        }
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("signature-suffix")
                .ForImplementationType(typeof(MethodSignatureMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<string> matches;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("pattern")
                .WithValueConstructorParameter(out matches)
                .WithValueConstructorParameter(true)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, matches.Count());
            Assert.AreEqual(typeof(object).FullName, matches.ElementAt(0));
            Assert.AreEqual(typeof(int).FullName, matches.ElementAt(1));
        }
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
                        new MatchData("foo", true), 
                        new MatchData("bar", false) }
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("namespace-suffix")
                .ForImplementationType(typeof(NamespaceMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<MatchingInfo> matches;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out matches)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, matches.Count());
            Assert.AreEqual("foo", matches.ElementAt(0).Match);
            Assert.AreEqual(true, matches.ElementAt(0).IgnoreCase);
            Assert.AreEqual("bar", matches.ElementAt(1).Match);
            Assert.AreEqual(false, matches.ElementAt(1).IgnoreCase);
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
                        new ParameterTypeMatchData("foo", ParameterKind.Input, true), 
                        new ParameterTypeMatchData("bar", ParameterKind.ReturnValue,  false) }
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("parameters-suffix")
                .ForImplementationType(typeof(ParameterTypeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<ParameterTypeMatchingInfo> matches;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out matches)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, matches.Count());
            Assert.AreEqual("foo", matches.ElementAt(0).Match);
            Assert.AreEqual(ParameterKind.Input, matches.ElementAt(0).Kind);
            Assert.AreEqual(true, matches.ElementAt(0).IgnoreCase);
            Assert.AreEqual("bar", matches.ElementAt(1).Match);
            Assert.AreEqual(ParameterKind.ReturnValue, matches.ElementAt(1).Kind);
            Assert.AreEqual(false, matches.ElementAt(1).IgnoreCase);
        }
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("properties-suffix")
                .ForImplementationType(typeof(PropertyMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<PropertyMatchingInfo> matches;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out matches)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, matches.Count());
            Assert.AreEqual("foo", matches.ElementAt(0).Match);
            Assert.AreEqual(PropertyMatchingOption.Get, matches.ElementAt(0).Option);
            Assert.AreEqual(true, matches.ElementAt(0).IgnoreCase);
            Assert.AreEqual("bar", matches.ElementAt(1).Match);
            Assert.AreEqual(PropertyMatchingOption.Set, matches.ElementAt(1).Option);
            Assert.AreEqual(false, matches.ElementAt(1).IgnoreCase);
        }
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("returnType-suffix")
                .ForImplementationType(typeof(ReturnTypeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(typeof(object).FullName)
                .WithValueConstructorParameter(true)
                .VerifyConstructorParameters();
        }
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("tag-suffix")
                .ForImplementationType(typeof(TagAttributeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("match")
                .WithValueConstructorParameter(true)
                .VerifyConstructorParameters();
        }
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("type-suffix")
                .ForImplementationType(typeof(TypeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<MatchingInfo> matches;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out matches)
                .VerifyConstructorParameters();

            Assert.AreEqual(1, matches.Count());
            Assert.AreEqual(typeof(object).FullName, matches.ElementAt(0).Match);
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
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForIMatchingRuleWithNameAndImplementationType()
        {
            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("custom-suffix")
                .ForImplementationType(typeof(AlwaysMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            NameValueCollection attributes;

            var registrations = matchingRuleData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out attributes)
                .VerifyConstructorParameters();

            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("bar", attributes["foo"]);
            Assert.AreEqual("baz", attributes["bar"]);
        }
    }
}
