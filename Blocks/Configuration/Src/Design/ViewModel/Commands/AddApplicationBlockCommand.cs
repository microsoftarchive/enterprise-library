using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Services;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.Commands
{
    public class AddApplicationBlockCommand : CommandModel
    {
        string sectionName;
        ConfigurationSourceModel configurationModel;
        Type configurationSectionType;

        public AddApplicationBlockCommand(ConfigurationSourceModel configurationModel, AddApplicationBlockCommandAttribute commandAttribute)
            : base(commandAttribute)
        {
            this.configurationModel = configurationModel;
            this.configurationModel.Sections.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Sections_CollectionChanged);
            this.sectionName = commandAttribute.SectionName;
            this.configurationSectionType = commandAttribute.ConfigurationSectionType;
        }

        public string SectionName
        {
            get { return sectionName; }
        }

        void Sections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        protected virtual ConfigurationSection CreateConfigurationSection()
        {
            return (ConfigurationSection)Activator.CreateInstance(configurationSectionType);
        }

        public override bool CanExecute(object parameter)
        {
            return !configurationModel.HasSection(sectionName);
        }

        public override void Execute(object parameter)
        {
            configurationModel.AddSection(sectionName, CreateConfigurationSection());

            OnCanExecuteChanged();
        }
    }
}
