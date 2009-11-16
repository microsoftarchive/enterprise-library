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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class PiabSectionViewModel : PositionedSectionViewModel
    {
        public PiabSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
            Positioning.PositionCollection("Matching Rules", 
                    typeof(MatchingRuleData), 
                    new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            Positioning.PositionCollection("Policies", 
                    typeof(NamedElementCollection<PolicyData>), 
                    typeof(PolicyData), 
                    new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });

            Positioning.PositionCollection("Call Handlers", 
                    typeof(CallHandlerData), 
                    new PositioningInstructions { FixedColumn = 2, FixedRow = 0 });
        }
    }
}
