//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="ILoggingConfigurationOptions"/> extensions to configure custom <see cref="ILogFilter"/> instances.
    /// </summary>
    /// <seealso cref="ILogFilter"/>
    /// <seealso cref="CustomLogFilterData"/>
    public static class CustomFilterBuilderExtensions
    {
        /// <summary>
        /// Adds an custom <see cref="ILogFilter"/> instance of type <typeparamref name="TCustomFilter"/> to the logging configuration.
        /// </summary>
        /// <typeparam name="TCustomFilter">Concrete type of the custom <see cref="ILogFilter"/> instance.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customFilterName">Name of the <see cref="ILogFilter"/> instance added to configuration.</param>
        /// <seealso cref="ILogFilter"/>
        /// <seealso cref="CustomLogFilterData"/>
        public static ILoggingConfigurationOptions FilterCustom<TCustomFilter>(this ILoggingConfigurationOptions context, string customFilterName)
            where TCustomFilter : ILogFilter
        {
            return FilterCustom(context, customFilterName, typeof(TCustomFilter), new NameValueCollection());
        }

        /// <summary>
        /// Adds an custom <see cref="ILogFilter"/> instance of type <paramref name="customFilterType"/> to the logging configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customFilterName">Name of the <see cref="ILogFilter"/> instance added to configuration.</param>
        /// <param name="customFilterType">Concrete type of the custom <see cref="ILogFilter"/> instance.</param>
        /// <seealso cref="ILogFilter"/>
        /// <seealso cref="CustomLogFilterData"/>
        public static ILoggingConfigurationOptions FilterCustom(this ILoggingConfigurationOptions context, string customFilterName, Type customFilterType)
        {
            return FilterCustom(context, customFilterName, customFilterType, new NameValueCollection());
        }

        /// <summary>
        /// Adds an custom <see cref="ILogFilter"/> instance of type <typeparamref name="TCustomFilter"/> to the logging configuration.
        /// </summary>
        /// <typeparam name="TCustomFilter">Concrete type of the custom <see cref="ILogFilter"/> instance.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customFilterName">Name of the <see cref="ILogFilter"/> instance added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomFilter"/> when creating an instance.</param>
        /// <seealso cref="ILogFilter"/>
        /// <seealso cref="CustomLogFilterData"/>
        public static ILoggingConfigurationOptions FilterCustom<TCustomFilter>(this ILoggingConfigurationOptions context, string customFilterName, NameValueCollection attributes)
            where TCustomFilter : ILogFilter
        {
            return FilterCustom(context, customFilterName, typeof(TCustomFilter), attributes);
        }

        /// <summary>
        /// Adds an custom <see cref="ILogFilter"/> instance of type <paramref name="customFilterType"/> to the logging configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="customFilterName">Name of the <see cref="ILogFilter"/> instance added to configuration.</param>
        /// <param name="customFilterType">Concrete type of the custom <see cref="ILogFilter"/> instance.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customFilterType"/> when creating an instance.</param>
        /// <seealso cref="ILogFilter"/>
        /// <seealso cref="CustomLogFilterData"/>
        public static ILoggingConfigurationOptions FilterCustom(this ILoggingConfigurationOptions context, string customFilterName, Type customFilterType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(customFilterName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "customFilterName");

            if (customFilterType == null)
                throw new ArgumentNullException("customFilterType");

            if (attributes == null)
                throw new ArgumentNullException("attributes");

            if (!typeof(ILogFilter).IsAssignableFrom(customFilterType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustImplementInterface, typeof(ILogFilter)), "customFilterType");

            var builder = new FilterCustomBuilder(context, customFilterName, customFilterType, attributes);
            return context;
        }

        private class FilterCustomBuilder : LoggingConfigurationExtension
        {
            public FilterCustomBuilder(ILoggingConfigurationOptions context, string logFilterName, Type customFilterType, NameValueCollection attributes)
                : base(context)
            {
                CustomLogFilterData customFilter = new CustomLogFilterData
                {
                    Name = logFilterName,
                    Type = customFilterType
                };
                customFilter.Attributes.Add(attributes);

                base.LoggingSettings.LogFilters.Add(customFilter);
            }
        }
    }
}
