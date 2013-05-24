#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Configuration;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    public class ConfigurationHelper
    {
        public static string GetSetting(string settingName)
        {
            string value;
            using (var subKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\EntLib") ?? Registry.CurrentUser)
            {
                value = (string)subKey.GetValue(settingName);
            }
            if (string.IsNullOrEmpty(value))
            {
                value = ConfigurationManager.AppSettings[settingName];
            }
            return value;
        }
    }
}
