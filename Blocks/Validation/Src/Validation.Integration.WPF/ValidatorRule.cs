//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Properties;
using SWC = System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF
{
    /// <summary>
    /// Validation rule that performs validation of a converted value using the validators associated to the source
    /// property of a binding.
    /// </summary>
    /// <remarks>
    /// A <see cref="ValidatorRule"/> can be initialized with a <see cref="BindingExpression"/> from which it can get
    /// the source item and the source property to extract the appropriate validators.
    /// <para/>
    /// This rule is intended to be used on the <see cref="ValidationStep.ConvertedProposedValue"/> step, after the
    /// raw value has been converted but before it is applied to the source of the binding.
    /// </remarks>
    public class ValidatorRule : ValidationRule
    {
        // regex for a simple identifier
        private static readonly Regex pathValidationRegex =
            new Regex(
                @"^
                    (?:\p{Lu}|\p{Ll}|\p{Lt}|\p{Lm}|\p{Lo}|\p{Nl}|_)
                    (?:\p{Lu}|\p{Ll}|\p{Lt}|\p{Lm}|\p{Lo}|\p{Nl}|\p{Nd}|\p{Pc}|\p{Mn}|\p{Mc}|\p{Cf})*
                $",
                RegexOptions.IgnorePatternWhitespace);

        private readonly BindingExpression bindingExpression;
        private Type sourceType;
        private string sourcePropertyName;
        private ValidationSpecificationSource validationSpecificationSource = ValidationSpecificationSource.All;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorRule"/> class..
        /// </summary>
        public ValidatorRule()
            : base(ValidationStep.ConvertedProposedValue, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorRule"/> class for a binding expression.
        /// </summary>
        /// <param name="bindingExpression">The expression to validate.</param>
        public ValidatorRule(BindingExpression bindingExpression)
            : base(ValidationStep.ConvertedProposedValue, false)
        {
            if (bindingExpression == null)
            {
                throw new ArgumentNullException("bindingExpression");
            }

            this.bindingExpression = bindingExpression;
        }

        /// <summary>
        /// Validates <paramref name="value"/> using the validators associated with the source property.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>A <see cref="ValidationResult"/> object.</returns>
        public override SWC.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            SWC.ValidationResult validationResult = SWC.ValidationResult.ValidResult;

            var validator = GetValidator();

            if (validator == null)
            {
                return SWC.ValidationResult.ValidResult;
            }

            var results = validator.Validate(new ExternalValue { PropertyValue = value });

            if (!results.IsValid)
            {
                var errorContent =
                    results.Select(vr => vr.Message).Aggregate((acc, message) => acc + Environment.NewLine + message);

                validationResult = new SWC.ValidationResult(false, errorContent);
            }

            return validationResult;
        }

        /// <summary>
        /// Gets or sets the type from which explicitly configured <see cref="ValidatorRule"/> instances should get
        /// their validators.
        /// </summary>
        public Type SourceType
        {
            get
            {
                return this.sourceType;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (this.IsDynamic) throw new ArgumentException(Resources.ExceptionCannotModifyDynamicRule, "value");
                this.sourceType = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the property for which explicitly configured <see cref="ValidatorRule"/> instances
        /// should get their validators.
        /// </summary>
        public string SourcePropertyName
        {
            get
            {
                return this.sourcePropertyName;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (this.IsDynamic) throw new ArgumentException(Resources.ExceptionCannotModifyDynamicRule, "value");
                this.sourcePropertyName = value;
            }
        }

        /// <summary>
        /// Gets or sets the source for validation information.
        /// </summary>
        public ValidationSpecificationSource ValidationSpecificationSource
        {
            get { return this.validationSpecificationSource; }
            set { this.validationSpecificationSource = value; }
        }

        /// <summary>
        /// Gets or sets the ruleset name to use when validating.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ruleset")]
        public string RulesetName { get; set; }

        internal bool IsDynamic
        {
            get { return this.bindingExpression != null; }
        }

        private Validator GetValidator()
        {
            PropertyInfo property = null;

            if (this.bindingExpression != null)
            {
                var type = GetSourceItemType();
                if (type == null)
                {
                    throw new InvalidOperationException(Resources.ExceptionUnexpectedMissingType);
                }

                property = type.GetProperty(GetSourcePropertyName());
                if (property == null)
                {
                    throw new InvalidOperationException(Resources.ExceptionUnexpectedPropertyType);
                }
            }
            else
            {
                if (this.SourceType == null)
                {
                    throw new InvalidOperationException(Resources.ExceptionSourceTypeNotSet);
                }
                if (this.SourcePropertyName == null)
                {
                    throw new InvalidOperationException(Resources.ExceptionSourcePropertyNameNotSet);
                }
                property = this.SourceType.GetProperty(this.SourcePropertyName);
                if (property == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionSourcePropertyNotFound,
                            this.SourceType.Name,
                            this.SourcePropertyName));
                }
            }

            return CreateValidator(property);
        }

        private Validator CreateValidator(PropertyInfo validatedProperty)
        {
            var validator =
                PropertyValidationFactory.GetPropertyValidator(
                    validatedProperty.ReflectedType,
                    validatedProperty,
                    this.RulesetName ?? string.Empty,
                    this.ValidationSpecificationSource,
                    new FixedPropertyMemberValueAccessBuilder(validatedProperty));
            return validator;
        }

        private Type GetSourceItemType()
        {
            return this.bindingExpression.DataItem != null ? this.bindingExpression.DataItem.GetType() : null;
        }

        private string GetSourcePropertyName()
        {
            return GetCheckedPath(this.bindingExpression);
        }

        internal static string GetCheckedPath(BindingExpression bindingExpression)
        {
            var pathText = bindingExpression.ParentBinding.Path.Path;

            if (!pathValidationRegex.IsMatch(pathText))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionComplexBindingPath,
                        pathText));
            }

            return pathText;
        }

        private class FixedPropertyMemberValueAccessBuilder : MemberValueAccessBuilder
        {
            private readonly PropertyInfo property;

            public FixedPropertyMemberValueAccessBuilder(PropertyInfo property)
            {
                this.property = property;
            }

            protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
            {
                throw new NotSupportedException();
            }

            protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
            {
                throw new NotSupportedException();
            }

            protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
            {
                if (this.property == propertyInfo)
                {
                    return new ExternalValueValueAccess();
                }

                throw new NotSupportedException();
            }
        }

        private class ExternalValueValueAccess : ValueAccess
        {
            public ExternalValueValueAccess()
            { }

            public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
            {
                var companion = source as ExternalValue;
                if (companion != null)
                {
                    value = companion.PropertyValue;
                    valueAccessFailureMessage = null;
                    return true;
                }

                throw new ArgumentException(Resources.ExceptionUnexpectedValueSource, "source");
            }

            public override string Key
            {
                get { return ""; }
            }
        }

        private class ExternalValue
        {
            public object PropertyValue { get; set; }
        }
    }
}
