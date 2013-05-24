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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class TraceListenerElementCollectionViewModel : ElementCollectionViewModel
    {

        public TraceListenerElementCollectionViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(parentElementModel, declaringProperty)
        {
        }

        public override IEnumerable<Type> PolymorphicCollectionElementTypes
        {
            get
            {
                return base.PolymorphicCollectionElementTypes.Union(
                            new Type[]{
                                typeof(SystemDiagnosticsTraceListenerData), 
                                typeof(CustomTraceListenerData)
                            }.Where(t => ConfigurationTypesService.CheckType(t))).ToArray();
            }
        }
    }
#pragma warning restore 1591
}
