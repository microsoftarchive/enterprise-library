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
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
    public class MockValidatedElement : IValidatedElement
    {
        public MockValidatedElement(bool ignoreNulls,
            string ignoreNullsMessageTemplate,
            string ignoreNullsTag,
            CompositionType compositionType,
            string compositionMessageTemplate,
            string compositionTag)
        {
            this.ignoreNulls = ignoreNulls;
            this.ignoreNullsMessageTemplate = ignoreNullsMessageTemplate;
            this.ignoreNullsTag = ignoreNullsTag;
            this.compositionType = compositionType;
            this.compositionMessageTemplate = compositionMessageTemplate;
            this.compositionTag = compositionTag;
        }

        private bool ignoreNulls;
        bool IValidatedElement.IgnoreNulls
        {
            get { return ignoreNulls; }
        }

        private string ignoreNullsMessageTemplate;
        string IValidatedElement.IgnoreNullsMessageTemplate
        {
            get { return ignoreNullsMessageTemplate; }
        }

        private string ignoreNullsTag;
        string IValidatedElement.IgnoreNullsTag
        {
            get { return ignoreNullsTag; }
        }

        private CompositionType compositionType;
        CompositionType IValidatedElement.CompositionType
        {
            get { return compositionType; }
        }

        private string compositionMessageTemplate;
        string IValidatedElement.CompositionMessageTemplate
        {
            get { return compositionMessageTemplate; }
        }

        private string compositionTag;
        string IValidatedElement.CompositionTag
        {
            get { return compositionTag; }
        }

        IEnumerable<IValidatorDescriptor> IValidatedElement.GetValidatorDescriptors()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        MemberInfo IValidatedElement.MemberInfo
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        Type IValidatedElement.TargetType
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }
}
