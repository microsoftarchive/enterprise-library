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
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace Console.Wpf.ViewModel.Services
{
    static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider) where T: class
        {
            return provider.GetService(typeof(T)) as T;
        }

        public static T EnsuredGetService<T>(this IServiceProvider serviceProvider) where T : new()
        {
            if (serviceProvider.GetService(typeof(T)) == null)
            {
                IServiceContainer container = (IServiceContainer)serviceProvider.GetService(typeof(IServiceContainer));
                container.AddService(typeof(T), new T());
            }

            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
