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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class ListViewModel : ViewModel
    {
        IEnumerable<ViewModel> elements;

        public ListViewModel(IEnumerable<ViewModel> elements)
        {
            this.elements = elements;
        }

        public IEnumerable<ViewModel> Elements
        {
            get { return this.elements; }
        }
    }

    public class ElementListViewModel : ViewModel
    {
        IEnumerable<ElementViewModel> elements;

        public ElementListViewModel(IEnumerable<ElementViewModel> elements)
        {
            this.elements = elements;
        }

        public IEnumerable<ElementViewModel> Elements
        {
            get { return this.elements; }
        }
    }
}
