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

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    ///<summary>
    /// A <see cref="UnityContainerExtension"/> that allows the container
    /// to directly resolve <see cref="Validator{T}"/> instances.
    ///</summary>
    public class ValidationBlockExtension : UnityContainerExtension
    {
        /// <summary>
        /// Ensure that this container has been configured to resolve Validation
        /// block objects.
        /// </summary>
        protected override void Initialize()
        {
            Context.Strategies.Add(new ValidatorCreationStrategy(), UnityBuildStage.PreCreation);
        }
    }
}
