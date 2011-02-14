//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel.Unity
{
    partial class WhenTheConfiguratorInitializesAUnityContainer
    {
        [TestMethod]
        public void ThenTheContainerGetsTheInterceptionExtension()
        {


            Assert.IsNotNull(this.container.Configure<Microsoft.Practices.Unity.InterceptionExtension.Interception>());
        }

        [TestMethod]
        public void ThenTheContainerGetsTheReaderWriterLockExtension()
        {
            Assert.IsNotNull(this.container.Configure<ReaderWriterLockExtension>());
        }
    }

    public abstract class ReconfigurableConfiguredContainerContext : ConfiguredContainerContext
    {
        protected abstract TypeRegistration GetUpdatedRegistration();

        protected override void Act()
        {
            base.Act();

            ActBeforeReconfigured();

            containerConfigurator.Reconfigure(
                new TypeRegistration[]{
                    GetUpdatedRegistration()
                });

        }

        protected virtual void ActBeforeReconfigured()
        {
        }
    }

    [TestClass]
    public class WhenATransientTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() =>
                    new TypeWithOneArgumentConstructor("original value"))
                {
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(
                        () => new TypeWithOneArgumentConstructor("modified value"))
                    {
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }

        [TestMethod]
        public void ThenTheContainerResolvesTheUpdatedRegistrations()
        {
            var instance = ResolveUnnamed<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("modified value", instance.ConstructorParameter);
        }

        [TestMethod]
        public void ThenTheUpdatedRegistrationStillHasTransientLifetime()
        {
            var instance = ResolveUnnamed<TypeWithOneArgumentConstructor>();
            var secondInstance = ResolveUnnamed<TypeWithOneArgumentConstructor>();

            Assert.AreNotSame(instance, secondInstance);
        }
    }

    [TestClass]
    public class WhenASingletonTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() =>
                new TypeWithOneArgumentConstructor("original value"))
                {
                    Lifetime = TypeRegistrationLifetime.Singleton
                };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(
                        () => new TypeWithOneArgumentConstructor("modified value")
                    )
                    {
                        Lifetime = TypeRegistrationLifetime.Singleton
                    };
        }

        [TestMethod]
        public void ThenTheContainerResolvesTheUpdatedRegistrations()
        {
            var instance = ResolveUnnamed<TypeWithOneArgumentConstructor>();
            Assert.AreEqual("modified value", instance.ConstructorParameter);
        }

        [TestMethod]
        public void ThenTheUpdatedRegistrationStillHasSingletonLifetime()
        {
            var instance = ResolveUnnamed<TypeWithOneArgumentConstructor>();
            var secondInstance = ResolveUnnamed<TypeWithOneArgumentConstructor>();

            Assert.AreSame(instance, secondInstance);
        }
    }

    [TestClass]
    public class WhenASingletonTypeRegistrationChangesAfterResolving : ReconfigurableConfiguredContainerContext
    {
        private TypeWithOneArgumentConstructor originalInstance;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(() =>
                new TypeWithOneArgumentConstructor("original value"))
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<TypeWithOneArgumentConstructor>(
                        () => new TypeWithOneArgumentConstructor("modified value")
                    )
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        protected override void ActBeforeReconfigured()
        {
            originalInstance = ResolveUnnamed<TypeWithOneArgumentConstructor>();
        }

        [TestMethod]
        public void ThenTheContainerResolvestheOriginalInstance()
        {
            var instance = ResolveUnnamed<TypeWithOneArgumentConstructor>();
            Assert.AreSame(originalInstance, instance);
        }
    }

    [TestClass]
    public class WhenAMappedSingletonTypeRegistrationChangesAfterResolving : ReconfigurableConfiguredContainerContext
    {
        private IFoo originalInstance;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                                              new TypeWithOneArgumentConstructor("original value"))
                       {
                           Lifetime = TypeRegistrationLifetime.Singleton,
                           IsDefault = true
                       };

        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(
                      () => new TypeWithOneArgumentConstructor("modified value"))
            {
                Lifetime = TypeRegistrationLifetime.Singleton,
                IsDefault = true
            };
        }

        protected override void ActBeforeReconfigured()
        {
            originalInstance = ResolveDefault<IFoo>();
        }

        [TestMethod]
        public void ThenTheContainerResolvestheOriginalInstance()
        {
            var instance = ResolveDefault<IFoo>();
            Assert.AreSame(originalInstance, instance);
            Assert.IsInstanceOfType(originalInstance, typeof(TypeWithOneArgumentConstructor));
        }
    }

    [TestClass]
    public class WhenASingletonConcreteTypeOnMappedTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Tofu())
            {
                Lifetime = TypeRegistrationLifetime.Singleton
            };
        }

        [TestMethod]
        public void ThenTheLifetimeShoudlStillBeSingleton()
        {
            var instance = ResolveUnnamed<IFoo>();
            var secondInstance = ResolveUnnamed<IFoo>();
            Assert.AreSame(instance, secondInstance);
        }
    }


    [TestClass]
    public class WhenADefaultTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            Assert.IsNotNull(container.Resolve<IFoo>());
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Name = "test instance",
                IsDefault = false,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void ThenResolveDefaultShouldThrow()
        {
            container.Resolve<IFoo>();
        }
    }

    [TestClass]
    public class WhenImplementationTypeForTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        private IFoo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    Name = "Foo"
                })
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = container.Resolve<IFoo>();
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Tofu())
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenResolveReturnsInstanceOfNewImplementationType()
        {
            IFoo instanceOfTfoe = container.Resolve<IFoo>();

            Assert.IsInstanceOfType(instanceOfTfoe, typeof(Tofu));
        }
    }


    [TestClass]
    public class WhenPropertySetsOnTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        private IFoo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    Name = "Foo"
                })
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = container.Resolve<IFoo>();
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                Name = "test instance",
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenPreviouslyRegisteredPropertiesAreNotSet()
        {
            Foo reconfiguredInstance = container.Resolve<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.IsNull(reconfiguredInstance.Name);

        }
    }

    [TestClass]
    public class WhenConstructorOnTypeRegistrationChanges : ReconfigurableConfiguredContainerContext
    {
        private Foo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo("string ctor"))
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = ResolveUnnamed<IFoo>() as Foo;
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo(42))
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenNewlyConfiguredConstrutorIsSet()
        {
            Assert.IsNotNull(instancePriorToReconfigure);
            Assert.AreEqual("string ctor", instancePriorToReconfigure.Name);

            Foo reconfiguredInstance = ResolveUnnamed<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.AreEqual(42.ToString(), reconfiguredInstance.Name);

        }
    }

    [TestClass]
    public class WhenConstructorOnTypeRegistrationChangesToParameterless : ReconfigurableConfiguredContainerContext
    {
        private Foo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo("string ctor"))
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = ResolveUnnamed<IFoo>() as Foo;
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo())
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenNewlyConfiguredConstrutorIsSet()
        {
            Assert.IsNotNull(instancePriorToReconfigure);
            Assert.AreEqual("string ctor", instancePriorToReconfigure.Name);

            Foo reconfiguredInstance = ResolveUnnamed<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.IsNull(reconfiguredInstance.Name);
        }
    }

    [TestClass]
    public class WhenNumberOfPropretiesOnTypeRegistrationChangesToFewer : ReconfigurableConfiguredContainerContext
    {
        private Foo instancePriorToReconfigure;

        protected override TypeRegistration GetTypeRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    Name = "Name",
                    LastName = "LastName",
                    Initials = "Initials"
                })
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }

        protected override void ActBeforeReconfigured()
        {
            instancePriorToReconfigure = ResolveUnnamed<IFoo>() as Foo;
        }

        protected override TypeRegistration GetUpdatedRegistration()
        {
            return new TypeRegistration<IFoo>(() =>
                new Foo()
                {
                    LastName = "different"
                })
            {
                IsDefault = true,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        [TestMethod]
        public void ThenNewlyConfiguredConstrutorIsSet()
        {
            Assert.IsNotNull(instancePriorToReconfigure);
            Assert.AreEqual("Name", instancePriorToReconfigure.Name);
            Assert.AreEqual("LastName", instancePriorToReconfigure.LastName);
            Assert.AreEqual("Initials", instancePriorToReconfigure.Initials);

            Foo reconfiguredInstance = ResolveUnnamed<IFoo>() as Foo;
            Assert.IsNotNull(reconfiguredInstance);
            Assert.IsNull(reconfiguredInstance.Name);
            Assert.AreEqual("different", reconfiguredInstance.LastName);
            Assert.IsNull(reconfiguredInstance.Initials);
        }
    }

    partial class TestableUnityConfigurator
    {
        public void Reconfigure(IEnumerable<TypeRegistration> updatedRegistrations)
        {
            base.RegisterUpdates(updatedRegistrations);
        }
    }
}
