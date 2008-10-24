//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that stores the configuration information
    /// for a call handler that doesn't have specific configuration console support..
    /// </summary>
    public class CustomCallHandlerNode : CallHandlerNode
    {
        List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        string typeName;

        /// <summary>
        /// Create a new <see cref="CustomCallHandlerNode"/> with default settings.
        /// </summary>
        public CustomCallHandlerNode()
            : this(new CustomCallHandlerData(Resources.CustomCallHandlerNodeName, string.Empty)) { }

        /// <summary>
        /// Create a new <see cref="CustomCallHandlerNode"/> with the supplied settings.
        /// </summary>
        /// <param name="callHandlerData">Settings read from configuration source for this handler.</param>
        public CustomCallHandlerNode(CustomCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            typeName = callHandlerData.TypeName;
            foreach (string key in callHandlerData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, callHandlerData.Attributes[key]));
            }
        }

        /// <summary>
        /// List of editable attributes that will be used to configure the call handler.
        /// </summary>
        /// <value>List of attributes.</value>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("HandlerProviderExtensionsDescription", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get { return editableAttributes; }
        }

        /// <summary>
        /// Specifies the type of call handler to create.
        /// </summary>
        /// <value>Type of call handler to create.</value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ICallHandler), typeof(CustomCallHandlerData))]
        [SRDescription("HandlerProviderTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Convert the data stored into this node into the corresponding
        /// configuration class (<see cref="CustomCallHandlerData"/>).
        /// </summary>
        /// <returns>Newly created <see cref="CustomCallHandlerData"/> containing
        /// the configuration data from this node.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            CustomCallHandlerData callHandlerData = new CustomCallHandlerData(Name, typeName, Order);
            foreach (EditableKeyValue kv in editableAttributes)
            {
                callHandlerData.Attributes.Add(kv.Key, kv.Value);
            }
            return callHandlerData;
        }
    }
}
