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
using System.Text;
using System.ComponentModel.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    internal class ComponentChangeService : IComponentChangeService
    {
        #region IComponentChangeService Members

#pragma  warning disable 67
        public event ComponentEventHandler ComponentAdded;
#pragma  warning restore 67

#pragma  warning disable 67
        public event ComponentEventHandler ComponentAdding;
#pragma  warning restore 67

#pragma  warning disable 67
        public event ComponentEventHandler ComponentRemoved;
#pragma  warning restore 67

#pragma  warning disable 67
        public event ComponentEventHandler ComponentRemoving;
#pragma  warning restore 67

#pragma  warning disable 67
        public event ComponentRenameEventHandler ComponentRename;
#pragma  warning restore 67

        public event ComponentChangedEventHandler ComponentChanged;

        public event ComponentChangingEventHandler ComponentChanging;


        public void OnComponentChanged(object component, System.ComponentModel.MemberDescriptor member, object oldValue, object newValue)
        {
            if (ComponentChanged != null)
            {
                ComponentChanged(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
            }
        }

        public void OnComponentChanging(object component, System.ComponentModel.MemberDescriptor member)
        {
            if (ComponentChanging != null)
            {
                ComponentChanging(this, new ComponentChangingEventArgs(component, member));
            }
        }

        #endregion
    }
}
