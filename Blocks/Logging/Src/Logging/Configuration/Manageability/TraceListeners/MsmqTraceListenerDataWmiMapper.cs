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
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Represents a mapper for <see cref="MsmqTraceListenerDataWmiMapper"/> configuration to Wmi.
    /// </summary>
    public static class MsmqTraceListenerDataWmiMapper 
	{
        /// <summary>
        /// Creates the <see cref="MsmqTraceListenerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(MsmqTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new MsmqTraceListenerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.Formatter,
					configurationObject.MessagePriority.ToString(),
					configurationObject.QueuePath,
					configurationObject.Recoverable,
					Convert.ToString(configurationObject.TimeToBeReceived, CultureInfo.CurrentCulture),
					Convert.ToString(configurationObject.TimeToReachQueue, CultureInfo.CurrentCulture),
					configurationObject.TraceOutputOptions.ToString(),
					configurationObject.TransactionType.ToString(),
					configurationObject.UseAuthentication,
					configurationObject.UseDeadLetterQueue,
					configurationObject.UseEncryption,
					configurationObject.Filter.ToString()));
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(MsmqTraceListenerSetting));
		}

        internal static bool SaveChanges(MsmqTraceListenerSetting setting,
                                         ConfigurationElement sourceElement)
        {
            MsmqTraceListenerData element = (MsmqTraceListenerData)sourceElement;

            element.Filter = ParseHelper.ParseEnum<SourceLevels>(setting.Filter, false);
            element.Formatter = setting.Formatter;
            element.MessagePriority = ParseHelper.ParseEnum<MessagePriority>(setting.MessagePriority, false);
            element.Name = setting.Name;
            element.QueuePath = setting.QueuePath;
            element.Recoverable = setting.Recoverable;
            element.TimeToBeReceived = TimeSpan.Parse(setting.TimeToBeReceived);
            element.TimeToReachQueue = TimeSpan.Parse(setting.TimeToReachQueue);
            element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);
            element.TransactionType = ParseHelper.ParseEnum<MessageQueueTransactionType>(setting.TransactionType,false);
            element.UseAuthentication = setting.UseAuthentication;
            element.UseDeadLetterQueue = setting.UseDeadLetterQueue;
            element.UseEncryption = setting.UseEncryption;

            SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }

            return true;
        }
	}
}
