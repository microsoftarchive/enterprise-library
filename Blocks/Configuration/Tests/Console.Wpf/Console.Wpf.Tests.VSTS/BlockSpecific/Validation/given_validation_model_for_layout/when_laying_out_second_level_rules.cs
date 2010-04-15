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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Moq;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Validation.given_validation_model_for_layout
{
    public abstract class ValidationWithMultipleAndValidators : ContainerContext
    {
        protected SectionViewModel ValidationViewModel;
        protected readonly string DeepestRuleValidatorName = "AndCompositeValidator2";

        protected override void Arrange()
        {
            base.Arrange();

            var assemblyDiscoveryService = new Mock<IAssemblyDiscoveryService>();
            Container.RegisterInstance(assemblyDiscoveryService.Object);

            var section = new ValidationSettings()
            {
                Types =
                  {
                      new ValidatedTypeReference(typeof (when_laying_out_second_level_rules))
                      {
                          Rulesets =
                          {
                              new ValidationRulesetData("ruleSet")
                              {
                                  Validators =
                                  {
                                      new AndCompositeValidatorData("AndComposite1")
                                      {
                                          Validators =
                                          {
                                              new AndCompositeValidatorData(DeepestRuleValidatorName)
                                          }
                                      }
                                  }
                              }
                          }
                      }
                  }

            };

            ValidationViewModel = SectionViewModel.CreateSection(Container, ValidationSettings.SectionName, section);
            Container.Resolve<ElementLookup>().AddSection(ValidationViewModel);
        }       
    }

    public abstract class ValidationSectionLayoutContext : ValidationWithMultipleAndValidators
    {
        protected HorizontalListLayout Layout { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();
            Layout = (HorizontalListLayout)ValidationViewModel.Bindable;
        }

        protected HorizontalListLayout FindHorizontalList(string columnName)
        {
            return FindHorizontalList(this.Layout, h => h.ColumnName == columnName);
        }

        private HorizontalListLayout FindHorizontalList(HorizontalListLayout root, Predicate<HorizontalListLayout> predicate)
        {
            if (root == null) return null;

            if (predicate(root))
                return root;

            return FindHorizontalList(root.Next, predicate);
        }
    }

    [TestClass]
    public class when_laying_out_second_level_rules : ValidationSectionLayoutContext
    {
     
        [TestMethod]
        public void then_deepest_column_splitter_is_visible()
        {
            var walker = new ElementListLayoutWalker(Layout.Contained);
            var deepestRule = ValidationViewModel.DescendentConfigurationsOfType<AndCompositeValidatorData>().Where(
                                    x => string.Equals(x.NameProperty.Value, DeepestRuleValidatorName)).First();

            var layoutElement = walker.LayoutElements().OfType<TwoColumnsLayout>().Where(l => l.Left == deepestRule).First();
            Assert.IsTrue(FindHorizontalList(layoutElement.ColumnName).CanResize);
        }

        [TestMethod]
        public void then_next_deepest_header_column_is_not_visible()
        {
            var walker = new ElementListLayoutWalker(Layout.Contained);
            var deepestRule = ValidationViewModel.DescendentConfigurationsOfType<AndCompositeValidatorData>().Where(
                                    x => string.Equals(x.NameProperty.Value, DeepestRuleValidatorName)).First();

            var layoutElement = walker.LayoutElements().OfType<TwoColumnsLayout>().Where(l => l.Left == deepestRule).First();
            var nextDeepestHeader = FindHorizontalList(layoutElement.ColumnName).Next;
            Assert.IsFalse(nextDeepestHeader.CanResize);
        }
    }

    [TestClass]
    public class when_adding_another_rule_level : ValidationSectionLayoutContext
    {
        private ElementViewModel deepestRule;
        private ElementViewModel newDeepestRule;
        private ElementListLayoutWalker walker;
        private PropertyChangedListener propertyChangedListener;

        protected override void Arrange()
        {
            base.Arrange();
            
            deepestRule = ValidationViewModel.DescendentConfigurationsOfType<AndCompositeValidatorData>()
                    .Where(x => string.Equals(x.NameProperty.Value, DeepestRuleValidatorName)).First();

            walker = new ElementListLayoutWalker(Layout.Contained);
            var currentDeepestLayout = walker.LayoutElements().OfType<TwoColumnsLayout>().Where(l => l.Left == deepestRule).First();
            var layoutHeader = FindHorizontalList(currentDeepestLayout.ColumnName);
            propertyChangedListener = new PropertyChangedListener(layoutHeader);
        }

        protected override void Act()
        {
            var validators = deepestRule.ChildElements.OfType<ElementCollectionViewModel>().Single();
            newDeepestRule = validators.AddNewCollectionElement(typeof(AndCompositeValidatorData));
        }

        [TestMethod]
        public void then_header_splitter_is_now_visible()
        {

            var layoutElement = walker.LayoutElements().OfType<TwoColumnsLayout>().Where(l => l.Left == newDeepestRule).First();
            var layoutHeader = FindHorizontalList(layoutElement.ColumnName);
            Assert.IsTrue(layoutHeader.CanResize);
        }

        [TestMethod]
        public void then_header_splitter_notifies_property_changed()
        {
            Assert.IsTrue(propertyChangedListener.ChangedProperties.Contains("CanResize"));
        }
    }

    [TestClass]
    public class when_removing_deepest_rule_level_without_header : ValidationSectionLayoutContext
    {
        private ElementListLayoutWalker walker;
        private PropertyChangedListener propertyChangedListener;
        private ElementCollectionViewModel ruleCollection;
        private HorizontalListLayout layoutHeader;

        protected override void Arrange()
        {
            base.Arrange();

            var deepestRule = ValidationViewModel.DescendentConfigurationsOfType<AndCompositeValidatorData>()
                    .Where(x => string.Equals(x.NameProperty.Value, DeepestRuleValidatorName)).First();

            ruleCollection = (ElementCollectionViewModel) deepestRule.ParentElement;

            walker = new ElementListLayoutWalker(Layout.Contained);
            var currentDeepestLayout = walker.LayoutElements().OfType<TwoColumnsLayout>().Where(l => l.Left == deepestRule).First();
            layoutHeader = FindHorizontalList(currentDeepestLayout.ColumnName);
            propertyChangedListener = new PropertyChangedListener(layoutHeader);
        }

        protected override void Act()
        {
            var children = ruleCollection.ChildElements.OfType<CollectionElementViewModel>().ToArray();
            foreach(var child in children)
            {
                ruleCollection.Delete(child);
            }
        }

        [TestMethod]
        public void then_layout_header_notifies_of_splitter_change()
        {
            Assert.IsTrue(propertyChangedListener.ChangedProperties.Contains("CanResize"));
        }

        [TestMethod]
        public void then_layout_header_now_longer_shows_splitter()
        {
            Assert.IsFalse(layoutHeader.CanResize);
        }
    }
}
