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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    /// <summary>
    /// Represents a <see cref="TextFormatterData"/> configuration object.
    /// </summary>
    public sealed class TextFormatterNode : FormatterNode
    {
        private string template;

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterNode"/> class.
        /// </summary>
        public TextFormatterNode() 
            :this(new TextFormatterData(Resources.TextFormatterNode, DefaultValues.TextFormatterFormat))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="TextFormatterNode"/> class with a <see cref="TextFormatterData"/> instance.
		/// </summary>
		/// <param name="textFormatterData">A <see cref="TextFormatterData"/> instance.</param>
        public TextFormatterNode(TextFormatterData textFormatterData) 
        {
			if (null == textFormatterData) throw new ArgumentNullException("textFormatterData");

            this.template = textFormatterData.Template;
			Rename(textFormatterData.Name);
        }

        /// <summary>
        /// Gets the <see cref="Template"/> for the formatter.
        /// </summary>
		/// <value>
		/// The <see cref="Template"/> for the formatter.
		/// </value>
        [Required, Editor(typeof(TemplateEditor), typeof(UITypeEditor))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public Template Template
        {
            get { return new Template(template); }
            set { template = value.Text; }
        }

		/// <summary>
		/// Gets the <see cref="TextFormatterData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="TextFormatterData"/> this node represents.
		/// </value>
		public override FormatterData  FormatterData
		{
			get { return new TextFormatterData(Name, template); }
		}
    }
}
