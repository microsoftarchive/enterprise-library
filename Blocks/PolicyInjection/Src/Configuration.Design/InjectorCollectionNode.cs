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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents a configuration node for a injector collection.
    /// </summary>
    [Image(typeof(InjectorCollectionNode))]
    [SelectedImage(typeof(InjectorCollectionNode))]
    public class InjectorCollectionNode : ConfigurationNode
    {
        InjectorNode defaultInjectorNode;
        readonly EventHandler<ConfigurationNodeChangedEventArgs> defaultInjectorNodeRemovedHandler;

        /// <summary>
        /// Initialize a new instance of the <see cref="InjectorCollectionNode"/> class.
        /// </summary>
        public InjectorCollectionNode()
        {
            defaultInjectorNodeRemovedHandler = OnDefaultInjectorNodeRemoved;
        }

        /// <summary>
        /// The default injector node - this is the configured injector
        /// that will be used by default when you call PolicyInjector.create/wrap.
        /// </summary>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(InjectorNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("DefaultInjectorDescription", typeof(Resources))]
        public InjectorNode DefaultInjector
        {
            get { return defaultInjectorNode; }
            set
            {
                defaultInjectorNode = LinkNodeHelper.CreateReference(
                    defaultInjectorNode,
                    value,
                    defaultInjectorNodeRemovedHandler,
                    null);
            }
        }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        /// <remarks>The name for a <see cref="InjectorCollectionNode"/> is fixed.</remarks>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public override string Name
        {
            get { return Resources.InjectorCollectionNodeName; }
            set { }
        }

        /// <summary>
        /// Gets if children added to the node are sorted.
        /// </summary>
        /// <value>
        /// <see langword="false"/> as nodes in the collection should not be sorted.
        /// </value>
        public override bool SortChildren
        {
            get { return false; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ConfigurationNode"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (defaultInjectorNode != null)
                {
                    defaultInjectorNode.Removed -= OnDefaultInjectorNodeRemoved;
                }
            }
            base.Dispose(disposing);
        }

        void OnDefaultInjectorNodeRemoved(object sender,
                                          ConfigurationNodeChangedEventArgs e)
        {
            defaultInjectorNode = null;
        }

        /// <summary>
        /// Perform custom validation for this node.
        /// </summary>
        /// <param name="errors">The list of errors to add any validation errors.</param>
        public override void Validate(IList<ValidationError> errors)
        {
            if (Nodes.Count > 0 && defaultInjectorNode == null)
            {
                errors.Add(new ValidationError(this, "DefaultInjector", Resources.MustHaveDefaultInjectorIfInjectorsAreDefined));
            }
        }
    }
}