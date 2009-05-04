//===============================================================================
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
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Represents the description of how validation must be performed on a language element.
    /// </summary>
    public interface IValidatedElement
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IValidatorDescriptor> GetValidatorDescriptors();

        /// <summary>
        /// 
        /// </summary>
        CompositionType CompositionType { get; }

        /// <summary>
        /// 
        /// </summary>
        string CompositionMessageTemplate { get; }

        /// <summary>
        /// 
        /// </summary>
        string CompositionTag { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IgnoreNulls { get; }

        /// <summary>
        /// 
        /// </summary>
        string IgnoreNullsMessageTemplate { get; }

        /// <summary>
        /// 
        /// </summary>
        string IgnoreNullsTag { get; }

        /// <summary>
        /// 
        /// </summary>
        MemberInfo MemberInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        Type TargetType { get; }
    }
}
