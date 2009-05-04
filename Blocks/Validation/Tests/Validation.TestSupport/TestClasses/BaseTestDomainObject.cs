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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class BaseTestDomainObject
	{
		public const string Base1Value = "base1";
		public const string Base2Value = "base2";
		public const string Base3Value = "base3";

		public string Field1 = Base1Value;
		public string Field2 = Base2Value;
		public string Field3 = Base3Value;

		public string Property1
		{
			get { return Base1Value; }
		}

		public virtual string Property2
		{
			get { return Base2Value; }
		}

		public string Property3
		{
			get { return Base3Value; }
		}

		public string Method1()
		{
			return Base1Value;
		}

		public virtual string Method2()
		{
			return Base2Value;
		}

		public string Method3()
		{
			return Base3Value;
		}
	}
}
