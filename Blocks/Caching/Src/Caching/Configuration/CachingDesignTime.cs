//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    internal static class CachingDesignTime
    {
        public static class ViewModelTypeNames
        {
            public const string CacheManagerBackingStoreProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.CacheManagerBackingStoreProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string CacheManagerSectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.CacheManagerSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class CommandTypeNames
        {
            public const string AddCachingBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddCachingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string AddCacheManagerCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddCacheManagerCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class ValidatorTypes
        {
            public const string NameValueCollectionValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
