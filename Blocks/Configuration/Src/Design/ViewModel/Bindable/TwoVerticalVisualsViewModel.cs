using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class TwoVerticalVisualsViewModel : ViewModel
    {
        ViewModel first, second;

        public TwoVerticalVisualsViewModel(ViewModel first, ViewModel second)
        {
            this.first = first;
            this.second = second;
        }

        public ViewModel First
        {
            get { return first; }
        }

        public ViewModel Second
        {
            get { return second; }
        }
    }
}
