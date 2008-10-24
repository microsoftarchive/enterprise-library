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
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Diagnostics;

namespace DomainModel
{
	/// <summary>
	/// Summary description for DomainObjects
	/// </summary>
	public static class DomainObjects
	{
		public static ICollection<DomainObject> GetDomainObjects()
		{
			List<DomainObject> businessObjects = new List<DomainObject>();

			businessObjects.Add(new DomainObject("abc", "123", 10, TraceOptions.Callstack));
			businessObjects.Add(new DomainObject("def", "456", 20, TraceOptions.DateTime));
			businessObjects.Add(new DomainObject("ghi", "789", 30, TraceOptions.LogicalOperationStack));

			return businessObjects;
		}
	}
}
