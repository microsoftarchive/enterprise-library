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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ruleset")]
    public class ValidationRulesetDataViewModel: CollectionElementViewModel
    {
        public ValidationRulesetDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return new TwoColumnsLayout(
                this,
                new ListLayout(new ViewModel[]{
                    new HierarchicalLayout(ChildElement("Validators"), ChildElement("Validators").ChildElements, 2),
                    new ElementListLayout(ChildElement("Properties").ChildElements),
                    new ElementListLayout(ChildElement("Fields").ChildElements),
                    new ElementListLayout(ChildElement("Methods").ChildElements)
                }), 1);
        }
    }
#pragma warning restore 1591
}
