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

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Service.Properties;
using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Service
{
    internal static class Application
    {
        internal static int Main(string[] args)
        {
            if (args.Length == 0 && false == Environment.UserInteractive)
            {
                // Called by SCM
                ServiceBase.Run(new TraceEventServiceHost());
                return (int)ApplicationExitCode.Success;
            }

            var options = new ParameterOptions();
            var parameters = new ParameterSet()
            {
                { "i|install", Resources.InstallArgDescription, (p) => options.Install() },
                { "u|uninstall", Resources.UninstallArgDescription, (p) => options.Uninstall() },
                { "s|start", Resources.StartArgDescription, (p) => options.Start() },
                { "c|console", Resources.ConsoleArgDescription, (p) => options.ConsoleMode() },
                { "h|help|?", Resources.HelpArgDescription, (p) => options.ShowHelp(p) }
            };

            options.ShowHeader();

            if (!parameters.Parse(args))
            {
                options.ShowHelp(parameters);
                return (int)ApplicationExitCode.InputError;
            }

            return (int)options.ExitCode;
        }
    }
}
