using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
    [TestClass]
    public class WMIPushFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomTraceListenerSetting));
            //ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(EmailTraceListenerSetting));
            //ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FlatFileTraceListenerSetting));
            //ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(MsmqTraceListenerSetting));
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(RollingFlatFileTraceListenerSetting));
            NamedConfigurationSetting.ClearPublishedInstances();
        }

        [TestCleanup]
        public void TearDown()
        {
            ManagementEntityTypesRegistrar.UnregisterAll();
            NamedConfigurationSetting.ClearPublishedInstances();
        }

        [TestMethod]
        public void SaveCustomTraceListenerObject()
        {
            var sourceElement = new CustomTraceListenerData("name", typeof(bool), "initdata")
            {
                Filter = SourceLevels.Information
            };

            sourceElement.Attributes.Add("attr3", "val3");
            sourceElement.Attributes.Add("attr4", "val4");
            sourceElement.Attributes.Add("attr5", "val5");
            sourceElement.Formatter = "formatter";
            sourceElement.TraceOutputOptions = TraceOptions.Callstack;

            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            CustomTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);


            CustomTraceListenerSetting setting = settings[0] as CustomTraceListenerSetting;
            Assert.IsNotNull(setting);

            setting.Filter = SourceLevels.All.ToString();

            setting.Commit();

            Assert.AreEqual(SourceLevels.All, sourceElement.Filter);

            string[] attributes = new string[] { "attr1", "attr2" };
            setting = new CustomTraceListenerSetting(sourceElement, "name", "listenertype",
                    "init data", attributes, "traceoutputoption", SourceLevels.Critical.ToString(),
                    "formatter");
            setting.ApplicationName = "updated testappname";
            setting.SectionName = LoggingSettings.SectionName;

            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("CustomTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
            Assert.AreEqual(SourceLevels.Critical.ToString(), sourceElement.Filter.ToString());
        }

        [TestMethod]
        public void SaveRollingFlatFileTraceListenerObject()
        {
            var sourceElement = new RollingFlatFileTraceListenerData
            {
                Filter = SourceLevels.Information
            };

            var settings = new List<ConfigurationSetting>(1);
            RollingFlatFileTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);


            var setting = settings[0] as RollingFlatFileTraceListenerSetting;
            Assert.IsNotNull(setting);

            setting.Filter = SourceLevels.All.ToString();

            setting.Commit();

            Assert.AreEqual(SourceLevels.All, sourceElement.Filter);

            //setting = new RollingFlatFileTraceListenerSetting(null, "name", "FileName", "Header", "Footer", "Formatter",
            //                    "RollFileExistsBehavior", "RollInterval", 256, "TimeStampPattern", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "updated testappname";
            setting.SectionName = LoggingSettings.SectionName;

            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM RollingFlatFileTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("RollingFlatFileTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                var managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);

                Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
                managementObject.SetPropertyValue("Filter", SourceLevels.Critical.ToString());
                managementObject.Put();
            }
            Assert.AreEqual(SourceLevels.Critical.ToString(), sourceElement.Filter.ToString());
        }

    }
}
