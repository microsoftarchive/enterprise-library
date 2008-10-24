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
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [DataContract(Namespace = "http://FaultContracts/2006/03/MockFaultContract")]
    public class MockFaultContract 
    {
        private string message;
        private IDictionary data;
        private Guid id;
        private double someNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MockFaultContract"/> class.
        /// </summary>
        public MockFaultContract()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MockFaultContract"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MockFaultContract(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Gets or sets a user defined data collection.
        /// </summary>
        /// <value>The data collection.</value>
        [DataMember]
        public IDictionary Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Gets or sets some number.
        /// </summary>
        /// <value>Some number.</value>
        [DataMember]
        public double SomeNumber
        {
            get { return someNumber; }
            set { someNumber = value; }
        }
    }
}
