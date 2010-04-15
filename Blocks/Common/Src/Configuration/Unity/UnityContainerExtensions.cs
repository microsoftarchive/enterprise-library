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
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
    /// <summary>
    /// Extension methods on <see cref="IUnityContainer"/> that provides
    /// some convenience wrappers.
    /// </summary>
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Add a new extension to the given <paramref name="container"/>, only
        /// if the extension hasn't already been added to the container.
        /// </summary>
        /// <param name="container">The container to add the extension to.</param>
        /// <param name="extension">The extension object.</param>
        /// <returns><paramref name="container"/></returns>
        public static IUnityContainer AddExtensionIfNotPresent(this IUnityContainer container, UnityContainerExtension extension)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (extension == null) throw new ArgumentNullException("extension");

            Type extensionType = extension.GetType();
            if(container.Configure(extensionType) == null)
            {
                container.AddExtension(extension);
            }
            return container;
        }

        /// <summary>
        /// Add a new extension to the given <paramref name="container"/>, only
        /// if the extension hasn't already been added to the container.
        /// </summary>
        /// <typeparam name="TExtension">Type of extension to add.</typeparam>
        /// <param name="container">Container to add the extension to.</param>
        /// <returns><paramref name="container"/></returns>
        public static IUnityContainer AddNewExtensionIfNotPresent<TExtension>(this IUnityContainer container)
            where TExtension : UnityContainerExtension, new()
        {
            if(container.Configure<TExtension>() == null)
            {
                container.AddNewExtension<TExtension>();
            }
            return container;
        }
    }
}
