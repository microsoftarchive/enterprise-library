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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that will run the validation for the node the command encapsulates.
    /// </summary>
    public class ValidateNodeCommand : ConfigurationNodeCommand
    {
        private bool validationSucceeded;
        private bool reportErrorsOnFailure;

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidateNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public ValidateNodeCommand(IServiceProvider serviceProvider) : base(serviceProvider, true)
        {
            reportErrorsOnFailure = true;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidateNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        public ValidateNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidateNodeCommand"/> class with an <see cref="IServiceProvider"/>, if the error service should be cleared after the command executes and if the command should report the failures after executing.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        /// <param name="reportErrorsOnFailure">
        /// Determines if the command should report errors on failure.
        /// </param>
        public ValidateNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog, bool reportErrorsOnFailure) : base(serviceProvider, clearErrorLog)
        {
            this.reportErrorsOnFailure = reportErrorsOnFailure;
        }

        /// <summary>
        /// Determines if the validation succeeded.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the validation succeeded; otherwise, <see langword="false"/>.
        /// The default value is <see langword="false"/>.
        /// </value>
        public bool ValidationSucceeded
        {
            get { return validationSucceeded; }
        }

        /// <summary>
        /// Determines if a message should be shown when validation fails.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a message should be shown when validation fails, <see langword="false"/>.
        /// The default value is <see langword="true"/>.
        /// </value>
        public bool ReportErrorsOnFailure
        {
            get { return reportErrorsOnFailure; }
            set { reportErrorsOnFailure = value; }
        }

        /// <summary>
        /// 
        /// Executes the validation for the current node and all the child nodes.
        /// 
        /// </summary>
        /// <param name="node">
        /// The <see cref="ConfigurationNode"/> to validate.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            List<ValidationError> errors = new List<ValidationError>();
            Validate(node, errors);
            if (errors.Count > 0)
            {
                foreach(ValidationError error in errors)
                {
                    ErrorLogService.LogError(error);    
                }
                
            }
            if (ErrorLogService.ValidationErrorCount > 0)
            {
                if (ErrorLogService.ValidationErrorCount > 0)
                {
                    UIService.DisplayErrorLog(ErrorLogService);
                }

                if (reportErrorsOnFailure)
                {
                    UIService.ShowMessage(Resources.ValidationErrorsMessage, Resources.ValidationCaption);
                }
                ErrorLogService.ClearErrorLog();
                validationSucceeded = false;
            }
            else
            {
                validationSucceeded = true;
            }
        }

        private void Validate(ConfigurationNode node, List<ValidationError> errors)
        {
            Type t = node.GetType();
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            ValidateProperties(node, properties, errors);
            ValidateChildNodeProperties(node, errors);
			node.Validate(errors);
        }

        private void ValidateProperties(ConfigurationNode node, PropertyInfo[] properties, List<ValidationError> errors)
        {
            foreach (PropertyInfo property in properties)
            {
                ValidationAttribute[] validationAttributes = (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
                foreach (ValidationAttribute validationAttribute in validationAttributes)
                {
                    validationAttribute.Validate(node, property, errors, ServiceProvider);
                }
            }
        }

        private void ValidateChildNodeProperties(ConfigurationNode node, List<ValidationError> errors)
        {
            foreach (ConfigurationNode childNode in node.Nodes)
            {
                Validate(childNode, errors);
            }
        }
    }
}