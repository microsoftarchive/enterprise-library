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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity;
using Microsoft.Practices.Unity.Configuration.Design;

[assembly: HandlesSection(UnityConfigurationSection.SectionName)]
[assembly: AddApplicationBlockCommand(
                UnityConfigurationSection.SectionName,
                typeof(UnityConfigurationSection),
                TitleResourceName = "AddUnitySettings",
                TitleResourceType = typeof(DesignResources))]
