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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity
{
    public class ContainerElementViewModel : CollectionElementViewModel
    {
        public ContainerElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        protected override object CreateBindable()
        {
            return
                new TwoColumnsLayout(this,
                    new TwoVerticalsLayout(
                        new ElementListLayout(this.ChildElement("Registrations").ChildElements),
                        new HorizontalColumnBindingLayout(
                            new TwoVerticalsLayout(
                                new ElementListLayout(this.ChildElement("Instances").ChildElements),
                                new ElementListLayout(this.ChildElement("Extensions").ChildElements)),
                                1)
                        ), 
                        0
                    );
        }
    }
}
