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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class CompositeKeyElement : NamedConfigurationElement
    {
        private const string OtherKeyPartName = "otherKeyPart";

        [ConfigurationProperty(OtherKeyPartName, IsKey=true, IsRequired=true)]
        public string OtherKeyPart
        {
            get { return (string)base[OtherKeyPart]; }
            set { base[OtherKeyPartName] = value; }
        }

    }
}
