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
using System.Configuration;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class CacheManagerBackingStoreProperty : ElementReferenceProperty
    {
        BackingStoreReferenceConverter backingStoreReferenceConverter;
        CacheManagerSectionViewModel cacheManagerViewModel;

        public CacheManagerBackingStoreProperty(IServiceProvider serviceProvider, ElementLookup elementLookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, elementLookup, parent, declaringProperty)
        {
            
        }

        public override TypeConverter Converter
        {
            get
            {
                return backingStoreReferenceConverter;
            }
        }

        public override void Initialize(InitializeContext context)
        {
            cacheManagerViewModel = (CacheManagerSectionViewModel)ContainingSection;
            backingStoreReferenceConverter = new BackingStoreReferenceConverter(cacheManagerViewModel.NullBackingStoreName);

            base.Initialize(context);
        }
    }
}
