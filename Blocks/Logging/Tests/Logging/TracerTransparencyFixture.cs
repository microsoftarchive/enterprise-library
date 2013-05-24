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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TracerTransparencyFixture
{
    public class BasePartialTrustContext : ArrangeActAssert
    {
        protected AppDomain appDomain;
        protected LoggerProxy loggerProxy;

        protected override void Arrange()
        {
            base.Arrange();

            var fullyTrustedAssemblies = this.GetFullyTrustedAssemblies().ToArray();
            var unsignedAssemblies = fullyTrustedAssemblies.Where(sn => sn.PublicKey.ToString() == "");
            if (unsignedAssemblies.Any())
            {
                Assert.Inconclusive("Full trust assemblies must be signed. This test will be ignored. Unsigned assemblies: " + unsignedAssemblies.Aggregate("", (a, sn) => a + sn.Name + " "));
            }

            var evidence = new Evidence();
            evidence.AddHostEvidence(new Zone(SecurityZone.Intranet));
            var set = SecurityManager.GetStandardSandbox(evidence);
            this.AddPermissions(set);
            this.appDomain =
                AppDomain.CreateDomain(
                    "partial trust",
                    null,
                    AppDomain.CurrentDomain.SetupInformation,
                    set,
                    fullyTrustedAssemblies);

            this.loggerProxy = ((LoggerProxy)this.appDomain.CreateInstanceAndUnwrap(typeof(LoggerProxy).Assembly.FullName, typeof(LoggerProxy).FullName));
            this.loggerProxy.Setup();
        }

        protected override void Teardown()
        {
            if (this.appDomain != null)
            {
                AppDomain.Unload(this.appDomain);
            }
        }

        protected virtual void AddPermissions(PermissionSet set)
        {
            // These allow to look at the security exceptions
            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy));
        }

        protected virtual IEnumerable<StrongName> GetFullyTrustedAssemblies()
        {
            return new StrongName[0];
        }
    }

    public class given_log_writer_in_partial_trust_app_domain_without_unmanaged_code_permission : BasePartialTrustContext
    {
        [TestClass]
        public class when_tracing_operation : given_log_writer_in_partial_trust_app_domain_without_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;

            protected override void Act()
            {
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple");
            }

            [TestMethod]
            public void then_writes_log_entry_only()
            {
                Assert.AreEqual(1, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[0]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_does_not_have_tracing_category()
            {
                Assert.IsFalse(this.entryProperties[0]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[0]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_does_not_have_activity_id()
            {
                Assert.AreEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }
        }

        [TestClass]
        public class when_tracing_operation_with_explicit_activity_id : given_log_writer_in_partial_trust_app_domain_without_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;
            private Guid activityId;

            protected override void Act()
            {
                this.activityId = Guid.NewGuid();
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple", activityId);
            }

            [TestMethod]
            public void then_writes_log_entry_only()
            {
                Assert.AreEqual(1, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[0]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_does_not_have_tracing_category()
            {
                Assert.IsFalse(this.entryProperties[0]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[0]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_does_not_have_activity_id()
            {
                Assert.AreEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }
        }
    }

    public class given_log_writer_in_partial_trust_app_domain_with_unmanaged_code_permission : BasePartialTrustContext
    {
        protected override void AddPermissions(PermissionSet set)
        {
            base.AddPermissions(set);

            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
        }

        [TestClass]
        public class when_tracing_operation : given_log_writer_in_partial_trust_app_domain_with_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;

            protected override void Act()
            {
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple");
            }

            [TestMethod]
            public void then_writes_tracing_and_log_entries()
            {
                Assert.AreEqual(3, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_start_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[0]["Categories"]);
            }

            [TestMethod]
            public void then_start_entry_has_activity_id()
            {
                Assert.AreNotEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[1]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_has_tracing_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_has_activity_id()
            {
                Assert.AreNotEqual(Guid.Empty.ToString(), this.entryProperties[1]["ActivityId"]);
            }

            [TestMethod]
            public void then_end_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[0]["Categories"]);
            }

            [TestMethod]
            public void then_end_entry_has_activity_id()
            {
                Assert.AreNotEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }
        }

        [TestClass]
        public class when_tracing_operation_with_explicit_activity_id : given_log_writer_in_partial_trust_app_domain_with_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;
            private Guid activityId;

            protected override void Act()
            {
                this.activityId = Guid.NewGuid();
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple", activityId);
            }

            [TestMethod]
            public void then_writes_tracing_and_log_entries()
            {
                Assert.AreEqual(3, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_start_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[0]["Categories"]);
            }

            [TestMethod]
            public void then_start_entry_has_activity_id()
            {
                Assert.AreEqual(this.activityId.ToString(), this.entryProperties[0]["ActivityId"]);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[1]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_has_tracing_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_has_activity_id()
            {
                Assert.AreEqual(this.activityId.ToString(), this.entryProperties[1]["ActivityId"]);
            }

            [TestMethod]
            public void then_end_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[2]["Categories"]);
            }

            [TestMethod]
            public void then_end_entry_has_activity_id()
            {
                Assert.AreEqual(this.activityId.ToString(), this.entryProperties[2]["ActivityId"]);
            }
        }
    }

    public class given_log_writer_in_partial_trust_app_domain_with_fully_trusted_logging_and_without_unmanaged_code_permission : BasePartialTrustContext
    {
        protected override IEnumerable<StrongName> GetFullyTrustedAssemblies()
        {
            var commonAssembly = typeof(Guard).Assembly.GetName();
            var loggingAssembly = typeof(LogWriter).Assembly.GetName();

            return base.GetFullyTrustedAssemblies()
                .Concat(new[] { 
                    new StrongName(new StrongNamePublicKeyBlob(commonAssembly.GetPublicKey()), commonAssembly.Name, commonAssembly.Version),
                    new StrongName(new StrongNamePublicKeyBlob(loggingAssembly.GetPublicKey()), loggingAssembly.Name, loggingAssembly.Version)
                });
        }

        [TestClass]
        public class when_tracing_operation : given_log_writer_in_partial_trust_app_domain_with_fully_trusted_logging_and_without_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;

            protected override void Act()
            {
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple");
            }

            [TestMethod]
            public void then_writes_log_entry_only()
            {
                Assert.AreEqual(1, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[0]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_does_not_have_tracing_category()
            {
                Assert.IsFalse(this.entryProperties[0]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[0]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_does_not_have_activity_id()
            {
                Assert.AreEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }
        }

        [TestClass]
        public class when_tracing_operation_with_explicit_activity_id : given_log_writer_in_partial_trust_app_domain_with_fully_trusted_logging_and_without_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;
            private Guid activityId;

            protected override void Act()
            {
                this.activityId = Guid.NewGuid();
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple", activityId);
            }

            [TestMethod]
            public void then_writes_log_entry_only()
            {
                Assert.AreEqual(1, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[0]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_does_not_have_tracing_category()
            {
                Assert.IsFalse(this.entryProperties[0]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[0]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_does_not_have_activity_id()
            {
                Assert.AreEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }
        }
    }

    public class given_log_writer_in_partial_trust_app_domain_with_fully_trusted_logging_and_unmanaged_code_permission : BasePartialTrustContext
    {
        protected override void AddPermissions(PermissionSet set)
        {
            base.AddPermissions(set);

            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
        }

        protected override IEnumerable<StrongName> GetFullyTrustedAssemblies()
        {
            var commonAssembly = typeof(Guard).Assembly.GetName();
            var loggingAssembly = typeof(LogWriter).Assembly.GetName();

            return base.GetFullyTrustedAssemblies()
                .Concat(new[] { 
                    new StrongName(new StrongNamePublicKeyBlob(commonAssembly.GetPublicKey()), commonAssembly.Name, commonAssembly.Version),
                    new StrongName(new StrongNamePublicKeyBlob(loggingAssembly.GetPublicKey()), loggingAssembly.Name, loggingAssembly.Version)
                });
        }

        [TestClass]
        public class when_tracing_operation : given_log_writer_in_partial_trust_app_domain_with_fully_trusted_logging_and_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;

            protected override void Act()
            {
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple");
            }

            [TestMethod]
            public void then_writes_tracing_and_log_entries()
            {
                Assert.AreEqual(3, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_start_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[0]["Categories"]);
            }

            [TestMethod]
            public void then_start_entry_has_activity_id()
            {
                Assert.AreNotEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[1]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_has_tracing_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_has_activity_id()
            {
                Assert.AreNotEqual(Guid.Empty.ToString(), this.entryProperties[1]["ActivityId"]);
            }

            [TestMethod]
            public void then_end_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[0]["Categories"]);
            }

            [TestMethod]
            public void then_end_entry_has_activity_id()
            {
                Assert.AreNotEqual(Guid.Empty.ToString(), this.entryProperties[0]["ActivityId"]);
            }
        }

        [TestClass]
        public class when_tracing_operation_with_explicit_activity_id : given_log_writer_in_partial_trust_app_domain_with_fully_trusted_logging_and_unmanaged_code_permission
        {
            private IDictionary<string, string>[] entryProperties;
            private Guid activityId;

            protected override void Act()
            {
                this.activityId = Guid.NewGuid();
                this.entryProperties = this.loggerProxy.TraceOperation("inside trace", "category", "simple", activityId);
            }

            [TestMethod]
            public void then_writes_tracing_and_log_entries()
            {
                Assert.AreEqual(3, this.entryProperties.Length);
            }

            [TestMethod]
            public void then_start_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[0]["Categories"]);
            }

            [TestMethod]
            public void then_start_entry_has_activity_id()
            {
                Assert.AreEqual(this.activityId.ToString(), this.entryProperties[0]["ActivityId"]);
            }

            [TestMethod]
            public void then_log_entry_has_message()
            {
                Assert.AreEqual("inside trace", this.entryProperties[1]["Message"]);
            }

            [TestMethod]
            public void then_log_entry_has_tracing_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("simple"));
            }

            [TestMethod]
            public void then_log_entry_has_specified_category()
            {
                Assert.IsTrue(this.entryProperties[1]["Categories"].Contains("category"));
            }

            [TestMethod]
            public void then_log_entry_has_activity_id()
            {
                Assert.AreEqual(this.activityId.ToString(), this.entryProperties[1]["ActivityId"]);
            }

            [TestMethod]
            public void then_end_entry_has_tracing_category_only()
            {
                Assert.AreEqual("simple-", this.entryProperties[2]["Categories"]);
            }

            [TestMethod]
            public void then_end_entry_has_activity_id()
            {
                Assert.AreEqual(this.activityId.ToString(), this.entryProperties[2]["ActivityId"]);
            }
        }
    }

    public class LoggerProxy : MarshalByRefObject
    {
        private MockTraceListener traceListener;

        public void Setup()
        {
            MockTraceListener.Reset();
            this.traceListener = new MockTraceListener();

            var logWriter =
                new LogWriter(
                    new ILogFilter[0],
                    new LogSource[0],
                    new LogSource("all", new TraceListener[] { traceListener }, SourceLevels.All),
                    new LogSource("not processed"),
                    new LogSource("errors"),
                    "default",
                    true,
                    false);

            Logger.SetLogWriter(logWriter, false);
        }

        public IDictionary<string, string>[] TraceOperation(string message, string category, string operationName)
        {
            using (var tracer = new Tracer(operationName))
            {
                Logger.Write(message, category);
            }

            return GetEntriesProperties();
        }

        public IDictionary<string, string>[] TraceOperation(string message, string category, string operationName, Guid activityId)
        {
            using (var tracer = new Tracer(operationName, activityId))
            {
                Logger.Write(message, category);
            }

            return GetEntriesProperties();
        }

        private static IDictionary<string, string>[] GetEntriesProperties()
        {
            var dictionaries = new List<Dictionary<string, string>>();

            foreach (var entry in MockTraceListener.Entries)
            {
                var dictionary = new Dictionary<string, string>();
                foreach (var property in typeof(LogEntry).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.CanRead && pi.PropertyType == typeof(string) || pi.PropertyType.IsValueType))
                {
                    dictionary[property.Name] = (property.GetValue(entry) ?? "").ToString();
                }

                dictionary["Categories"] = entry.Categories.Aggregate("", (s, c) => s + c + "-");

                dictionaries.Add(dictionary);
            }

            return dictionaries.ToArray();
        }
    }
}
