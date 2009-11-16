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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.TestSupport
{
    public static class ElementViewModelExtensions
    {
        public static IEnumerable<ElementViewModel> GetDescendentsOfType<T>(this ElementViewModel model)
        {
            if (model == null) throw new ArgumentNullException("model");
            return GetDescendentsOfType<T, ElementViewModel>(model);
        }

        public static IEnumerable<V> GetDescendentsOfType<T, V>(this ElementViewModel model)
        {
            return model.DescendentElements().Where(x => typeof(T).IsAssignableFrom(x.ConfigurationType)).OfType<V>();
        }
    }
}
