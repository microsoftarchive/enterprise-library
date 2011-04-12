using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Design.ViewModel.BlockSpecifics.Caching
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class CachingSettingsViewModel : SectionViewModel
    {
        public CachingSettingsViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }

        protected override object CreateBindable()
        {
            var caches = DescendentElements()
                .Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<CacheData, CustomCacheData>))
                .First();

            return new HorizontalListLayout(
                new HeaderedListLayout(caches));
        }
    }
#pragma warning restore 1591
}
