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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS.TestSupport
{
    /// <summary>
    /// Helper class to build up <see cref="ExceptionHandlerSetting"/> objects
    /// programatically using a fluent builder pattern.
    /// </summary>
    public class ExceptionSectionBuilder : IAddPolicy, IAddTo
    {
        private readonly List<IExceptionPolicyBuilder> builders = new List<IExceptionPolicyBuilder>();

        public IAddException AddPolicy(string name)
        {
            var builder = new ExceptionPolicyBuilder(name, this);
            builders.Add(builder);
            return builder;
        }

        public void AddTo(IConfigurationSource configurationSource)
        {
            var section = new ExceptionHandlingSettings();

            foreach (var builder in builders)
            {
                section.ExceptionPolicies.Add(builder.GetPolicyData());
            }

            configurationSource.Add(ExceptionHandlingSettings.SectionName,
                section);
        }
    }

    public class ExceptionPolicyBuilder : IExceptionPolicyBuilder, IAddPolicy, IAddException, IAddTo
    {
        private readonly ExceptionPolicyData policyData = new ExceptionPolicyData();
        private readonly List<IExceptionTypeBuilder> builders = new List<IExceptionTypeBuilder>();
        private readonly ExceptionSectionBuilder parent;

        public ExceptionPolicyBuilder(string name, ExceptionSectionBuilder parent)
        {
            policyData.Name = name;
            this.parent = parent;
        }

        ExceptionPolicyData IExceptionPolicyBuilder.GetPolicyData()
        {
            foreach (var builder in builders)
            {
                policyData.ExceptionTypes.Add(builder.GetTypeData());
            }
            return policyData;
        }

        IAddException IAddPolicy.AddPolicy(string name)
        {
            return ((IAddPolicy)parent).AddPolicy(name);
        }

        public INamedException AddException(Type exceptionType)
        {
            var builder = new ExceptionTypeBuilder(exceptionType, this);
            builders.Add(builder);
            return builder;
        }

        INamedException IAddException.AddException<TException>()
        {
            return AddException(typeof(TException));
        }

        void IAddTo.AddTo(IConfigurationSource configurationSource)
        {
            ((IAddTo)parent).AddTo(configurationSource);
        }
    }

    public class ExceptionTypeBuilder : IExceptionTypeBuilder, INamedException, IPostHandlingAction, IAddExceptionAddHandler, IAddTo
    {
        private readonly List<IHandlerBuilder> builders = new List<IHandlerBuilder>();
        private readonly ExceptionTypeData exceptionData = new ExceptionTypeData();
        private readonly ExceptionPolicyBuilder parent;

        public ExceptionTypeBuilder(Type exceptionType, ExceptionPolicyBuilder parent)
        {
            exceptionData.Type = exceptionType;
            this.parent = parent;
        }

        ExceptionTypeData IExceptionTypeBuilder.GetTypeData()
        {
            foreach (var builder in builders)
            {
                exceptionData.ExceptionHandlers.Add(builder.GetHandlerData());
            }
            return exceptionData;
        }

        public IPostHandlingAction Named(string name)
        {
            exceptionData.Name = name;
            return this;
        }

        IAddExceptionAddHandler IPostHandlingAction.NoPostHandling
        {
            get
            {
                exceptionData.PostHandlingAction = PostHandlingAction.None;
                return this;
            }
        }

        IAddExceptionAddHandler IPostHandlingAction.NotifyRethrow
        {
            get
            {
                exceptionData.PostHandlingAction = PostHandlingAction.NotifyRethrow;
                return this;
            }
        }

        IAddExceptionAddHandler IPostHandlingAction.ThrowNew
        {
            get
            {
                exceptionData.PostHandlingAction = PostHandlingAction.ThrowNewException;
                return this;
            }
        }

        INamedException IAddException.AddException(Type exceptionType)
        {
            return ((IAddException)parent).AddException(exceptionType);
        }

        INamedException IAddException.AddException<TException>()
        {
            return ((IAddException)this).AddException(typeof(TException));
        }

        INamedWrapHandler IAddHandler.AddWrapHandler()
        {
            var builder = new WrapHandlerBuilder(this);
            builders.Add(builder);
            return builder;
        }

        void IAddTo.AddTo(IConfigurationSource configurationSource)
        {
            ((IAddTo)parent).AddTo(configurationSource);
        }
    }

    public class WrapHandlerBuilder : IHandlerBuilder, IAddHandlerAddPolicyAddTo, INamedWrapHandler, IWrapWith, IWithMessageWrapHandler, IAddException
    {
        private readonly WrapHandlerData handlerData = new WrapHandlerData();
        private readonly ExceptionTypeBuilder parent;

        public WrapHandlerBuilder(ExceptionTypeBuilder parent)
        {
            this.parent = parent;
        }

        public IWithMessageWrapHandler Named(string name)
        {
            handlerData.Name = name;
            return this;
        }

        IWrapWith IWithMessageWrapHandler.WithMessage(string message)
        {
            handlerData.ExceptionMessage = message;
            return this;
        }

        IAddHandlerAddPolicyAddTo IWrapWith.WrapWith(Type wrappingType)
        {
            handlerData.WrapExceptionType = wrappingType;
            return this;
        }

        IAddHandlerAddPolicyAddTo IWrapWith.WrapWith<TWrapping>()
        {
            return ((IWrapWith)this).WrapWith(typeof(TWrapping));
        }

        ExceptionHandlerData IHandlerBuilder.GetHandlerData()
        {
            return handlerData;
        }

        INamedWrapHandler IAddHandler.AddWrapHandler()
        {
            return ((IAddHandler)parent).AddWrapHandler();
        }

        IAddException IAddPolicy.AddPolicy(string name)
        {
            return ((IAddPolicy)parent).AddPolicy(name);
        }

        void IAddTo.AddTo(IConfigurationSource configurationSource)
        {
            ((IAddTo)parent).AddTo(configurationSource);
        }

        INamedException IAddException.AddException(Type exceptionType)
        {
            return ((IAddException)parent).AddException(exceptionType);
        }

        INamedException IAddException.AddException<TException>()
        {
            return ((IAddException)this).AddException(typeof(TException));
        }
    }

    public interface IAddPolicy
    {
        IAddException AddPolicy(string name);
    }

    public interface IAddException
    {
        INamedException AddException(Type exceptionType);
        INamedException AddException<TException>();
    }

    public interface INamedException
    {
        IPostHandlingAction Named(string name);
    }

    public interface IPostHandlingAction
    {
        IAddExceptionAddHandler NoPostHandling { get; }
        IAddExceptionAddHandler NotifyRethrow { get; }
        IAddExceptionAddHandler ThrowNew { get; }
    }

    public interface IAddHandler
    {
        INamedWrapHandler AddWrapHandler();
    }

    public interface INamedWrapHandler
    {
        IWithMessageWrapHandler Named(string name);
    }

    public interface IWithMessageWrapHandler
    {
        IWrapWith WithMessage(string message);
    }

    public interface IWrapWith
    {
        IAddHandlerAddPolicyAddTo WrapWith(Type wrappingType);
        IAddHandlerAddPolicyAddTo WrapWith<TWrapping>();
    }

    public interface IAddExceptionAddHandler : IAddException, IAddHandler
    {

    }

    public interface IAddHandlerAddPolicyAddTo : IAddHandler, IAddPolicy, IAddTo
    {

    }

    public interface IHandlerBuilder
    {
        ExceptionHandlerData GetHandlerData();
    }

    internal interface IExceptionTypeBuilder
    {
        ExceptionTypeData GetTypeData();
    }

    internal interface IExceptionPolicyBuilder
    {
        ExceptionPolicyData GetPolicyData();
    }
}
