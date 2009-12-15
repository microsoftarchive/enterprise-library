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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_validation_configuration_section
{

    [TestClass]
    public class when_creating_validation_view_model : given_validation_configuration
    {
        SectionViewModel validationModel;

        protected override void Act()
        {
            validationModel = SectionViewModel.CreateSection(Container, ValidationSettings.SectionName, ValidationSection);
        }

        //[TestMethod]
        //public void then_type_references_are_in_first_column()
        //{
        //    var typeReferenceElements = validationModel.GetDescendentsOfType<ValidatedTypeReference>();
        //    Assert.IsTrue(typeReferenceElements.Any());
        //    Assert.IsFalse(typeReferenceElements.Where(x => x.Column != 0).Any());
        //}

        //[TestMethod]
        //public void then_rulsets_are_in_second_column()
        //{
        //    var ruleSetElements = validationModel.GetDescendentsOfType<ValidationRulesetData>();
        //    Assert.IsTrue(ruleSetElements.Any());
        //    Assert.IsFalse(ruleSetElements.Where(x => x.Column != 1).Any());
        //}

        //[TestMethod]
        //public void then_targets_are_in_third_column()
        //{
        //    var targetElements = validationModel.GetDescendentsOfType<ValidatedMemberReference>();
        //    Assert.IsTrue(targetElements.Any());
        //    Assert.IsFalse(targetElements.Where(x => x.Column != 2).Any());
        //}

        //[TestMethod]
        //public void then_targets_are_shown()
        //{
        //    var targetElements = validationModel.GetDescendentsOfType<ValidatedMemberReference>();
        //    Assert.IsTrue(targetElements.Any());
        //    Assert.IsFalse(targetElements.Where(x => !x.IsShown).Any());
        //}

        //[TestMethod]
        //public void then_each_validation_ruleset_has_a_this_validation_target_next_to_it()
        //{
        //    var ruleSets = validationModel.GetDescendentsOfType<ValidationRulesetData>();
        //    foreach (var typeReference in ruleSets)
        //    {
        //        var visualNextToElement = validationModel.DescendentElements(x => x.Column == typeReference.Column + 1 && x.Row == typeReference.Row).FirstOrDefault();
        //        Assert.IsNotNull(visualNextToElement);
        //        Assert.AreEqual(typeof(ValidatorDataCollection), visualNextToElement.ConfigurationType);
        //    }   
        //}

        //[TestMethod]
        //public void then_validators_are_in_fourth_column_and_up()
        //{
        //    var validatorElements = validationModel.GetDescendentsOfType<ValidatorData>();
        //    Assert.IsTrue(validatorElements.Any());
        //    Assert.IsFalse(validatorElements.Where(x => !(x.Column >= 3)).Any());
        //}
    }
}
