//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Represents a lexical analyzer for boolean expressions.
    /// </summary>
    public class LexicalAnalyzer
    {
        private const string Pattern =
            @"\(|\)|[Ii][:]|[Rr][:]|[Oo][Rr]|[Aa][Nn][Dd]|[Nn][Oo][Tt]|(""[^""]+"")|[^\u0000-\u0020""\(\)]+";

        private Match match;
        private Match previousMatch;
        private string expression;

        /// <summary>
        /// The expression to analyze.
        /// </summary>
        /// <param name="expression">A boolean expression.</param>
        public LexicalAnalyzer(string expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Gets the string value of the current token.
        /// </summary>
        /// <returns>The value of the current</returns>
        public string Current
        {
            get { return this.match.Value; }
        }        

        /// <summary>
        /// Gets the previous match of the regular expression.
        /// </summary>
        public Match PreviousMatch
        {
            get { return this.previousMatch; }
        }

        /// <summary>
        /// Gets the current match of the regular expression.
        /// </summary>
        /// <value>A <see cref="Match"/></value>
        public Match CurrentMatch
        {
            get { return this.match; }
        }

        /// <summary>
        /// Gets the type of the next token.
        /// </summary>
        /// <returns>A <see cref="TokenType"/> value.</returns>
        public TokenType MoveNext()
        {
            TokenType token;
            this.previousMatch = this.match;
            if (this.match == null)
            {
                this.match = Regex.Match(
                    this.expression,
                    Pattern,
                    RegexOptions.Compiled);
            }
            else
            {
                this.match = this.match.NextMatch();
            }

            if (this.match.Success)
            {
                string current = this.Current.ToLower(CultureInfo.InvariantCulture);
                switch (current)
                {
                    case "or":
                        token = TokenType.Or;
                        break;
                    case "and":
                        token = TokenType.And;
                        break;
                    case "i:":
                        token = TokenType.Identity;
                        break;
                    case "r:":
                        token = TokenType.Role;
                        break;
                    case "not":
                        token = TokenType.Not;
                        break;
                    case "(":
                        token = TokenType.LeftParenthesis;
                        break;
                    case ")":
                        token = TokenType.RightParenthesis;
                        break;
                    default:
                        if (current.IndexOf("\"") == 0)
                        {
                            if (current.LastIndexOf("\"") == current.Length - 1)
                            {
                                token = TokenType.QuotedString;
                            }
                            else
                            {
                                token = TokenType.InvalidCharacter;
                            }
                        }
                        else if (current == "*")
                        {
                            token = TokenType.Any;
                        }
                        else if (current == "?")
                        {
                            token = TokenType.Anonymous;
                        }
                        else
                        {
                            token = TokenType.Word;
                        }
                        break;
                }
            }
            else
            {
                token = TokenType.EndOfFile;
            }
            return token;
        }
    }
}