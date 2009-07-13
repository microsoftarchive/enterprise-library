//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration
{

    public abstract class Given_ConfigurationSourceBuilder : ArrangeActAssert
    {
        protected ConfigurationSourceBuilder configurationSourceBuilder;

        protected override void Arrange()
        {
            base.Arrange();
            configurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        protected IConfigurationSource GetConfigurationSource()
        {
            var configSource = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }
    }


    public abstract class Given_ExceptionPolicyInConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        
        protected IExceptionConfigurationForExceptionType policy;
        protected string policyName = "Some Policy";

        protected override void Arrange()
        {
            base.Arrange();

            policy = configurationSourceBuilder
                        .ConfigureExceptionHandling()
                            .GivenPolicyWithName(policyName);
        }

        protected ExceptionPolicyData GetExceptionPolicyData()
        {
            var source = GetConfigurationSource();
            return ((ExceptionHandlingSettings)source.GetSection(ExceptionHandlingSettings.SectionName))
                .ExceptionPolicies.Get(policyName);
        }
    }

    public abstract class Given_ExceptionTypeInConfigurationSourceBuilder : Given_ExceptionPolicyInConfigurationSourceBuilder
    {
        protected IExceptionConfigurationAddExceptionHandlers exception;
        protected Type exceptionType = typeof(ArgumentException);

        protected override void Arrange()
        {
            base.Arrange();

            exception = policy.ForExceptionType(exceptionType);
        }

        protected ExceptionTypeData GetExceptionTypeData()
        {
            return base.GetExceptionPolicyData().ExceptionTypes.Where(x => x.Type == exceptionType).First();
        }
    }

    public abstract class Given_ReplaceHandlerInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        protected IExceptionConfigurationForExceptionTypeOrPostHandling replaceException;
        protected Type replaceExceptionType = typeof(ApplicationException);

        protected override void Arrange()
        {
            base.Arrange();

            replaceException = exception.ReplaceWith(replaceExceptionType);
        }

        protected ReplaceHandlerData GetReplaceHandlerData()
        {
            return base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<ReplaceHandlerData>()
                .Where(x => x.ReplaceExceptionType == replaceExceptionType)
                .First();
        }
    }

    [TestClass]
    public class When_AddingExceptionPolicyConfigurationSourceBuilder : Given_ExceptionPolicyInConfigurationSourceBuilder
    {
        protected override void Act()
        {
           base.policy
                .ForExceptionType(typeof(ArgumentNullException))
                    .ReplaceWith<ApplicationException>()
                        .UsingMessage("An exception ocurred somewhere")
                    .ThenDoNothing();
        }


        [TestMethod]
        public void Then_ConfigurationSectionMatchesConfiguration()
        {
            var policyData = base.GetExceptionPolicyData();
            Assert.AreEqual(policyName, policyData.Name);

            var exceptionData = policyData.ExceptionTypes.Single<ExceptionTypeData>();
            Assert.AreSame(typeof(ArgumentNullException), exceptionData.Type);
            Assert.AreEqual(PostHandlingAction.None, exceptionData.PostHandlingAction);
            
            ReplaceHandlerData handlerData = exceptionData.ExceptionHandlers.Single() as ReplaceHandlerData;
            Assert.AreEqual("An exception ocurred somewhere", handlerData.ExceptionMessage);
        }
    }

    [TestClass]
    public class When_AddingTwoExceptionPoliciesToConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected override void Act()
        {
            configurationSourceBuilder
                .ConfigureExceptionHandling()
                    .GivenPolicyWithName("somePolicy")
                        .ForExceptionType(typeof(ArgumentNullException))
                            .ReplaceWith(typeof(ApplicationException)).UsingMessage("An exception ocurred somewhere")
                            .ThenDoNothing()
                    .GivenPolicyWithName("anotherPolicy")
                        .ForExceptionType(typeof(TimeZoneNotFoundException))
                            .ThenNotifyRethrow();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsBothPolicies()
        {
            var exceptionHandlingSection = (ExceptionHandlingSettings)GetConfigurationSource().GetSection(ExceptionHandlingSettings.SectionName);
            Assert.AreEqual(2, exceptionHandlingSection.ExceptionPolicies.Count());
        }
    }

    [TestClass]
    public class When_AddingTwoTypesToSinglePolicy : Given_ConfigurationSourceBuilder
    {
        protected override void Act()
        {
            configurationSourceBuilder
                .ConfigureExceptionHandling()
                    .GivenPolicyWithName("somePolicy")
                        .ForExceptionType(typeof(ArgumentNullException))
                            .ReplaceWith(typeof(ApplicationException)).UsingMessage("An exception ocurred somewhere")
                            .ThenDoNothing()
                    .GivenPolicyWithName("anotherPolicy")
                        .ForExceptionType(typeof(TimeZoneNotFoundException))
                            .ThenNotifyRethrow()
                        .ForExceptionType(typeof(ArithmeticException));

        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsBothPolicies()
        {
            ExceptionHandlingSettings settings = (ExceptionHandlingSettings)GetConfigurationSource().GetSection(ExceptionHandlingSettings.SectionName);

            var anotherPolicy = settings.ExceptionPolicies.Get("anotherPolicy");
            Assert.AreEqual(2, anotherPolicy.ExceptionTypes.Count());
            Assert.IsTrue(anotherPolicy.ExceptionTypes.Any(x => x.Type == typeof(TimeZoneNotFoundException))); ;
            Assert.IsTrue(anotherPolicy.ExceptionTypes.Any(x => x.Type == typeof(ArithmeticException)));
        }
    }

    [TestClass]
    public class When_AddingPolicyToConfigurationSourceBuilder : Given_ExceptionPolicyInConfigurationSourceBuilder
    {
        [TestMethod]
        public void Then_ExceptionPolicyHasNoExceptions()
        {
            Assert.AreEqual(0, GetExceptionPolicyData().ExceptionTypes.Count());
        }
    }

    [TestClass]
    public class When_AddingExceptionTypeToConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        public void Then_ExceptionTypeHasNoHandlers()
        {
            Assert.AreEqual(0, GetExceptionTypeData().ExceptionHandlers.Count());
        }

        [TestMethod]
        public void Then_ExceptionTypeHasHandlingActionNotifyRetrow()
        {
            Assert.AreEqual(PostHandlingAction.NotifyRethrow, GetExceptionTypeData().PostHandlingAction);
        }
    }

    [TestClass]
    public class When_CallingDoNothingOnExceptionTypeInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.exception.ThenDoNothing();
        }

        [TestMethod]
        public void Then_ExceptionTypeHasHandlingActionNone()
        {
            Assert.AreEqual(PostHandlingAction.None, GetExceptionTypeData().PostHandlingAction);
        }
    }

    [TestClass]
    public class When_CallingThrowNewExceptionOnExceptionTypeInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.exception.ThenThrowNewException();
        }

        [TestMethod]
        public void Then_ExceptionTypeHasHandlingActionNone()
        {
            Assert.AreEqual(PostHandlingAction.ThrowNewException, GetExceptionTypeData().PostHandlingAction);
        }
    }

    [TestClass]
    public class When_WrappingReplacedExceptionInConfigurationSourceBuilder : Given_ReplaceHandlerInConfigurationSourceBuilder
    {
        private Type wrapExceptionType = typeof(InvalidCastException);
        string wrapExceptionMessage = "wrap message";

        protected override void Act()
        {
            replaceException.WrapWith(wrapExceptionType).UsingMessage(wrapExceptionMessage);

        }

        [TestMethod]
        public void Then_ExceptionExceptionTypeHas2Handlers()
        {
            var exceptionTypeData = GetExceptionTypeData();

            Assert.AreEqual(2, exceptionTypeData.ExceptionHandlers.Count);
        }
        
        [TestMethod]
        public void Then_ExceptionHandlersAreInCorrectOrder()
        {
            var exceptionTypeData = GetExceptionTypeData();

            Assert.AreEqual(typeof(WrapHandlerData), exceptionTypeData.ExceptionHandlers.Get(1).GetType());
        }
        
        [TestMethod]
        public void Then_WrapExceptionHandlersHasApproriateMessage()
        {
            var exceptionTypeData = GetExceptionTypeData();
            WrapHandlerData wrapHandler = (WrapHandlerData) exceptionTypeData.ExceptionHandlers.Get(1);
            Assert.AreEqual(wrapExceptionMessage, wrapHandler.ExceptionMessage);
        }
    }

    [TestClass]
    public class When_WrappingUsingGenericOverload : Given_ReplaceHandlerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            replaceException.WrapWith<InvalidCastException>();
        }

        [TestMethod]
        public void Then_WrapExceptionTypeIsProvided()
        {
            var exceptionTypeData = GetExceptionTypeData();
            WrapHandlerData wrapHandler = (WrapHandlerData)exceptionTypeData.ExceptionHandlers.Get(1);
            Assert.AreEqual(typeof(InvalidCastException), wrapHandler.WrapExceptionType);
        }

    }

    [TestClass]
    public class When_WrappingReplacedExceptionUsingLocalizedMessageInConfigurationSourceBuilder : Given_ReplaceHandlerInConfigurationSourceBuilder
    {
        private Type wrapExceptionType = typeof(InvalidCastException);
        Type wrapMessageResourceType = typeof(object);
        string wrapMessageResourceName = "messageName";

        protected override void Act()
        {
            replaceException.WrapWith(wrapExceptionType).UsingResourceMessage(wrapMessageResourceType, wrapMessageResourceName);
        }

        [TestMethod]
        public void Then_WrapExceptionHandlersHasApproriateMessage()
        {
            var exceptionTypeData = GetExceptionTypeData();
            WrapHandlerData wrapHandler = (WrapHandlerData)exceptionTypeData.ExceptionHandlers.Get(1);
            Assert.AreEqual(wrapExceptionType, wrapHandler.WrapExceptionType);
            Assert.AreEqual(wrapMessageResourceName, wrapHandler.ExceptionMessageResourceName);
            Assert.AreEqual(wrapMessageResourceType.AssemblyQualifiedName, wrapHandler.ExceptionMessageResourceType);
        }
    }

    [TestClass]
    public class When_AddingCustomHandlerToExceptionTypeInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        private Type customHandlerType = typeof(object);

        protected override void Act()
        {
            exception.HandleCustom(customHandlerType);
        }

        [TestMethod]
        public void ThenExceptionTypeContainsCustomHandler()
        {
            Assert.IsTrue(GetExceptionTypeData().ExceptionHandlers.Any(x => x.GetType() == typeof(CustomHandlerData)));
        }

        [TestMethod]
        public void ThenCustomHandlerHasSpecifiedType()
        {
            CustomHandlerData customHandler = (CustomHandlerData) GetExceptionTypeData()
                .ExceptionHandlers
                .Where(x => x.GetType() == typeof(CustomHandlerData))
                .First();

            Assert.AreEqual(customHandlerType, customHandler.Type);
        }

        [TestMethod]
        public void ThenCustomHandlerHasNoAttributes()
        {
            CustomHandlerData customHandler = (CustomHandlerData)GetExceptionTypeData()
                .ExceptionHandlers
                .Where(x => x.GetType() == typeof(CustomHandlerData))
                .First();

            Assert.AreEqual(0, customHandler.Attributes.Count);
        }
    }

    [TestClass]
    public class When_AddingCustomHandlerWithAttributesToExceptionTypeInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        private Type customHandlerType = typeof(object);
        private NameValueCollection attributes = new NameValueCollection();

        protected override void Act()
        {
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            exception.HandleCustom(customHandlerType, attributes);
        }

        [TestMethod]
        public void ThenCustomHandlerContainsAllAttributes()
        {
            CustomHandlerData customHandler = (CustomHandlerData)GetExceptionTypeData()
                  .ExceptionHandlers
                  .Where(x => x.GetType() == typeof(CustomHandlerData))
                  .First();

            Assert.AreEqual(attributes.Count, customHandler.Attributes.Count);
            foreach (string key in attributes)
            {
                Assert.AreEqual(attributes[key], customHandler.Attributes[key]);
            }
        }
    }
}
