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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{

    [TemplatePart(Name = "PART_Header", Type = typeof(DockPanel))]
    public class SectionModelContainer : Control
    {
        private DockPanel Header;
        static SectionModelContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SectionModelContainer), new FrameworkPropertyMetadata(typeof(SectionModelContainer)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Header = (DockPanel)Template.FindName("PART_Header", this);
            Header.MouseLeftButtonDown += new MouseButtonEventHandler(Header_MouseLeftButtonDown);
        }

        void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SectionViewModel section = DataContext as SectionViewModel;
            if (section != null)
            {
                section.Select();
            }
        }
    }
}
