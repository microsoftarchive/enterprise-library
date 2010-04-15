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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class used to receive an event if the designers content is cleared, e.g. if the user opens a new configuration file.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    public class ConfigurationSourceDependency
    {
        /// <summary>
        /// Raises the <see cref="Cleared"/> event.
        /// </summary>
        public void OnCleared()
        {
            var handler = Cleared;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the designers content is cleared, e.g. if the user opens a new configuration file.
        /// </summary>
        public event EventHandler Cleared;


    }
}
