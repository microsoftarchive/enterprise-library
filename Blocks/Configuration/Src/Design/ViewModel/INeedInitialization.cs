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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class InitializeContext
    {
        private readonly IDesignConfigurationSource loadSource;

        public InitializeContext(IDesignConfigurationSource loadSource)
        {
            this.loadSource = loadSource;
        }

        public bool WasLoadedFromSource
        {
            get { return loadSource != null; }
        }

        public IDesignConfigurationSource LoadSource
        {
            get { return loadSource; }
        }
    }

    public interface INeedInitialization
    {
        void Initialize(InitializeContext context);
    }
}
