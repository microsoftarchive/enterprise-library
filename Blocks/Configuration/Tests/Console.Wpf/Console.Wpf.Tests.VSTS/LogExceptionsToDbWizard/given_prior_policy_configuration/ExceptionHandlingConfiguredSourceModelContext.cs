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

using System.Collections.Generic;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;
using System.Linq;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_prior_policy_configuration
{
    public abstract class ExceptionHandlingConfiguredSourceModelContext : ExceptionHandlingSettingsContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            this.ConfigurationSourceModel.AddSection(ExceptionHandlingSettings.SectionName, Section);

        }

        protected ConfigurationSourceModel ConfigurationSourceModel { get; private set; }

        public IEnumerable<ElementViewModel> GetDescendentsOfType<T>()
        {
            return ConfigurationSourceModel.Sections.SelectMany(s => s.GetDescendentsOfType<T>());
        }
    }
}
