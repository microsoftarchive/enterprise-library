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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    /// <summary>
    /// Provides extension methods on <see cref="IServiceLocator"/> for convenience.
    /// </summary>
    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// Disposes <paramref name="locator"/> if it implements the <see cref="IDisposable"/> interface.
        /// </summary>
        /// <param name="locator">The service locator to dispose, if possible.</param>
        public static void Dispose(this IServiceLocator locator)
        {
            if (locator == null) return;

            var disposable = locator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
