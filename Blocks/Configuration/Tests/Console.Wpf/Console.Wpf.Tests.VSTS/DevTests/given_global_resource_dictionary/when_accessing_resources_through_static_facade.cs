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
using System.Reflection;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_global_resource_dictionary
{
    [TestClass]
    public class when_accessing_resources_through_static_facade : ArrangeActAssert
    {
        protected override void Arrange()
        {
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }

        protected override void Act()
        {
            GlobalResources.Add("testDictionary",
                                new Uri("/Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime;component/Themes/Generic.xaml", UriKind.Relative),
                                true);

            GlobalResources.Add("testDictionary2",
                                "/Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime;component/Themes/Generic.xaml",
                                true);

        }

        [TestMethod]
        public void then_can_retrieve_well_known()
        {
            Assert.IsNotNull(GlobalResources.Get("ExpanderDictionary"));
        }

        [TestMethod]
        public void then_can_add_dictionairy()
        {
            Assert.IsNotNull(GlobalResources.Get("testDictionary"));
        }

        [TestMethod]
        public void then_can_add_as_uri_string_dictionairy()
        {
            Assert.IsNotNull(GlobalResources.Get("testDictionary2"));
        }
    }

    [TestClass]
    public class when_adding_weakly_held_dictionaries_through_static_facade : ArrangeActAssert
    {
        private ResourceDictionary dictionary;

        protected override void Arrange()
        {
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }

        protected override void Act()
        {
            dictionary = GlobalResources.Add("testDictionary",
                                new Uri("/Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime;component/Themes/Generic.xaml", UriKind.Relative),
                                false);

        }

        [TestMethod]
        public void then_added_dictionaries_are_weakly_held()
        {
            var dictionaryRef = new WeakReference(dictionary);
            dictionary = null;
            GC.Collect();
            Assert.IsFalse(dictionaryRef.IsAlive);
        }
    }

    [TestClass]
    public class when_adding_strongly_held_dictionaries_through_static_facade : ArrangeActAssert
    {
        protected override void Arrange()
        {
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }

        protected override void Act()
        {
            GlobalResources.Add("testDictionary1",
                                "/Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime;component/Themes/Generic.xaml",
                                true);

            GlobalResources.Add("testDictionary2",
                                new Uri(
                                    "/Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime;component/Resources/Brushes.xaml",
                                    UriKind.Relative));


            GlobalResources.Add("testDictionary3", new ResourceDictionary(), true);
        }

        [TestMethod]
        public void then_added_dictionaries_are_strongly_held()
        {
            var dictionaryRef1 = new WeakReference(GlobalResources.Get("testDictionary1"));
            var dictionaryRef2 = new WeakReference(GlobalResources.Get("testDictionary2"));
            var dictionaryRef3 = new WeakReference(GlobalResources.Get("testDictionary3"));
            GC.Collect();
            Assert.IsTrue(dictionaryRef1.IsAlive);
            Assert.IsTrue(dictionaryRef2.IsAlive);
            Assert.IsTrue(dictionaryRef3.IsAlive);
        }

    }

    [TestClass]
    public class when_removing_dictionary_through_static_facade : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionary;

        protected override void Arrange()
        {
            keyedDictionary = new KeyedResourceDictionary();
            GlobalResources.SetDictionary(keyedDictionary);
            GlobalResources.Add("testDictionary1",
                               "/Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime;component/Themes/Generic.xaml",
                               true);
        }

        protected override void Act()
        {
            GlobalResources.Remove("testDictionary1");
        }

        [TestMethod]
        public void then_dictionary_removed()
        {
            Assert.IsFalse(keyedDictionary.Contains("testDictionary1"));
        }
    }

    [TestClass]
    public class when_checking_empty_global_facade : ArrangeActAssert
    {
        protected override void Act()
        {
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }

        [TestMethod]
        public void then_always_has_extended_dictionary()
        {
            Assert.IsNotNull(GlobalResources.Get("ExtendedDictionary"));
        }
    }

    [TestClass]
    public class when_adding_extended_resource : ArrangeActAssert
    {
        private ResourceDictionary extension = new ResourceDictionary();
        private ResourceDictionary extendedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
            extendedDictionary = GlobalResources.Get("ExtendedDictionary");
            extendedDictionary.MergedDictionaries.Clear();
        }
        protected override void Act()
        {
            GlobalResources.AddExtendedDictionary(extension);
        }

        [TestMethod]
        public void then_extensions_appear_in_extended_dictionary()
        {
            Assert.IsTrue(extendedDictionary.MergedDictionaries.Contains(extension));
        }
    }

    [TestClass]
    public class when_adding_extension_dictionaries_with_matching_sources : ArrangeActAssert
    {
        private ResourceDictionary extendedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
            extendedDictionary = GlobalResources.Get("ExtendedDictionary");
        }

        protected override void Act()
        {
            Uri extensionsUri = UriTestHelper.GetTestResourceUri("SomeResourceFile.xaml");

            GlobalResources.AddExtendedDictionary((ResourceDictionary)Application.LoadComponent(extensionsUri));
            GlobalResources.AddExtendedDictionary((ResourceDictionary)Application.LoadComponent(extensionsUri));
        }


        [TestMethod]
        public void then_only_one_appears()
        {
            Assert.AreEqual(1, extendedDictionary.MergedDictionaries.Count);
        }
    }

    [TestClass]
    public class when_adding_extensions_from_heterogenous_sources : ArrangeActAssert
    {
        private ResourceDictionary extendedDictionary;
        private ResourceDictionary dictionaryOne;
        private ResourceDictionary dictionaryTwo;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
            extendedDictionary = GlobalResources.Get("ExtendedDictionary");
        }

        protected override void Act()
        {
            Uri extensionsUri = UriTestHelper.GetTestResourceUri("SomeResourceFile.xaml");

            dictionaryOne = (ResourceDictionary)Application.LoadComponent(extensionsUri);
            dictionaryTwo = new ResourceDictionary();

            GlobalResources.AddExtendedDictionary(dictionaryOne);
            GlobalResources.AddExtendedDictionary(dictionaryTwo);
        }

        [TestMethod]
        public void then_both_dictionaries_appear()
        {
            Assert.IsTrue(extendedDictionary.MergedDictionaries.Contains(dictionaryOne));
            Assert.IsTrue(extendedDictionary.MergedDictionaries.Contains(dictionaryTwo));
        }
    }

    [TestClass]
    public class when_removing_extensions : ArrangeActAssert
    {
        private ResourceDictionary extendedDictionary;
        private ResourceDictionary dictionaryOne;
        private ResourceDictionary dictionaryTwo;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
            extendedDictionary = GlobalResources.Get("ExtendedDictionary");

            Uri extensionsUri = UriTestHelper.GetTestResourceUri("SomeResourceFile.xaml");
            dictionaryOne = (ResourceDictionary)Application.LoadComponent(extensionsUri);

            dictionaryTwo = new ResourceDictionary();
            GlobalResources.AddExtendedDictionary(dictionaryOne);
            GlobalResources.AddExtendedDictionary(dictionaryTwo);
        }

        protected override void Act()
        {
            GlobalResources.RemoveExtendedDictionary(dictionaryTwo);
        }

        [TestMethod]
        public void then_first_dictionary_remains()
        {
            Assert.IsTrue(extendedDictionary.MergedDictionaries.Contains(dictionaryOne));
        }

        [TestMethod]
        public void then_second_dictionary_removed()
        {
            Assert.IsFalse(extendedDictionary.MergedDictionaries.Contains(dictionaryTwo));
        }

        [TestMethod]
        public void then_re_removing_does_not_throw()
        {
            GlobalResources.RemoveExtendedDictionary(dictionaryTwo);
        }
    }

    static class UriTestHelper
    {
        public static Uri GetTestResourceUri(string resourceName)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var resourcePath = typeof(UriTestHelper).Namespace.Replace(assemblyName, string.Empty).Replace('.', '/');

            return new Uri(
                string.Format("/{0};component{1}/{2}", assemblyName, resourcePath, resourceName),
                UriKind.Relative);
        }
    }

}
