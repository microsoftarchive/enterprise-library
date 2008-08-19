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

using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Represents a mapper for <see cref="EmailTraceListenerData"/> configuration to Wmi.
    /// </summary>
    public static class EmailTraceListenerDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="EmailTraceListenerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(EmailTraceListenerData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new EmailTraceListenerSetting(configurationObject,
                                              configurationObject.Name,
                                              configurationObject.Formatter,
                                              configurationObject.FromAddress,
                                              configurationObject.SmtpPort,
                                              configurationObject.SmtpServer,
                                              configurationObject.SubjectLineEnder,
                                              configurationObject.SubjectLineStarter,
                                              configurationObject.ToAddress,
                                              configurationObject.TraceOutputOptions.ToString(),
											  configurationObject.Filter.ToString()));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(EmailTraceListenerSetting));
        }

        internal static bool SaveChanges(EmailTraceListenerSetting setting,
                                         ConfigurationElement sourceElement)
        {
            EmailTraceListenerData element = (EmailTraceListenerData)sourceElement;

            element.Formatter = setting.Formatter;
            element.FromAddress = setting.FromAddress;
            element.SmtpPort = setting.SmtpPort;
            element.SmtpServer = setting.SmtpServer;
            element.SubjectLineEnder = setting.SubjectLineEnder;
            element.SubjectLineStarter = setting.SubjectLineStarter;
            element.ToAddress = setting.ToAddress;
            element.TraceOutputOptions = ParseHelper.ParseEnum<TraceOptions>(setting.TraceOutputOptions, false);

            SourceLevels filter;
            if(ParseHelper.TryParseEnum(setting.Filter, out filter))
            {
                element.Filter = filter;
            }
            return true;
        }
    }
}