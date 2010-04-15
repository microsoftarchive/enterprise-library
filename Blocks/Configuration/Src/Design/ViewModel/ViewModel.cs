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
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="ViewModel"/> provides a base definition for the logic backing a view-element within
    /// the Enterprise Library Configuration Design tool.
    /// </summary>
    /// <seealso cref="ElementViewModel"/>
    /// <seealso cref="Property"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class ViewModel : IDisposable 
    {
        /// <summary>
        /// When overridden, gets an object that is bound to the view.
        /// </summary>
        /// <remarks>
        /// By default, the <see cref="ViewModel"/> returns itself.
        /// </remarks>
        public virtual object Bindable
        {
            get { return this; }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="Dispose(bool)"/> was invoked through an explicit call to <see cref="Dispose()"/> instead of a finalizer call.</param>
        /// <filterpriority>2</filterpriority>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}
