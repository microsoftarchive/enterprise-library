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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents a custom configuration node for injector objects.
    /// </summary>
    public class CustomInjectorNode : InjectorNode
    {
        readonly List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        string typeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomInjectorNode"/> class.
        /// </summary>
        public CustomInjectorNode()
            : this(new CustomInjectorData(Resources.CustomInjectorNodeName)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomInjectorNode"/> class.
        /// </summary>
        public CustomInjectorNode(CustomInjectorData injectorData)
            : base(injectorData)
        {
            typeName = injectorData.TypeName;
            foreach (string key in injectorData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, injectorData.Attributes[key]));
            }
        }

        /// <summary>
        /// Gets the list of custom attributes for the represented configuration object.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("InjectorExtensionsDescription", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get { return editableAttributes; }
        }

        /// <summary>
        /// Gets or sets the name of the type of the custom rule for the represented configuration object.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(PolicyInjector), typeof(CustomInjectorData))]
        [SRDescription("InjectorTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Returs the represented <see cref="CustomMatchingRuleData"/> instance.
        /// </summary>
        /// <returns>A newly created <see cref="CustomMatchingRuleData"/> instance.</returns>
        public override InjectorData GetConfigurationData()
        {
            CustomInjectorData injectorData = new CustomInjectorData(Name, typeName);
            foreach (EditableKeyValue kv in editableAttributes)
            {
                injectorData.Attributes.Add(kv.Key, kv.Value);
            }
            return injectorData;
        }
    }
}