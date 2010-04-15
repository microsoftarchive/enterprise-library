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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    ///<summary>
    /// Provides <see cref="WrapHandlerData"/> configuration extensions to the ExceptionHandling fluent configuration interface.
    ///</summary>
    public static class WrapWithHandlerLoggingConfigurationSourceBuilderExtensions
    {
        /// <summary>
        /// Wrap exception with the new exception type.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="Exception"/> to wrap existing exception with.</typeparam>
        /// <returns></returns>
        public static IExceptionConfigurationWrapWithProvider WrapWith<T>(this IExceptionConfigurationAddExceptionHandlers context) where T : Exception
        {
            return WrapWith(context, typeof (T));
        }

        /// <summary>
        /// Wrap exception with the new exception type.
        /// </summary>
        /// <param name="context">The extension context for this handler extension.</param>
        /// <param name="wrappingExceptionType">Type of <see cref="Exception"/>to wrap existing exception with.</param>
        /// <returns></returns>
        public static IExceptionConfigurationWrapWithProvider WrapWith(this IExceptionConfigurationAddExceptionHandlers context, Type wrappingExceptionType)
        {
            if (wrappingExceptionType == null) 
                throw new ArgumentNullException("wrappingExceptionType");

            if (!typeof(Exception).IsAssignableFrom(wrappingExceptionType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustDeriveFromType, typeof(Exception)), "wrappingExceptionType");

            return new ExceptionConfigurationWrapHandlerBuilder(context, wrappingExceptionType);
        }


        private class ExceptionConfigurationWrapHandlerBuilder : ExceptionHandlerConfigurationExtension, IExceptionConfigurationWrapWithProvider
        {
            private readonly WrapHandlerData handlerData;
            
            public ExceptionConfigurationWrapHandlerBuilder(IExceptionConfigurationAddExceptionHandlers context, Type wrappingExceptionType)
                :base(context)
            {
                handlerData = new WrapHandlerData()
                                  {
                                      Name = wrappingExceptionType.FullName,
                                      WrapExceptionType = wrappingExceptionType
                                  };
                
                CurrentExceptionTypeData.ExceptionHandlers.Add(handlerData);
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling UsingMessage(string message)
            {
                handlerData.ExceptionMessage = message;
                return this;
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling UsingResourceMessage(Type resourceType, string resourceName)
            {
                handlerData.ExceptionMessageResourceType = resourceType.AssemblyQualifiedName;
                handlerData.ExceptionMessageResourceName = resourceName;
                return this;
            }
        }
    }
}
