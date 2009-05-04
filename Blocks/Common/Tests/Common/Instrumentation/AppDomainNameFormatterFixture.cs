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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class AppDomainNameFormatterFixture : MarshalByRefObject
    {
        [TestMethod]
        public void WillFormatNameWithAppDomainNamePrefix()
        {
            AppDomainNameFormatter nameFormatter = new AppDomainNameFormatter();

            string createdName = nameFormatter.CreateName("MyInstance");
            Assert.IsTrue(createdName.EndsWith(" - MyInstance"));
            Assert.IsTrue(createdName.Length <= 128);
        }

        [TestMethod]
        public void WillFormatNameUsingApplicationInstanceName()
        {
            string applicationInstanceName = "ApplicationInstanceName";
            string suffix = "MySuffix";
            string expectedInstanceName = string.Concat(applicationInstanceName, " - ", suffix);

            AppDomainNameFormatter formatter = new AppDomainNameFormatter(applicationInstanceName);

            string createdName = formatter.CreateName(suffix);
            Assert.AreEqual(expectedInstanceName, createdName);
        }

        /// <summary>
        /// Filter the invalid chars documented in http://msdn2.microsoft.com/en-us/library/aa373193.aspx
        /// </summary>
        [TestMethod]
        public void ShouldReplaceInvalidCharacters()
        {
            string invalidApplicationInstanceName = @"\\computer\object(parent/instance#index)\counter";
            string validApplicationIntanceName = "computerobjectparentinstanceindexcounter";

            //Normalize string length
            validApplicationIntanceName = validApplicationIntanceName.Substring(0, 32);

            string suffix = "MySuffix";
            string expectedInstanceName = string.Concat(validApplicationIntanceName, " - ", suffix);

            AppDomainNameFormatter formatter = new AppDomainNameFormatter(invalidApplicationInstanceName);

            string createdName = formatter.CreateName(suffix);
            Assert.AreEqual(expectedInstanceName, createdName);
        }
    }
}
