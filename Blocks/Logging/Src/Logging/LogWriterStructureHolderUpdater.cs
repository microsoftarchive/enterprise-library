//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	internal class LogWriterStructureHolderUpdater : ILogWriterStructureUpdater
	{
		private LogWriter logWriter;
		private IConfigurationSource configurationSource;

		public LogWriterStructureHolderUpdater(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
			configurationSource.AddSectionChangeHandler(LoggingSettings.SectionName, UpdateLogWriter);
		}

		public void Dispose()
		{
			configurationSource.RemoveSectionChangeHandler(LoggingSettings.SectionName, UpdateLogWriter);
		}

		public void SetLogWriter(LogWriter logWriter)
		{
			this.logWriter = logWriter;
		}

		public void UpdateLogWriter(object sender, ConfigurationChangedEventArgs args)
		{
			if (logWriter != null)
			{
				try
				{
					LogWriterStructureHolder newStructureHolder
						= EnterpriseLibraryFactory.BuildUp<LogWriterStructureHolder>(configurationSource);

					logWriter.ReplaceStructureHolder(newStructureHolder);
				}
				catch (ConfigurationErrorsException configurationException)
				{
					logWriter.ReportConfigurationFailure(configurationException);
				}
			}
		}
	}
}
