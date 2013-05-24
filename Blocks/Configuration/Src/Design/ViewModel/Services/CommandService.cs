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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class used to get top-level menu <see cref="CommandModel"/> instances.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    /// <see cref="HandlesSectionAttribute"/>
    public class MenuCommandService
    {
        AssemblyLocator assemblyLocator;
        IList<CommandModel> globalCommands;
        IUnityContainer builder;
        ConfigurationSourceDependency configurationSourceRefresh;
        Profile profile;

        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
        public MenuCommandService(IUnityContainer builder, AssemblyLocator assemblyLocator, ConfigurationSourceDependency configurationSourceRefresh, Profile profile)
        {
            this.configurationSourceRefresh = configurationSourceRefresh;
            this.profile = profile;
            this.builder = builder;
            this.assemblyLocator = assemblyLocator;
        }

        void configurationSourceRefresh_Refresh(object sender, EventArgs e)
        {
            foreach (var command in globalCommands)
            {
                command.OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Returns a list of <see cref="CommandModel"/> instances that belong to the specified <see cref="CommandPlacement"/>.
        /// </summary>
        /// <param name="placement">The <see cref="CommandPlacement"/> for which commands should be returned.</param>
        /// <returns>
        /// A list of <see cref="CommandModel"/> instances that belong to the specified <see cref="CommandPlacement"/>.
        /// </returns>
        public IEnumerable<CommandModel> GetCommands(CommandPlacement placement)
        {
            EnsureCommands();
            return globalCommands
                .Where(x => x.Placement == placement);
        }

        private void EnsureCommands()
        {
            if (globalCommands == null)
            {
                var globalAssemblyCommands = this.assemblyLocator.Assemblies
                   .FilterSelectManySafe(x => x.GetCustomAttributes(typeof(CommandAttribute), true).OfType<CommandAttribute>())
                   .Select(x => builder.Resolve(x.CommandModelType, new DependencyOverride(x.GetType(), x)))
                   .Cast<CommandModel>();

                this.globalCommands = globalAssemblyCommands
                    .Where(t => profile.Check(t))
                    .OrderBy(c => c.Title).ToList();

                configurationSourceRefresh.Cleared += configurationSourceRefresh_Refresh;
            }
        }

        /// <summary>
        /// Adds a <see cref="SectionViewModel"/> instance to the designer for the specified section name.
        /// </summary>
        /// <param name="sectionName">The configuration section name that should be added to the designer.</param>
        public void ExecuteAddBlockForSection(string sectionName)
        {
            EnsureCommands();
            var addBlockCommand = globalCommands.OfType<AddApplicationBlockCommand>().Where(x => x.SectionName == sectionName).FirstOrDefault();
            if (addBlockCommand != null && addBlockCommand.CanExecute(null)) addBlockCommand.Execute(null);
        }
    }
}
