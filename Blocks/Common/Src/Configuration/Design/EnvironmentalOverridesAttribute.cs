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
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EnvironmentalOverridesAttribute : Attribute
    {
        private readonly bool canOverride;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canOverride"></param>
        public EnvironmentalOverridesAttribute(bool canOverride)
        {
            this.canOverride = canOverride;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanOverride
        {
            get { return canOverride; }
        }
    }
}
