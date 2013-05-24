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
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Messaging;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using ComponentModel = System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="MsmqTraceListener"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataDisplayName")]
    [SecurityCritical]
    public class MsmqTraceListenerData : TraceListenerData
    {
        private const string queuePathProperty = "queuePath";
        private const string formatterNameProperty = "formatter";
        private const string messagePriorityProperty = "messagePriority";
        private const string timeToReachQueueProperty = "timeToReachQueue";
        private const string timeToBeReceivedProperty = "timeToBeReceived";
        private const string recoverableProperty = "recoverable";
        private const string useAuthenticationProperty = "useAuthentication";
        private const string useDeadLetterQueueProperty = "useDeadLetterQueue";
        private const string useEncryptionProperty = "useEncryption";
        private const string transactionTypeProperty = "transactionType";

        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The Priority value for the Priority property.
        /// </summary>
        public const MessagePriority DefaultPriority = MessagePriority.Normal;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the Recoverable property.
        /// </summary>
        public const bool DefaultRecoverable = false;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the UseAuthentication property.
        /// </summary>
        public const bool DefaultUseAuthentication = false;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the UseDeadLetter property.
        /// </summary>
        public const bool DefaultUseDeadLetter = false;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the UseEncryption property.
        /// </summary>
        public const bool DefaultUseEncryption = false;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the TimeToReachQueue property.
        /// </summary>
        public static readonly TimeSpan DefaultTimeToReachQueue = Message.InfiniteTimeout;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the TimeToBeReceived property.
        /// </summary>
        public static readonly TimeSpan DefaultTimeToBeReceived = Message.InfiniteTimeout;
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// The default value for the TransactionType property.
        /// </summary>
        public const MessageQueueTransactionType DefaultTransactionType = MessageQueueTransactionType.None;


        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class with default values.
        /// </summary>
        public MsmqTraceListenerData()
            : base(typeof(MsmqTraceListener))
        {
            ListenerDataType = typeof(MsmqTraceListenerData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class with name, path and formatter name.
        /// </summary>
        /// <param name="name">The name for the represented trace listener.</param>
        /// <param name="queuePath">The path name for the represented trace listener.</param>
        /// <param name="formatterName">The formatter name for the represented trace listener.</param>
        public MsmqTraceListenerData(string name, string queuePath, string formatterName)
            : this(name, queuePath, formatterName, DefaultPriority, DefaultRecoverable,
                DefaultTimeToBeReceived, DefaultTimeToReachQueue, DefaultUseAuthentication,
                DefaultUseDeadLetter, DefaultUseEncryption, DefaultTransactionType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the represented trace listener.</param>
        /// <param name="queuePath">The path name for the represented trace listener.</param>
        /// <param name="formatterName">The formatter name for the represented trace listener.</param>
        /// <param name="messagePriority">The priority for the represented trace listener.</param>
        /// <param name="recoverable">The recoverable flag for the represented trace listener.</param>
        /// <param name="timeToReachQueue">The timeToReachQueue for the represented trace listener.</param>
        /// <param name="timeToBeReceived">The timeToReachQueue for the represented trace listener.</param>
        /// <param name="useAuthentication">The use authentication flag for the represented trace listener.</param>
        /// <param name="useDeadLetterQueue">The use dead letter flag for the represented trace listener.</param>
        /// <param name="useEncryption">The use encryption flag for the represented trace listener.</param>
        /// <param name="transactionType">The transaction type for the represented trace listener.</param>
        public MsmqTraceListenerData(string name, string queuePath, string formatterName,
                                     MessagePriority messagePriority, bool recoverable,
                                     TimeSpan timeToReachQueue, TimeSpan timeToBeReceived,
                                     bool useAuthentication, bool useDeadLetterQueue, bool useEncryption,
                                     MessageQueueTransactionType transactionType)
            : this(name, queuePath, formatterName, messagePriority, recoverable, timeToReachQueue, timeToBeReceived,
                                     useAuthentication, useDeadLetterQueue, useEncryption, transactionType, TraceOptions.None, SourceLevels.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the represented trace listener.</param>
        /// <param name="queuePath">The path name for the represented trace listener.</param>
        /// <param name="formatterName">The formatter name for the represented trace listener.</param>
        /// <param name="messagePriority">The priority for the represented trace listener.</param>
        /// <param name="recoverable">The recoverable flag for the represented trace listener.</param>
        /// <param name="timeToReachQueue">The timeToReachQueue for the represented trace listener.</param>
        /// <param name="timeToBeReceived">The timeToReachQueue for the represented trace listener.</param>
        /// <param name="useAuthentication">The use authentication flag for the represented trace listener.</param>
        /// <param name="useDeadLetterQueue">The use dead letter flag for the represented trace listener.</param>
        /// <param name="useEncryption">The use encryption flag for the represented trace listener.</param>
        /// <param name="transactionType">The transaction type for the represented trace listener.</param>
        /// <param name="traceOutputOptions">The trace output options for the represented trace listener.</param>
        /// <param name="filter">The filter for the represented trace listener.</param>
        public MsmqTraceListenerData(string name, string queuePath, string formatterName,
                                     MessagePriority messagePriority, bool recoverable,
                                     TimeSpan timeToReachQueue, TimeSpan timeToBeReceived,
                                     bool useAuthentication, bool useDeadLetterQueue, bool useEncryption,
                                     MessageQueueTransactionType transactionType, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(MsmqTraceListener), traceOutputOptions, filter)
        {
            this.QueuePath = queuePath;
            this.Formatter = formatterName;
            this.MessagePriority = messagePriority;
            this.Recoverable = recoverable;
            this.TimeToReachQueue = timeToReachQueue;
            this.TimeToBeReceived = timeToBeReceived;
            this.UseAuthentication = useAuthentication;
            this.UseDeadLetterQueue = useDeadLetterQueue;
            this.UseEncryption = useEncryption;
            this.TransactionType = transactionType;
        }

        /// <summary>
        /// Gets or sets the message queue path.
        /// </summary>
        [ConfigurationProperty(queuePathProperty, Options = ConfigurationPropertyOptions.IsRequired)]
        [DesigntimeDefaultAttribute(".\\Private$\\myQueue")]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataQueuePathDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataQueuePathDisplayName")]
        public string QueuePath
        {
            get
            {
                return (string)this[queuePathProperty];
            }
            set
            {
                this[queuePathProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets formatter name.
        /// </summary>
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [ConfigurationProperty(formatterNameProperty, Options = ConfigurationPropertyOptions.IsRequired)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataFormatterDisplayName")]
        public string Formatter
        {
            get
            {
                return (string)this[formatterNameProperty];
            }
            set
            {
                this[formatterNameProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the message priority.
        /// </summary>
        [ConfigurationProperty(messagePriorityProperty, DefaultValue = DefaultPriority)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataMessagePriorityDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataMessagePriorityDisplayName")]
        public MessagePriority MessagePriority
        {
            get
            {
                return (MessagePriority)this[messagePriorityProperty];
            }
            set
            {
                this[messagePriorityProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the time to reach queue.
        /// </summary>
        [ConfigurationProperty(timeToReachQueueProperty, DefaultValue = "49710.06:28:15")] //DefaultValue = Message.InfiniteTimeout
        [EnvironmentalOverrides(true, StorageConverterType = typeof(ComponentModel.TimeSpanConverter))]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataTimeToReachQueueDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataTimeToReachQueueDisplayName")]
        public TimeSpan TimeToReachQueue
        {
            get
            {
                return (TimeSpan)this[timeToReachQueueProperty];
            }
            set
            {
                this[timeToReachQueueProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the time to be received.
        /// </summary>
        [ConfigurationProperty(timeToBeReceivedProperty, DefaultValue = "49710.06:28:15")] //DefaultValue = Message.InfiniteTimeout
        [EnvironmentalOverrides(true, StorageConverterType = typeof(ComponentModel.TimeSpanConverter))]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataTimeToBeReceivedDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataTimeToBeReceivedDisplayName")]
        public TimeSpan TimeToBeReceived
        {
            get
            {
                return (TimeSpan)this[timeToBeReceivedProperty];
            }
            set
            {
                this[timeToBeReceivedProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the recoverable value.
        /// </summary>
        [ConfigurationProperty(recoverableProperty, DefaultValue = DefaultRecoverable)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataRecoverableDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataRecoverableDisplayName")]
        public bool Recoverable
        {
            get
            {
                return (bool)this[recoverableProperty];
            }
            set
            {
                this[recoverableProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the use authentication value.
        /// </summary>
        [ConfigurationProperty(useAuthenticationProperty, DefaultValue = DefaultUseAuthentication)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataUseAuthenticationDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataUseAuthenticationDisplayName")]
        public bool UseAuthentication
        {
            get
            {
                return (bool)this[useAuthenticationProperty];
            }
            set
            {
                this[useAuthenticationProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the use dead letter value.
        /// </summary>
        [ConfigurationProperty(useDeadLetterQueueProperty, DefaultValue = DefaultUseDeadLetter)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataUseDeadLetterQueueDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataUseDeadLetterQueueDisplayName")]
        public bool UseDeadLetterQueue
        {
            get
            {
                return (bool)this[useDeadLetterQueueProperty];
            }
            set
            {
                this[useDeadLetterQueueProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the use encryption value.
        /// </summary>
        [ConfigurationProperty(useEncryptionProperty, DefaultValue = DefaultUseEncryption)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataUseEncryptionDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataUseEncryptionDisplayName")]
        public bool UseEncryption
        {
            get
            {
                return (bool)this[useEncryptionProperty];
            }
            set
            {
                this[useEncryptionProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        [ConfigurationProperty(transactionTypeProperty, DefaultValue = DefaultTransactionType)]
        [ResourceDescription(typeof(DesignResources), "MsmqTraceListenerDataTransactionTypeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MsmqTraceListenerDataTransactionTypeDisplayName")]
        public MessageQueueTransactionType TransactionType
        {
            get
            {
                return (MessageQueueTransactionType)this[transactionTypeProperty];
            }
            set
            {
                this[transactionTypeProperty] = value;
            }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// A <see cref="MsmqTraceListener"/>.
        /// </returns>
        [SecuritySafeCritical]
        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            this.CheckQueuePath();
            this.CheckFormatter();

            var formatter = this.BuildFormatterSafe(settings, this.Formatter);

            return new MsmqTraceListener(
                this.Name,
                this.QueuePath,
                formatter,
                this.MessagePriority,
                this.Recoverable,
                this.TimeToReachQueue,
                this.TimeToBeReceived,
                this.UseAuthentication,
                this.UseDeadLetterQueue,
                this.UseEncryption,
                this.TransactionType);
        }

        private void CheckQueuePath()
        {
            if (string.IsNullOrEmpty(this.QueuePath))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionMsmqQueuePathMissing,
                        this.ElementInformation.Source,
                        this.ElementInformation.LineNumber,
                        this.Name));
            }
        }

        private void CheckFormatter()
        {
            if (string.IsNullOrEmpty(this.Formatter))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionFormatterNameRequired,
                        this.ElementInformation.Source,
                        this.ElementInformation.LineNumber,
                        this.Name));
            }
        }
    }
}
