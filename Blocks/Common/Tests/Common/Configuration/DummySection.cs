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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    public class DummySection : SerializableConfigurationSection
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
