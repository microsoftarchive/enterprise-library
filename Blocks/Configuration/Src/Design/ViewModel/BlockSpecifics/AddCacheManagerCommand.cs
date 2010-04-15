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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddCacheManagerCommand : DefaultCollectionElementAddCommand
    {
        CacheManagerSectionViewModel cachingSection;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public AddCacheManagerCommand(ElementCollectionViewModel collection, IUIServiceWpf uiService)
            : base(new ConfigurationElementType(typeof(CacheManagerData)), collection, uiService)
        {
            Guard.ArgumentNotNull(collection, "collection");

            this.cachingSection = collection.ContainingSection as CacheManagerSectionViewModel;
        }

        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

            AddedElementViewModel.Property("CacheStorage").Value = cachingSection.NullBackingStoreName;
        }
    }
#pragma warning restore 1591
}
