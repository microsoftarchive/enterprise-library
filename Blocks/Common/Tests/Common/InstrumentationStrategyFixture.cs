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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
	[TestClass]
	public class InstrumentationStrategyFixture
	{
		[TestMethod]
		public void InstancesWithNamesWillBeAttachedToTheirInstrumentationListeners()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
							 new InstrumentationConfigurationSection(true, true, true, "fooApplicationInstanceName"));
			NamedSource namedSource = new NamedSource();
			BuilderContext context
				= new BuilderContext(
					new StrategyChain(),
					null,
					null,
					new PolicyList(GetPolicies(configSource)),
					NamedTypeBuildKey.Make<NamedSource>("Foo"),
					namedSource);
			InstrumentationStrategy strategy = new InstrumentationStrategy();
			strategy.PreBuildUp(context);

			Assert.IsTrue(((NamedSource)context.Existing).IsWired);
		}

		#region NamedMocks

		[InstrumentationListener(typeof(NameExpectingListener))]
		public class NamedSource : IInstrumentationEventProvider
		{
			public bool IsWired
			{
				get { return myA != null; }
			}

			[InstrumentationProvider("A")]
			public event EventHandler<EventArgs> myA;

			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}

		public class NameExpectingListener : InstrumentationListener
		{
			public NameExpectingListener(string instanceName,
										 bool a,
										 bool b,
										 bool c,
										 string applicationInstanceName)
				: base(instanceName, a, b, c, new NoPrefixNameFormatter()) { }

			[InstrumentationConsumer("A")]
			public void A(object sender,
						  EventArgs e) { }
		}

		#endregion

		[TestMethod]
		public void InstancesWithMadeUpNameWillBeAttachedToTheirInstrumentationListeners()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
							 new InstrumentationConfigurationSection(true, true, true, "fooApplicationInstanceName"));
			MadeUpNamedSource namedSource = new MadeUpNamedSource();
			BuilderContext context
				= new BuilderContext(
					new StrategyChain(),
					null,
					null,
					new PolicyList(GetPolicies(configSource)),
					NamedTypeBuildKey.Make<MadeUpNamedSource>(ConfigurationNameProvider.MakeUpName()),
					namedSource);
			InstrumentationStrategy strategy = new InstrumentationStrategy();
			strategy.PreBuildUp(context);

			Assert.IsTrue(((MadeUpNamedSource)context.Existing).IsWired);
		}

		[TestMethod]
		public void InstancesWithNoNameWillBeAttachedToTheirInstrumentationListeners()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
							 new InstrumentationConfigurationSection(true, true, true, "fooApplicationInstanceName"));
			UnnamedSource source = new UnnamedSource();
			BuilderContext context
				= new BuilderContext(
					new StrategyChain(),
					null,
					null,
					new PolicyList(GetPolicies(configSource)),
					NamedTypeBuildKey.Make<UnnamedSource>(),
					source);
			InstrumentationStrategy strategy = new InstrumentationStrategy();
			strategy.PreBuildUp(context);

			Assert.IsTrue(((UnnamedSource)context.Existing).IsWired);
		}

		#region NoNameMocks

		[InstrumentationListener(typeof(UnnamedListener))]
		public class MadeUpNamedSource : IInstrumentationEventProvider
		{
			public bool IsWired
			{
				get { return myA != null; }
			}

			[InstrumentationProvider("A")]
			public event EventHandler<EventArgs> myA;

			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}

		[InstrumentationListener(typeof(UnnamedListener))]
		public class UnnamedSource : IInstrumentationEventProvider
		{
			public bool IsWired
			{
				get { return myA != null; }
			}

			[InstrumentationProvider("A")]
			public event EventHandler<EventArgs> myA;

			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}

		public class UnnamedListener : InstrumentationListener
		{
			public UnnamedListener(bool a,
								   bool b,
								   bool c,
								   string applicationInstanceName)
				: base(a, b, c, new NoPrefixNameFormatter()) { }

			[InstrumentationConsumer("A")]
			public void A(object sender,
						  EventArgs e) { }
		}

		#endregion

		private static PolicyList GetPolicies(IConfigurationSource configurationSource)
		{
			PolicyList policyList = new PolicyList();
			policyList.Set<IConfigurationObjectPolicy>(new ConfigurationObjectPolicy(configurationSource),
													   typeof(IConfigurationSource));

			return policyList;
		}
	}
}
