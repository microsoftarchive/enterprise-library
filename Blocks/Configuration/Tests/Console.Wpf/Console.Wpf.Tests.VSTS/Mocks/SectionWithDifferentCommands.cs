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
using System.Configuration;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class SectionWithDifferentCommands : ConfigurationSection
    {
        public SectionWithDifferentCommands()
        {
            Element = new DefaultConfigurationElement();
            CustomCommandElement = new ConfigurationElementWithCustomCommand();
            ConfigurationElementWithCustomDelete = new CustomDeleteConfigurationElement();
        }

        [ConfigurationProperty("Element")]
        public DefaultConfigurationElement Element
        {
            get { return (DefaultConfigurationElement)base["Element"]; }
            set { base["Element"] = value; }
        }

        [ConfigurationProperty("CustomCommandElement")]
        public ConfigurationElementWithCustomCommand CustomCommandElement
        {
            get { return (ConfigurationElementWithCustomCommand)base["CustomCommandElement"]; }
            set { base["CustomCommandElement"] = value; }
        }

        [ConfigurationProperty("CollectionElement")]
        public NameValueConfigurationCollection CollectionElement
        {
            get { return (NameValueConfigurationCollection)base["CollectionElement"]; }
            set { base["CollectionElement"] = value; }
        }

        [ConfigurationProperty("CollectionElement2")]
        [MyCustomCommandAttribute(Replace = CommandReplacement.DefaultAddCommandReplacement, CommandPlacement = CommandPlacement.ContextAdd)]
        public ConnectionStringSettingsCollection CollectionElementWithCustomAddCommand
        {
            get { return (ConnectionStringSettingsCollection)base["CollectionElement2"]; }
            set { base["CollectionElement2"] = value; }
        }

        [ConfigurationProperty("ConfigurationElementWithCustomDelete")]
        [MyCustomCommandAttribute(Replace = CommandReplacement.DefaultDeleteCommandReplacement, CommandPlacement = CommandPlacement.ContextDelete)]
        public CustomDeleteConfigurationElement ConfigurationElementWithCustomDelete
        {
            get { return (CustomDeleteConfigurationElement)base["ConfigurationElementWithCustomDelete"]; }
            set { base["ConfigurationElementWithCustomDelete"] = value; }
        }

        [ConfigurationProperty("PolymorphicCollectionElement")]
        public PolymorphicCollection PolymorphicCollectionElement
        {
            get { return (PolymorphicCollection)base["PolymorphicCollectionElement"]; }
            set { base["PolymorphicCollectionElement"] = value; }
        }
    }

    public class CustomDeleteConfigurationElement : ConfigurationElement
    {
    }

    public class DefaultConfigurationElement : ConfigurationElement
    {
    }
    [ConfigurationCollection(typeof(PolymorphicCollectionItem))]
    public class PolymorphicCollection : PolymorphicConfigurationElementCollection<PolymorphicCollectionItem>
    {
        protected override Type RetrieveConfigurationElementType(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    public class PolymorphicCollectionItem : NamedConfigurationElement
    {
    }

    [MyCustomCommandAttribute]
    public class ConfigurationElementWithCustomCommand : ConfigurationElement
    {
    }

    public class MyCustomCommandAttribute : CommandAttribute
    {
        public MyCustomCommandAttribute()
            : base(typeof(CustomCommand).AssemblyQualifiedName)
        {
            TitleResourceName = "Custom command";
            TitleResourceType = typeof(object);
        }
    }

    public class CustomCommand : CommandModel
    {
        public CustomCommand(MyCustomCommandAttribute commandAttribute, IUIServiceWpf uiService)
            : base(commandAttribute, uiService)
        {

        }
    }
}
