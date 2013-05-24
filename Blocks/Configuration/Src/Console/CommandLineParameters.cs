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
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    internal class CommandLineParameters
    {
        public CommandLineParameters(string[] arguments)
        {
            foreach (var argument in arguments)
            {
                string option, value;

                if (ExtractOption(argument, out option, out value))
                {
                    switch (option)
                    {
                        case "profile":
                            this.ProfileFileName = value;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    this.StartingConfigurationFileName = argument;
                }
            }
        }

        public string StartingConfigurationFileName { get; private set; }

        public string ProfileFileName { get; private set; }

        private static readonly Regex optionRegex = new Regex(@"^?-(?<option>\w+)(:(?<value>.*))?$");

        private static bool ExtractOption(string arg, out string option, out string value)
        {
            option = null;
            value = null;
            var match = optionRegex.Match(arg);

            if (match.Success)
            {
                option = match.Groups["option"].Value;
                value = match.Groups["value"].Value;
            }

            return match.Success;
        }
    }
}
