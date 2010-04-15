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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup
{
    /// <summary>
    /// Specifies a resolver that knows how to locate a <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The resolver is often used to more clearly express the dependency a class
    /// may have on something that builds instances.  This is often used in lieu
    /// of injecting an entire container into a constructor.
    /// <typeparam name="T"></typeparam>
    /// </remarks>
    public interface IResolver<T>
    {
        /// <summary>
        /// Resolves an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        T Resolve();

        /// <summary>
        /// Returns an instance of <paramref name="childType"/> whose ancestor
        /// must be of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="childType"></param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        T Resolve(Type childType);
    }
}
