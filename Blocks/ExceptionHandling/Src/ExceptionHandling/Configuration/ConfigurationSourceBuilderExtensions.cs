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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationSourceBuilder"/> extensions to support creation of exception handling configuration sections.
    /// </summary>
    public static class ExceptionHandlingConfigurationSourceBuilderExtensions
    {

        /// <summary>
        /// Main entry point to configuration a <see cref="ExceptionHandlingSettings"/> section.
        /// </summary>
        /// <param name="configurationSourceBuilder">The builder interface to extend.</param>
        /// <returns></returns>
        public static IExceptionConfigurationGivenPolicyWithName ConfigureExceptionHandling(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            return new ExceptionPolicyBuilder(configurationSourceBuilder);
        }


        private class ExceptionPolicyBuilder :
            IExceptionConfigurationGivenPolicyWithName,
            IExceptionConfigurationForExceptionType,
            IExceptionConfigurationAddExceptionHandlers,
            IExceptionConfigurationThenDoPostHandlingAction,
            IExceptionConfigurationForExceptionTypeOrPostHandling,
            IExceptionConfigurationReplaceWithMessage,
            IExceptionConfigurationWrapWithMessage,
            IExceptionHandlerExtension
        {
            ExceptionHandlingSettings section = new ExceptionHandlingSettings();
            ExceptionPolicyData currentPolicy;
            ExceptionTypeData currentExceptionTypeData;
            ReplaceHandlerData currentHandlerData;
            WrapHandlerData currentWrapHandlerData;

            internal ExceptionPolicyBuilder(IConfigurationSourceBuilder configurationSourceBuilder)
            {
                configurationSourceBuilder.AddSection(ExceptionHandlingSettings.SectionName, section);
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationGivenPolicyWithName.GivenPolicyWithName(string name)
            {
                currentPolicy = new ExceptionPolicyData(name);
                section.ExceptionPolicies.Add(currentPolicy);
                return this;
            }

            IExceptionConfigurationAddExceptionHandlers IExceptionConfigurationForExceptionType.ForExceptionType(Type exceptionType)
            {
                currentExceptionTypeData = new ExceptionTypeData();

                currentExceptionTypeData.Name = exceptionType.FullName;
                currentExceptionTypeData.Type = exceptionType;
                currentPolicy.ExceptionTypes.Add(currentExceptionTypeData);

                return this;
            }


            IExceptionConfigurationReplaceWithMessage IExceptionConfigurationAddExceptionHandlers.ReplaceWith(Type replaceExeptionType)
            {
                BuildReplaceHandlerData(replaceExeptionType);
                return this;
            }

            IExceptionConfigurationReplaceWithMessage IExceptionConfigurationAddExceptionHandlers.ReplaceWith<T>()
            {
                return ((IExceptionConfigurationAddExceptionHandlers) this).ReplaceWith(typeof (T));
            }

            private void BuildReplaceHandlerData(Type replaceExeptionType)
            {
                currentHandlerData = new ReplaceHandlerData();
                currentHandlerData.Name = replaceExeptionType.FullName;
                currentHandlerData.ReplaceExceptionType = replaceExeptionType;
                currentExceptionTypeData.ExceptionHandlers.Add(currentHandlerData);
            }

            IExceptionConfigurationWrapWithMessage IExceptionConfigurationAddExceptionHandlers.WrapWith<T>()
            {
                return ((IExceptionConfigurationAddExceptionHandlers) this).WrapWith(typeof (T));
            }

            IExceptionConfigurationWrapWithMessage IExceptionConfigurationAddExceptionHandlers.WrapWith(Type wrapExceptionType)
            {
                currentWrapHandlerData = new WrapHandlerData()
                {
                    Name = wrapExceptionType.FullName,
                    WrapExceptionType = wrapExceptionType
                };

                currentExceptionTypeData.ExceptionHandlers.Add(currentWrapHandlerData);

                return this;
            }


            IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenDoNothing()
            {
                currentExceptionTypeData.PostHandlingAction = PostHandlingAction.None;

                return this;
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenNotifyRethrow()
            {
                currentExceptionTypeData.PostHandlingAction = PostHandlingAction.NotifyRethrow;

                return this;
            }

            IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenThrowNewException()
            {
                currentExceptionTypeData.PostHandlingAction = PostHandlingAction.ThrowNewException;

                return this;
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom(Type customHandlerType)
            {
                return HandleCustom(customHandlerType, new NameValueCollection());
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom<T>() where T:IExceptionHandler
            {
                return HandleCustom(typeof (T), new NameValueCollection());
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom(Type customHandlerType, NameValueCollection customHandlerSettings)
            {
                CustomHandlerData custumHandler = new CustomHandlerData
                   {
                       Name = customHandlerType.FullName,
                       Type = customHandlerType
                   };

                custumHandler.Attributes.Add(customHandlerSettings);

                currentExceptionTypeData.ExceptionHandlers.Add(custumHandler);
                return this;
            }

            public IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom<T>(NameValueCollection customHandlerSettings) where T : IExceptionHandler
            {
                return HandleCustom(typeof (T), customHandlerSettings);
            }

            ExceptionTypeData IExceptionHandlerExtension.CurrentExceptionTypeData
            {
                get { return currentExceptionTypeData; }
            }

            IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationReplaceWithMessage.UsingMessage(string message)
            {
                currentHandlerData.ExceptionMessage = message;
                return this;
            }

            IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationReplaceWithMessage.UsingResourceMessage(Type resourceType, string resourceName)
            {
                currentHandlerData.ExceptionMessageResourceName = resourceName;
                currentHandlerData.ExceptionMessageResourceType = resourceType.FullName;
                return this;
            }


            IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationWrapWithMessage.UsingMessage(string message)
            {
                currentWrapHandlerData.ExceptionMessage = message;
                return this;
            }

            IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationWrapWithMessage.UsingResourceMessage(Type resourceType, string resourceName)
            {
                currentWrapHandlerData.ExceptionMessageResourceType = resourceType.AssemblyQualifiedName;
                currentWrapHandlerData.ExceptionMessageResourceName = resourceName;
                return this;
            }
        }
    }

    /// <summary>
    /// Used to provide context to extensions of the Exception Handling fluent configuration interface.
    /// </summary>
    public interface IExceptionHandlerExtension : IFluentInterface
    {

        /// <summary>
        /// Retrieves data about the currently built up ExceptionTypeData.
        /// </summary>
        ExceptionTypeData CurrentExceptionTypeData { get; }
    }

    /// <summary>
    /// Exception policy fluent interface.
    /// </summary>
    public interface IExceptionConfigurationGivenPolicyWithName : IFluentInterface
    {
        /// <summary>
        /// Defines new policy with a given name.
        /// </summary>
        /// <param name="name">Name of policy</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionType GivenPolicyWithName(string name);
    }

    /// <summary/>
    public interface IExceptionConfigurationForExceptionType : IExceptionConfigurationGivenPolicyWithName
    {
        /// <summary/>
        IExceptionConfigurationAddExceptionHandlers ForExceptionType(Type exceptionType);
    }

    /// <summary/>
    public interface IExceptionConfigurationForExceptionTypeOrPostHandling :
        IExceptionConfigurationGivenPolicyWithName,
        IExceptionConfigurationAddExceptionHandlers,
        IExceptionConfigurationThenDoPostHandlingAction
    {
    }

    /// <summary>
    /// Defines interface for adding messages when configuring a <see cref="ReplaceHandler"/> for an exception.
    /// <seealso cref="ReplaceHandlerData"/>
    /// </summary>
    public interface IExceptionConfigurationReplaceWithMessage : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        /// <summary>
        /// Use the provided message as part of the new exception.
        /// </summary>
        /// <param name="message">Message to use when wrapping another exception.</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionTypeOrPostHandling UsingMessage(string message);

        /// <summary>
        /// Use the message in the specified resource file and name.
        /// </summary>
        /// <param name="resourceType">The type from the assembly with the resource to use for a message</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionTypeOrPostHandling UsingResourceMessage(Type resourceType, string resourceName);
    }

    /// <summary>
    /// Defines interface for adding messages when configuring a <see cref="WrapHandler"/> for an exception.
    /// <seealso cref="WrapHandlerData"/>
    /// </summary>
    public interface IExceptionConfigurationWrapWithMessage : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        /// <summary>
        /// Use the provided message as part of the new exception.
        /// </summary>
        /// <param name="message">Message to use when wrapping another exception.</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionTypeOrPostHandling UsingMessage(string message);

        /// <summary>
        /// Use the message in the specified resource file and name.
        /// </summary>
        /// <param name="resourceType">The type from the assembly with the resource to use for a message</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionTypeOrPostHandling UsingResourceMessage(Type resourceType, string resourceName);
    }


    /// <summary />
    public abstract class ExceptionConfigurationAddExceptionHandlers : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        IExceptionConfigurationForExceptionTypeOrPostHandling context;

        /// <summary />
        protected ExceptionConfigurationAddExceptionHandlers(IExceptionConfigurationForExceptionTypeOrPostHandling context)
        {
            this.context = context;
        }

        IExceptionConfigurationReplaceWithMessage IExceptionConfigurationAddExceptionHandlers.ReplaceWith(Type replaceExeptionType)
        {
            return context.ReplaceWith(replaceExeptionType);
        }

        IExceptionConfigurationReplaceWithMessage IExceptionConfigurationAddExceptionHandlers.ReplaceWith<T>()
        {
            return context.ReplaceWith(typeof(T));
        }


        IExceptionConfigurationWrapWithMessage IExceptionConfigurationAddExceptionHandlers.WrapWith(Type wrapExceptionType)
        {
            return context.WrapWith(wrapExceptionType);
        }

        IExceptionConfigurationWrapWithMessage IExceptionConfigurationAddExceptionHandlers.WrapWith<T>()
        {
            return context.WrapWith(typeof (T));
        }

        IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationAddExceptionHandlers.HandleCustom(Type customHandlerType)
        {
            return context.HandleCustom(customHandlerType);
        }

        IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationAddExceptionHandlers.HandleCustom<T>()
        {
            return context.HandleCustom<T>();
        }

        IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationAddExceptionHandlers.HandleCustom(Type customHandlerType, System.Collections.Specialized.NameValueCollection attributes)
        {
            return context.HandleCustom(customHandlerType, attributes);
        }

        IExceptionConfigurationForExceptionTypeOrPostHandling IExceptionConfigurationAddExceptionHandlers.HandleCustom<T>(NameValueCollection customHandlerSettings)
        {
            return context.HandleCustom<T>(customHandlerSettings);
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenDoNothing()
        {
            return context.ThenDoNothing();
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenNotifyRethrow()
        {
            return context.ThenNotifyRethrow();
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenThrowNewException()
        {
            return context.ThenThrowNewException();
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationGivenPolicyWithName.GivenPolicyWithName(string name)
        {
            return context.GivenPolicyWithName(name);
        }
    }

    /// <summary/>
    public interface IExceptionConfigurationAddExceptionHandlers : IExceptionConfigurationThenDoPostHandlingAction
    {
        /// <summary>
        /// Replace exception with new exception type.
        /// </summary>
        /// <param name="replaceExeptionType">Replacement <see cref="Exception"/> type.</param>
        /// <returns></returns>
        IExceptionConfigurationReplaceWithMessage ReplaceWith(Type replaceExeptionType);

        /// <summary>
        /// Replace exception with new exception type.
        /// </summary>
        /// <typeparam name="T">Replacement <see cref="Exception"/> type.</typeparam>
        /// <returns></returns>
        IExceptionConfigurationReplaceWithMessage ReplaceWith<T>() where T : Exception;

        /// <summary>
        /// Wrap exception with the new exception type.
        /// </summary>
        /// <param name="wrapExceptionType">Type of <see cref="Exception"/>to wrap existing exception with.</param>
        /// <returns></returns>
        IExceptionConfigurationWrapWithMessage WrapWith(Type wrapExceptionType);

        /// <summary>
        /// Wrap exception with the new exception type.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="Exception"/> to wrap existing exception with.</typeparam>
        /// <returns></returns>
        IExceptionConfigurationWrapWithMessage WrapWith<T>() where T : Exception;


        /// <summary>
        /// Handle with a custom Exception handler.
        /// </summary>
        /// <param name="customHandlerType">Custom handler type</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom(Type customHandlerType);

        ///<summary>
        /// Handle with a custom Exception handler.
        ///</summary>
        ///<typeparam name="T">Custom handler type</typeparam>
        ///<returns></returns>
        IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom<T>() where T : IExceptionHandler;

        /// <summary/>
        IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom(Type customHandlerType, NameValueCollection nameValueCollection);

        /// <summary/>
        IExceptionConfigurationForExceptionTypeOrPostHandling HandleCustom<T>(NameValueCollection nameValueCollection)
            where T : IExceptionHandler;

    }

    /// <summary />
    public interface IExceptionConfigurationThenDoPostHandlingAction : IFluentInterface
    {
        /// <summary/>
        IExceptionConfigurationForExceptionType ThenDoNothing();

        /// <summary/>
        IExceptionConfigurationForExceptionType ThenNotifyRethrow();

        /// <summary/>
        IExceptionConfigurationForExceptionType ThenThrowNewException();
    }

}
