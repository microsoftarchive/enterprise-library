//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    public partial class DateRangeValidatorDataFixture
    {
        [TestMethod]
        public void CanBeLoadedFromConfiguration()
        {
            DictionaryConfigurationSource.FromXaml(new Uri("/Microsoft.Practices.EnterpriseLibrary.Validation.Silverlight.Tests;component/Configuration.xaml", UriKind.Relative));
        }
    }
}
