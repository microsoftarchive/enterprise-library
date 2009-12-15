using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Hosting
{
    /// <summary>
    /// Interaction logic for ConfigurationEditor.xaml
    /// </summary>
    public partial class ConfigurationEditorUI : UserControl, IUIService, IUIServiceWpf
    {
        public ConfigurationEditorUI()
        {
            InitializeComponent();
        }

        ConfigurationSourceModel configurationSourceModel;
        OpenEnvironmentConfigurationDeltaCommand openEnvironmentDeltaCommand;
        
        [InjectionMethod]
        public void Intialize(ConfigurationSourceModel configurationSourceModel, OpenEnvironmentConfigurationDeltaCommand openEnvironmentDeltaCommand, MenuCommandService commandService)
        {
            this.configurationSourceModel = configurationSourceModel;
            this.openEnvironmentDeltaCommand = openEnvironmentDeltaCommand;

            this.AddApplicationBlockMenu.ItemsSource = commandService.GetCommands(CommandPlacement.BlocksMenu);

            DataContext = configurationSourceModel;
        }

        public void UpdateSize(double height, double width)
        {
            this.Height = height;
            this.Width = width;
        }

        private void EnvironemntMenu_Open_Click(object sender, RoutedEventArgs e)
        {
            openEnvironmentDeltaCommand.Execute(null);
        }

        private void EnvironemntMenu_New_Click(object sender, RoutedEventArgs e)
        {
            configurationSourceModel.NewEnvironment();
        }

        #region infrastructural services
        bool IUIService.CanShowComponentEditor(object component)
        {
            throw new NotImplementedException();
        }

        System.Windows.Forms.IWin32Window IUIService.GetDialogOwnerWindow()
        {
            throw new NotImplementedException();
        }

        void IUIService.SetUIDirty()
        {
            throw new NotImplementedException();
        }

        bool IUIService.ShowComponentEditor(object component, System.Windows.Forms.IWin32Window parent)
        {
            throw new NotImplementedException();
        }

        System.Windows.Forms.DialogResult IUIService.ShowDialog(System.Windows.Forms.Form form)
        {
            throw new NotImplementedException();
        }

        void IUIService.ShowError(Exception ex, string message)
        {
            MessageBox.Show(string.Format("{0}\n\n{1}", message, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }

        void IUIService.ShowError(Exception ex)
        {
            throw new NotImplementedException();
        }

        void IUIService.ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        System.Windows.Forms.DialogResult IUIService.ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons buttons)
        {
            return System.Windows.Forms.MessageBox.Show(message, caption, buttons);
        }

        void IUIService.ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        void IUIService.ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        bool IUIService.ShowToolWindow(Guid toolWindow)
        {
            throw new NotImplementedException();
        }

        Dictionary<object, object> styles = new Dictionary<object, object>();
        System.Collections.IDictionary IUIService.Styles
        {
            get { return styles; }
        }
        public FileDialogResult ShowFileDialog(FileDialog dialog)
        {
            var result = dialog.ShowDialog();
            return new FileDialogResult { FileName = dialog.FileName, DialogResult = result };
        }


        public MessageBoxResult ShowMessageWpf(string message, string caption, MessageBoxButton buttons)
        {
            return MessageBox.Show(message, caption, buttons);
        }

        #endregion


    }
}
