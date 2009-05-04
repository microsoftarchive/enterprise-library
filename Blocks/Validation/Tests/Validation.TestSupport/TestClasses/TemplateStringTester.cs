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
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport
{
	public class TemplateStringTester
	{
		private static readonly Regex escapedParameterReferenceRegex = new Regex(@"\\\{(?<parameter>\d+)}");
		private Dictionary<string, string> parameters = new Dictionary<string, string>();

		public static bool IsMatch(string template, string target)
		{
			Regex templateRegex = Translate(template);
			return templateRegex.IsMatch(target);
		}

		public static Match Match(string template, string target)
		{
			Regex templateRegex = Translate(template);
			return templateRegex.Match(target);
		}

		internal static Regex Translate(string template)
		{
			return new TemplateStringTester().DoTranslate(template);
		}

		private Regex DoTranslate(string template)
		{
			template = Regex.Escape(template);
			template = escapedParameterReferenceRegex.Replace(template, this.ReplaceParameterReference);

			return new Regex(template);
		}

		private string ReplaceParameterReference(Match match)
		{
			string parameter = match.Groups["parameter"].Value;

			if (this.parameters.ContainsKey(parameter))
			{
				return string.Format(@"\k<param{0}>", parameter);
			}
			else
			{
				this.parameters.Add(parameter, parameter);
				return string.Format(@"(?<param{0}>.*)", parameter);
			}
		}
	}
}
