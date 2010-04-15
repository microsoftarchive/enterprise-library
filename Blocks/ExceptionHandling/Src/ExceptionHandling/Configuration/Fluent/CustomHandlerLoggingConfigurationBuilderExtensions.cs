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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Defines configuration extensions to <see cref="IExceptionConfigurationAddExceptionHandlers"/> for <see cref="ReplaceHandler"/>
    /// configuration.
    /// </summary>
    public static class CustomHandlerLoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Handle the <see cref="Exception"/> with a custom exception handler.
        /// </summary>
        /// <param name="context">Interface to extend to add custom handler options.</param>
        /// <param name="customHandlerType">The <see cref="Type"/> of the custom handler.
        /// <remarks>This must derive from <see cref="IExceptionHandler"/></remarks></param>
        /// <returns></returns>
        public static IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom(this IExceptionConfigurationAddExceptionHandlers context, Type customHandlerType)
        {
            return HandleCustom(context, customHandlerType, new NameValueCollection());
        }

        /// <summary>
        /// Handle the <see cref="Exception"/> with a custom exception handler.
        /// </summary>
        /// <param name="context">Interface to extend to add custom handler options.</param>
        /// <typeparam name="T">The Type of the custom handler.</typeparam>
        /// <returns></returns>
        public static IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom<T>(this IExceptionConfigurationAddExceptionHandlers context)
            where T : IExceptionHandler
        {
            return HandleCustom(context, typeof(T));
        }

        /// <summary>
        /// Handle the <see cref="Exception"/> with a custom exception handler.
        /// </summary>
        /// <param name="context">Interface to extend to add custom handler options.</param>
        /// <param name="customHandlerType">The <see cref="Type"/> of the custom handler.  </param>
        ///<param name="customHandlerSettings">Name-Value collection of attributes the custom handler can use to initialize itself.</param>
        ///<returns></returns>
        public static IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom(this IExceptionConfigurationAddExceptionHandlers context,
                                                                                        Type customHandlerType,
                                                                                        NameValueCollection customHandlerSettings)
        {
            if (customHandlerType == null) throw new ArgumentNullException("customHandlerType");
            if (customHandlerSettings == null) throw new ArgumentNullException("customHandlerSettings");

            if (!typeof(IExceptionHandler).IsAssignableFrom(customHandlerType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustDeriveFromType, typeof(IExceptionHandler)), "customHandlerType");


            return new ExceptionConfigurationCustomHandlerBuilder(context, customHandlerType, customHandlerSettings);
        }

        private class ExceptionConfigurationCustomHandlerBuilder : ExceptionHandlerConfigurationExtension
        {
            public ExceptionConfigurationCustomHandlerBuilder(IExceptionConfigurationAddExceptionHandlers context, 
                                                                Type customHandlerType, 
                                                                NameValueCollection customHandlerSettings)
                : base(context)
            {
                var customHandler = new CustomHandlerData
                {
                    Name = customHandlerType.FullName,
                    Type = customHandlerType
                };

                customHandler.Attributes.Add(customHandlerSettings);
                CurrentExceptionTypeData.ExceptionHandlers.Add(customHandler);
            }
        }
    }
}
