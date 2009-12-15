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
using System.Windows.Shapes;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for TypeBrowser.xaml
    /// </summary>
    public partial class TypeBrowser : Window
    {
        public TypeBrowser()
        {
            InitializeComponent();
        }

        public TypeBrowser(TypeBrowserViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }

        private void TypesTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TypesTree_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TypesTree_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
