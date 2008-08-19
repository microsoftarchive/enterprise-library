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
using System.Messaging;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
    /// Represents a <see cref="MsmqTraceListenerData"/> configuration element.
    /// </summary>
    public class MsmqTraceListenerNode : TraceListenerNode
    {
		private FormatterNode formatterNode;
		private bool useEncryption;
		private bool useDeadLetterQueue;
		private bool useAuthentication;
		private MessageQueueTransactionType transactionType;
		private MessagePriority messagePriority;
		private TimeSpan timeToReachQueue;
		private TimeSpan timeToBeReceived;
		private bool recoverable;
		private string queuePath;
		private string formatterName;
        

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqTraceListenerNode"/> class.
        /// </summary>
        public MsmqTraceListenerNode()
			: this(new MsmqTraceListenerData(Resources.MsmqTraceListenerNode, DefaultValues.MsmqQueuePath, string.Empty))
        {            
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="MsmqTraceListenerNode"/> class with a <see cref="MsmqTraceListenerData"/> instance.
		/// </summary>
		/// <param name="msmqTraceListenerData">A <see cref="MsmqTraceListenerData"/> instance.</param>
        public MsmqTraceListenerNode(MsmqTraceListenerData msmqTraceListenerData)
        {
			if (null == msmqTraceListenerData) throw new ArgumentNullException("msmqTraceListenerData");

			Rename(msmqTraceListenerData.Name);
            Filter = msmqTraceListenerData.Filter;
			TraceOutputOptions = msmqTraceListenerData.TraceOutputOptions;
			this.useEncryption = msmqTraceListenerData.UseEncryption;
			this.useDeadLetterQueue = msmqTraceListenerData.UseDeadLetterQueue;
			this.useAuthentication = msmqTraceListenerData.UseAuthentication;
			this.transactionType = msmqTraceListenerData.TransactionType;
			this.messagePriority = msmqTraceListenerData.MessagePriority;
			this.timeToReachQueue = msmqTraceListenerData.TimeToReachQueue;
			this.timeToBeReceived = msmqTraceListenerData.TimeToBeReceived;
			this.recoverable = msmqTraceListenerData.Recoverable;
			this.queuePath = msmqTraceListenerData.QueuePath;
			this.formatterName = msmqTraceListenerData.Formatter;
        }

        /// <summary>
        /// Gets or sets the queue path.
        /// </summary>
		/// <value>
		/// The queue path.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("QueuePathDescription", typeof(Resources))]
        [Required]
        public string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }

        /// <summary>
        /// Gets or sets if the queue is recoverable.
        /// </summary>
		/// <value>
		/// <see langword="true"/> if the queue is recoverable; otherwise, <see langword="false"/>.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("RecoverableDescription", typeof(Resources))]
        public bool Recoverable
        {
            get { return recoverable; }
            set { recoverable = value; }
        }

        /// <summary>
        /// Gets or sets the time to be received.
        /// </summary>
		/// <value>
		/// The time to be received.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TimeToBeReceivedDescription", typeof(Resources))]
        public TimeSpan TimeToBeReceived
        {
            get { return timeToBeReceived; }
            set { timeToBeReceived = value; }
        }

		/// <summary>
		/// Gets or sets the time to reach the queue.
		/// </summary>
		/// <value>
		/// The time to reach the queue.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TimeToReachQueueDescription", typeof(Resources))]
        public TimeSpan TimeToReachQueue
        {
            get { return timeToReachQueue; }
            set { timeToReachQueue = value; }
        }

		/// <summary>
		/// Gets or sets the message priority.
		/// </summary>
		/// <value>
		/// One of the <see cref="MessagePriority"/> values.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("MessagePriorityDescription", typeof(Resources))]
        public MessagePriority MessagePriority
        {
            get { return messagePriority; }
            set { messagePriority = value; }
        }
				
        /// <summary>
        /// Gets or sets the message queue transaction type.
        /// </summary>
		/// <value>
		/// One of the <see cref="MessageQueueTransactionType"/> value.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TransactionTypeDescription", typeof(Resources))]
        public MessageQueueTransactionType TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

		/// <summary>
        /// Gets or sets if authentication should be used.
        /// </summary>
		/// <value>
		/// <see langword="true"/> if authentication should be used; otherwise, <see langword="false"/>.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UseAuthenticationDescription", typeof(Resources))]
        public bool UseAuthentication
        {
            get { return useAuthentication; }
            set { useAuthentication = value; }
        }

		
        /// <summary>
        /// Gets or sets if the queue uses the dead letter queue.
        /// </summary>
		/// <value>
		/// <see langword="true"/> if the queue uses the dead letter queue; otherwise, <see langword="false"/>.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UseDeadLetterQueueDescription", typeof(Resources))]
        public bool UseDeadLetterQueue
        {
            get { return useDeadLetterQueue; }
            set { useDeadLetterQueue = value; }
        }

		
        /// <summary>
        /// Gets or sets if encryption should be used.
        /// </summary>
		/// <value>
		/// <see langword="true"/> if encryption should be used; otherwise, <see langword="false"/>.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UseEncryptionDescription", typeof(Resources))]
        public bool UseEncryption
        {
            get { return useEncryption; }
            set { useEncryption = value; }
        }

		/// <summary>
		/// Gets or sets the formatter to use.
		/// </summary>
		/// <value>
		/// The formatter to use.
		/// </value>
        [Required]
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);

                formatterName = formatterNode == null ? String.Empty : formatterNode.Name;
            }
        }

		/// <summary>
		/// Gets the <see cref="MsmqTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="MsmqTraceListenerData"/> this node represents.
		/// </value>
		public override TraceListenerData TraceListenerData
		{
			get 
			{
				MsmqTraceListenerData data = new MsmqTraceListenerData(Name, queuePath, formatterName, messagePriority,
					recoverable, timeToReachQueue, timeToBeReceived, useAuthentication,
					useDeadLetterQueue, useEncryption, transactionType);
				data.TraceOutputOptions = TraceOutputOptions;
                data.Filter = Filter;
				return data;
			}
		}

		/// <summary>
		/// Sets the formatter to use for this listener.
		/// </summary>
		/// <param name="formatterNodeReference">
		/// A <see cref="FormatterNode"/> reference or <see langword="null"/> if no formatter is defined.
		/// </param>
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterNodeReference.Name == formatterName) Formatter = (FormatterNode)formatterNodeReference;
		}

		private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.formatterNode = null;
        }

        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.formatterName = e.Node.Name;
        }
    }
}