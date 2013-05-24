#region license
//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Application Block Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
#endregion
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    using System.Collections.Generic;
    using System.Linq;
    using EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
    using EnterpriseLibrary.Configuration.Design.ViewModel;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;

    /// <summary>
    /// The command model implementation for the add command on retry strategies. 
    /// Will show known retry strategies without the concrete class having to have the ConfigurationElementType, allowing portability of the block.
    /// </summary>
    public class WellKnownRetryStrategyElementCollectionCommand : DefaultElementCollectionAddCommand
    {
        private readonly CommandModel[] childCommands;
        private readonly SectionViewModel section;

        /// <summary>
        /// Initializes a new instance of the <see cref="WellKnownRetryStrategyElementCollectionCommand"/> class. 
        /// </summary>
        /// <param name="collection">The collection that will be affected by the add command.</param>
        /// <param name="uiService">The <see cref="IUIServiceWpf"/> for displaying messages.</param>
        public WellKnownRetryStrategyElementCollectionCommand(ElementCollectionViewModel collection, IUIServiceWpf uiService)
            : base(collection, uiService)
        {
            this.section = collection.ContainingSection;

            var knownTypes = new[]
                {
                    typeof(IncrementalData), typeof(FixedIntervalData), typeof(ExponentialBackoffData),
                    typeof(CustomRetryStrategyData)
                };

            this.childCommands = knownTypes
                .SelectMany(x => this.section.CreateCollectionElementAddCommand(x, collection))
                .OrderBy(x => x.Title)
                .ToArray();
        }

        /// <summary>
        /// Gets the child <see cref="CommandModel"/> commands for this command.
        /// </summary>
        public override IEnumerable<CommandModel> ChildCommands
        {
            get
            {
                return this.childCommands.Where(x => x.Browsable);
            }
        }
    }
}
