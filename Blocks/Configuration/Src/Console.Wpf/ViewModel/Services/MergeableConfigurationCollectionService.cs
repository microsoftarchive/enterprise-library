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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.ViewModel.Services
{
    public class MergeableConfigurationCollectionService
    {
        public IMergeableConfigurationElementCollection GetMergeableCollection(ConfigurationElementCollection collection)
        {
            return MergeableConfigurationCollectionFactory.GetCreateMergeableCollection(collection);
        }
    }
}
