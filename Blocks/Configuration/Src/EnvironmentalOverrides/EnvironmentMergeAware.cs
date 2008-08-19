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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    class EnvironmentMergeAware : IEnvironmentMergeService
    {
        private bool environmentMergeInProgress = false;

        #region IEnvironmentMergeAware Members

        public bool EnvironmentMergeInProgress
        {
            get { return environmentMergeInProgress; }
        }

        #endregion

        internal void SetEnvironmentalMergeInProgress(bool value)
        {
            environmentMergeInProgress = value;
        }
    }
}
