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

using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Represents the description of how validation must be performed on a type.
    /// </summary>
    public interface IValidatedType : IValidatedElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<IValidatedElement> GetValidatedProperties();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<IValidatedElement> GetValidatedFields();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<IValidatedElement> GetValidatedMethods();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<MethodInfo> GetSelfValidationMethods();
    }
}
