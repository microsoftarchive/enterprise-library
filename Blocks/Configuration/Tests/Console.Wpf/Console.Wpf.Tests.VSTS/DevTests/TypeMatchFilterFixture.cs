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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    public class given_new_filter : ArrangeActAssert
    {
        protected TypeMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new TypeMatchFilter { };
        }

        [TestClass]
        public class when_filtering_type : given_new_filter
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(TraceListener));
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }

    public class given_filter_configured_with_invalid_type_name : ArrangeActAssert
    {
        protected TypeMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new TypeMatchFilter { Name = "an invalid type name" };
        }

        [TestClass]
        public class when_filtering_type : given_filter_configured_with_invalid_type_name
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(TraceListener));
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }

    public class given_filter_configured_with_valid_type_name : ArrangeActAssert
    {
        protected TypeMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new TypeMatchFilter { Name = typeof(TraceListener).AssemblyQualifiedName };
        }

        [TestClass]
        public class when_filtering_type : given_filter_configured_with_valid_type_name
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(TraceListener));
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }

        [TestClass]
        public class when_filtering_derived_type : given_filter_configured_with_valid_type_name
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(EventLogTraceListener));
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }

        [TestClass]
        public class when_filtering_ancestor_type : given_filter_configured_with_valid_type_name
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(object));
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }


    public class given_filter_configured_with_valid_type_name_and_match_subclass : ArrangeActAssert
    {
        protected TypeMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new TypeMatchFilter { Name = typeof(TraceListener).AssemblyQualifiedName, MatchSubclasses = true };
        }

        [TestClass]
        public class when_filtering_type : given_filter_configured_with_valid_type_name_and_match_subclass
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(TraceListener));
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }

        [TestClass]
        public class when_filtering_derived_type : given_filter_configured_with_valid_type_name_and_match_subclass
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(EventLogTraceListener));
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }

        [TestClass]
        public class when_filtering_ancestor_type : given_filter_configured_with_valid_type_name_and_match_subclass
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(object));
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }
}
