using System.Linq;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    public abstract class Context : ContainerContext
    {
        protected CachingSettings TestCachingSettings;
        protected SectionViewModel CachingViewModel;
        protected ElementCollectionViewModel TestCachesViewModel;

        protected DefaultElementCollectionAddCommand AddNewObjectCache;
        protected DefaultCollectionElementAddCommand AddNewInMemoryCache;
        protected DefaultCollectionElementAddCommand AddNewIsolatedStorageCache;

        protected override void Arrange()
        {
            base.Arrange();

            TestCachingSettings = new CachingSettings();

            CachingViewModel = SectionViewModel.CreateSection(Container, CachingSettings.SectionName,
                                                                          TestCachingSettings);

            TestCachesViewModel = (ElementCollectionViewModel)CachingViewModel.GetDescendentsOfType
                                      <NameTypeConfigurationElementCollection<CacheData, CustomCacheData>>().First();

            // Create the child
            AddNewObjectCache = TestCachesViewModel.Commands.OfType<DefaultElementCollectionAddCommand>().First();

            // Create the cache from the command
            AddNewInMemoryCache =
                (DefaultCollectionElementAddCommand)
                TestCachesViewModel.Commands.SelectMany(x => x.ChildCommands).ElementAt(0);
            AddNewIsolatedStorageCache =
                (DefaultCollectionElementAddCommand)
                TestCachesViewModel.Commands.SelectMany(x => x.ChildCommands).ElementAt(1);
        }
    }
}
