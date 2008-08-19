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
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Platform Invocation methods used to support Tracer.
    /// </summary>
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }

        // Constants for use with GetSecurityInfo
        internal const uint OWNER_SECURITY_INFORMATION = 0x00000001;
        internal const uint GROUP_SECURITY_INFORMATION = 0x00000002;
        internal const uint DACL_SECURITY_INFORMATION = 0x00000004;
        internal const uint SACL_SECURITY_INFORMATION = 0x00000008;

        // HRESULTS
        internal const int CONTEXT_E_NOCONTEXT = unchecked((int)(0x8004E004));
        internal const int E_NOINTERFACE = unchecked((int)(0x80004002));

        [DllImport("kernel32.dll")]
        internal static extern int QueryPerformanceCounter(out Int64 lpPerformanceCount);

        [DllImport("kernel32.dll")]
        internal static extern int QueryPerformanceFrequency(out Int64 lpPerformanceCount);

        [DllImport("mtxex.dll", CallingConvention=CallingConvention.Cdecl)]
        internal static extern int GetObjectContext([Out]
        [MarshalAs(UnmanagedType.Interface)] out IObjectContext pCtx);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();

		/// <summary>
		/// Made public for testing purposes.
		/// </summary>
		/// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentProcessId();

		/// <summary>
		/// Made public for testing purposes.
		/// </summary>
		/// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [PreserveSig]
        public static extern int GetModuleFileName([In] IntPtr hModule, [Out] StringBuilder lpFilename, [In]
        [MarshalAs(UnmanagedType.U4)] int nSize);

		/// <summary>
		/// Made public for testing purposes.
		/// </summary>
		/// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("secur32.dll", CharSet=CharSet.Unicode, EntryPoint="GetUserNameExW", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool GetUserNameEx([In] ExtendedNameFormat nameFormat, StringBuilder nameBuffer, ref uint size);

        [DllImport("advapi32.dll")]
        internal static extern int GetSecurityInfo(IntPtr handle, SE_OBJECT_TYPE objectType, uint securityInformation, ref IntPtr ppSidOwner, ref IntPtr ppSidGroup, ref IntPtr ppDacl, ref IntPtr ppSacl, out IntPtr ppSecurityDescriptor);

        [DllImport("advapi32.dll", CharSet=CharSet.Unicode)]
        internal static extern bool LookupAccountSid(
            IntPtr systemName, // name of local or remote computer
            IntPtr sid, // security identifier
            StringBuilder accountName, // account name buffer
            ref uint accountNameLength, // size of account name buffer
            StringBuilder domainName, // domain name
            ref uint domainNameLength, // size of domain name buffer
            out int sidType // SID type
            );

		/// <summary>
		/// Made public for testing purposes.
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [ComImport]
        [Guid("51372AE0-CAE7-11CF-BE81-00AA00A2FA25")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IObjectContext
        {
            [return : MarshalAs(UnmanagedType.Interface)]
            Object CreateInstance([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);

            void SetComplete();

            void SetAbort();

            void EnableCommit();

            void DisableCommit();

            [PreserveSig]
            [return : MarshalAs(UnmanagedType.Bool)]
            bool IsInTransaction();

            [PreserveSig]
            [return : MarshalAs(UnmanagedType.Bool)]
            bool IsSecurityEnabled();

            [return : MarshalAs(UnmanagedType.Bool)]
            bool IsCallerInRole([In]
            [MarshalAs(UnmanagedType.BStr)] String role);
        }

        internal enum ExtendedNameFormat : int
        {
            // Examples for the following formats assume a fictitous company
            // which hooks into the global X.500 and DNS name spaces as follows.
            //
            // Enterprise root domain in DNS is
            //
            //      widget.com
            //
            // Enterprise root domain in X.500 (RFC 1779 format) is
            //
            //      O=Widget, C=US
            //
            // There exists the child domain
            //
            //      engineering.widget.com
            //
            // equivalent to
            //
            //      OU=Engineering, O=Widget, C=US
            //
            // There exists a container within the Engineering domain
            //
            //      OU=Software, OU=Engineering, O=Widget, C=US
            //
            // There exists the user
            //
            //      CN=John Doe, OU=Software, OU=Engineering, O=Widget, C=US
            //
            // And this user's downlevel (pre-ADS) user name is
            //
            //      Engineering\JohnDoe

            // unknown name type
            NameUnknown = 0,

            // CN=John Doe, OU=Software, OU=Engineering, O=Widget, C=US
            NameFullyQualifiedDN = 1,

            // Engineering\JohnDoe
            NameSamCompatible = 2,

            // Probably "John Doe" but could be something else.  I.e. The
            // display name is not necessarily the defining RDN.
            NameDisplay = 3,

            // String-ized GUID as returned by IIDFromString().
            // eg: {4fa050f0-f561-11cf-bdd9-00aa003a77b6}
            NameUniqueId = 6,

            // engineering.widget.com/software/John Doe
            NameCanonical = 7,

            // johndoe@engineering.com
            NameUserPrincipal = 8,

            // Same as NameCanonical except that rightmost '/' is
            // replaced with '\n' - even in domain-only case.
            // eg: engineering.widget.com/software\nJohn Doe
            NameCanonicalEx = 9,

            // www/SRv.engineering.com/engineering.com
            NameServicePrincipal = 10,

            /// <summary>
            /// DNS domain name + SAM username 
            /// eg: engineering.widget.com\JohnDoe
            /// </summary>
            NameDnsDomain = 12
        }

        internal enum SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE = 0,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT
        }
    }

}