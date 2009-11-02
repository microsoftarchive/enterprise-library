using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console.Wpf.ViewModel.Services
{
    public class ConfigurationSourceDependency
    {
        public void OnCleared()
        {
            var handler = Cleared;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler Cleared;


    }
}
