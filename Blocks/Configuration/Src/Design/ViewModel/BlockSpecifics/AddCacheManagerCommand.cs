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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class AddCacheManagerCommand : DefaultCollectionElementAddCommand
    {
        CacheManagerSectionViewModel cachingSection;
        public AddCacheManagerCommand(ElementCollectionViewModel collection)
            :base(new ConfigurationElementType(typeof(CacheManagerData)), collection)
        {
            this.cachingSection = collection.ContainingSection as CacheManagerSectionViewModel;
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            AddedElementViewModel.Property("CacheStorage").Value = cachingSection.NullBackingStoreName;
        }
    }
}
