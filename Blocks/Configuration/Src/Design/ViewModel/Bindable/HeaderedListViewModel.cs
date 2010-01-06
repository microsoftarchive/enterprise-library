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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class HeaderedListViewModel : TwoVerticalVisualsViewModel
    {
        public HeaderedListViewModel(ElementViewModel containgElement)
            : base(new HeaderViewModel(containgElement.Name, containgElement.Commands), new ElementListViewModel(containgElement.ChildElements))
        {
        }
        public HeaderedListViewModel(ElementViewModel containgElement, IEnumerable<CommandModel> commands)
            : base(new HeaderViewModel(containgElement.Name, commands), new ElementListViewModel(containgElement.ChildElements))
        {
        }
    }
}
