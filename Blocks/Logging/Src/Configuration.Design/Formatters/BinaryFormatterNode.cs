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
    /// Represents a <see cref="BinaryLogFormatterData"/> configuration element.
    /// </summary>
    public sealed class BinaryFormatterNode : FormatterNode
    {
        /// <summary>
		/// Initialize a new instance of the <see cref="BinaryFormatterNode"/> class.
        /// </summary>
        public BinaryFormatterNode()
            : this(new BinaryLogFormatterData(Resources.BinaryFormatterNode))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="BinaryFormatterNode"/> class with a <see cref="BinaryLogFormatterData"/> instance.
		/// </summary>
		/// <param name="binaryLogFormatter">A <see cref="BinaryLogFormatterData"/> instance.</param>
        public BinaryFormatterNode(BinaryLogFormatterData binaryLogFormatter)
            : base()
        {
			if (null == binaryLogFormatter) throw new ArgumentNullException("binaryLogFormatter");

			Rename(binaryLogFormatter.Name);
        }

		/// <summary>
		/// Gets the <see cref="BinaryLogFormatterData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="BinaryLogFormatterData"/> this node represents.
		/// </value>
		public override FormatterData FormatterData
		{
			get { return new BinaryLogFormatterData(Name); }
		}
    }
}
