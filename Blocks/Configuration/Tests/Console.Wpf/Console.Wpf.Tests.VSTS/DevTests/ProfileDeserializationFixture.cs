//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.IO;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    public class given_empty_profile_xml : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document = @"<Profile/>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_empty_profile_xml
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_profile_is_empty()
            {
                Assert.IsNull(this.profile.Platform);
                Assert.AreEqual(0, this.profile.MatchFilters.Length);
            }
        }
    }

    public class given_profile_xml_with_empty_type_filters : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document = @"<Profile><TypeFilters/></Profile>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_profile_xml_with_empty_type_filters
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_type_filters_is_empty()
            {
                Assert.AreEqual(0, this.profile.MatchFilters.Length);
            }
        }
    }

    public class given_profile_xml_with_single_type_filter : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document = @"<Profile><TypeFilters><Type name=""test"" matchSubclasses=""true"" matchKind=""Allow""/></TypeFilters></Profile>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_profile_xml_with_single_type_filter
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_type_filters_has_single_element()
            {
                Assert.AreEqual(1, this.profile.MatchFilters.Length);
            }

            [TestMethod]
            public void then_single_filter_is_type_matching_filter()
            {
                var filter = (TypeMatchFilter)this.profile.MatchFilters[0];

                Assert.AreEqual("test", filter.Name);
                Assert.IsTrue(filter.MatchSubclasses);
                Assert.AreEqual(MatchKind.Allow, filter.MatchKind);
            }
        }
    }

    public class given_profile_xml_with_single_assembly_filter : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document = @"<Profile><TypeFilters><Assembly name=""test"" matchKind=""Deny""/></TypeFilters></Profile>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_profile_xml_with_single_assembly_filter
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_type_filters_has_single_element()
            {
                Assert.AreEqual(1, this.profile.MatchFilters.Length);
            }

            [TestMethod]
            public void then_single_filter_is_type_matching_filter()
            {
                var filter = (AssemblyMatchFilter)this.profile.MatchFilters[0];

                Assert.AreEqual("test", filter.Name);
                Assert.AreEqual(MatchKind.Deny, filter.MatchKind);
            }
        }
    }

    public class given_profile_xml_with_single_add_block_command_filter : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document = @"<Profile><TypeFilters><AddBlockCommand name=""test"" matchKind=""Deny"" sectionName=""sectionName""/></TypeFilters></Profile>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_profile_xml_with_single_add_block_command_filter
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_type_filters_has_single_element()
            {
                Assert.AreEqual(1, this.profile.MatchFilters.Length);
            }

            [TestMethod]
            public void then_single_filter_is_type_matching_filter()
            {
                var filter = (AddBlockCommandMatchFilter)this.profile.MatchFilters[0];

                Assert.AreEqual("test", filter.Name);
                Assert.AreEqual(MatchKind.Deny, filter.MatchKind);
                Assert.AreEqual("sectionName", filter.SectionName);
            }
        }
    }

    public class given_profile_xml_with_single_command_filter : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document = @"<Profile><TypeFilters><Command name=""test"" matchKind=""Deny""/></TypeFilters></Profile>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_profile_xml_with_single_command_filter
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_type_filters_has_single_element()
            {
                Assert.AreEqual(1, this.profile.MatchFilters.Length);
            }

            [TestMethod]
            public void then_single_filter_is_type_matching_filter()
            {
                var filter = (CommandMatchFilter)this.profile.MatchFilters[0];

                Assert.AreEqual("test", filter.Name);
                Assert.AreEqual(MatchKind.Deny, filter.MatchKind);
            }
        }
    }

    public class given_profile_xml_with_six_filters : ArrangeActAssert
    {
        protected string document;

        protected override void Arrange()
        {
            base.Arrange();

            this.document =
@"<Profile>
    <TypeFilters>
        <Assembly name=""test1"" matchKind=""Deny""/>
        <Type name=""test2"" matchKind=""Deny""/>
        <Type name=""test3"" matchKind=""Deny""/>
        <Assembly name=""test4"" matchKind=""Deny""/>
        <Command name=""test5"" matchKind=""Deny""/>
        <AddBlockCommand name=""test6"" matchKind=""Allow"" sectionName=""sectionName6""/>
    </TypeFilters>
</Profile>";
        }

        [TestClass]
        public class when_deserializing_into_a_profile : given_profile_xml_with_six_filters
        {
            private Profile profile;

            protected override void Act()
            {
                var serializer = new XmlSerializer(typeof(Profile));

                this.profile = (Profile)serializer.Deserialize(new StringReader(this.document));
            }

            [TestMethod]
            public void then_type_filters_has_four_elements()
            {
                Assert.AreEqual(6, this.profile.MatchFilters.Length);
            }

            [TestMethod]
            public void then_filters_have_the_expected_type()
            {
                Assert.AreEqual("test1", ((AssemblyMatchFilter)this.profile.MatchFilters[0]).Name);
                Assert.AreEqual("test2", ((TypeMatchFilter)this.profile.MatchFilters[1]).Name);
                Assert.AreEqual("test3", ((TypeMatchFilter)this.profile.MatchFilters[2]).Name);
                Assert.AreEqual("test4", ((AssemblyMatchFilter)this.profile.MatchFilters[3]).Name);
                Assert.AreEqual("test5", ((CommandMatchFilter)this.profile.MatchFilters[4]).Name);
                Assert.AreEqual("test6", ((AddBlockCommandMatchFilter)this.profile.MatchFilters[5]).Name);
            }
        }
    }
}
