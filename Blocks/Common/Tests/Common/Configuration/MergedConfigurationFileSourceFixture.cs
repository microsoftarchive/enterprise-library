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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_ConfigurationFileWithParentSource : ArrangeActAssert
    {
        protected string FileSourceDummySectionName = "externaldummy.filesource";
        protected IConfigurationSource MergedSource;

        protected override void Arrange()
        {
            MergedSource = new FileConfigurationSource(@"MergedConfigurationFile.config");
        }
    }

    [TestClass]
    public class When_CallingGetSection : Given_ConfigurationFileWithParentSource
    {
        DummySection section;

        protected override void Act()
        {
            section = (DummySection)MergedSource.GetSection(FileSourceDummySectionName);
        }

        [TestMethod]
        public void Then_LocalSourceReturnsValueFromParentSource()
        {
            Assert.AreEqual(11, section.Value);
        }
    }

}
