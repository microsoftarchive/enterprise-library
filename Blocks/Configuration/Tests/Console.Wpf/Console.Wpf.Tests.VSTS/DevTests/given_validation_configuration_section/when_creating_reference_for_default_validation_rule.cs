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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_validation_configuration_section
{
    public abstract class given_validation_configuration : Contexts.ContainerContext
    {
        protected ValidationSettings ValidationSection;
        
        protected override void Arrange()
        {
            base.Arrange();

            ValidationSection = new ValidationSettings();
            ValidationSection.Types.Add(new ValidatedTypeReference(typeof(String))
            {
                Rulesets = 
                {
                    { 
                        new ValidationRulesetData("string-rules")
                        {
                            Properties = 
                            {
                                new ValidatedPropertyReference("Length")
                                {
                                    Validators = 
                                    {
                                        new RangeValidatorData("RG-validator")
                                    }
                                }
                            },
                            Validators = 
                            {
                                new StringLengthValidatorData("SL-validator")
                            }
                        }
                    }
                }
            });
            ValidationSection.Types.Add(new ValidatedTypeReference(typeof(int))
            {
                Rulesets = 
                {
                    { 
                        new ValidationRulesetData("int-rules")
                        {
                            Properties = 
                            {
                                new ValidatedPropertyReference("Length")
                                {
                                    Validators = 
                                    {
                                        new RangeValidatorData("RG-validator")
                                    }
                                }
                            },
                            Validators = 
                            {
                                new StringLengthValidatorData("SL-validator")
                            }
                        }
                    }
                }
            });
        }
    }

    [TestClass]
    public class when_creating_reference_for_default_validation_rule : given_validation_configuration
    {
        SectionViewModel validationModel;
        ElementViewModel stringTypeReference;

        protected override void Act()
        {
            var configurationSource = Container.Resolve<ConfigurationSourceModel>();
            validationModel = configurationSource.AddSection(ValidationSettings.SectionName, ValidationSection);
            stringTypeReference = validationModel.DescendentElements(x => x.ConfigurationType == typeof(ValidatedTypeReference)).First();
        }

        [TestMethod]
        public void then_suggested_values_doesnt_contain_rules_contained_in_other_types()
        {
            var defaultRuleSetProperty = stringTypeReference.Property("DefaultRuleset");
            Assert.AreEqual(2, ((SuggestedValuesBindableProperty)defaultRuleSetProperty.BindableProperty).BindableSuggestedValues.Count());
            Assert.IsFalse(((SuggestedValuesBindableProperty)defaultRuleSetProperty.BindableProperty).BindableSuggestedValues.Any( x=> x == "int-rules"));
        }

        [TestMethod]
        public void then_type_name_does_not_include_qualified_full_name()
        {
            var type = validationModel.ChildElements.First().ChildElements.First();

            Assert.AreEqual(typeof(string).Name, type.Name);
        }
    }
}
