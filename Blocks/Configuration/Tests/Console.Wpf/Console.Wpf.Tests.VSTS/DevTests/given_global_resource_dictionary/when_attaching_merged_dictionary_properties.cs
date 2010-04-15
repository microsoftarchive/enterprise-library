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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.Collections;

namespace Console.Wpf.Tests.VSTS.DevTests.given_global_resource_dictionary
{
    [TestClass]
    public class when_attaching_single_merged_dictionary : ArrangeActAssert
    {
        private ResourceDictionary addedResource;
        private FrameworkElement frameworkElement;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.Clear();
            addedResource = new ResourceDictionary();
            GlobalResources.Add("testDictionaryOne", addedResource, false);
        }

        protected override void Act()
        {
            frameworkElement = new FrameworkElement();
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "testDictionaryOne");
        }

        [TestMethod]
        public void then_resources_merged_into_elements_resources()
        {
            Assert.IsTrue(frameworkElement.Resources.MergedDictionaries.Contains(addedResource));
        }
    }

    [TestClass]
    public class when_attaching_merge_dictionary_with_extra_spaces : ArrangeActAssert
    {
        private ResourceDictionary addedResource;
        private FrameworkElement frameworkElement;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.Clear();
            addedResource = new ResourceDictionary();
            GlobalResources.Add("testDictionaryOne", addedResource, false);
        }

        protected override void Act()
        {
            frameworkElement = new FrameworkElement();
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "  testDictionaryOne  ");
        }

        [TestMethod]
        public void then_trims_for_dictionary_matching()
        {
            Assert.IsTrue(frameworkElement.Resources.MergedDictionaries.Contains(addedResource));
        }
    }

    public abstract class global_dictionaries_context : ArrangeActAssert
    {
        protected Dictionary<string, ResourceDictionary> AddedDictionaries
           = new Dictionary<string, ResourceDictionary>()
                  {
                      {"dictionary1", new ResourceDictionary()},
                      {"dictionary2", new ResourceDictionary()},
                      {"dictionary3", new ResourceDictionary()},
                  };

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.Clear();
            foreach (var entry in AddedDictionaries)
            {
                GlobalResources.Add(entry.Key, entry.Value, false);
            }
        }
    }

    [TestClass]
    public class when_attaching_multiple_dictionaries : global_dictionaries_context
    {
        private FrameworkElement frameworkElement;

        protected override void Act()
        {
            frameworkElement = new FrameworkElement();

            ConfigurationResources.SetMergedDictionaries(frameworkElement, "dictionary3;dictionary1;dictionary2");
        }

        [TestMethod]
        public void then_all_dictionaries_merged()
        {
            CollectionAssert.AreEquivalent(frameworkElement.Resources.MergedDictionaries.ToArray(), AddedDictionaries.Values.ToArray());
        }

        [TestMethod]
        public void then_order_is_preserved()
        {
            Assert.AreSame(AddedDictionaries["dictionary3"], frameworkElement.Resources.MergedDictionaries[0]);
            Assert.AreSame(AddedDictionaries["dictionary1"], frameworkElement.Resources.MergedDictionaries[1]);
            Assert.AreSame(AddedDictionaries["dictionary2"], frameworkElement.Resources.MergedDictionaries[2]);
        }
    }

    [TestClass]
    public class when_dictionary_list_changes : global_dictionaries_context
    {

        private FrameworkElement frameworkElement;

        protected override void Arrange()
        {
            base.Arrange();

            frameworkElement = new FrameworkElement();
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "dictionary3;dictionary1");
        }

        protected override void Act()
        {
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "dictionary2");
        }

        [TestMethod]
        public void then_removes_old_dictionaries()
        {
            Assert.IsFalse(frameworkElement.Resources.MergedDictionaries.Contains(AddedDictionaries["dictionary3"]));
            Assert.IsFalse(frameworkElement.Resources.MergedDictionaries.Contains(AddedDictionaries["dictionary1"]));
        }

        [TestMethod]
        public void then_adds_new_dictionaries()
        {
            Assert.IsTrue(frameworkElement.Resources.MergedDictionaries.Contains(AddedDictionaries["dictionary2"]));
        }
    }

    [TestClass]
    public class when_dictionary_list_changes_to_null : global_dictionaries_context
    {
        private FrameworkElement frameworkElement;

        protected override void Arrange()
        {
            base.Arrange();

            frameworkElement = new FrameworkElement();
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "dictionary3;dictionary1");
        }

        protected override void Act()
        {
            ConfigurationResources.SetMergedDictionaries(frameworkElement, null);
        }

        [TestMethod]
        public void then_removes_old_dictionaries()
        {
            Assert.IsFalse(frameworkElement.Resources.MergedDictionaries.Contains(AddedDictionaries["dictionary3"]));
            Assert.IsFalse(frameworkElement.Resources.MergedDictionaries.Contains(AddedDictionaries["dictionary1"]));
        }

        [TestMethod]
        public void then_element_has_no_merged_dictionaries()
        {
            Assert.AreEqual(0, frameworkElement.Resources.MergedDictionaries.Count());
        }
    }

    [TestClass]
    public class when_merging_to_element_with_existing_dictionaries : global_dictionaries_context
    {
        private FrameworkElement frameworkElement;
        private ResourceDictionary priorMergedDictionary;

        protected override void Arrange()
        {
            base.Arrange();
            frameworkElement = new FrameworkElement();
            priorMergedDictionary = new ResourceDictionary();
            frameworkElement.Resources.MergedDictionaries.Add(priorMergedDictionary);
        }

        protected override void Act()
        {
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "dictionary3");
            ConfigurationResources.SetMergedDictionaries(frameworkElement, "dictionary2");
        }

        [TestMethod]
        public void then_prior_dictionary_still_merged()
        {
            Assert.IsTrue(frameworkElement.Resources.MergedDictionaries.Contains(priorMergedDictionary));
        }
    }
}
