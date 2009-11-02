using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using System.Windows;

namespace Console.Wpf.ViewModel.Services
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
