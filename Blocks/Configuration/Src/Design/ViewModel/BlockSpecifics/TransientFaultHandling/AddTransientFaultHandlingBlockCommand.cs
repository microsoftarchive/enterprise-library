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
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;

#pragma warning disable 1591
    /// <summary>
    /// This class provides block-specific configuration design-time support and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddTransientFaultHandlingBlockCommand : AddApplicationBlockCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddTransientFaultHandlingBlockCommand"/> class. 
        /// </summary>
        /// <param name="configurationSourceModel">The ConfigurationSourceModel.</param>
        /// <param name="attribute">The AddApplicationBlockCommandAttribute.</param>
        /// <param name="uiService">The IUIServiceWpf.</param>
        public AddTransientFaultHandlingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute, IUIServiceWpf uiService)
            : base(configurationSourceModel, attribute, uiService)
        {
        }

        protected override ConfigurationSection CreateConfigurationSection()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "Fixed Interval Retry Strategy",
                RetryStrategies = 
                {
                    new IncrementalData("Incremental Retry Strategy"),
                    new FixedIntervalData("Fixed Interval Retry Strategy"),                    
                    new ExponentialBackoffData("Exponential Backoff Retry Strategy")
                }
            };
        }
    }
#pragma warning restore 1591
}
