//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ExceptionHandlingQuickStart
{
	/// <summary>
	/// Summary description for AppTextExceptionFormatter.
	/// </summary>	
	public class AppTextExceptionFormatter : TextExceptionFormatter
	{
		public AppTextExceptionFormatter(TextWriter writer, Exception exception)
			: base (writer, exception) 
			{
			}
		
		protected override void WriteDescription() 
		{
			// An exception of type {0} occurred and was caught.
			string line = String.Format("An exception of type {0} occurred and was caught.", base.Exception.GetType().FullName);
			this.Writer.WriteLine(line);
		}

		protected override void WriteExceptionType(Type exceptionType) 
		{
			base.Indent();
			base.Writer.WriteLine("Type : {0}", exceptionType.FullName);
		}

		public override void Format() 
		{
			//this.Writer.WriteLine("Message : {0}", message);
			this.WriteDescription();
			//this.WriteExceptionType(base.Exception.GetType());
			base.WriteMessage(base.Exception.Message);
		}

	}
}
