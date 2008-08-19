//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    internal class App
    {
        private MainForm form;

        private App(string [] files)
        {
            this.form = new MainForm(files);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string [] args)
        {
            App app = new App(args);

            app.Run();
        }

        private void Run()
        {
            Application.Run(this.form);
        }
    }
}