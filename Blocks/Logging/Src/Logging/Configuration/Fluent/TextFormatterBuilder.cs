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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{

    /// <summary>
    /// Builder class used to configure a <see cref="TextFormatter"/> instance.
    /// </summary>
    /// <seealso cref="TextFormatter"/>
    /// <seealso cref="TextFormatterData"/>
    public class TextFormatterBuilder : ITextFormatterBuilder, IFormatterBuilder, IFluentInterface
    {
        TextFormatterData formatterData = new TextFormatterData();


        internal TextFormatterBuilder(string name)
        {
            formatterData.Name = name;
        }

        /// <summary>
        /// Specifies the text template that should be used when formatting a log message.
        /// </summary>
        /// <param name="template">The text template that should be used when formatting a log message.</param>
        /// <seealso cref="TextFormatter"/>
        /// <seealso cref="TextFormatterData"/>
        public IFormatterBuilder UsingTemplate(string template)
        {
            if (string.IsNullOrEmpty(template))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "template");

            formatterData.Template = template;
            return this;
        }

        FormatterData IFormatterBuilder.GetFormatterData()
        {
            return formatterData;
        }

        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetHashCode()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Redeclaration that hides the <see cref="object.ToString()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Redeclaration that hides the <see cref="object.Equals(object)"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

}
