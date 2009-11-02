using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.ViewModel;
using Microsoft.Practices.Unity;

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
            Title = "Custom command";
        }
    }

    public class CustomCommand : CommandModel
    {
        public CustomCommand(MyCustomCommandAttribute commandAttribute)
            : base(commandAttribute)
        {

        }
    }
}
