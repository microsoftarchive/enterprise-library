using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    public abstract class given_host_adapter_and_type_descriptor : given_host_adapter
    {
        protected ICustomTypeDescriptor CacheManagerTypeDescriptor;

        protected override void Arrange()
        {
            base.Arrange();

            CacheManagerTypeDescriptor = new ComponentModelElement(base.CacheManager, HostAdapter);
        }

    }
}
