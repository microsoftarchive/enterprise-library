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

using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Represents a parser for identity role rule expresssions.
    /// </summary>
    public class Parser
    {
        private LexicalAnalyzer lexer;
        private TokenType token;

        /// <summary>
        /// Parses the the specified expression into a
        /// <see cref="BooleanExpression"/>.
        /// </summary>
        /// <param name="expression">An expression.</param>
        /// <returns>A <see cref="BooleanExpression"/>
        /// object that is the root of the parse tree.</returns>
        public BooleanExpression Parse(string expression)
        {
            this.lexer = new LexicalAnalyzer(expression);
            this.MoveNext();
            BooleanExpression c = this.ParseOrOperator();
            this.AssertTokenType(TokenType.EndOfFile);
            return c;
        }

        private BooleanExpression ParseOrOperator()
        {
            BooleanExpression c = this.ParseAndOperator();
            while (this.token == TokenType.Or)
            {
                this.MoveNext();
                c = new OrOperator(c, this.ParseAndOperator());
            }
            return c;
        }

        private BooleanExpression ParseAndOperator()
        {
            BooleanExpression c = this.ParseSimpleExpression();
            while (this.token == TokenType.And)
            {
                this.MoveNext();
                c = new AndOperator(c, this.ParseSimpleExpression());
            }
            return c;
        }

        private BooleanExpression ParseSimpleExpression()
        {
            BooleanExpression expression = null;

            switch (this.token)
            {
                case TokenType.LeftParenthesis:
                    this.MoveNext();
                    BooleanExpression c = this.ParseOrOperator();
                    this.AssertTokenType(TokenType.RightParenthesis);
                    expression = c;
                    break;
                case TokenType.Identity:
                    this.MoveNext();
                    expression = new IdentityExpression(this.ParseWordExpression());
                    break;
                case TokenType.Role:
                    this.MoveNext();
                    expression = new RoleExpression(this.ParseWordExpression());
                    break;
                case TokenType.Not:
                    this.MoveNext();
                    expression = new NotOperator(this.ParseOrOperator());
                    break;
                default:
                    TokenType[] types = new TokenType[]
                        {
                            TokenType.LeftParenthesis,
                            TokenType.Identity,
                            TokenType.Role,
                            TokenType.Not
                        };

                    throw new SyntaxException(
                        string.Format(Resources.Culture, Properties.Resources.UnexpectedTokenMessage, 
                            ConcatTokenNames(types),
                            this.lexer.Current,
                            this.GetIndex()),
                        this.GetIndex());
            }

            return expression;
        }

        private WordExpression ParseWordExpression()
        {
            if (this.token != TokenType.Word
                && this.token != TokenType.QuotedString
                && this.token != TokenType.Anonymous
                && this.token != TokenType.Any)
            {
                this.AssertTokenType(TokenType.Word);
            }

            string word = this.lexer.Current;

            WordExpression wordExpression = null;

            switch (this.token)
            {
                case TokenType.Word:
                    wordExpression = new WordExpression(word);
                    break;
                case TokenType.QuotedString:
                    string wordWithoutQuotes = word.Substring(1, word.Length - 2);
                    wordExpression = new WordExpression(wordWithoutQuotes);
                    break;
                case TokenType.Any:
                    wordExpression = new AnyExpression();
                    break;
                case TokenType.Anonymous:
                    wordExpression = new AnonymousExpression();
                    break;
            }

            this.MoveNext();
            return wordExpression;
        }

        /// <devdoc>Get the next token from the lexer.</devdoc>
        private void MoveNext()
        {
            this.token = this.lexer.MoveNext();
        }

        /// <devdoc>Asserts that the current token is 
        /// of the specified type.</devdoc>
        private void AssertTokenType(TokenType expected)
        {
            if (this.token != expected)
            {
                string message = string.Format(Resources.Culture, Properties.Resources.UnexpectedTokenMessage, 
                    this.GetTokenName(this.token),
                    this.GetTokenName(expected),
                    this.GetIndex());
                throw new SyntaxException(message, this.GetIndex());
            }
            this.MoveNext();
        }

        private int GetIndex()
        {
            int index = 0;
            if (this.lexer.PreviousMatch != null)
            {
                index = this.lexer.PreviousMatch.Index +
                    this.lexer.PreviousMatch.Length - 1;
            }
            return index;
        }

        private string GetTokenName(TokenType t)
        {
            switch (t)
            {
                case TokenType.Or:
                    return "OR";
                case TokenType.And:
                    return "AND";
                case TokenType.Identity:
                    return "I:";
                case TokenType.Role:
                    return "R:";
                case TokenType.Not:
                    return "NOT";
                case TokenType.Any:
                    return "*";
                case TokenType.Anonymous:
                    return "?";
                case TokenType.Word:
                    return "word";
                case TokenType.LeftParenthesis:
                    return "(";
                case TokenType.RightParenthesis:
                    return ")";
                case TokenType.QuotedString:
                    return "quoted string";
                case TokenType.EndOfFile:
                    return "end of file";
                default:
                    return "???";
            }
        }

        private string ConcatTokenNames(TokenType[] tokenTypes)
        {
            StringBuilder message = new StringBuilder();

            for (int i = 0; i < tokenTypes.Length; i++)
            {
                message.Append('"');
                message.Append(GetTokenName(tokenTypes[i]));
                message.Append('"');

                if (tokenTypes.Length > 2 && i < tokenTypes.Length - 1)
                {
                    message.Append(", ");
                }

                if (i == tokenTypes.Length - 2)
                {
                    message.Append(Properties.Resources.Or);
                    message.Append(' ');
                }
            }

            return message.ToString();
        }
    }
}