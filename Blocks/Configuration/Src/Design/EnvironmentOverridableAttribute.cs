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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class EnvironmentOverridableAttribute : Attribute
    {
        private readonly bool canOverride;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canOverride"></param>
        public EnvironmentOverridableAttribute(bool canOverride)
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
