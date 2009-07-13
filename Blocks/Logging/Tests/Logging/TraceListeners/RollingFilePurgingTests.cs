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
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{

    public class TemporaryFolderBasedTest : ArrangeActAssert
    {
        protected string BaseDirectory { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            this.BaseDirectory = Guid.NewGuid().ToString("N");
            Directory.CreateDirectory(this.BaseDirectory);
        }

        protected override void Teardown()
        {
            base.Teardown();

            Directory.Delete(this.BaseDirectory, true);
        }
    }

    public class Given_a_directory_with_five_matching_files : TemporaryFolderBasedTest
    {
        protected string baseFileName;

        protected override void Arrange()
        {
            base.Arrange();

            this.baseFileName = "trace.log";

            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace001.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace003.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace002.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace004.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace005.log"), "test1");
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_six_files_purges
            : Given_a_directory_with_five_matching_files
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 6).Purge();
            }

            [TestMethod]
            public void Then_No_Files_Are_Deleted()
            {
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_five_files_purges
            : Given_a_directory_with_five_matching_files
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 5).Purge();
            }

            [TestMethod]
            public void Then_No_Files_Are_Deleted()
            {
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_four_files_purges
            : Given_a_directory_with_five_matching_files
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 4).Purge();
            }

            [TestMethod]
            public void Then_The_Oldest_File_Is_Deleted()
            {
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_three_files_purges
            : Given_a_directory_with_five_matching_files
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 3).Purge();
            }

            [TestMethod]
            public void Then_The_Two_Oldest_File_are_deleted()
            {
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_three_files_for_a_different_base_FileName_purges
            : Given_a_directory_with_five_matching_files
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, "some_pattern.log", 3).Purge();
            }

            [TestMethod]
            public void Then_No_Files_Are_Deleted()
            {
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_two_files_for_a_base_FileName_with_an_extension_contained_in_the_existing_files
            : Given_a_directory_with_five_matching_files
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, "trace.lo", 2).Purge();
            }

            [TestMethod]
            public void Then_No_Files_Are_Deleted()
            {
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }
    }

    public class Given_a_directory_with_five_matching_files_with_long_extensions : TemporaryFolderBasedTest
    {
        protected string directory;
        protected string baseFileName;

        protected override void Arrange()
        {
            base.Arrange();

            this.baseFileName = "trace.logged";

            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace001.logged"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace003.logged"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace002.logged"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace004.logged"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace005.logged"), "test1");
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_three_files_purges
            : Given_a_directory_with_five_matching_files_with_long_extensions
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 3).Purge();
            }

            [TestMethod]
            public void Then_the_two_oldest_files_are_deleted()
            {
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace001.logged")));
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace003.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.logged")));
            }
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_three_files_for_a_base_FileName_with_an_three_chars_extension_contained_in_the_existing_files
            : Given_a_directory_with_five_matching_files_with_long_extensions
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, "trace.log", 3).Purge();
            }

            [TestMethod]
            public void Then_No_Files_Are_Deleted()
            {
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace001.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.logged")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.logged")));
            }
        }
    }

    public class Given_a_directory_with_five_matching_files_one_of_them_readonly : TemporaryFolderBasedTest
    {
        protected string baseFileName;

        protected override void Arrange()
        {
            base.Arrange();

            this.baseFileName = "trace.log";

            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace001.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace003.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace002.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace004.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace005.log"), "test1");

            File.SetAttributes(Path.Combine(this.BaseDirectory, "trace003.log"), FileAttributes.ReadOnly);
        }

        protected override void Teardown()
        {
            File.SetAttributes(Path.Combine(this.BaseDirectory, "trace003.log"), FileAttributes.Normal);

            base.Teardown();
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_three_files_purges
            : Given_a_directory_with_five_matching_files_one_of_them_readonly
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 3).Purge();
            }

            [TestMethod]
            public void Then_only_the_non_readonly_file_is_deleted_among_the_two_oldest_files()
            {
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }
    }

    public class Given_a_directory_with_five_matching_files_one_of_them_opened : TemporaryFolderBasedTest
    {
        private Stream stream;
        protected string baseFileName;

        protected override void Arrange()
        {
            base.Arrange();

            this.baseFileName = "trace.log";

            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace001.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace003.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace002.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace004.log"), "test1");
            Thread.Sleep(50);
            File.WriteAllText(Path.Combine(this.BaseDirectory, "trace005.log"), "test1");


            stream = File.OpenWrite(Path.Combine(this.BaseDirectory, "trace003.log"));
        }

        protected override void Teardown()
        {
            stream.Close();

            base.Teardown();
        }

        [TestClass]
        public class When_a_purger_with_a_cap_of_three_files_purges
            : Given_a_directory_with_five_matching_files_one_of_them_opened
        {
            protected override void Act()
            {
                new RollingFlatFilePurger(this.BaseDirectory, this.baseFileName, 3).Purge();
            }

            [TestMethod]
            public void Then_only_the_non_opened_file_is_deleted_among_the_two_oldest_files()
            {
                Assert.IsFalse(File.Exists(Path.Combine(this.BaseDirectory, "trace001.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace003.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace002.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace004.log")));
                Assert.IsTrue(File.Exists(Path.Combine(this.BaseDirectory, "trace005.log")));
            }
        }
    }

    public class Given_a_purger_configured_for_a_non_existing_directory : ArrangeActAssert
    {
        private RollingFlatFilePurger purger;

        protected override void Arrange()
        {
            base.Arrange();

            this.purger = new RollingFlatFilePurger(Guid.NewGuid().ToString("N"), "trace.log", 4);
        }

        [TestClass]
        public class When_purging : Given_a_purger_configured_for_a_non_existing_directory
        {
            protected override void Act()
            {
                this.purger.Purge();
            }

            [TestMethod]
            public void Then_no_exception_is_thrown()
            {
            }
        }
    }

    [TestClass]
    public partial class Given
    {
        [TestMethod]
        public void Then_creating_a_purger_with_a_null_directory_throws()
        {
            try
            {
                new RollingFlatFilePurger(null, "trace.log", 10);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }

        [TestMethod]
        public void Then_creating_a_purger_with_a_null_filename_throws()
        {
            try
            {
                new RollingFlatFilePurger(Environment.CurrentDirectory, null, 10);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }

        [TestMethod]
        public void Then_creating_a_purger_with_a_negative_cap_throws()
        {
            try
            {
                new RollingFlatFilePurger(Environment.CurrentDirectory, "trace.log", -10);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentOutOfRangeException)
            {
                // expected
            }
        }

        [TestMethod]
        public void Then_creating_a_purger_with_a_zero_cap_throws()
        {
            try
            {
                new RollingFlatFilePurger(Environment.CurrentDirectory, "trace.log", 0);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentOutOfRangeException)
            {
                // expected
            }
        }

        [TestMethod]
        public void Then_creating_a_purger_with_a_cap_of_one_does_not_throw()
        {
            new RollingFlatFilePurger(Environment.CurrentDirectory, "trace.log", 1);
        }
    }
}
