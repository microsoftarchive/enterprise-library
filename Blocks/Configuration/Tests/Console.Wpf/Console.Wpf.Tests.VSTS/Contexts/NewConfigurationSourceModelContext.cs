using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging
{
    public abstract class NewConfigurationSourceModelContext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
        }

        protected ConfigurationSourceModel ConfigurationSourceModel { get; private set; }
    }
}