#region license
//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Application Block Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
#endregion
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    using System.Configuration;
    using System.Linq;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;
    using Microsoft.Practices.Unity;

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class RetryPolicyConfigurationSettingsViewModel : SectionViewModel
    {
        public RetryPolicyConfigurationSettingsViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }

        protected override object CreateBindable()
        {
            var retryStrategies = DescendentElements()
                .Where(x => x.ConfigurationType == typeof(RetryStrategyCollection))
                .First();

            return new HorizontalListLayout(new HeaderedListLayout(retryStrategies));
        }
    }
#pragma warning restore 1591
}
