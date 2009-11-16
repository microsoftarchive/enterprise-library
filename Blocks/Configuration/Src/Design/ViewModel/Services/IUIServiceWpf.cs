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
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    public interface IUIServiceWpf : IUIService
    {
        FileDialogResult ShowFileDialog(FileDialog dialog);

        MessageBoxResult ShowMessageWpf(string message, string caption, MessageBoxButton buttons);
    }


    public class FileDialogResult
    {
        public bool? DialogResult { get; set; }
        public string FileName { get; set; }
    }
}
