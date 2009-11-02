using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.ViewModel.Services;

namespace Console.Wpf.ViewModel.Commands
{
    public class AddSateliteProviderCommand : DefaultCollectionElementAddCommand
    {
        MenuCommandService commandService;
        AddSateliteProviderCommandAttribute commandAttribute;

        public AddSateliteProviderCommand(AddSateliteProviderCommandAttribute commandAttribute, MenuCommandService commandService, ConfigurationElementType configurationElementType, ElementCollectionViewModel collection)
            : base(commandAttribute, configurationElementType, collection)
        {
            this.commandService = commandService;
            this.commandAttribute = commandAttribute;
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            commandService.ExecuteAddBlockForSection(commandAttribute.SectionName);

        }
    }
}
