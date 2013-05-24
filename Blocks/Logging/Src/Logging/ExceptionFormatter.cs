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
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// <para>Provides exception formatting when not using the Exception Handling Application Block.</para>
    /// </summary>
    public class ExceptionFormatter
    {
        /// <summary>
        /// Name of the additional information entry that holds the header.
        /// </summary>
        public static string Header = Properties.Resources.ExceptionFormatterHeader;

        private const string LineSeparator = "======================================";
        private NameValueCollection additionalInfo;
        private string applicationName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionFormatter"/> class.</para>
        /// </summary>
        public ExceptionFormatter()
            : this(new NameValueCollection(), string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionFormatter"/> class with the additional information and the application name.</para>
        /// </summary>
        /// <param name="additionalInfo">
        /// <para>The additional information to log.</para>
        /// </param>
        /// <param name="applicationName">
        /// <para>The application name.</para>
        /// </param>
        public ExceptionFormatter(NameValueCollection additionalInfo, string applicationName)
        {
            this.additionalInfo = additionalInfo;
            this.applicationName = applicationName;
        }

        /// <summary>
        /// <para>Get the formatted message to be logged.</para>
        /// </summary>
        /// <param name="exception"><para>The exception object whose information should be written to log file.</para></param>
        /// <returns><para>The formatted message.</para></returns>
        public string GetMessage(Exception exception)
        {
            StringBuilder eventInformation = new StringBuilder();
            CollectAdditionalInfo();

            // Record the contents of the AdditionalInfo collection.
            eventInformation.AppendFormat("{0}\n\n", this.additionalInfo.Get(Header));

            eventInformation.AppendFormat("\n{0} {1}:\n{2}",
                                          Resources.ExceptionSummary, this.applicationName, LineSeparator);

            foreach (string info in this.additionalInfo)
            {
                if (info != Header)
                {
                    eventInformation.AppendFormat("\n--> {0}", this.additionalInfo.Get(info));
                }
            }

            if (exception != null)
            {
                Exception currException = exception;
                int exceptionCount = 1;
                do
                {
                    eventInformation.AppendFormat("\n\n{0}\n{1}", Resources.ExceptionDetails, LineSeparator);
                    eventInformation.AppendFormat("\n{0}: {1}", Resources.ExceptionType, currException.GetType().FullName);

                    ReflectException(currException, eventInformation);

                    // Record the StackTrace with separate label.
                    if (currException.StackTrace != null)
                    {
                        eventInformation.AppendFormat("\n\n{0} \n{1}",
                                                      Resources.ExceptionStackTraceDetails, LineSeparator);
                        eventInformation.AppendFormat("\n{0}", currException.StackTrace);
                    }

                    // Reset the temp exception object and iterate the counter.
                    currException = currException.InnerException;
                    exceptionCount++;
                } while (currException != null);
            }

            return eventInformation.ToString();
        }

        private static void ReflectException(Exception currException, StringBuilder strEventInfo)
        {
            PropertyInfo[] arrPublicProperties = currException.GetType().GetProperties();
            foreach (PropertyInfo propinfo in arrPublicProperties)
            {
                // Do not log information for the InnerException or StackTrace. This information is 
                // captured later in the process.
                if (propinfo.Name != "InnerException" && propinfo.Name != "StackTrace")
                {
                    if (propinfo.CanRead && propinfo.GetIndexParameters().Length == 0)
                    {
                        object propValue = null;

                        try
                        {
                            propValue = propinfo.GetValue(currException, null);
                        }
                        catch (TargetInvocationException)
                        {
                            propValue = Resources.PropertyAccessFailed;
                        }

                        if (propValue == null)
                        {
                            strEventInfo.AppendFormat("\n{0}: NULL", propinfo.Name);
                        }
                        else
                        {
                            ProcessAdditionalInfo(propinfo, propValue, strEventInfo);
                        }
                    }
                }
            }
        }

        private static void ProcessAdditionalInfo(PropertyInfo propinfo, object propValue, StringBuilder stringBuilder)
        {
            NameValueCollection currAdditionalInfo;

            // Loop through the collection of AdditionalInformation if the exception type is a BaseApplicationException.
            if (propinfo.Name == "AdditionalInformation")
            {
                if (propValue != null)
                {
                    // Cast the collection into a local variable.
                    currAdditionalInfo = (NameValueCollection)propValue;

                    // Check if the collection contains values.
                    if (currAdditionalInfo.Count > 0)
                    {
                        stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "\nAdditionalInformation:");

                        // Loop through the collection adding the information to the string builder.
                        for (int i = 0; i < currAdditionalInfo.Count; i++)
                        {
                            stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "\n{0}: {1}", currAdditionalInfo.GetKey(i), currAdditionalInfo[i]);
                        }
                    }
                }
            }
            else
            {
                // Otherwise just write the ToString() value of the property.
                stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "\n{0}: {1}", propinfo.Name, propValue);
            }
        }

        /// <devdoc>
        /// Add additional 'environment' information. 
        /// </devdoc>
        private void CollectAdditionalInfo()
        {
            if (this.additionalInfo["MachineName:"] != null)
            {
                return;
            }

            this.additionalInfo.Add("MachineName:", "MachineName: " + GetMachineName());
            this.additionalInfo.Add("TimeStamp:", "TimeStamp: " + DateTime.UtcNow.ToString(CultureInfo.CurrentCulture));
            this.additionalInfo.Add("FullName:", "FullName: " + Assembly.GetExecutingAssembly().FullName);
            this.additionalInfo.Add("AppDomainName:", "AppDomainName: " + AppDomain.CurrentDomain.FriendlyName);
            this.additionalInfo.Add("WindowsIdentity:", "WindowsIdentity: " + GetWindowsIdentity());
        }

        private static string GetWindowsIdentity()
        {
            try
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
                return "Permission Denied";
            }
        }

        private static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch (SecurityException)
            {
                return "Permission Denied";
            }
        }
    }
}
