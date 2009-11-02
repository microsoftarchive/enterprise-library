using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console.Wpf.ViewModel.Services
{
    public interface IPropertyDirtyStateListener
    {
        void SetDirty();
    }
}
