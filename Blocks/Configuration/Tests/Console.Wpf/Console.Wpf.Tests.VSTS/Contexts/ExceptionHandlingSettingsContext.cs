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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;


namespace Console.Wpf.Tests.VSTS.DevTests.Contexts
{
    public class ExceptionHandlingSettingsContext : ContainerContext
    {
        protected internal ExceptionHandlingSettings Section { get; set; }
        protected internal IConfigurationSource Source { get; set; }

        protected override void Arrange()
        {
            base.Arrange();

            Source = new DictionaryConfigurationSource();

            ConfigurationSourceBuilder sourceBuilder = new ConfigurationSourceBuilder();
            sourceBuilder.ConfigureExceptionHandling()
                .GivenPolicyWithName("SomePolicy")
                    .ForExceptionType<Exception>()
                        .ReplaceWith<ApplicationException>()
                        .WrapWith<ArgumentException>()
                        .ThenThrowNewException()
                    .ForExceptionType<ArithmeticException>()
                        .LogToCategory("ArithmicExceptions")
                        .ThenDoNothing()
                .GivenPolicyWithName("Global Policy")
                    .ForExceptionType<Exception>()
                        .ReplaceWith<ApplicationException>()
                        .UsingMessage("replacement message")
                        .WrapWith<Exception>()
                        .ThenNotifyRethrow()
                    .ForExceptionType<InvalidCastException>()
                        .WrapWith<ApplicationException>()
                        .WrapWith<Exception>()
                        .UsingMessage("yes, thats a known bug")
                        .ThenThrowNewException();

            sourceBuilder.UpdateConfigurationWithReplace(Source);

            Section = (ExceptionHandlingSettings)Source.GetSection(ExceptionHandlingSettings.SectionName);
        }
    }
}
