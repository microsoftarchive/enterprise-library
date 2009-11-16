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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public abstract class PositionedSectionViewModel : SectionViewModel
    {
        protected PositionedSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            :base(builder, sectionName, section)
        {
        }

        public PositionedSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section, IEnumerable<Attribute> metadataAttributes)
            : base(null, sectionName, section, metadataAttributes)
        {
        }


        public override int Columns
        {
            get { return Positioning.EndColumn; }
        }

        public override int Rows
        {
            get { return Positioning.EndRow; }
        }


        int lastNumberOfGridVisuals = 0;
        public override void UpdateLayout()
        {
            Positioning.Update(this);

            if (lastNumberOfGridVisuals != Positioning.GetGridVisuals().Count())
            {
                lastNumberOfGridVisuals = Positioning.GetGridVisuals().Count();

                OnUpdateVisualGrid();
            }
        }

        public override IEnumerable<ViewModel> GetGridVisuals()
        {
            return Positioning.GetGridVisuals();
        }
    }
}
