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

using System;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Abstract base for all <see cref="TokenFunction"></see>-derived classes. 
    /// Provides default algorithm for formatting a token.
    /// </summary>
	/// <remarks>
	/// Extending this class is not the recommended approach for handling tokens.
	/// </remarks>
	/// <seealso cref="TextFormatter(string, System.Collections.Generic.IDictionary{string, TokenHandler{LogEntry}})"/>
	/// <seealso cref="TokenHandler{T}"/>
	/// <seealso cref="Formatter{T}"/>
	[Obsolete("Use the TokenHandler delegate instead.")]
    public abstract class TokenFunction
    {
        private string startDelimiter = string.Empty;
        private string endDelimiter = ")}";

        /// <summary>
        /// Initializes an instance of a TokenFunction with a start delimiter and the default end delimiter.
        /// </summary>
        /// <param name="tokenStartDelimiter">Start delimiter.</param>
        protected TokenFunction(string tokenStartDelimiter)
        {
            if (tokenStartDelimiter == null || tokenStartDelimiter.Length == 0)
            {
                throw new ArgumentNullException("tokenStartDelimiter");
            }

            this.startDelimiter = tokenStartDelimiter;
        }

        /// <summary>
        /// Initializes an instance of a TokenFunction with a start and end delimiter.
        /// </summary>
        /// <param name="tokenStartDelimiter">Start delimiter.</param>
        /// <param name="tokenEndDelimiter">End delimiter.</param>
        protected TokenFunction(string tokenStartDelimiter, string tokenEndDelimiter)
        {
            if (tokenStartDelimiter == null || tokenStartDelimiter.Length == 0)
            {
                throw new ArgumentNullException("tokenStartDelimiter");
            }
            if (tokenEndDelimiter == null || tokenEndDelimiter.Length == 0)
            {
                throw new ArgumentNullException("tokenEndDelimiter");
            }

            this.startDelimiter = tokenStartDelimiter;
            this.endDelimiter = tokenEndDelimiter;
        }

        /// <summary>
        /// Searches for token functions in the message and replace all with formatted values.
        /// </summary>
        /// <param name="messageBuilder">Message template containing tokens.</param>
        /// <param name="log">Log entry containing properties to format.</param>
        public virtual void Format(StringBuilder messageBuilder, LogEntry log)
        {
            int pos = 0;
            while (pos < messageBuilder.Length)
            {
                string messageString = messageBuilder.ToString();
                if (messageString.IndexOf(this.startDelimiter, StringComparison.Ordinal) == -1)
                {
                    break;
                }

                string tokenTemplate = GetInnerTemplate(pos, messageString);
                string tokenToReplace = this.startDelimiter + tokenTemplate + this.endDelimiter;
                pos = messageBuilder.ToString().IndexOf(tokenToReplace, StringComparison.Ordinal);

                string tokenValue = FormatToken(tokenTemplate, log);

                messageBuilder.Replace(tokenToReplace, tokenValue);
            }
        }

        /// <summary>
        /// Abstract method to process the token value between the start and end delimiter.
        /// </summary>
        /// <param name="tokenTemplate">Token value between the start and end delimiters.</param>
        /// <param name="log">Log entry to process.</param>
        /// <returns>Formatted value to replace the token.</returns>
        public abstract string FormatToken(string tokenTemplate, LogEntry log);

        /// <summary>
        /// Returns the template in between the paratheses for a token function.
        /// Expecting tokens in this format: {keyvalue(myKey1)}.
        /// </summary>
        /// <param name="startPos">Start index to search for the next token function.</param>
        /// <param name="message">Message template containing tokens.</param>
        /// <returns>Inner template of the function.</returns>
        protected virtual string GetInnerTemplate(int startPos, string message)
        {
            int tokenStartPos = message.IndexOf(this.startDelimiter, startPos, StringComparison.Ordinal) + this.startDelimiter.Length;
            int endPos = message.IndexOf(this.endDelimiter, tokenStartPos, StringComparison.Ordinal);
            return message.Substring(tokenStartPos, endPos - tokenStartPos);
        }
    }
}
