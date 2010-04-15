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
using System.Globalization;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ValidatedTypeReferenceViewModel : CollectionElementViewModel
    {
        public ValidatedTypeReferenceViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        public override string Name
        {
            get
            {
                var baseName = (string) base.Property("Name").Value;

                return TypeNameParserHelper.ParseTypeName(baseName);
            }
        }

        protected override object CreateBindable()
        {
            return new HierarchicalLayout(
                this,
                this.ChildElement("Rulesets").ChildElements,
                0);
        }

        
    }
#pragma warning restore 1591
}
