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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class SelectValidatedTypeReferenceMembersCommand : CommandModel
    {
        TypeMemberChooser typeMemberChooser;

        ElementViewModel validatedTypeReferenceElement;
        ElementCollectionViewModel fieldCollectionElement;
        ElementCollectionViewModel propertyCollectionElement;
        ElementCollectionViewModel methodsCollectionElement;

        public SelectValidatedTypeReferenceMembersCommand(TypeMemberChooser typeMemberChooser, ElementViewModel context, CommandAttribute attribute, IUIServiceWpf uiService)
            : base(attribute, uiService)
        {
            if (context.ConfigurationType != typeof(ValidationRulesetData)) throw new InvalidOperationException();

            this.validatedTypeReferenceElement = context.AncestorElements().Where(x => x.ConfigurationType == typeof(ValidatedTypeReference)).First();
            this.fieldCollectionElement = (ElementCollectionViewModel)context.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatedFieldReferenceCollection)).First();
            this.propertyCollectionElement = (ElementCollectionViewModel)context.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatedPropertyReferenceCollection)).First();
            this.methodsCollectionElement = (ElementCollectionViewModel)context.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatedMethodReferenceCollection)).First();

            this.typeMemberChooser = typeMemberChooser;
        }


        private Type GetValidationType()
        {
            string typeString = string.Format(CultureInfo.CurrentCulture, "{0}, {1}",
                    validatedTypeReferenceElement.Property("Name").Value,
                    validatedTypeReferenceElement.Property("AssemblyName").Value);

            return new TypeResolver().GetType(typeString);
        }

        protected override void InnerExecute(object parameter)
        {
            Type typeReferenceType = GetValidationType();

            foreach (var member in typeMemberChooser.ChooseMembers(typeReferenceType))
            {
                if (member is FieldInfo)
                {
                    var fieldElement = fieldCollectionElement.AddNewCollectionElement(typeof(ValidatedFieldReference));
                    fieldElement.Property("Name").Value = member.Name;
                }
                else if (member is PropertyInfo)
                {
                    var propertyElement = propertyCollectionElement.AddNewCollectionElement(typeof(ValidatedPropertyReference));
                    propertyElement.Property("Name").Value = member.Name;
                }
                else if (member is MethodInfo)
                {
                    var methodElement = methodsCollectionElement.AddNewCollectionElement(typeof(ValidatedMethodReference));
                    methodElement.Property("Name").Value = member.Name;
                }
            }
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return GetValidationType() != null;
        }

        public override string Title
        {
            get
            {
                return Resources.SelectValidatedMembersCommandTitle;
            }
        }

        public override string HelpText
        {
            get
            {
                return Resources.SelectValidatedMembersCommandHelpText;
            }
        }

        private class TypeResolver
        {
            private readonly Thread currentThread;

            public TypeResolver()
            {
                this.currentThread = Thread.CurrentThread;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, ControlAppDomain = true)]
            public Type GetType(string name)
            {
                try
                {
                    AppDomain.CurrentDomain.AssemblyResolve += this.OnAssemblyResolve;

                    return Type.GetType(name);
                }
                catch
                {
                    return null;
                }
                finally
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= this.OnAssemblyResolve;
                }
            }

            private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
            {
                if (this.currentThread != Thread.CurrentThread)
                {
                    return null;
                }

                var requestedAssemblyName = new AssemblyName(args.Name);
                var requestedAsmPublicKeyToken = requestedAssemblyName.GetPublicKeyToken();
                var convertedRequestedAsmPublicKeyToken =
                    requestedAsmPublicKeyToken != null
                        ? Convert.ToBase64String(requestedAsmPublicKeyToken)
                        : null;
                var requestedAssemblyCulture = requestedAssemblyName.CultureInfo;

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var assemblyName = assembly.GetName();
                    if (assemblyName.Name == requestedAssemblyName.Name)
                    {
                        if (requestedAssemblyName.Version != null &&
                            requestedAssemblyName.Version.CompareTo(assemblyName.Version) != 0)
                        {
                            continue;
                        }
                        if (requestedAsmPublicKeyToken != null)
                        {
                            byte[] cachedAssemblyPublicKeyToken = assemblyName.GetPublicKeyToken();

                            if (!string.Equals(
                                    convertedRequestedAsmPublicKeyToken,
                                    Convert.ToBase64String(cachedAssemblyPublicKeyToken),
                                    StringComparison.Ordinal))
                            {
                                continue;
                            }
                        }

                        if (requestedAssemblyCulture != null
                            && requestedAssemblyCulture.LCID != CultureInfo.InvariantCulture.LCID)
                        {
                            if (assemblyName.CultureInfo.LCID != requestedAssemblyCulture.LCID)
                            {
                                continue;
                            }
                        }

                        return assembly;
                    }
                }

                return null;
            }
        }
    }
#pragma warning restore 1591
}
