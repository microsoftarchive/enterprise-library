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
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    /// <summary>
    /// Provides useful diagnostic information from the debug subsystem.
    /// </summary>
    public class DebugInformationProvider : IExtraInformationProvider
    {
        IDebugUtils debugUtils;

        /// <summary>
        /// Creates a new instance of <see cref="DebugInformationProvider"/>.
        /// </summary>
        public DebugInformationProvider()
            : this(new DebugUtils()) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="DebugInformationProvider"/> class..
        /// </summary>
        /// <param name="debugUtils">Alternative <see cref="IDebugUtils"/> to use.</param>
        public DebugInformationProvider(IDebugUtils debugUtils)
        {
            this.debugUtils = debugUtils;
        }

        /// <summary>
        /// Populates an <see cref="IDictionary{TKey,TValue}"/> with helpful diagnostic information.
        /// </summary>
        /// <param name="dict">Dictionary used to populate the <see cref="DebugInformationProvider"></see></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed")]
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            string value;

            try
            {
                value = debugUtils.GetStackTraceWithSourceInfo(new StackTrace(true));
            }
            catch (SecurityException)
            {
                value = String.Format(CultureInfo.CurrentCulture, Resources.ExtendedPropertyError, Resources.DebugInfo_StackTraceSecurityException);
            }
            catch
            {
                value = String.Format(CultureInfo.CurrentCulture, Resources.ExtendedPropertyError, Resources.DebugInfo_StackTraceException);
            }

            dict.Add(Resources.DebugInfo_StackTrace, value);
        }
    }
}
