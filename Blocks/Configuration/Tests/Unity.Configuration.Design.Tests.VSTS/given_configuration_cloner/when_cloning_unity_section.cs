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
using Console.Wpf.Tests.VSTS.BlockSpecific.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.IO;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_configuration_cloner
{
    [TestClass]
    [DeploymentItem("unity.config")]
    public class when_cloning_unity_section : given_unity_section
    {
        ConfigurationSectionCloner cloner;
        UnityConfigurationSection sectionToClone;
        UnityConfigurationSection clonedSection;

        protected override void Arrange()
        {
            base.Arrange();

            var configurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "unity.config");
            var source = new FileConfigurationSource(configurationFilePath);
            sectionToClone = (UnityConfigurationSection)source.GetSection("unity");

            //editableSection = (UnityConfigurationSection)base.UnitySectionViewModel.ConfigurationElement;
            //editableSection.TypeAliases.Add(new AliasElement("alias1", typeof(int))); 
            //editableSection.TypeAliases.Add(new AliasElement("alias2", typeof(string)));

            //editableSection.Containers.Add(new ContainerElement()
            //{
            //    Name = "cloned",
            //    Registrations = {{
            //    new RegisterElement
            //    {
            //        Name = "Registration1",
            //        TypeName = "RegistrationTypeName",
            //        InjectionMembers = {{
            //                            new PropertyElement 
            //                            {
            //                                Name = "Property1",
            //                                Value = new ValueElement
            //                                {
            //                                    Value = "PropertyValue"
            //                                }
            //                            }
            //                         },
            //                         {
            //                             new ConstructorElement 
            //                             {
            //                                 Parameters = 
            //                                 {{
            //                                      new ParameterElement
            //                                      {
            //                                          Name = "array",
            //                                          Value = new ArrayElement
            //                                          {
            //                                              TypeName = "t",
            //                                              Values = {{new ArrayElement()},{new ValueElement
            //                                                                             {
            //                                                                                 Value = "v"
            //                                                                             }}}
            //                                          }
            //                                      }
            //                                 }}
            //                             }
            //                         }}
            //    }}}
            //});


            cloner = new ConfigurationSectionCloner();
        }

        protected override void Act()
        {
            clonedSection = (UnityConfigurationSection) cloner.Clone(sectionToClone);
        }

        [TestMethod]
        public void then_aliases_are_cloned()
        {
            Assert.AreEqual(2, clonedSection.TypeAliases.Count);
            Assert.IsTrue(clonedSection.TypeAliases.Any(x => x.Alias == "alias1"));
            Assert.IsTrue(clonedSection.TypeAliases.Any(x => x.Alias == "alias2"));
        }

        [TestMethod]
        public void then_registration_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            Assert.IsTrue(container.Registrations.Any(x => x.Name == "Registration1"));
        }

        [TestMethod]
        public void then_injection_property_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            var registration = container.Registrations.First(x => x.Name == "Registration1");
            Assert.IsTrue(registration.InjectionMembers.OfType<PropertyElement>().Any(x=>x.Name == "Property1"));
        }

        [TestMethod]
        public void then_injection_property_value_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            var registration = container.Registrations.First(x => x.Name == "Registration1");
            var parameter = registration.InjectionMembers.OfType<PropertyElement>().First(x => x.Name == "Property1");
            
            Assert.IsTrue(parameter.Value is ValueElement);
            Assert.AreEqual("PropertyValue", ((ValueElement)parameter.Value).Value);
        }

        [TestMethod]
        public void then_injection_ctor_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            var registration = container.Registrations.First(x => x.Name == "Registration1");

            Assert.IsTrue(registration.InjectionMembers.OfType<ConstructorElement>().Any());
        }

        [TestMethod]
        public void then_injection_ctor_param_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            var registration = container.Registrations.First(x => x.Name == "Registration1");
            var constructor = registration.InjectionMembers.OfType<ConstructorElement>().First();

            Assert.IsTrue(constructor.Parameters.Any(x=>x.Name == "array"));
        }

        [TestMethod]
        public void then_injection_ctor_param_value_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            var registration = container.Registrations.First(x => x.Name == "Registration1");
            var constructor = registration.InjectionMembers.OfType<ConstructorElement>().First();
            var arrayParameter = constructor.Parameters.First(x => x.Name == "array");

            Assert.IsTrue(arrayParameter.Value is ArrayElement);
            Assert.AreEqual("t", ((ArrayElement)arrayParameter.Value).TypeName);
            Assert.AreEqual(2, ((ArrayElement)arrayParameter.Value).Values.Count);
        }

        [TestMethod]
        public void then_nested_array_value_is_cloned()
        {
            var container = clonedSection.Containers.Where(x => x.Name == "cloned").First();
            var registration = container.Registrations.First(x => x.Name == "Registration1");
            var constructor = registration.InjectionMembers.OfType<ConstructorElement>().First();
            var arrayParameter = constructor.Parameters.First(x => x.Name == "array");
            var arrayParameterValue = (ArrayElement)arrayParameter.Value;

            Assert.IsInstanceOfType(arrayParameterValue.Values[0], typeof(ArrayElement));
            Assert.IsInstanceOfType(arrayParameterValue.Values[1], typeof(ValueElement));

            Assert.AreEqual("v", ((ValueElement)arrayParameterValue.Values[1]).Value);
        }

    }
}
