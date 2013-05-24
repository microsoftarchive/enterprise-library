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

using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers
{
    [SecurityCritical]
    internal class ContextUtils : IContextUtils
    {
        private SecurityCallContext currentCall;

        [SecurityCritical]
        public string GetActivityId()
        {
            return ContextUtil.ActivityId.ToString();
        }

        [SecurityCritical]
        public string GetApplicationId()
        {
            return ContextUtil.ApplicationId.ToString();
        }

        [SecurityCritical]
        public string GetTransactionId()
        {
            return ContextUtil.TransactionId.ToString();
        }

        [SecurityCritical]
        public string GetDirectCallerAccountName()
        {
            string directCallerAccountName;
            SecurityCallContext currentCall = this.CurrentCall;

            if (currentCall != null)
            {
                if (currentCall.IsSecurityEnabled)
                {
                    directCallerAccountName = currentCall.DirectCaller.AccountName;
                }
                else
                {
                    directCallerAccountName = string.Empty;
                }
            }
            else
            {
                directCallerAccountName = string.Empty;
            }

            return directCallerAccountName;
        }

        [SecurityCritical]
        public string GetOriginalCallerAccountName()
        {
            string originalCallerAccountName;
            SecurityCallContext currentCall = this.CurrentCall;

            if (currentCall != null)
            {
                if (currentCall.IsSecurityEnabled)
                {
                    originalCallerAccountName = currentCall.OriginalCaller.AccountName;
                }
                else
                {
                    originalCallerAccountName = string.Empty;
                }
            }
            else
            {
                originalCallerAccountName = string.Empty;
            }
            return originalCallerAccountName;
        }

        private SecurityCallContext CurrentCall
        {
            get
            {
                NativeMethods.IObjectContext objectContext = ObjectContext;

                if (objectContext != null)
                {
                    if (objectContext.IsSecurityEnabled())
                    {
                        currentCall = SecurityCallContext.CurrentCall;
                    }
                    else
                    {
                        currentCall = null;
                    }
                }

                return currentCall;
            }
        }

        private NativeMethods.IObjectContext ObjectContext
        {
            get
            {
                NativeMethods.IObjectContext objectContext;

                int hr = NativeMethods.GetObjectContext(out objectContext);

                if (!(hr == 0 || hr == NativeMethods.E_NOINTERFACE || hr == NativeMethods.CONTEXT_E_NOCONTEXT))
                {
                    Marshal.ThrowExceptionForHR(hr);
                }

                return objectContext;
            }
        }
    }
}
