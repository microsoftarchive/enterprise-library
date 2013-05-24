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
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    [DesignerCategory("Code")]
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        private const string ConfigurationFileName = "MsmqDistributor.exe.config";
        private string serviceName = string.Empty;
        private string serviceDependency = "Message Queuing";

        /// <summary/>
        /// <exclude/>
        public ProjectInstaller()
        {
            SetName();
            InitializeComponent();

            this.serviceProcessInstaller.Account = ServiceAccount.User;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.Password = null;

            InstallEventSource(this.serviceName, Resources.ApplicationLogName);
        }

        private void InstallEventSource(string sourceName, string logName)
        {
            EventLogInstaller defaultLogDestinationSinkNameInstaller = new EventLogInstaller();
            defaultLogDestinationSinkNameInstaller.Source = sourceName;
            defaultLogDestinationSinkNameInstaller.Log = logName;
            Installers.Add(defaultLogDestinationSinkNameInstaller);
        }

        private void SetName()
        {
            this.serviceName = DistributorService.DefaultApplicationName;

            string configurationFilepath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe.config");

            try
            {
                XmlDocument configurationDoc = new XmlDocument();
                using (var reader = XmlReader.Create(configurationFilepath))
                {
                    configurationDoc.Load(reader);

                    XmlNode serviceNameNode = configurationDoc.SelectSingleNode("/configuration/msmqDistributorSettings/@serviceName");

                    if (serviceNameNode != null && !string.IsNullOrEmpty(serviceNameNode.Value))
                    {
                        this.serviceName = serviceNameNode.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new LoggingException(Resources.InstallerCannotReadServiceName, ex);
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </devdoc>
        private void InitializeComponent()
        {
            // Add dependencies to the dependency array
            string[] dependencyArray = new string[] { this.serviceDependency };

            this.serviceProcessInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();

            this.serviceInstaller.ServiceName = this.serviceName;
            this.serviceInstaller.ServicesDependedOn = dependencyArray;

            this.Installers.AddRange(new Installer[]
                {
                    this.serviceProcessInstaller,
                    this.serviceInstaller
                });
        }
    }
}
