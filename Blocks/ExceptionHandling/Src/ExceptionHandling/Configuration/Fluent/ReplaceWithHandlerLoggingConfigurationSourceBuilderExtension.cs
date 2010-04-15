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
    public static class ReplaceWithHandlerLoggingConfigurationSourceBuilderExtensions
    {
        /// <summary>
        /// Replace exception with new exception type.
        /// </summary>
        /// <typeparam name="T">Replacement <see cref="Exception"/> type.</typeparam>
        /// <returns></returns>
        public static IExceptionConfigurationReplaceWithProvider ReplaceWith<T>(this IExceptionConfigurationAddExceptionHandlers context) where T: Exception
        {
            return ReplaceWith(context, typeof (T));
        }

        /// <summary>
        /// Replace exception with new exception type.
        /// </summary>
        /// <param name="context">Interface to extend to add ReplaceWith options.</param>
        /// <param name="replacingExceptionType">Replacement <see cref="Exception"/> type.</param>
        /// <returns></returns>
        public static IExceptionConfigurationReplaceWithProvider ReplaceWith(this IExceptionConfigurationAddExceptionHandlers context, Type replacingExceptionType)
        {
            if (replacingExceptionType == null) 
                throw new ArgumentNullException("replacingExceptionType");

            if (!typeof(Exception).IsAssignableFrom(replacingExceptionType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustDeriveFromType, typeof(Exception)), "replacingExceptionType");

            return new ExceptionConfigurationReplaceWithBuilder(context, replacingExceptionType);
        }

        private class ExceptionConfigurationReplaceWithBuilder : ExceptionHandlerConfigurationExtension, IExceptionConfigurationReplaceWithProvider
        {
            private readonly ReplaceHandlerData replaceHandlerData;

            public ExceptionConfigurationReplaceWithBuilder(IExceptionConfigurationAddExceptionHandlers context, Type replacingExceptionType) :
                base(context)
            {
                replaceHandlerData = new ReplaceHandlerData()
                                         {
                                             Name = replacingExceptionType.FullName,
                                             ReplaceExceptionType = replacingExceptionType
                                         };

                base.CurrentExceptionTypeData.ExceptionHandlers.Add(replaceHandlerData);
            }


            public IExceptionConfigurationForExceptionTypeOrPostHandling UsingMessage(string message)
            {
                replaceHandlerData.ExceptionMessage = message;
                return this;
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling UsingResourceMessage(Type resourceType, string resourceName)
            {
                replaceHandlerData.ExceptionMessageResourceType = resourceType.AssemblyQualifiedName;
                replaceHandlerData.ExceptionMessageResourceName = resourceName;
                return this;
            }

            public ReplaceHandlerData GetHandler()
            {
                return replaceHandlerData;
            }
        }
    }
}
