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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Console.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                PrintUsage();
                return 1;
            }

            string mainConfigurationFile = Path.GetFullPath(args[0]);
            string configurationMergeFile = Path.GetFullPath(args[1]);
            string mergedConfigurationFile = (args.Length == 3) ? Path.GetFullPath(args[2]) : string.Empty;


            if (!File.Exists(mainConfigurationFile))
            {
                string errorMessage = string.Format(Resources.MainConfigurationFileNotFound, mainConfigurationFile);
                System.Console.WriteLine(errorMessage);

                return 1;
            }

            if (!File.Exists(configurationMergeFile))
            {
                string errorMessage = string.Format(Resources.ConfigurationMergeFileNotFound, configurationMergeFile);
                System.Console.WriteLine(errorMessage);

                return 1;
            }

            try
            {
                ConfigurationMerger mergeTool = new ConfigurationMerger(mainConfigurationFile, configurationMergeFile);
                mergeTool.MergeConfiguration(mergedConfigurationFile);

                string message = String.Format(Resources.MergeSucceeded, mergeTool.MergedConfigurationFile);
                System.Console.WriteLine(message);
                
            }
            catch (Exception e)
            {
                System.Console.WriteLine(Resources.UnhandledExceptionMessage);
                System.Console.WriteLine(e);
                return 1;
            }
            return 0;
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine(Resources.Usage);
        }
    }
}
