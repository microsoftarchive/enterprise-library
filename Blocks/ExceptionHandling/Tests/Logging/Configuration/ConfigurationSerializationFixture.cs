//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Tests
{
    [TestClass]
    public class ConfigurationSerializationFixture
    {
        const string policyName1 = "policy1";

        const string typeName11 = "type11";

        const string handlerName111 = "handler111";
        const string handlerCategory111 = "category";
        const string handlerMessage111 = "hander message 111";

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            ExceptionHandlingSettings settings = new ExceptionHandlingSettings();

            ExceptionTypeData typeData11 = new ExceptionTypeData(typeName11, typeof(ArgumentNullException), PostHandlingAction.None);
            typeData11.ExceptionHandlers.Add(new LoggingExceptionHandlerData(handlerName111, handlerCategory111, 100, TraceEventType.Information, handlerMessage111, typeof(ExceptionFormatter), 101));

            ExceptionPolicyData policyData1 = new ExceptionPolicyData(policyName1);
            policyData1.ExceptionTypes.Add(typeData11);

            settings.ExceptionPolicies.Add(policyData1);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ExceptionHandlingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            ExceptionHandlingSettings roSettigs = (ExceptionHandlingSettings)configurationSource.GetSection(ExceptionHandlingSettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(1, roSettigs.ExceptionPolicies.Count);

            Assert.IsNotNull(roSettigs.ExceptionPolicies.Get(policyName1));
            Assert.AreEqual(1, roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Count);

            Assert.IsNotNull(roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11));
            Assert.AreSame(typeof(ArgumentNullException), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).Type);
            Assert.AreEqual(1, roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Count);
            Assert.AreSame(typeof(LoggingExceptionHandlerData), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111).GetType());
            Assert.AreEqual(100, ((LoggingExceptionHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).EventId);
            Assert.AreEqual(typeof(ExceptionFormatter), ((LoggingExceptionHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).FormatterType);
            Assert.AreEqual(handlerCategory111, ((LoggingExceptionHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).LogCategory);
            Assert.AreEqual(101, ((LoggingExceptionHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).Priority);
            Assert.AreEqual(TraceEventType.Information, ((LoggingExceptionHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).Severity);
            Assert.AreEqual(handlerMessage111, ((LoggingExceptionHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).Title);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void DeserializingLoggingHandlerDataWithoutFormatterThrows()
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?><exceptionHandling>
<exceptionPolicies>
<add name=""Policy"">
<exceptionTypes>
<add name=""All Exceptions"" type=""System.Exception, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""
postHandlingAction=""NotifyRethrow"">
<exceptionHandlers>
<add name=""Logging Exception Handler"" type=""Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.LoggingExceptionHandler, Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging""
logCategory=""General"" eventId=""100"" severity=""Error"" title=""Enterprise Library Exception Handling""
priority=""0"" />
</exceptionHandlers>
</add>
</exceptionTypes>
</add>
</exceptionPolicies>
</exceptionHandling>
";
            using (var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { CloseInput = true }))
            {
                new ExceptionHandlingSettings().ReadXml(reader);
            }
        }
    }
}
