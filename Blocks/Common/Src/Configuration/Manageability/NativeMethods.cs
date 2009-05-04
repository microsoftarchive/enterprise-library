//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	internal static class NativeMethods
	{
		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterGPNotification(SafeWaitHandle hEvent,
			[MarshalAs(UnmanagedType.Bool)] bool bMachine);

		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterGPNotification(SafeWaitHandle hEvent);

		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr EnterCriticalPolicySection([MarshalAs(UnmanagedType.Bool)] bool bMachine);

		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LeaveCriticalPolicySection(IntPtr handle);
	}
}
