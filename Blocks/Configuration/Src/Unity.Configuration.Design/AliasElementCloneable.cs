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
    public class AliasElementCloneable : ICloneableConfigurationElement
    {
        private readonly AliasElement aliasToClone;

        public AliasElementCloneable(AliasElement aliasToClone)
        {
            this.aliasToClone = aliasToClone;
        }
        public System.Configuration.ConfigurationElement CreateFullClone()
        {
            AliasElement target = new AliasElement();
            AliasElement clonedAlias = (AliasElement)ConfigurationSectionCloner.CloneElement(aliasToClone, target);
            clonedAlias.Alias = aliasToClone.Alias;

            return clonedAlias;
        }
    }
}
