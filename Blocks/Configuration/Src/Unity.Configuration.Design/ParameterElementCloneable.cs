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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public class ParameterElementCloneable : ICloneableConfigurationElement
    {
        readonly ParameterElement paramToClone;
        public ParameterElementCloneable(ParameterElement paramToClone)
        {
            this.paramToClone = paramToClone;
        }

        #region ICloneableConfigurationElement Members

        public System.Configuration.ConfigurationElement CreateFullClone()
        {
            ParameterElement target = new ParameterElement();
            ParameterElement clonedProperty = (ParameterElement)ConfigurationSectionCloner.CloneElement(paramToClone, target);
            clonedProperty.Value = paramToClone.Value;

            return clonedProperty;
        }

        #endregion
    }
}
