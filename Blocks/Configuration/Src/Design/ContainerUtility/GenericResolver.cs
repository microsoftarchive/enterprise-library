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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ContainerUtility
{
    /// <summary>
    /// Resolve a <typeparamref name="T"/> from the underlying container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericResolver<T> : IResolver<T>
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Creates an instance of the GenericResolver class.
        /// </summary>
        /// <param name="container"></param>
        public GenericResolver(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Returns a <typeparamref name="T"/> from the underlying container.
        /// </summary>
        /// <returns></returns>
        public T Resolve()
        {
            return container.Resolve<T>();
        }


        /// <summary>
        /// Returns an instance of <paramref name="childType"/> whose ancestor
        /// must be of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="childType"></param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        /// <exception cref="InvalidCastException">If the <paramref name="childType"/> cannot be cast to <typeparamref name="T"/></exception>
        public T Resolve(Type childType)
        {
            return (T) container.Resolve(childType);
        }
    }
}
