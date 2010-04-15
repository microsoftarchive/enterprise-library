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
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_global_resource_dictionary
{
    [TestClass]
    public class when_retrieving_resource : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionairy;

        protected override void Arrange()
        {
            base.Arrange();
            keyedDictionairy = new KeyedResourceDictionary();
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void then_throws_on_invalid_key()
        {
            keyedDictionairy.Get("notAKeyItem");
        }

        [TestMethod]
        public void then_does_not_contain_invalid_key()
        {
            Assert.IsFalse(keyedDictionairy.Contains("notAKeyItem"));
        }
    }

    [TestClass]
    public class when_adding_new_dictionary : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            keyedDictionary = new KeyedResourceDictionary();
        }

        protected override void Act()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            keyedDictionary.Add("testResources", dictionary, false);
        }

        [TestMethod]
        public void then_can_retrieve_added_dictionary_by_name()
        {
            Assert.IsNotNull(keyedDictionary.Get("testResources"));
        }

        [TestMethod]
        public void then_contains_returns_true()
        {
            Assert.IsTrue(keyedDictionary.Contains("testResources"));
        }

        [TestMethod]
        public void then_retrieving_wellknown_item_auto_discovers()
        {
            Assert.IsNotNull(keyedDictionary.Get("ExpanderDictionary"));
        }

        [TestMethod]
        public void then_dictionaries_are_weakly_held()
        {
            var testResource = new WeakReference(keyedDictionary.Get("testResources"));
            GC.Collect();
            Assert.IsFalse(testResource.IsAlive);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void then_getting_collected_item_throws_if_not_autoloadable()
        {
            var testResource = new WeakReference(keyedDictionary.Get("testResources"));
            GC.Collect();
            Assert.IsFalse(testResource.IsAlive);

            keyedDictionary.Get("testResources");
        }

        [TestMethod]
        public void then_reloads_collected_autoloaded_item()
        {
            var testResource = new WeakReference(keyedDictionary.Get("ExpanderDictionary"));
            GC.Collect();
            Assert.IsFalse(testResource.IsAlive);

            Assert.IsNotNull(keyedDictionary.Get("ExpanderDictionary"));
        }

    }

    [TestClass]
    public class when_adding_multiple_dictionaries : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            keyedDictionary = new KeyedResourceDictionary();
        }

        protected override void Act()
        {
            keyedDictionary.Add("one", new ResourceDictionary(), false);
            keyedDictionary.Add("two", new ResourceDictionary(), false);
            keyedDictionary.Add("three", new ResourceDictionary(), false);
        }

        [TestMethod]
        public void then_can_enumerate_keys()
        {
            CollectionAssert.AreEquivalent(keyedDictionary.Keys.ToArray(), "one,two,three".Split(','));
        }   
    }

    [TestClass]
    public class when_removing_dictionary : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            keyedDictionary = new KeyedResourceDictionary();
            keyedDictionary.Add("one", new ResourceDictionary(), false);
            keyedDictionary.Add("two", new ResourceDictionary(), false);
        }

        protected override void Act()
        {
            keyedDictionary.Remove("two");
        }

        [TestMethod]
        public void then_item_removed()
        {
            Assert.IsFalse(keyedDictionary.Contains("two"));
        }

        [TestMethod]
        public void then_nonremoved_items_remain()
        {
            Assert.IsTrue(keyedDictionary.Contains("one"));
        }
    }

    [TestClass]
    public class when_adding_new_dictionary_with_keepalive : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            keyedDictionary = new KeyedResourceDictionary();
        }

        protected override void Act()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            keyedDictionary.Add("testResources", dictionary, true);
        }

        [TestMethod]
        public void then_dictionary_is_strongly_held()
        {
            var testResource = new WeakReference(keyedDictionary.Get("testResources"));
            GC.Collect();
            Assert.IsTrue(testResource.IsAlive);
        }
    }

    [TestClass]
    public class when_adding_with_name_of_collected_element : ArrangeActAssert
    {
        private KeyedResourceDictionary keyedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            keyedDictionary = new KeyedResourceDictionary();
            
            keyedDictionary.Add("testResources", new ResourceDictionary(), false);
            GC.Collect();
        }

        [TestMethod]
        public void then_should_replace_without_throwing()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            keyedDictionary.Add("testResources", dictionary, false);
            Assert.AreSame(dictionary, keyedDictionary.Get("testResources"));
        }
    }




    
}
