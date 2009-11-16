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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class StringHeaderViewModel : HeaderViewModel
    {
        private readonly string name;

        public StringHeaderViewModel(string name)
        {
            this.name = name;
        }

        public override string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
