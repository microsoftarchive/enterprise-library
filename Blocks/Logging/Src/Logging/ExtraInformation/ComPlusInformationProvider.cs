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
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    /// <summary>
    /// A helper facade class that provides easy access to commonly needed COM+ diagnostic information.
    /// </summary>
    public class ComPlusInformationProvider : IExtraInformationProvider
    {
        /// <summary>
        /// Creates an instance of the <see cref="ComPlusInformationProvider"/> class.
        /// </summary>
        public ComPlusInformationProvider()
        {
            contextUtils = new ContextUtils();
        }

        internal ComPlusInformationProvider(IContextUtils contextUtils)
        {
            this.contextUtils = contextUtils;
        }

        /// <summary>
		  /// Populates an <see cref="IDictionary{K,T}"/> with the COM+ properties provided by <see cref="ComPlusInformationProvider"/>.
        /// </summary>
        /// <param name="dict">Dictionary used to populate the <see cref="ComPlusInformationProvider"></see></param>
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            dict.Add(Properties.Resources.ComPlusInfo_ActivityId, ActivityId);
            dict.Add(Properties.Resources.ComPlusInfo_ApplicationId, ApplicationId);
            dict.Add(Properties.Resources.ComPlusInfo_TransactionID, TransactionId);
            dict.Add(Properties.Resources.ComPlusInfo_DirectCallerAccountName, DirectCallerAccountName);
            dict.Add(Properties.Resources.ComPlusInfo_OriginalCallerAccountName, OriginalCallerAccountName);
        }

        /// <summary>
        /// Returns the COM+ Original Caller Account Name
        /// </summary>
        public string OriginalCallerAccountName
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetOriginalCallerAccountName)); }
        }

        /// <summary>
        /// Returns the COM+ Direct Caller Name
        /// </summary>
        public string DirectCallerAccountName
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetDirectCallerAccountName)); }
        }

        /// <summary>
        /// Returns the COM+ Transaction ID
        /// </summary>
        public string TransactionId
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetTransactionId)); }
        }

        /// <summary>
        /// Returns the COM+ Application ID
        /// </summary>
        public string ApplicationId
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetApplicationId)); }
        }

        /// <summary>
        /// Returns the COM+ Activity ID
        /// </summary>
        public string ActivityId
        {
            get { return GetSafeProperty(new ContextAccessorDelegate(contextUtils.GetActivityId)); }
        }

        private string GetSafeProperty(ContextAccessorDelegate accessorDelegate)
        {
            string result;
            try
            {
                result = accessorDelegate();
            }
            catch (Exception e)
            {
				result = String.Format(Properties.Resources.Culture, Properties.Resources.ExtendedPropertyError, e.Message);
            }            
            return result;
        }

        private delegate string ContextAccessorDelegate();

        private IContextUtils contextUtils;
    }
}