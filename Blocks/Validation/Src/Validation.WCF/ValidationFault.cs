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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// This class is used to return information to a WCF
    /// client when validation fails on a service parameter.
    /// </summary>
    [DataContract(Name = ValidationConstants.FaultContractName,
        Namespace = ValidationConstants.FaultContractNamespace)]
    public class ValidationFault
    {
        private IList<ValidationDetail> details;

		/// <summary>
		/// 
		/// </summary>
        public ValidationFault()
        : this( new ValidationDetail[] { })
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="details"></param>
        public ValidationFault(IEnumerable<ValidationDetail> details)
        {
            this.details = new List<ValidationDetail>(details);
        }
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="detail"></param>
        public void Add( ValidationDetail detail )
        {
            details.Add(detail);
        }
        
		/// <summary>
		/// 
        /// </summary>
        public bool IsValid
        {
            get { return details.Count == 0;  }
        }
        
		/// <summary>
		/// 
		/// </summary>
        [DataMember]
        public IList<ValidationDetail> Details
        {
            get { return details; }
            set { details = value; }
        }
    }
}
