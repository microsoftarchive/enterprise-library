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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    /// <summary>
    /// Represents a <see cref="TraceListenerReferenceData"/> configuration element.
    /// </summary>
    [Image(typeof(TraceListenerReferenceNode))]
    public sealed class TraceListenerReferenceNode : ConfigurationNode
    {
		private TraceListenerNode referencedTraceListener;

        /// <summary>
        /// Initialize a new instance of the <see cref="TraceListenerReferenceNode"/> class.
        /// </summary>
        public TraceListenerReferenceNode()
            : this(new TraceListenerReferenceData(Resources.TraceListenerReferenceNode))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="TraceListenerReferenceNode"/> class with a <see cref="TraceListenerReferenceData"/> instance.
        /// </summary>
		/// <param name="traceListenerReferenceData">A <see cref="TraceListenerReferenceData"/> instance.</param>
        public TraceListenerReferenceNode(TraceListenerReferenceData traceListenerReferenceData)
            : base()
        {
            if (traceListenerReferenceData == null)
            {
                throw new ArgumentNullException("traceListenerReferenceData");
            }
            Rename(traceListenerReferenceData.Name);
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        [SRDescription("TraceListenerReferenceNode", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public override string Name
        {
            get { return base.Name; }   
        }

        /// <summary>
        /// Gets the referenced trace listener.
        /// </summary>
		/// <value>
		/// The referenced trace listener.
		/// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(TraceListenerNode))]
        [SRDescription("ReferencedTraceListenerDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public TraceListenerNode ReferencedTraceListener
        {
            get { return referencedTraceListener; }
            set
            {
                
                referencedTraceListener = LinkNodeHelper.CreateReference<TraceListenerNode>(referencedTraceListener,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);

                if (referencedTraceListener != null)
                {
                    try
                    {
                        Name = referencedTraceListener.Name;
                    }
                    catch (InvalidOperationException)
                    {
                        string message = string.Format(Resources.Culture, Resources.ReferenceAlreadyExists, referencedTraceListener.Name);
                        ServiceHelper.GetUIService(Site).ShowError(message);
                        referencedTraceListener = null;
                    }
                }
            }
        }

		/// <summary>
		/// Gets the <see cref="TraceListenerReferenceData"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="TraceListenerReferenceData"/> that this node represents.
		/// </value>
		[Browsable(false)]
		public TraceListenerReferenceData TraceListenerReferenceData
		{
			get
			{
				return new TraceListenerReferenceData(Name);
			}
		}

        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            ReferencedTraceListener = null;
        }

        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Name = e.Node.Name;
        }
    }
}
