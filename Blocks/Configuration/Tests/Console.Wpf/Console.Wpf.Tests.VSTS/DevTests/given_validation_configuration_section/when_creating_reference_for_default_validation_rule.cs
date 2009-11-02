using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.ViewModel;
using System.ComponentModel.Design;

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
            validationModel = SectionViewModel.CreateSection(Container, ValidationSettings.SectionName, ValidationSection);
            stringTypeReference = validationModel.DescendentElements(x => x.ConfigurationType == typeof(ValidatedTypeReference)).First();
        }

        [TestMethod]
        public void then_suggested_values_doesnt_contain_rules_contained_in_other_types()
        {
            var defaultRuleSetProperty = stringTypeReference.Property("DefaultRuleset");
            Assert.AreEqual(1, defaultRuleSetProperty.BindableSuggestedValues.Count());
            Assert.IsFalse(defaultRuleSetProperty.BindableSuggestedValues.Any( x=> x == "int-rules"));
        }
    }
}
