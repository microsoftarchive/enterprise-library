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
            matchingRuleData = new AssemblyMatchingRuleData
            {
                Name = "assembly",
                Match = typeof(object).Assembly.FullName
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredAttributesAreMissing()
        {
            new AssemblyMatchingRuleData().GetRegistrations("").ToArray();
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
                 new CustomAttributeMatchingRuleData
                {
                    Name = "custom attribute",
                    AttributeTypeName = typeof(CLSCompliantAttribute).AssemblyQualifiedName,
                    SearchInheritanceChain = true
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredAttributesAreMissing()
        {
            new CustomAttributeMatchingRuleData().GetRegistrations("").ToArray();
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
                new MemberNameMatchingRuleData
                {
                    Name = "member",
                    Matches =
                        {
                            new MatchData { Match = "foo", IgnoreCase = true },
                            new MatchData { Match = "bar", IgnoreCase = false }
                        }
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

#if SILVERLIGHT
        [Ignore]
#endif
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredAttributesAreMissing()
        {
            new MemberNameMatchingRuleData
            {
                Name = "member",
                Matches = { new MatchData() }
            }
            .GetRegistrations("").ToArray();
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
                new MethodSignatureMatchingRuleData
                    {
                        Name = "signature",
                        Match = "pattern",
                        IgnoreCase = true,
                        Parameters =
                            {
                                new ParameterTypeElement { Name = "foo", ParameterTypeName = typeof(object).FullName },
                                new ParameterTypeElement { Name = "bar", ParameterTypeName = typeof(int).FullName },
                            }
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredAttributesAreMissing()
        {
            new MethodSignatureMatchingRuleData().GetRegistrations("").ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new MethodSignatureMatchingRuleData
            {
                Name = "signature",
                Match = "pattern",
                IgnoreCase = true,
                Parameters =
                            {
                                new ParameterTypeElement { Name = "foo" },
                            }
            }
            .GetRegistrations("").ToArray();
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
                new NamespaceMatchingRuleData()
                {
                    Name = "namespace",
                    Matches =
                    { 
                        new MatchData { Match = "foo", IgnoreCase = true }, 
                        new MatchData { Match = "bar", IgnoreCase = false }, 
                    }
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

#if SILVERLIGHT
        [Ignore]
#endif
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new NamespaceMatchingRuleData
                {
                    Name = "namespace",
                    Matches = 
                    { 
                        new MatchData() 
                    }
                }.GetRegistrations("").ToArray();
        }
    }

    [TestClass]
    public class GivenAParameterTypeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData = new ParameterTypeMatchingRuleData
                {
                    Name = "parameters",
                    Matches =
                        {
                            new ParameterTypeMatchData { Match = "foo", ParameterKind = ParameterKind.Input, IgnoreCase = true },
                            new ParameterTypeMatchData { Match = "bar", ParameterKind = ParameterKind.ReturnValue, IgnoreCase = false },
                        }
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

#if SILVERLIGHT
        [Ignore]
#endif
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new ParameterTypeMatchingRuleData
                {
                    Name = "parameters",
                    Matches =
                        {
                            new ParameterTypeMatchData { }
                        }
                }.GetRegistrations("").ToArray();
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
                new PropertyMatchingRuleData
                {
                    Name = "properties",
                    Matches = 
                    { 
                        new PropertyMatchData { Match = "foo", MatchOption = PropertyMatchingOption.Get, IgnoreCase = true }, 
                        new PropertyMatchData { Match = "bar", MatchOption = PropertyMatchingOption.Set, IgnoreCase = false },
                    }
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

#if SILVERLIGHT
        [Ignore]
#endif
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new PropertyMatchingRuleData
            {
                Name = "properties",
                Matches = { new PropertyMatchData() }
            }
            .GetRegistrations("").ToArray();
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
                new ReturnTypeMatchingRuleData
                {
                    Name = "returnType",
                    Match = typeof(object).FullName,
                    IgnoreCase = true
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new ReturnTypeMatchingRuleData().GetRegistrations("").ToArray();
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
                new TagAttributeMatchingRuleData
                {
                    Name = "tag",
                    Match = "match",
                    IgnoreCase = true
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new TagAttributeMatchingRuleData().GetRegistrations("").ToArray();
        }
    }

    [TestClass]
    public class GivenATypeMatchingRuleData
    {
        private MatchingRuleData matchingRuleData;

        [TestInitialize]
        public void Setup()
        {
            matchingRuleData = new TypeMatchingRuleData
            {
                Name = "type",
                Matches = { new MatchData { Match = typeof(object).FullName } }
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

#if SILVERLIGHT
        [Ignore]
#endif
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenCreatesRegistrations_ThenThrowInvalidOperationExceptionIfRequiredChildAttributesAreMissing()
        {
            new TypeMatchingRuleData { Matches = { new MatchData() } }.GetRegistrations("").ToArray();
        }
    }

#if !SILVERLIGHT
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
#endif
}
