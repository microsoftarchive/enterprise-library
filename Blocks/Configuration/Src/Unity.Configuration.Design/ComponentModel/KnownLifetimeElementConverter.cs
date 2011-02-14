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
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;

namespace Microsoft.Practices.Unity.Configuration.Design.ComponentModel
{
    public class KnownLifetimeElementConverter : StringConverter
    {
        public Type[] LifetimeManagerTypes = new[] { typeof(ContainerControlledLifetimeManager), typeof(TransientLifetimeManager), typeof(PerThreadLifetimeManager), typeof(ExternallyControlledLifetimeManager), typeof(HierarchicalLifetimeManager), typeof(PerResolveLifetimeManager) };
        
        public string NullLifetimeManagerDisplay
        {
            get { return DesignResources.RegistrationNoLifetime; }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public bool IsCustomLifetimeManager(string lifetimeManagerType)
        {
            if (lifetimeManagerType == NullLifetimeManagerDisplay)
                return false;

            if (LifetimeManagerTypes.Any(x => x.Name == lifetimeManagerType))
                return false;

            return true;
        }


        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var standardValues = new[] { NullLifetimeManagerDisplay }
                .Union(LifetimeManagerTypes.Select(x => x.Name))
                .Union(new[] {DesignResources.RegistrationLifetimeCustom});

            return new StandardValuesCollection(standardValues.ToArray());
        }
    }
}
