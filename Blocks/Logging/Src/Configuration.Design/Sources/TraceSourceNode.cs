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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    /// <summary>
    /// Represents a <see cref="TraceSourceData"/> configuration element. This class is abstract.
    /// </summary>
    [Image(typeof(TraceSourceNode))]
    public abstract class TraceSourceNode : ConfigurationNode
    {
		private SourceLevels switchLevel;
        private bool autoFlush;
        private const string CategoryName = "CategoryGeneral";

        /// <summary>
        /// Initialize a new instance of the <see cref="TraceSourceNode"/> class.
        /// </summary>
		protected TraceSourceNode() : this(string.Empty, SourceLevels.All, LogSource.DefaultAutoFlushProperty)   
        {			
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="TraceSourceNode"/> class with a name.
		/// </summary>
		/// <param name="name">The name of the node.</param>
		protected TraceSourceNode(string name) : this(name, SourceLevels.All)
		{
		}


		/// <summary>
		/// Initialize a new instance of the <see cref="TraceSourceNode"/> class with a name and a <see cref="SourceLevels"/>.
		/// </summary>
		/// <param name="name">The name of the node.</param>
		/// <param name="switchLevel">One of the <see cref="SourceLevels"/> values.</param>
		protected TraceSourceNode(string name, SourceLevels switchLevel) : base(name)
		{
			this.switchLevel = switchLevel;
		}

        /// <summary>
        /// Initialize a new instance of the <see cref="TraceSourceNode"/> class with a name, a <see cref="SourceLevels"/> and the auto flush.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="switchLevel">One of the <see cref="SourceLevels"/> values.</param>
        /// <param name="autoFlush">Whether Flush is call after every write or not</param>
        protected TraceSourceNode(string name, SourceLevels switchLevel, bool autoFlush)
            : this(name, switchLevel)
        {
            this.autoFlush = autoFlush;
        }

        /// <summary>
        /// Gets or sets the source level.
        /// </summary>
		/// <value>
		/// One of the <see cref="SourceLevels"/> values.
		/// </value>
        [SRCategory(CategoryName, typeof(Resources))]
        [SRDescription("DefaultLevelDescription", typeof(Resources))]
        public SourceLevels SourceLevels
        {
            get { return switchLevel; }
            set { switchLevel = value; }
        }

        /// <summary>
        /// Gets or sets the auto flush.
        /// </summary>
        [SRCategory(CategoryName, typeof(Resources))]
        [SRDescription("AutoFlush", typeof(Resources))]
        [DefaultValue(LogSource.DefaultAutoFlushProperty)]
        public bool AutoFlush
        {
            get { return autoFlush; }
            set { autoFlush = value; }
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }

		/// <summary>
		/// Gets the <see cref="TraceSourceData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="TraceSourceData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public TraceSourceData TraceSourceData 
		{
			get { return new TraceSourceData(Name, switchLevel);  }
		}		
    }
}