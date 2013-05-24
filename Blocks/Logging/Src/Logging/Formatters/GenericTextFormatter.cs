//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Formats an instance for <typeparamref name="T"/> with a sequence of <see cref="Formatter{T}"/> instances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A formatter will convert a template into a sequence of <see cref="Formatter{T}"/> instances with the help
    /// of a set of <see cref="TokenHandler{T}"/> instances that help parse the tokens in the template.
    /// </para>
    /// <para>
    /// The character '{' is used to determine the start of a token, and token handlers perform the actual parsing of 
    /// each token.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of object to format.</typeparam>
    public class GenericTextFormatter<T>
    {
        private IEnumerable<Formatter<T>> formatters;

        /// <summary>
        /// Initializes a new instance of <see cref="GenericTextFormatter{T}"/> with a template and a set of
        /// token handlers.
        /// </summary>
        /// <param name="template">The template to use when </param>
        /// <param name="tokenHandlers">The handlers to use when parsing the template.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "As designed")]
        public GenericTextFormatter(string template, IDictionary<string, TokenHandler<T>> tokenHandlers)
        {
            this.formatters = SetUpFormatters(template, tokenHandlers);
        }

        private static IEnumerable<Formatter<T>> SetUpFormatters(
            string template,
            IDictionary<string, TokenHandler<T>> tokenHandlers)
        {
            List<Formatter<T>> formatters = new List<Formatter<T>>();

            int currentRunStart = 0;
            int currentIndex = 0;
            while (currentIndex < template.Length)
            {
                if ('{' == template[currentIndex++])
                {
                    // candidate token start
                    // look for all consecutive letters
                    int currentTokenStart = currentIndex;
                    while (currentIndex < template.Length && char.IsLetterOrDigit(template[currentIndex]))
                    {
                        // keep going until a non-letter or the end of template are found
                        currentIndex++;
                    }
                    if (currentIndex == template.Length)
                    {
                        break;	// nothing left on the template, let the final run be handled outside
                    }

                    // by now all consecutive letters have been detected, so the name is uses as 
                    // a candidate token name
                    string tokenName = template.Substring(currentTokenStart, currentIndex - currentTokenStart);

                    // is it a known token name?
                    TokenHandler<T> tokenHandler;
                    if (tokenHandlers.TryGetValue(tokenName, out tokenHandler))
                    {
                        Formatter<T> formatter = tokenHandler(template, ref currentIndex);
                        if (formatter != null)
                        {
                            // a token name was recognized and its handler took care of whatever came after the token name
                            // leaving currentIndex pointing to the first char after the token

                            // first end the previous run right before the token was detected
                            string previousRun = template.Substring(currentRunStart, currentTokenStart - currentRunStart - 1);
                            if (previousRun.Length > 0)
                            {
                                formatters.Add(o => previousRun);
                            }

                            // add the formatter
                            formatters.Add(formatter);

                            // start new run where the handler left
                            currentRunStart = currentIndex;
                        }
                    }
                }
            }

            // take care of last run
            {
                string previousRun = template.Substring(currentRunStart, currentIndex - currentRunStart);
                if (previousRun.Length > 0)
                {
                    formatters.Add(o => previousRun);
                }
            }

            return formatters;
        }

        /// <summary>
        /// Utility method to create a handler for tokens without parameters that parse a template into a formatter
        /// for a constant string.
        /// </summary>
        /// <param name="constant">The constant for the token handler's formatter</param>
        /// <returns>A token handler.</returns>
        public static TokenHandler<T> CreateSimpleTokenHandler(string constant)
        {
            return CreateSimpleTokenHandler(o => constant);
        }

        /// <summary>
        /// Utility method to create a handler for tokens without parameters that parse a template into a given formatter.
        /// </summary>
        /// <param name="formatter">The formatter to be returned by the created token handler.</param>
        /// <returns>A token handler.</returns>
        public static TokenHandler<T> CreateSimpleTokenHandler(Formatter<T> formatter)
        {
            return delegate(string template, ref int currentIndex)
            {
                if ('}' == template[currentIndex])
                {
                    currentIndex++;		// advance the current index
                    return formatter;
                }
                return null;
            };
        }

        /// <summary>
        /// Utility method to create a handler for tokens with parameters surrounded by parenthesis.
        /// </summary>
        /// <param name="formatterFactory">The factory delegate to create a formatter based on the token parameter.</param>
        /// <returns>A token handler.</returns>
        public static TokenHandler<T> CreateParameterizedTokenHandler(ParameterizedFormatterFactory<T> formatterFactory)
        {
            return delegate(string template, ref int currentIndex)
            {
                string parameter = string.Empty;

                if (template[currentIndex] == '(')
                {
                    int parameterStart = ++currentIndex;

                    while (true)
                    {
                        // skip until ending ')' is found
                        while (currentIndex < template.Length && template[currentIndex++] != ')')
                            ;

                        // template finished?
                        if (currentIndex == template.Length)
                            return null;

                        if (template[currentIndex] == '}')
                        {
                            break;		// got to the end of the template
                        }
                    }

                    // then the stopped looking after finding a ')'
                    parameter = template.Substring(parameterStart, currentIndex - parameterStart - 1);
                }

                // should be looking at the last char in the template
                currentIndex++;	// advance the current index
                return formatterFactory(parameter);
            };
        }

        /// <summary>
        /// Formats <paramref name="instance"/> based on the template specified for the formatter.
        /// </summary>
        /// <param name="instance">The instance to format.</param>
        /// <param name="output">The result of formatting the instance.</param>
        public void Format(T instance, StringBuilder output)
        {
            foreach (var formatter in this.formatters)
            {
                output.Append(formatter(instance));
            }
        }
    }

    /// <summary>
    /// Transforms a token definition from a string template into a <see cref="Formatter{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A token handler gets control of the template parsing process right after the token name has been consumed, 
    /// and consumes the rest of the token definition advancing the <paramref name="currentIndex"/> pointer to the end
    /// of the token.
    /// </para>
    /// <para>
    /// If the text following the token name cannot be parsed into the expected token, the <paramref name="currentIndex"/>
    /// should still be updated and <see langword="null"/> should be returned.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type to format.</typeparam>
    /// <param name="template">The template being parsed.</param>
    /// <param name="currentIndex">The current index in the template.</param>
    /// <returns>The <see cref="Formatter{T}"/> representing the token, or <see langword="null"/> if the parsing of the token
    /// was not successful.</returns>
    public delegate Formatter<T> TokenHandler<T>(string template, ref int currentIndex);

    /// <summary>
    /// Returns a string representation of <paramref name="instance"/>
    /// </summary>
    /// <typeparam name="T">The type to format.</typeparam>
    /// <param name="instance">The instance to format.</param>
    /// <returns>A string representing <paramref name="instance"/>.</returns>
    public delegate string Formatter<T>(T instance);

    /// <summary>
    /// Creates a <see cref="Formatter{T}"/> based on a <paramref name="parameter"/>.
    /// </summary>
    /// <remarks>
    /// This delegate is used by <see cref="GenericTextFormatter{T}.CreateParameterizedTokenHandler"/>.
    /// </remarks>
    /// <typeparam name="T">The type to format.</typeparam>
    /// <param name="parameter">The parameter to use when creating a formatter, extracted from the token on a template.</param>
    /// <returns>The <see cref="Formatter{T}"/> based on the parameter.</returns>
    public delegate Formatter<T> ParameterizedFormatterFactory<T>(string parameter);
}
