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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
	[TestClass]
	public class InstrumentationAttacherFactoryFixture
	{
		[TestMethod]
		public void NoBinderInstanceReturnedIfNoAttributeOnSourceClass()
		{
			NoListenerSource source = new NoListenerSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);

			Assert.AreSame(typeof(NoBindingInstrumentationAttacher), createdAttacher.GetType());
		}

		[TestMethod]
		public void ReflectionBinderInstanceReturnedIfSingleArgumentAttributeOnSourceClass()
		{
			ReflectionBindingSource source = new ReflectionBindingSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);

			Assert.AreSame(typeof(ReflectionInstrumentationAttacher), createdAttacher.GetType());
		}

		[TestMethod]
		public void ExplicitBinderInstanceReturnedIfTwoArgumentAttributeOnSourceClass()
		{
			ExplicitBindingSource source = new ExplicitBindingSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);

			Assert.AreSame(typeof(ExplicitInstrumentationAttacher), createdAttacher.GetType());
		}

		public class NoListenerSource { }

		[InstrumentationListener(typeof(FooListener))]
		public class ReflectionBindingSource : IInstrumentationEventProvider
		{
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}

		[InstrumentationListener(typeof(FooListener), typeof(FooBinder))]
		public class ExplicitBindingSource : IInstrumentationEventProvider
		{
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}

		public class FooListener { }

		public class FooBinder { }
	}
}
