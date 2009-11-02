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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.Contexts
{
    /// <summary>
    /// A context class that sets up a standard, non-trivial configuration
    /// of EHAB.
    /// </summary>
    public abstract class ExceptionHandlingConfigurationSectionContext : ContainerContext
    {
        private DictionaryConfigurationSource configSource;

        protected const string Policy1Name = TestConfigurationBuilder.Policy1Name;
        protected const string Policy2Name = TestConfigurationBuilder.Policy2Name;

        protected readonly Type[] Policy1Types = TestConfigurationBuilder.Policy1Types;
        protected readonly Type[] Policy2Types = TestConfigurationBuilder.Policy2Types;

        protected ExceptionHandlingSettings ExceptionSettings
        {
            get
            {
                return (ExceptionHandlingSettings)configSource.GetSection(BlockSectionNames.ExceptionHandling);
            }
        }

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new TestConfigurationBuilder();
            configSource = new DictionaryConfigurationSource();
            builder.AddExceptionSettings()
                .Build(configSource);
        }
    }
}
