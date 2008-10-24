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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Properties;


namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
    sealed class AppSettingsCommandRegistrar : CommandRegistrar
    {
        public AppSettingsCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{

		}
		
		public override void Register()
		{
            AddAppSettingsCommand();

            AddValidateCommand(typeof(AppSettingsNode));
            AddDefaultCommands(typeof(AppSettingNode));
		}

		private void AddAppSettingsCommand()
		{
			AddSingleChildNodeCommand(Resources.AppSettingsUICommandText,
                Resources.AppSettingsUICommandLongText, typeof(AppSettingsNode),
				typeof(ConfigurationApplicationNode));


            AddMultipleChildNodeCommand(Resources.AppSettingUICommandText,
                Resources.AppSettingUICommandLongText, typeof(AppSettingNode),
                typeof(AppSettingsNode));
		}
    }
}
