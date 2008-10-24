//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Logging
{
    /// <summary>
    /// A formatter object that allows for replacement of tokens in
    /// a log handler category string.
    /// </summary>
    /// <remarks>This class supports the following replacements:
    /// <list>
    /// <item><term>{method}</term><description>Target method name.</description></item>
    /// <item><term>{type}</term><description>Target method's implementing type.</description></item>
    /// <item><term>{namespace}</term><description>Namespace containing target's type.</description></item>
    /// <item><term>{assembly}</term><description>Assembly containing target's type.</description></item>
    /// </list></remarks>
    public class CategoryFormatter : ReplacementFormatter
    {
        /// <summary>
        /// Construct a new <see cref="CategoryFormatter"/> using information from the
        /// given method.
        /// </summary>
        /// <param name="method">Method used to generate the category replacements.</param>
        public CategoryFormatter(MethodBase method)
        {
            Add(
                new ReplacementToken("{method}",
                    delegate { return method.Name; }),
                new ReplacementToken("{type}",
                    delegate { return method.DeclaringType.Name; }),
                new ReplacementToken("{namespace}",
                    delegate { return method.DeclaringType.Namespace; }),
                new ReplacementToken("{assembly}",
                    delegate
                    {
                        return method.DeclaringType.Assembly.FullName;
                    })
                );
        }

        /// <summary>
        /// Perform the formatting operation, replaceing tokens in the template.
        /// </summary>
        /// <param name="template">Template string to do token replacement in.</param>
        /// <returns>The template, with tokens replaced.</returns>
        public string FormatCategory(string template)
        {
            return Format(template);
        }
    }
}
