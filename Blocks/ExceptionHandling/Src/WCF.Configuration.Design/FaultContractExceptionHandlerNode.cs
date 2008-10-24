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
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    /// <summary>
    /// Represents a <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/> configuration element. 
    /// </summary>
    public class FaultContractExceptionHandlerNode : ExceptionHandlerNode
    {
        string exceptionMessage;
        string faultContractType;
        List<FaultContractPropertyMapping> propertyMappings = new List<FaultContractPropertyMapping>();

        /// <summary>
        /// Initialize a new instance of the <see cref="FaultContractExceptionHandlerNode"/> class.
        /// </summary>
        public FaultContractExceptionHandlerNode()
            : this(new FaultContractExceptionHandlerData(Resources.FaultContractExceptionHandlerNodeName)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="FaultContractExceptionHandlerNode"/> class with a <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/> instance.
        /// </summary>
        /// <param name="data">A <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/> instance</param>
        public FaultContractExceptionHandlerNode(FaultContractExceptionHandlerData data)
        {
            if (null == data) throw new ArgumentNullException("data");

            Rename(data.Name);
            faultContractType = data.FaultContractType;
            exceptionMessage = data.ExceptionMessage;
            foreach (FaultContractExceptionHandlerMappingData mappingData in data.PropertyMappings)
            {
                FaultContractPropertyMapping mapping = new FaultContractPropertyMapping();
                mapping.Name = mappingData.Name;
                mapping.Source = mappingData.Source;
                PropertyMappings.Add(mapping);
            }
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/> this node represents.
        /// </value>
        [Browsable(false)]
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                FaultContractExceptionHandlerData data = new FaultContractExceptionHandlerData(Name, faultContractType);
                data.ExceptionMessage = exceptionMessage;

                foreach (FaultContractPropertyMapping mapping in propertyMappings)
                {
                    data.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData(mapping.Name, mapping.Source));
                }

                return data;
            }
        }

        /// <summary>
        /// Gets or sets the Exception Message
        /// </summary>
        [SRDescription("ExceptionMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// Gets or sets the Fault Contract Type
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(object))]
        [SRDescription("FaultContractTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string FaultContractType
        {
            get { return faultContractType; }
            set { faultContractType = value; }
        }

        /// <summary>
        /// Gets the fault contract property mappings for the handler.
        /// </summary>
        [SRDescription("PropertyMappingsMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [PropertyMappingsValidation]
        public List<FaultContractPropertyMapping> PropertyMappings
        {
            get { return propertyMappings; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FaultContractExceptionHandlerNode"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {}
            base.Dispose(disposing);
        }

        class PropertyMappingsValidationAttribute : ValidationAttribute
        {
            protected override void ValidateCore(object instance,
                                                 PropertyInfo propertyInfo,
                                                 IList<ValidationError> errors)
            {
                List<FaultContractPropertyMapping> mappings = propertyInfo.GetValue(instance, new object[0]) as List<FaultContractPropertyMapping>;
                if (mappings != null)
                {
                    int position = -1;
                    List<string> keys = new List<string>();
                    foreach (FaultContractPropertyMapping item in mappings)
                    {
                        position++;
                        if (string.IsNullOrEmpty(item.Name))
                        {
                            string errorMessage = string.Format(Resources.Culture, Resources.PropertyMappingNameNullError, position);
                            errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                            continue;
                        }
                        if (keys.Contains(item.Name))
                        {
                            string errorMessage = string.Format(Resources.Culture, Resources.PropertyMappingDuplicateNameError, item.Name);
                            errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                            continue;
                        }

                        keys.Add(item.Name);
                    }
                }
            }
        }
    }
}
