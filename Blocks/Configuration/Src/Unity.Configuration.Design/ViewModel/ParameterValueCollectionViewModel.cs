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
using System.ComponentModel;

namespace Microsoft.Practices.Unity.Configuration.Design.ViewModel
{
    public class ParameterValueCollectionViewModel: ElementCollectionViewModel
    {
        public ParameterValueCollectionViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            :base(parentElementModel, declaringProperty)
        {
            base.IsPolymorphicCollection = true;
        }

        public override IEnumerable<Type> PolymorphicCollectionElementTypes
        {
            get
            {
                return ValueElementProperty.KnownParameterValueTypes;
            }
        }
    }
}
