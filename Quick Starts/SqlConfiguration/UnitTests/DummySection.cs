//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Tests
{
    internal class DummySection : SerializableConfigurationSection
    {
        private const string nameProperty = "name";
        private const string valueProperty = "value";

        public DummySection()
        {
        }

        [ConfigurationProperty(nameProperty)]
        public string Name
        {
            get { return (string)base[nameProperty]; }
            set { base[nameProperty] = value; }
        }

        [ConfigurationProperty(valueProperty)]
        public int Value
        {
            get { return (int)base[valueProperty]; }
            set { base[valueProperty] = value; }
        }
    }
}
