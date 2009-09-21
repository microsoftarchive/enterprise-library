﻿//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Specifies the required source for validation information when invoking <see cref="Validator"/> creation methods.
    /// </summary>
    /// <seealso cref="ValidationFactory.CreateValidator{T}(string, IConfigurationSource)"/>
    [Flags]
    public enum ValidationSpecificationSource
    {
        /// <summary>
        /// Validation information is to be retrieved from attributes.
        /// </summary>
        Attributes = 1,

        /// <summary>
        /// Validation information is to be retrieved from configuration.
        /// </summary>
        Configuration = 2,

        /// <summary>
        /// Validation information is to be retrieved from both attributes and configuration.
        /// </summary>
        Both = 3,

        /// <summary>
        /// 
        /// </summary>
        DataAnnotations = 4,

        /// <summary>
        /// 
        /// </summary>
        All = 7
    }
}
