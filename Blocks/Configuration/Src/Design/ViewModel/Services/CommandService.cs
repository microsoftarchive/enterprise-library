using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;
using Console.Wpf.ViewModel.Commands;

namespace Console.Wpf.ViewModel.Services
{
    public class MenuCommandService
    {
        AssemblyLocator assemblyLocator;
        IList<CommandModel> globalCommands;
        IUnityContainer builder;
        ConfigurationSourceDependency configurationSourceRefresh;

        public MenuCommandService(IUnityContainer builder, AssemblyLocator assemblyLocator, ConfigurationSourceDependency configurationSourceRefresh)
        {
            this.configurationSourceRefresh = configurationSourceRefresh;
            this.builder = builder;
            this.assemblyLocator = assemblyLocator;
            this.globalCommands = this.assemblyLocator.Assemblies
                                                       .SelectMany(x => x.GetCustomAttributes(typeof(CommandAttribute), true).OfType<CommandAttribute>())
                                                       .Select(x => builder.Resolve(x.CommandModelType, new DependencyOverride(x.GetType(), x)))
                                                       .Cast<CommandModel>()
                                                       .ToList();

            configurationSourceRefresh.Cleared += new EventHandler(configurationSourceRefresh_Refresh);
        }

        void configurationSourceRefresh_Refresh(object sender, EventArgs e)
        {
            foreach (var command in globalCommands)
            {
                command.OnCanExecuteChanged();
            }
        }

        public IEnumerable<CommandModel> GetCommands(CommandPlacement placement)
        {
            return globalCommands.Where(x => x.Placement == placement);
        }


        public void ExecuteAddBlockForSection(string sectionName)
        {
            var addBlockCommand = globalCommands.OfType<AddApplicationBlockCommand>().Where(x => x.SectionName == sectionName).FirstOrDefault();
            if (addBlockCommand != null && addBlockCommand.CanExecute(null)) addBlockCommand.Execute(null);
        }
    }
}
