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
    /// Represents a design time representation of a <see cref="ReplaceHandlerData"/> configuration element.
    /// </summary>
    public sealed class ReplaceHandlerNode : ExceptionHandlerNode
    {
        private string message;
        private string messageResourceName;
        private string messageResourceType;
        private string typeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReplaceHandlerNode"/> class.
        /// </summary>
        public ReplaceHandlerNode()
            : this(new ReplaceHandlerData(Resources.DefaultReplaceHandlerNodeName, string.Empty, string.Empty))
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ReplaceHandlerNode"/> class with a <see cref="ReplaceHandlerData"/> instance.
        /// </summary>
        /// <param name="replaceHandlerData">A <see cref="ReplaceHandlerData"/> instance</param>
        public ReplaceHandlerNode(ReplaceHandlerData replaceHandlerData)
        {
            if (null == replaceHandlerData) throw new ArgumentNullException("replaceHandlerData");
            Rename(replaceHandlerData.Name);

            this.message = replaceHandlerData.ExceptionMessage;
            this.typeName = replaceHandlerData.ReplaceExceptionTypeName;
            this.messageResourceType = replaceHandlerData.ExceptionMessageResourceType;
            this.messageResourceName = replaceHandlerData.ExceptionMessageResourceName;
        }

        /// <summary>
        /// Gets or sets the exception message to use.
        /// </summary>
        /// <value>
        /// The exception message to use.
        /// </value>
        [SRDescription("ExceptionReplaceMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return message; }
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
        /// Gets or sets the <see cref="Type"/> of <see cref="Exception"/> to use for replacement.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of <see cref="Exception"/> to use for replacement.
        /// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [SRDescription("ExceptionReplaceTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ReplaceExceptionType
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Gets the <see cref="ReplaceHandlerData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="ReplaceHandlerData"/> this node represents.
        /// </value>
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                ReplaceHandlerData handlerData = new ReplaceHandlerData(Name, message, typeName);
                handlerData.ExceptionMessageResourceType = messageResourceType;
                handlerData.ExceptionMessageResourceName = messageResourceName;

                return handlerData;
            }
        }
    }
}