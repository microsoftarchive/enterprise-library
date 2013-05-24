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

namespace Console.Wpf.Tests.VSTS.TestSupport
{
    /// <summary>
    /// A helper class to encapsulate the configuration used in
    /// various tests.
    /// </summary>
    public class TestConfigurationBuilder
    {
        public const string Policy1Name = "Policy1";
        public const string Policy2Name = "Policy2";

        public static readonly Type[] Policy1Types = new[] { typeof(ArgumentOutOfRangeException), typeof(ArgumentNullException) };
        public static readonly Type[] Policy2Types = new[] { typeof(InvalidCastException) };

        private ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();

        public TestConfigurationBuilder AddExceptionSettings()
        {
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName(Policy1Name)
                    .ForExceptionType(Policy1Types[0])
                        .WrapWith<Exception>()
                        .ReplaceWith<InvalidOperationException>()
                        .ThenThrowNewException()
                    .ForExceptionType(Policy1Types[1])
                        .ReplaceWith<Exception>()
                        .ThenThrowNewException()
                .GivenPolicyWithName(Policy2Name)
                    .ForExceptionType(Policy2Types[0])
                        .WrapWith<InvalidOperationException>()
                        .ThenNotifyRethrow();

            return this;
        }

        public TestConfigurationBuilder AddLoggingSettings()
        {
            builder.ConfigureLogging()
                .WithOptions
                .FilterOnPriority("PriorityFilter")
                    .StartingWithPriority(10)
                    .UpToPriority(20)
                .LogToCategoryNamed(DefaultCategoryName)
                    .WithOptions.SetAsDefaultCategory()
                    .SendTo.EventLog("EventLogListener")
                    .ToLog("Application");

            return this;
        }

        public void Build(IConfigurationSource source)
        {
            builder.UpdateConfigurationWithReplace(source);
        }

        public const string DefaultCategoryName = "General";
    }
}
