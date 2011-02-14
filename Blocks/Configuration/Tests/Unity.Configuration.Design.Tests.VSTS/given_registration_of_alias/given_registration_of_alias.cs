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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_registration_of_alias
{
    public abstract class given_registration_of_alias : given_registration_element
    {
        protected ElementViewModel StringAlias;
        protected ElementViewModel Int32Alias;

        protected override void Arrange()
        {
            base.Arrange();

            var aliasesCollection = (ElementCollectionViewModel)base.UnitySectionViewModel.ChildElement("TypeAliases");
            StringAlias = aliasesCollection.AddNewCollectionElement(typeof(AliasElement));
            StringAlias.Property("Alias").Value = "s";
            StringAlias.Property("TypeName").Value = "System.String, mscorlib";

            Int32Alias = aliasesCollection.AddNewCollectionElement(typeof(AliasElement));
            Int32Alias.Property("Alias").Value = "i";
            Int32Alias.Property("TypeName").Value = "System.Int32, mscorlib";

            base.RegistrationElement.Property("TypeName").Value = "s";
        }
    }
}
