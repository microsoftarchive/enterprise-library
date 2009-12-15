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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF
{
    /// <summary>
    /// Provides methods and attached properties that support validation of bindings using the 
    /// Validation Application Block.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Identifies the BindingForProperty attached property.
        /// </summary>
        public static readonly DependencyProperty BindingForPropertyProperty =
            DependencyProperty.RegisterAttached(
                "BindingForProperty",
                typeof(string),
                typeof(Validate),
                new PropertyMetadata(OnBindingForPropertyChanged));

        /// <summary>
        /// Identifies the UsingSource attached property.
        /// </summary>
        public static readonly DependencyProperty UsingSourceProperty =
            DependencyProperty.RegisterAttached(
                "UsingSource",
                typeof(ValidationSpecificationSource),
                typeof(Validate),
                new PropertyMetadata(ValidationSpecificationSource.All, OnConfigurationPropertyChanged));

        /// <summary>
        /// Identifies the UsingRulesetName attached property.
        /// </summary>
        public static readonly DependencyProperty UsingRulesetNameProperty =
            DependencyProperty.RegisterAttached(
                "UsingRulesetName",
                typeof(string),
                typeof(Validate),
                new PropertyMetadata(string.Empty, OnConfigurationPropertyChanged));

        /// <summary>
        /// Sets the value of the BindingForProperty attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="property">The name of the bound property to validate.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetBindingForProperty(FrameworkElement element, string property)
        {
            element.SetValue(BindingForPropertyProperty, property);
        }

        /// <summary>
        /// Gets the value of the BindingForProperty attached property for a specified element.
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The BindingForProperty property value for the element.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetBindingForProperty(FrameworkElement element)
        {
            return (string)element.GetValue(BindingForPropertyProperty);
        }

        /// <summary>
        /// Sets the value of the UsingSource attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="property">The source for validation information.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetUsingSource(FrameworkElement element, ValidationSpecificationSource property)
        {
            element.SetValue(UsingSourceProperty, property);
        }

        /// <summary>
        /// Gets the value of the UsingSource attached property for a specified element.
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The UsingSource property value for the element.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static ValidationSpecificationSource GetUsingSource(FrameworkElement element)
        {
            return (ValidationSpecificationSource)element.GetValue(UsingSourceProperty);
        }

        /// <summary>
        /// Sets the value of the UsingRulesetName attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="rulesetName">The ruleset name.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetUsingRulesetName(FrameworkElement element, string rulesetName)
        {
            element.SetValue(UsingRulesetNameProperty, rulesetName);
        }

        /// <summary>
        /// Gets the value of the UsingRulesetName attached property for a specified element.
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The UsingRulesetName property value for the element.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetUsingRulesetName(FrameworkElement element)
        {
            return (string)element.GetValue(UsingRulesetNameProperty);
        }

        private static void OnBindingForPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            FrameworkElement element = dependencyObject as FrameworkElement;
            ResetValidatorRule(element, args);
        }

        private static void ResetValidatorRule(FrameworkElement element, DependencyPropertyChangedEventArgs args)
        {
            // Defer the initialization if not yet initialized
            if (!element.IsInitialized)
            {
                EventHandler handler = null;
                handler =
                    (o, e) =>
                    {
                        element.Initialized -= handler;
                        ResetValidatorRule(element, args);
                    };

                element.Initialized += handler;
                return;
            }

            if (args.OldValue != null)
            {
                var bindingExpressionBase =
                    GetBindingExpressionBaseForPropertyName(element, (string)args.OldValue, false);

                if (bindingExpressionBase != null)
                {
                    NavigateBindingExpressionTree(
                        bindingExpressionBase,
                        bindingExpression =>
                        {
                            Binding binding = bindingExpression.ParentBinding;

                            // remove the previous validation rule, if any
                            var currentRule =
                                binding.ValidationRules.OfType<ValidatorRule>().FirstOrDefault(vr => vr.IsDynamic);
                            if (currentRule != null)
                            {
                                binding.ValidationRules.Remove(currentRule);
                            }
                        });
                }
            }
            if (args.NewValue != null)
            {
                var bindingExpressionBase =
                    GetBindingExpressionBaseForPropertyName(element, (string)args.NewValue, true);

                NavigateBindingExpressionTree(
                    bindingExpressionBase,
                    bindingExpression =>
                    {
                        ValidatorRule.GetCheckedPath(bindingExpression);

                        Binding binding = bindingExpression.ParentBinding;

                        // Add the rule if necessary
                        if (!binding.ValidationRules.OfType<ValidatorRule>().Any(vr => vr.IsDynamic))
                        {
                            binding.ValidationRules.Add(
                                new ValidatorRule(bindingExpression)
                                {
                                    ValidationSpecificationSource = GetUsingSource(element),
                                    RulesetName = GetUsingRulesetName(element)
                                });
                        }
                    });
            }
        }

        private static BindingExpressionBase GetBindingExpressionBaseForPropertyName(
            FrameworkElement element,
            string dependencyPropertyName,
            bool thrownOnError)
        {
            BindingExpressionBase bindingExpressionBase = null;

            // Get the property to be validated
            var descriptor =
                DependencyPropertyDescriptor.FromName(dependencyPropertyName, element.GetType(), element.GetType());
            if (descriptor == null)
            {
                if (thrownOnError)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionMissingDependencyProperty,
                            dependencyPropertyName,
                            element.GetType().Name,
                            element.Name,
                            element.GetHashCode()));
                }
            }
            else
            {
                var dependencyProperty = descriptor.DependencyProperty;

                // Get the BindingExpression for the property to validate
                bindingExpressionBase = BindingOperations.GetBindingExpressionBase(element, dependencyProperty);
                if (bindingExpressionBase == null)
                {
                    if (thrownOnError)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.ExceptionDependencyPropertyHasNoBinding,
                                dependencyPropertyName,
                                element.GetType().Name,
                                element.Name,
                                element.GetHashCode()));
                    }
                }
            }

            return bindingExpressionBase;
        }

        private static void OnConfigurationPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            FrameworkElement element = dependencyObject as FrameworkElement;
            UpdateExistingValidatorRule(element, args);
        }

        private static void UpdateExistingValidatorRule(
            FrameworkElement element,
            DependencyPropertyChangedEventArgs args)
        {
            // Ignore the initialization if not yet initialized
            if (!element.IsInitialized)
            {
                return;
            }

            var bindingFor = GetBindingForProperty(element);
            if (bindingFor == null)
            {
                return;
            }

            var bindingExpressionBase = GetBindingExpressionBaseForPropertyName(element, bindingFor, false);
            if (bindingExpressionBase == null)
            {
                return;
            }

            NavigateBindingExpressionTree(
                bindingExpressionBase,
                bindingExpression =>
                {
                    var validatorRule =
                        bindingExpression.ParentBinding.ValidationRules.OfType<ValidatorRule>().First(vr => vr.IsDynamic);

                    if (validatorRule != null)
                    {
                        validatorRule.ValidationSpecificationSource = GetUsingSource(element);
                        validatorRule.RulesetName = GetUsingRulesetName(element);
                    }
                });
        }

        private static void NavigateBindingExpressionTree(
            BindingExpressionBase bindingExpressionBase,
            Action<BindingExpression> action)
        {
            BindingExpression bindingExpression = bindingExpressionBase as BindingExpression;
            if (bindingExpression != null)
            {
                action(bindingExpression);
                return;
            }

            MultiBindingExpression multiBindingExpression = bindingExpressionBase as MultiBindingExpression;
            if (multiBindingExpression != null)
            {
                foreach (var subExpression in multiBindingExpression.BindingExpressions)
                {
                    NavigateBindingExpressionTree(subExpression, action);
                }
                return;
            }

            PriorityBindingExpression priorityBindingExpression = bindingExpressionBase as PriorityBindingExpression;
            if (priorityBindingExpression != null)
            {
                foreach (var subExpression in priorityBindingExpression.BindingExpressions)
                {
                    NavigateBindingExpressionTree(subExpression, action);
                }
                return;
            }

            throw new InvalidOperationException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.ExceptionUnexpectedBindingExpression,
                    bindingExpression.GetType().Name));
        }
    }
}
