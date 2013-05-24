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
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="FormatterBuilder"/> extensions to configure custom <see cref="ILogFormatter"/> instances.
    /// </summary>
    /// <seealso cref="ILogFormatter"/>
    /// <seealso cref="CustomFormatterData"/>
    public static class CustomFormatterBuilderExtensions
    {
        /// <summary>
        /// Adds an custom <see cref="ILogFormatter"/> instance of type <typeparamref name="TCustomFormatter"/> to the logging configuration.
        /// </summary>
        /// <typeparam name="TCustomFormatter">Concrete type of the custom <see cref="ILogFormatter"/> instance.</typeparam>
        /// <param name="builder">Fluent interface extension point.</param>
        /// <param name="formatterName">Name of the <see cref="ILogFormatter"/> instance added to configuration.</param>
        /// <seealso cref="ILogFormatter"/>
        /// <seealso cref="CustomFormatterData"/>
        public static CustomFormatterBuilder CustomFormatterNamed<TCustomFormatter>(this FormatterBuilder builder, string formatterName)
            where TCustomFormatter : ILogFormatter
        {
            return CustomFormatterNamed(builder, formatterName, typeof(TCustomFormatter), new NameValueCollection());
        }

        /// <summary>
        /// Adds an custom <see cref="ILogFormatter"/> instance of type <paramref name="customFormatterType"/> to the logging configuration.
        /// </summary>
        /// <param name="builder">Fluent interface extension point.</param>
        /// <param name="formatterName">Name of the <see cref="ILogFormatter"/> instance added to configuration.</param>
        /// <param name="customFormatterType">Concrete type of the custom <see cref="ILogFormatter"/> instance.</param>
        /// <seealso cref="ILogFormatter"/>
        /// <seealso cref="CustomFormatterData"/>
        public static CustomFormatterBuilder CustomFormatterNamed(this FormatterBuilder builder, string formatterName, Type customFormatterType)
        {
            return CustomFormatterNamed(builder, formatterName, customFormatterType, new NameValueCollection());
        }

        /// <summary>
        /// Adds an custom <see cref="ILogFormatter"/> instance of type <typeparamref name="TCustomFormatter"/> to the logging configuration.
        /// </summary>
        /// <typeparam name="TCustomFormatter">Concrete type of the custom <see cref="ILogFormatter"/> instance.</typeparam>
        /// <param name="builder">Fluent interface extension point.</param>
        /// <param name="formatterName">Name of the <see cref="ILogFormatter"/> instance added to configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomFormatter"/> when creating an instance.</param>
        /// <seealso cref="ILogFormatter"/>
        /// <seealso cref="CustomFormatterData"/>
        public static CustomFormatterBuilder CustomFormatterNamed<TCustomFormatter>(this FormatterBuilder builder, string formatterName, NameValueCollection attributes)
            where TCustomFormatter : ILogFormatter
        {
            return CustomFormatterNamed(builder, formatterName, typeof(TCustomFormatter), attributes);
        }

        /// <summary>
        /// Adds an custom <see cref="ILogFormatter"/> instance of type <paramref name="customFormatterType"/> to the logging configuration.
        /// </summary>
        /// <param name="builder">Fluent interface extension point.</param>
        /// <param name="formatterName">Name of the <see cref="ILogFormatter"/> instance added to configuration.</param>
        /// <param name="customFormatterType">Concrete type of the custom <see cref="ILogFormatter"/> instance.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customFormatterType"/> when creating an instance.</param>
        /// <seealso cref="ILogFormatter"/>
        /// <seealso cref="CustomFormatterData"/>
        public static CustomFormatterBuilder CustomFormatterNamed(this FormatterBuilder builder, string formatterName, Type customFormatterType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(formatterName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "formatterName");
            if (customFormatterType == null) throw new ArgumentNullException("customFormatterType");
            if (attributes == null) throw new ArgumentNullException("attributes");

            if (!typeof(ILogFormatter).IsAssignableFrom(customFormatterType))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionTypeMustImplementInterface, 
                        typeof(ILogFormatter)), 
                    "customFormatterType");
            }

            return new CustomFormatterBuilder(formatterName, customFormatterType, attributes);
        }
    }
}
