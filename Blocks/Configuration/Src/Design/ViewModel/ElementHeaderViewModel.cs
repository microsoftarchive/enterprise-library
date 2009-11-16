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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class ElementHeaderViewModel : HeaderViewModel
    {
        private readonly ElementViewModel wrappedModel;
        private readonly bool offerAddCommands;

        public ElementHeaderViewModel(ElementViewModel wrappedModel, bool offerAddCommands)
        {
            this.wrappedModel = wrappedModel;
            this.offerAddCommands = offerAddCommands;
        }

        public string Path
        {
            get
            {
                return wrappedModel.TypePath;
            }
        }
        public override string Name
        {
            get { return wrappedModel.Name; }
        }

        public IEnumerable<CommandModel> Commands
        {
            get
            {
                if (offerAddCommands)
                {
                    return wrappedModel.Commands.Where(x => x.Placement == CommandPlacement.ContextAdd);
                }

                return Enumerable.Empty<CommandModel>();
            }
        }

        public override System.Windows.FrameworkElement CustomVisual
        {
            get
            {
                {
                    return new ElementModelContainer() {Content = this};
                }
            }
        }
    }
}
