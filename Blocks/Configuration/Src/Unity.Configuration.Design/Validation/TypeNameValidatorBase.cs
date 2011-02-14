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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;

namespace Microsoft.Practices.Unity.Configuration.Design.Validation
{
    public abstract class TypeNameValidatorBase : PropertyValidator
    {
        private TypeResolverImpl typeResolver;
        private ErrorReporter reporter;

        protected void Initialize(Property property, IList<ValidationResult> errors)
        {
            InitializeTypeResolver(property);
            InitializeReporter(property, errors);
        }

        protected Type ResolveTypeName(string typeName)
        {
            return typeResolver.ResolveType(typeName, false);
        }

        protected void ReportError(string format, params object[] args)
        {
            reporter.AddError(format, args);
        }

        protected void ReportWarning(string format, params object[] args)
        {
            reporter.AddWarning(format, args);
        }

        private void InitializeTypeResolver(Property property)
        {
            var aliases = property.ContainingSection.DescendentConfigurationsOfType<AliasElement>();
            var namespaces = property.ContainingSection.DescendentConfigurationsOfType<NamespaceElement>();
            var assemblies = property.ContainingSection.DescendentConfigurationsOfType<AssemblyElement>();

            typeResolver = new TypeResolverImpl(
                aliases.Select(evm => new KeyValuePair<string, string>(
                    (string)evm.Property("Alias").Value, 
                    (string)evm.Property("TypeName").Value)),
                namespaces.Select(evm => (string)evm.Property("Name").Value),
                assemblies.Select(evm => (string)evm.Property("Name").Value)
                );
        }

        private void InitializeReporter(Property property, IList<ValidationResult> errors)
        {
            reporter = new ErrorReporter(property, errors);
        }
    }
}
