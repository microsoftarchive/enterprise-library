//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]

[assembly: AssemblyTitle("Enterprise Library Exception Handling Logging Provider")]
[assembly: AssemblyDescription("Enterprise Library Exception Handling Logging Provider")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.1304.0")]

[assembly: SecurityTransparent]

[assembly: ComVisible(false)]
