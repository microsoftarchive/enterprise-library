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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    [ResourceDescription(typeof(DesignResources), "TextFormatterDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TextFormatterDataDisplayName")]
    partial class TextFormatterData
    {
        private const string templateProperty = "template";

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatterData"/> class with default values.
        /// </summary>
        public TextFormatterData()
        {
            Type = typeof(TextFormatter);
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a template.
        /// </summary>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        public TextFormatterData(string templateData)
            : this("unnamed", templateData)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        public TextFormatterData(string name, string templateData)
            : this(name, typeof(TextFormatter), templateData)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        /// <param name="formatterType">
        /// The type of the formatter.
        /// </param>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        private TextFormatterData(string name, Type formatterType, string templateData)
            : base(name, formatterType)
        {
            this.Template = templateData;
        }


        /// <summary>
        /// Gets or sets the template containing tokens to replace.
        /// </summary>
        [ConfigurationProperty(templateProperty, IsRequired = true, DefaultValue = DefaultTemplate)]
        [ResourceDescription(typeof(DesignResources), "TextFormatterDataTemplateDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TextFormatterDataTemplateDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TemplateEditor, CommonDesignTime.EditorTypes.UITypeEditor)]
        public string Template
        {
            get
            {
                return (string)this[templateProperty];
            }
            set
            {
                this[templateProperty] = value;
            }
        }
    }
}
