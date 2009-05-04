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

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Specifies the type of a token in an expression.
    /// </summary>
    public enum TokenType : int
    {
        /// <summary>
        /// Invalid character
        /// </summary>
        InvalidCharacter = -1,
        /// <summary>
        /// No token found
        /// </summary>
        NoToken = 0,
        /// <summary>
        /// Or token
        /// </summary>
        Or = 1,
        /// <summary>
        /// And token
        /// </summary>
        And = 2,
        /// <summary>
        /// Identity token 
        /// </summary>
        Identity = 3,
        /// <summary>
        /// Role token
        /// </summary>
        Role = 4,
        /// <summary>
        /// Not token
        /// </summary>
        Not = 5,
        /// <summary>
        /// Word token
        /// </summary>
        Word = 6,
        /// <summary>
        /// Any token
        /// </summary>
        Any = 7,
        /// <summary>
        /// Anonymous token
        /// </summary>
        Anonymous = 8,
        /// <summary>
        /// Left Paren token
        /// </summary>
        LeftParenthesis = 9,
        /// <summary>
        /// Right paren token
        /// </summary>
        RightParenthesis = 10,
        /// <summary>
        /// Quoted string token
        /// </summary>
        QuotedString = 11,
        /// <summary>
        /// EOF token
        /// </summary>
        EndOfFile = 12
    }
}
