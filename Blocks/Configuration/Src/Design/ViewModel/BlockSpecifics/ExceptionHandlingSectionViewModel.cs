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
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExceptionHandlingSectionViewModel : PositionedSectionViewModel
    {
        public ExceptionHandlingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            var policies = Positioning.PositionCollection("Exception Policies", 
                typeof(NamedElementCollection<ExceptionPolicyData>), 
                typeof(ExceptionPolicyData), 
                new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            var exceptionTypes = policies.PositionNestedCollection(typeof(ExceptionTypeData));
            exceptionTypes.PositionNestedCollection(typeof(ExceptionHandlerData));

            Positioning.PositionHeader("Exception Types", new PositioningInstructions { FixedRow = 0, FixedColumn = 1 });
            Positioning.PositionHeader("Handlers", new PositioningInstructions { FixedRow = 0, FixedColumn = 2 });

            
        }
    }
}
