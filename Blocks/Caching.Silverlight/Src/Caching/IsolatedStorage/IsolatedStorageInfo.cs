using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class IsolatedStorageInfo : IIsolatedStorageInfo
    {
        public long AvailableFreeSpace
        {
            get
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return store.AvailableFreeSpace;
                }
            }
        }
    }
}
