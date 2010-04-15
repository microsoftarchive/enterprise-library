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

using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Validation
{
    [TestClass]
    public class given_string_length_validator_with_lower_greater_than_upper : ContainerContext
    {
        protected SectionViewModel ValidationViewModel;
        protected override void Arrange()
        {
            base.Arrange();
            var section = new ValidationSettings()
            {
                Types =
                  {
                      new ValidatedTypeReference(typeof (given_string_length_validator_with_lower_greater_than_upper))
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
                                              new StringLengthValidatorData() { LowerBound = 10, UpperBound = 0, LowerBoundType = Microsoft.Practices.EnterpriseLibrary.Validation.Validators.RangeBoundaryType.Inclusive}
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

        [TestMethod]
        public void then_validator_throws_error_message()
        {
            ValidationViewModel.Validate();
            var lowerBoundProperty = ValidationViewModel.DescendentConfigurationsOfType<StringLengthValidatorData>().First().Properties.Single(p => p.PropertyName == "LowerBound");

            Assert.AreEqual(1, lowerBoundProperty.ValidationResults.Count());
        }
    }

    [TestClass]
    public class given_string_length_validator_with_ignore_lower_type : ContainerContext
    {
        protected SectionViewModel ValidationViewModel;
        protected override void Arrange()
        {
            base.Arrange();
            var section = new ValidationSettings()
            {
                Types =
                  {
                      new ValidatedTypeReference(typeof (given_string_length_validator_with_lower_greater_than_upper))
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
                                              new StringLengthValidatorData() { LowerBound = 10, UpperBound = 0, LowerBoundType = Microsoft.Practices.EnterpriseLibrary.Validation.Validators.RangeBoundaryType.Ignore}
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

        [TestMethod]
        public void then_validator_ignores_lower_bound()
        {
            ValidationViewModel.Validate();
            var lowerBoundProperty = ValidationViewModel.DescendentConfigurationsOfType<StringLengthValidatorData>().First().Properties.Single(p => p.PropertyName == "LowerBound");

            Assert.AreEqual(0, lowerBoundProperty.ValidationResults.Count());
        }
    }

    [TestClass]
    public class given_string_length_validator_with_ignore_upper_type : ContainerContext
    {
        protected SectionViewModel ValidationViewModel;
        protected override void Arrange()
        {
            base.Arrange();
            var section = new ValidationSettings()
            {
                Types =
                  {
                      new ValidatedTypeReference(typeof (given_string_length_validator_with_lower_greater_than_upper))
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
                                              new StringLengthValidatorData() { LowerBound = 10, UpperBound = 0, UpperBoundType = Microsoft.Practices.EnterpriseLibrary.Validation.Validators.RangeBoundaryType.Ignore}
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

        [TestMethod]
        public void then_validator_ignores_lower_bound()
        {
            ValidationViewModel.Validate();
            var lowerBoundProperty = ValidationViewModel.DescendentConfigurationsOfType<StringLengthValidatorData>().First().Properties.Single(p => p.PropertyName == "LowerBound");

            Assert.AreEqual(0, lowerBoundProperty.ValidationResults.Count());
        }
    }

    [TestClass]
    public class given_string_length_validator_with_upper_greater_than_lower : ContainerContext
    {
        protected SectionViewModel ValidationViewModel;
        protected override void Arrange()
        {
            base.Arrange();
            var section = new ValidationSettings()
            {
                Types =
                  {
                      new ValidatedTypeReference(typeof (given_string_length_validator_with_lower_greater_than_upper))
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
                                              new StringLengthValidatorData() { LowerBound = 10, UpperBound = 100}
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

        [TestMethod]
        public void then_validation_passes()
        {
            ValidationViewModel.Validate();
            var lowerBoundProperty = ValidationViewModel.DescendentConfigurationsOfType<StringLengthValidatorData>().First().Properties.Single(p => p.PropertyName == "LowerBound");

            Assert.AreEqual(0, lowerBoundProperty.ValidationResults.Count());
        }
    }
}
