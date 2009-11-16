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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;

namespace Console.Wpf.Tests.VSTS.DevTests.given_piab_configuration
{

    [TestClass]
    public class when_creating_section_view_model_for_piab : ContainerContext
    {
        PolicyInjectionSettings piabSettings;

        protected override void Arrange()
        {
            base.Arrange();

            piabSettings = new PolicyInjectionSettings();

            piabSettings.Policies.Add(new PolicyData
                                        {
                                            Name = "policy1",

                                            MatchingRules = new NameTypeConfigurationElementCollection<MatchingRuleData,CustomMatchingRuleData>
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

            piabSettings.Policies.Add(new PolicyData
                                        {
                                            Name = "policy2",

                                            MatchingRules = new NameTypeConfigurationElementCollection<MatchingRuleData,CustomMatchingRuleData>
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


        SectionViewModel piabViewModel;

        protected override void Act()
        {
            piabViewModel = SectionViewModel.CreateSection(Container, PolicyInjectionSettings.SectionName, piabSettings);
            piabViewModel.UpdateLayout();
        }

        [TestMethod]
        public void then_all_policies_end_up_in_second_column()
        {
            var policies = piabViewModel.GetDescendentsOfType<PolicyData>();
            Assert.IsTrue(policies.Any());
            Assert.IsFalse(policies.Where(x => x.Column != 1).Any());
        }
    }
}
