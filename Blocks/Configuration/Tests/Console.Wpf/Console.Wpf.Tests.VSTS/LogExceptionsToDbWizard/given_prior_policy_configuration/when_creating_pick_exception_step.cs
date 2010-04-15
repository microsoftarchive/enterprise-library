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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_prior_policy_configuration
{
    [TestClass]
    public class when_creating_pick_exception_step : ExceptionHandlingConfiguredSourceModelContext
    {
        private PickExceptionStep step;

        protected override void Act()
        {
            step = Container.Resolve<PickExceptionStep>();
        }

        [TestMethod]
        public void then_policy_has_suggested_values_suggests_true()
        {
            Assert.IsTrue(step.Policy.HasSuggestedValues);
        }

        [TestMethod]
        public void then_policy_suggested_values_list_populated()
        {
            Assert.IsTrue(step.Policy.SuggestedValues.Any());
        }

        [TestMethod]
        public void then_suggested_values_match_policy_list()
        {
            var policyList =
                ConfigurationSourceModel.Sections.Where(x => x.ConfigurationType == typeof (ExceptionHandlingSettings)).
                    Single()
                    .GetDescendentsOfType<ExceptionPolicyData>().Select(e => e.Name).ToArray();

            CollectionAssert.AreEquivalent(policyList, step.Policy.SuggestedValues.ToArray());
                                
        }
    }
}
