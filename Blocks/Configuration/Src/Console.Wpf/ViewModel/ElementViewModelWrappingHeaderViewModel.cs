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
using Console.Wpf.Controls;

namespace Console.Wpf.ViewModel
{
    public class ElementViewModelWrappingHeaderViewModel : HeaderViewModel
    {
        private readonly ElementViewModel wrappedModel;
        private readonly bool offerAddCommands;

        public ElementViewModelWrappingHeaderViewModel(ElementViewModel wrappedModel, bool offerAddCommands)
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

        public IEnumerable<ICommand> ChildAdders
        {
            get
            {
                if (offerAddCommands)
                {
                    return wrappedModel.ChildAdders;
                }

                return Enumerable.Empty<ICommand>();
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
