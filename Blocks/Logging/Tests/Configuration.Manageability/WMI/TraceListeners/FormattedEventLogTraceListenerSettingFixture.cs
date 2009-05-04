//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
	[TestClass]
	public class FormattedEventLogTraceListenerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FormattedEventLogTraceListenerSetting));
			NamedConfigurationSetting.ClearPublishedInstances();
		}

		[TestCleanup]
		public void TearDown()
		{
			ManagementEntityTypesRegistrar.UnregisterAll();
			NamedConfigurationSetting.ClearPublishedInstances();
		}

		[TestMethod]
		public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
		{
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FormattedEventLogTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			FormattedEventLogTraceListenerSetting setting
				= new FormattedEventLogTraceListenerSetting(null, "name", "source", "log", "machine", "formatter", TraceOptions.Callstack.ToString(), System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = LoggingSettings.SectionName;
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FormattedEventLogTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("formatter", resultEnumerator.Current.Properties["Formatter"].Value);
				Assert.AreEqual("source", resultEnumerator.Current.Properties["Source"].Value);
				Assert.AreEqual("log", resultEnumerator.Current.Properties["Log"].Value);
				Assert.AreEqual("machine", resultEnumerator.Current.Properties["MachineName"].Value);
				Assert.AreEqual(TraceOptions.Callstack.ToString(), resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			FormattedEventLogTraceListenerSetting setting
				= new FormattedEventLogTraceListenerSetting(null, "name", "source", "log", "machine", "formatter", TraceOptions.Callstack.ToString(), System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FormattedEventLogTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("FormattedEventLogTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				//should throw 
				managementObject.Put();
			}
		}

        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            FormattedEventLogTraceListenerData sourceElement = new FormattedEventLogTraceListenerData();
            sourceElement.Filter = SourceLevels.Error;
            sourceElement.Formatter = "formatter";
            sourceElement.Log = "original log";
            sourceElement.MachineName = "original machine";
            sourceElement.Source = "original source";
            sourceElement.TraceOutputOptions = TraceOptions.ProcessId;

            var settings = new List<ConfigurationSetting>(1);
            FormattedEventLogTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);

            FormattedEventLogTraceListenerSetting setting = settings[0] as FormattedEventLogTraceListenerSetting;
            Assert.IsNotNull(setting);

            setting.Formatter = "updated formatter";
            setting.Filter = SourceLevels.All.ToString();
            setting.Log = "updated log";
            setting.MachineName = "updated machine";
            setting.Source = "updated source";
            setting.TraceOutputOptions = TraceOptions.ThreadId.ToString();

            setting.Commit();

            Assert.AreEqual("updated formatter", sourceElement.Formatter);
            Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
            Assert.AreEqual("updated log", sourceElement.Log);
            Assert.AreEqual("updated machine", sourceElement.MachineName);
            Assert.AreEqual("updated source", sourceElement.Source);
            Assert.AreEqual(TraceOptions.ThreadId, sourceElement.TraceOutputOptions);
        }
	}
}
