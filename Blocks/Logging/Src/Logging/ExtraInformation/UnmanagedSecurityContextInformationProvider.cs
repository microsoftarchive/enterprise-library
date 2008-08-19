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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    /// <summary>
    /// Gets the security context information from the unmanaged world
    /// </summary>
    public class UnmanagedSecurityContextInformationProvider : IExtraInformationProvider
    {
        /// <summary>
        /// Populates an <see cref="IDictionary{K,T}"/> with helpful diagnostic information.
        /// </summary>
        /// <param name="dict">Dictionary used to populate the <see cref="UnmanagedSecurityContextInformationProvider"></see></param>
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            dict.Add(Properties.Resources.UnmanagedSecurity_CurrentUser, CurrentUser);
            dict.Add(Properties.Resources.UnmanagedSecurity_ProcessAccountName, ProcessAccountName);
        }

        /// <summary>
        ///		Gets the CurrentUser, calculating it if necessary. 
        /// </summary>
        public string CurrentUser
        {
            get
            {
                uint size = 256;
                StringBuilder userNameBuffer = new StringBuilder((int) size);
                if (NativeMethods.GetUserNameEx(NativeMethods.ExtendedNameFormat.NameSamCompatible, userNameBuffer, ref size))
                {
                    return userNameBuffer.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///		Gets the ProcessAccountName, calculating it if necessary. 
        /// </summary>
		public string ProcessAccountName
        {
			[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
			get
            {
                // Get Security Info
                // -----------------
                // Note that LookupAccountSid won't find an 
                // account name for pSidOwner unless you also
                // get pSidGroup.

                IntPtr processHandle = NativeMethods.GetCurrentProcess();
                IntPtr pSidOwner = IntPtr.Zero;
                IntPtr pSidGroup = IntPtr.Zero;
                IntPtr pDacl = IntPtr.Zero;
                IntPtr pSacl = IntPtr.Zero;
                IntPtr pSecurityDescriptor = IntPtr.Zero;
                string processAccountName;

                NativeMethods.GetSecurityInfo(processHandle, NativeMethods.SE_OBJECT_TYPE.SE_KERNEL_OBJECT, NativeMethods.OWNER_SECURITY_INFORMATION | NativeMethods.GROUP_SECURITY_INFORMATION,
                                              ref pSidOwner,
                                              ref pSidGroup,
                                              ref pDacl,
                                              ref pSacl,
                                              out pSecurityDescriptor);

                // Lookup the account name associated with sidOwner

                StringBuilder accountName = new StringBuilder(1024);
                uint accountNameLength = (uint) accountName.Capacity;

                StringBuilder domainName = new StringBuilder(1024);
                uint domainNameLength = (uint) domainName.Capacity;

                int sidType;
                bool successful = NativeMethods.LookupAccountSid(IntPtr.Zero,
                                                                 pSidOwner, // security identifier
                                                                 accountName, // account name buffer
                                                                 ref accountNameLength, // size of account name buffer (in TCHARs)
                                                                 domainName, // domain name
                                                                 ref domainNameLength, // size of domain name buffer (in TCHARs)
                                                                 out sidType // SID type
                    );
                if (successful)
                {
                    processAccountName = domainName.ToString() + Path.DirectorySeparatorChar + accountName.ToString();
                } // + " SID=" + sidOwner;
                else
                {
                    processAccountName = Properties.Resources.CouldNotLookupAccountSid;
                }

                Marshal.FreeHGlobal(pSecurityDescriptor);

                return processAccountName;
            }
        }
    }
}