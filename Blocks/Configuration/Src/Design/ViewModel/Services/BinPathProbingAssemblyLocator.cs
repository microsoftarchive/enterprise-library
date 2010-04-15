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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// This class supports the configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="AssemblyLocator"/> to obtain a list of assemblies that are used by the designer.
    /// </remarks>
    /// <seealso cref="AssemblyLocator"/>
    public class BinPathProbingAssemblyLocator : AssemblyLocator
    {
        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
        public BinPathProbingAssemblyLocator()
            :base(AppDomain.CurrentDomain.BaseDirectory)
        {
            
        }

    }
}
