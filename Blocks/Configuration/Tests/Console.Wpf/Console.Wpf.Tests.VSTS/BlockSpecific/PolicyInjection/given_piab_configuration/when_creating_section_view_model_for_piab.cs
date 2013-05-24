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
using System.Configuration;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.PolicyInjection.given_piab_configuration
{
    public abstract class PiabSectionContext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            PiabSettings = new PolicyInjectionSettings();

            PiabSettings.Policies.Add(new PolicyData
            {
                Name = "policy1",

                MatchingRules = new NameTypeConfigurationElementCollection<MatchingRuleData, CustomMatchingRuleData>
                                                                  {
                                                                      {new TypeMatchingRuleData("TypeMatch1")
                                                                           {
                                                                               Matches = new MatchDataCollection<MatchData>
                                                                                             {{
                                                                                                  new MatchData
                                                                                                      {
                                                                                                          IgnoreCase = true,
                                                                                                          Match = "Namespace"
                                                                                                      }
                                                                                                  }}
                                                                           }},
                                                                           {new PropertyMatchingRuleData("PropertyMatch1")
                                                                                {
                                                                                    Matches = new MatchDataCollection<PropertyMatchData>()
                                                                                    {
                                                                                        {
                                                                                            new PropertyMatchData()
                                                                                                {
                                                                                                    Match = "Property",
                                                                                                    IgnoreCase = true,
                                                                                                    MatchOption = PropertyMatchingOption.GetOrSet
                                                                                                }
                                                                                    }}
                                                                                }},

                                                                                {new ParameterTypeMatchingRuleData("ParameterTypeMatchingRule1")
                                                                                     {
                                                                                         Matches = new MatchDataCollection<ParameterTypeMatchData>()
                                                                                         {
                                                                                            new ParameterTypeMatchData()
                                                                                                {
                                                                                                    Match = "ParameterType",
                                                                                                    IgnoreCase = false,
                                                                                                    ParameterKind =  ParameterKind.ReturnValue
                                                                                                }
                                                                                         }
                                                                                     }
                                                                                },
                                                                      {new NamespaceMatchingRuleData("NamespaceMatch1")
                                                                           {
                                                                               Matches = new MatchDataCollection<MatchData>
                                                                                             {{
                                                                                                  new MatchData
                                                                                                      {
                                                                                                          IgnoreCase = true,
                                                                                                          Match = "Type"
                                                                                                      }
                                                                                                  }}
                                                                           }}
                                                                  },

                Handlers = new NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData>
                                                             {{
                                                                  new ExceptionCallHandlerData("ExceptionHandler")
                                                             }}
            });

            PiabSettings.Policies.Add(new PolicyData
            {
                Name = "policy2",

                MatchingRules = new NameTypeConfigurationElementCollection<MatchingRuleData, CustomMatchingRuleData>
                                                                  {{
                                                                       new TypeMatchingRuleData("TypeMatch1")
                                                                           {
                                                                               Matches = new MatchDataCollection<MatchData>
                                                                                             {{
                                                                                                  new MatchData
                                                                                                      {
                                                                                                          IgnoreCase = true,
                                                                                                          Match = "Namespace"
                                                                                                      }
                                                                                                  }}
                                                                           }
                                                                       }},

                Handlers = new NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData>
                                                             {
                                                                 {new ExceptionCallHandlerData("ExceptionHandler")},
                                                                 {new LogCallHandlerData("LogHandler")}
                                                             }
            });

        }

        protected PolicyInjectionSettings PiabSettings { get; private set; }
    }

    [TestClass]
    public class when_creating_section_view_model_for_piab : PiabSectionContext
    {
        SectionViewModel piabViewModel;

        protected override void Act()
        {
            piabViewModel = SectionViewModel.CreateSection(Container, PolicyInjectionSettings.SectionName, PiabSettings);
        }

        [TestMethod]
        public void then_piab_section_view_model_created()
        {
            Assert.IsInstanceOfType(piabViewModel, typeof(PolicyInjectionSectionViewModel));
        }
    }


    [TestClass]
    public class when_creating_piab_view_model_with_matching_rules : PiabSectionContext
    {
        private SectionViewModel piabViewModel;

        protected override void Act()
        {
            piabViewModel = SectionViewModel.CreateSection(Container, PolicyInjectionSettings.SectionName, PiabSettings);
        }

        [TestMethod]
        public void then_match_data_have_custom_property_overrides()
        {
            AssertProperty<MatchData>(p => (p.PropertyName == "Match" || p.PropertyName == "IgnoreCase"));
        }

        [TestMethod]
        public void then_property_match_data_properties_has_overrides()
        {
            AssertProperty<PropertyMatchData>(p => p.PropertyName == "MatchOption");
        }

        [TestMethod]
        public void then_parameter_type_match_data_properties_have_overrides()
        {
            AssertProperty<ParameterTypeMatchData>(p => p.PropertyName == "ParameterKind");
        }

        /// <summary>Asserts all properties have applied custom view model.</summary>
        private void AssertProperty<TConfigurationElement>(Func<Property, bool> propertyFilter)
            where TConfigurationElement : ConfigurationElement
        {
            var matchDataEntries = piabViewModel.DescendentConfigurationsOfType<TConfigurationElement>();

            IEnumerable<Property> properties =
                matchDataEntries.SelectMany(
                    e => e.Properties.Where(propertyFilter));

            Assert.IsTrue(properties.Any(), "Could not find any properties to match filter");

            Assert.IsTrue(properties.All(p => typeof(CollectionEditorContainedElementProperty).IsAssignableFrom(p.GetType())));
        }
    }
}
