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
    public class given_new_assembly_filter : ArrangeActAssert
    {
        protected AssemblyMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AssemblyMatchFilter { };
        }

        [TestClass]
        public class when_filtering_type : given_new_assembly_filter
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

    public class given_assembly_filter_configured_with_invalid_assembly_name : ArrangeActAssert
    {
        protected AssemblyMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AssemblyMatchFilter { Name = "an invalid assembly name" };
        }

        [TestClass]
        public class when_filtering_type : given_assembly_filter_configured_with_invalid_assembly_name
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

    public class given_assembly_filter_configured_with_valid_assembly_name : ArrangeActAssert
    {
        protected AssemblyMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AssemblyMatchFilter { Name = typeof(TraceListener).Assembly.GetName().Name };
        }

        [TestClass]
        public class when_filtering_type_from_the_configured_assembly : given_assembly_filter_configured_with_valid_assembly_name
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
        public class when_filtering_type_from_different_assembly : given_assembly_filter_configured_with_valid_assembly_name
        {
            private bool match = false;

            protected override void Act()
            {
                this.match = this.filter.Match(typeof(int));
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }

}
