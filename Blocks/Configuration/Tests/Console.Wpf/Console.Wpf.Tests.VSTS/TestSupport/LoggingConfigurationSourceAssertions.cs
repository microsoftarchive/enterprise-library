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
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace Console.Wpf.Tests.VSTS.TestSupport
{
    public static class LoggingConfigurationSourceAssertions
    {
        public static ILoggingVerifier Logging(this ConfigurationSourceModel model)
        {
            return new LoggingVerifier(model);
        }

        private class LoggingVerifier : ILoggingVerifier, ITraceSourceVerifier, IListenerVerifier
        {
            private readonly ConfigurationSourceModel model;
            private SectionViewModel currentSection;
            private ElementViewModel currentCategory;
            private ElementViewModel currentListener;

            public LoggingVerifier(ConfigurationSourceModel model)
            {
                this.model = model;

                currentSection =
                    model.Sections.Where(x => x.ConfigurationType == typeof(LoggingSettings)).FirstOrDefault();

                Assert.IsNotNull(currentSection, "Could not locate LoggingSettings in ConfigurationSourceModel");
            }

            public ITraceSourceVerifier HasCategory(string categoryName)
            {
                currentCategory =
                    currentSection.DescendentConfigurationsOfType<TraceSourceData>().Where(x => x.Name == categoryName).
                        FirstOrDefault();

                Assert.IsNotNull(currentCategory, string.Format("Could not locate category {0}", categoryName));
                return this;
            }

            public ITraceSourceVerifier WithListener(string listenerName)
            {
                return WithListenerOfType<TraceListenerData>(listenerName);
            }

            public ITraceSourceVerifier WithListenerOfType<T>(string listenerName)
                where T : ConfigurationElement
            {
                var reference = currentCategory.DescendentConfigurationsOfType<TraceListenerReferenceData>().Where(
                    x => x.Name == listenerName).FirstOrDefault();

                Assert.IsNotNull(reference, string.Format("Could not locate listener {0} in category listener reference.", listenerName));

                var listener =
                    currentSection.DescendentConfigurationsOfType<T>().Where(x => x.Name == listenerName).FirstOrDefault
                        ();

                Assert.IsNotNull(listener, string.Format("Could not find trace listener {0} in section.", listenerName));

                return this;
            }

            public IListenerVerifier HasListener(string listenerName)
            {
                currentListener =
                    currentSection.DescendentConfigurationsOfType<TraceListenerData>().Where(x => x.Name == listenerName)
                        .FirstOrDefault();
                Assert.IsNotNull(currentListener, string.Format("Could not find trace listener {0} in section.", listenerName));

                return this;
            }

            public void HasFormatter(string formatterName)
            {
                var formatter = currentSection.DescendentConfigurationsOfType<FormatterData>().Where(x => x.Name == formatterName).
                    FirstOrDefault();
                Assert.IsNotNull(formatter, string.Format("Could not find formatter {0} in section.", formatterName));
            }

            public IListenerVerifier OfConfigurationType(Type type)
            {
                Assert.AreEqual(currentListener.ConfigurationType, type);
                return this;
            }

            public IListenerPropertyVerifier<T> OfConfigurationType<T>() where T:ConfigurationElement
            {
                return new ListenerVerifier<T>(currentListener);
            }

            private class ListenerVerifier<T> : IListenerPropertyVerifier<T> where T:ConfigurationElement
            {
                private readonly ElementViewModel listener;

                public ListenerVerifier(ElementViewModel listener)
                {
                    this.listener = listener;
                    Assert.AreEqual(listener.ConfigurationType, typeof(T));
                    
                }

                public IListenerPropertyVerifier<T> WithProperty(Expression<Func<T, bool>> propertyValidation)
                {
                    var assertion = propertyValidation.Compile();
                    Assert.IsTrue(assertion((T) listener.ConfigurationElement), string.Format(propertyValidation.ToString()));
                    return this;
                }
            }
        }

    }

    public interface ILoggingVerifier
    {
        ITraceSourceVerifier HasCategory(string categoryName);
        IListenerVerifier HasListener(string listerName);
        void HasFormatter(string formatterName);
    }

    public interface ITraceSourceVerifier
    {
        ITraceSourceVerifier WithListener(string listenerName);
        ITraceSourceVerifier WithListenerOfType<T>(string listenername) where T : ConfigurationElement;
    }

    public interface IListenerPropertyVerifier<T> where T:ConfigurationElement
    {
        IListenerPropertyVerifier<T> WithProperty(Expression<Func<T, bool>> propertyValidation);
    }

    public interface IListenerVerifier
    {
        IListenerVerifier OfConfigurationType(Type type);
        IListenerPropertyVerifier<T> OfConfigurationType<T>() where T : ConfigurationElement;
    }
}
