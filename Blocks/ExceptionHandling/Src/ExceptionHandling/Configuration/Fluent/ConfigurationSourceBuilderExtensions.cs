//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationSourceBuilder"/> extensions to support creation of exception handling configuration sections.
    /// </summary>
    public static class ExceptionHandlingConfigurationSourceBuilderExtensions
    {

        /// <summary>
        /// Main entry point to configuration a <see cref="ExceptionHandlingSettings"/> section.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns></returns>
        public static IExceptionConfigurationGivenPolicyWithName ConfigureExceptionHandling(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ExceptionPolicyBuilder(configurationSourceBuilder);
        }


        private class ExceptionPolicyBuilder :
            IExceptionConfigurationGivenPolicyWithName,
            IExceptionConfigurationForExceptionType,
            IExceptionConfigurationAddExceptionHandlers,
            IExceptionConfigurationThenDoPostHandlingAction,
            IExceptionConfigurationForExceptionTypeOrPostHandling,
            IExceptionHandlerExtension
        {
            readonly ExceptionHandlingSettings section = new ExceptionHandlingSettings();
            ExceptionPolicyData currentPolicy;
            ExceptionTypeData currentExceptionTypeData;

            internal ExceptionPolicyBuilder(IConfigurationSourceBuilder configurationSourceBuilder)
            {
                configurationSourceBuilder.AddSection(ExceptionHandlingSettings.SectionName, section);
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationGivenPolicyWithName.GivenPolicyWithName(string name)
            {
                if (string.IsNullOrEmpty(name)) 
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "name");

                currentPolicy = new ExceptionPolicyData(name);
                section.ExceptionPolicies.Add(currentPolicy);
                return this;
            }

            
            IExceptionConfigurationAddExceptionHandlers IExceptionConfigurationForExceptionType.ForExceptionType(Type exceptionType)
            {
                if (exceptionType == null)
                    throw new ArgumentNullException("exceptionType");

                if (!typeof(Exception).IsAssignableFrom(exceptionType))
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Resources.ExceptionTypeMustDeriveFromType, typeof(Exception)), "exceptionType");

                currentExceptionTypeData = new ExceptionTypeData();

                currentExceptionTypeData.Name = exceptionType.FullName;
                currentExceptionTypeData.Type = exceptionType;
                currentPolicy.ExceptionTypes.Add(currentExceptionTypeData);

                return this;
            }

            IExceptionConfigurationAddExceptionHandlers IExceptionConfigurationForExceptionType.ForExceptionType<T>()
            {
                return ((IExceptionConfigurationForExceptionType) this).ForExceptionType(typeof (T));
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenDoNothing()
            {
                currentExceptionTypeData.PostHandlingAction = PostHandlingAction.None;

                return this;
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenNotifyRethrow()
            {
                currentExceptionTypeData.PostHandlingAction = PostHandlingAction.NotifyRethrow;

                return this;
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenThrowNewException()
            {
                currentExceptionTypeData.PostHandlingAction = PostHandlingAction.ThrowNewException;

                return this;
            }
           
            ExceptionTypeData IExceptionHandlerExtension.CurrentExceptionTypeData
            {
                get { return currentExceptionTypeData; }
            }
        }
    }
}
