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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Represents a design time representation of a <see cref="WrapHandlerData"/> configuration element.
    /// </summary>
    public sealed class WrapHandlerNode : ExceptionHandlerNode
    {
        private string message;
        private string messageResourceName;
        private string messageResourceType;
        private string typeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="WrapHandlerNode"/> class.
        /// </summary>
        public WrapHandlerNode()
            : this(new WrapHandlerData(Resources.DefaultWrapHandlerNodeName, string.Empty, string.Empty))
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="WrapHandlerNode"/> class with a <see cref="WrapHandlerData"/> instance.
        /// </summary>
        /// <param name="wrapHandlerData">A	<see cref="WrapHandlerData"/> instance</param>
        public WrapHandlerNode(WrapHandlerData wrapHandlerData)
        {
            if (null == wrapHandlerData) throw new ArgumentNullException("wrapHandlerData");

            Rename(wrapHandlerData.Name);
            this.message = wrapHandlerData.ExceptionMessage;
            this.typeName = wrapHandlerData.WrapExceptionTypeName;
            this.messageResourceType = wrapHandlerData.ExceptionMessageResourceType;
            this.messageResourceName = wrapHandlerData.ExceptionMessageResourceName;
        }

        /// <summary>
        /// Gets or sets the exception message to use.
        /// </summary>
        /// <value>
        /// The exception message to use.
        /// </value>
        [SRDescription("WrapHandlerNodeMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return message; ; }
            set { message = value; }
        }

        /// <summary>
        /// !~!
        /// </summary>
        [SRDescription("ExceptionMessageResourceNameDescription", typeof(Resources))]
        [SRCategory("CategoryLocalization", typeof(Resources))]
        public string ExceptionMessageResourceName
        {
            get { return messageResourceName; }
            set { messageResourceName = value; }
        }

        /// <summary>
        /// !~!
        /// </summary>
        [SRDescription("ExceptionMessageTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryLocalization", typeof(Resources))]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Object), TypeSelectorIncludes.None)]
        public string ExceptionMessageResourceType
        {
            get { return messageResourceType; }
            set { messageResourceType = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of <see cref="Exception"/> to use for wrapping.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of <see cref="Exception"/> to use for wrapping.
        /// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [SRDescription("ExceptionWrapTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string WrapExceptionType
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Gets the <see cref="WrapHandlerData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="WrapHandlerData"/> this node represents.
        /// </value>
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                WrapHandlerData handlerData = new WrapHandlerData(Name, message, typeName);
                handlerData.ExceptionMessageResourceName = messageResourceName;
                handlerData.ExceptionMessageResourceType = messageResourceType;

                return handlerData;
            }
        }
    }
}
