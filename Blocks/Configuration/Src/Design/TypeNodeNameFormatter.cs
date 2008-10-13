//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.IO;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Used to create a friendly name for configurations nodes that represent types.
    /// </summary>
    public class TypeNodeNameFormatter
    {
        /// <summary>
        /// Creates a friendly name based on a typeName, which can be used as a displayname within a graphical tool.
        /// </summary>
        /// <param name="typeName">The typeName that should be used.</param>
        /// <returns>A friendly name that can be used as a display name.</returns>
        public string CreateName(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                StringReader reader = new StringReader(typeName);
                StringBuilder nameBuilder = new StringBuilder();

                if (TryCreateName(reader, nameBuilder, TypeNameKind.Normal))
                {
                    return nameBuilder.ToString();
                }
                else
                {
                    return typeName;
                }
            }

            return string.Empty;
        }

        private bool TryCreateName(StringReader reader, StringBuilder nameBuilder, TypeNameKind nameEnd)
        {
            SkipLeadingWhitespace(reader);

            bool isGeneric;

            if (!TryAddTypeName(reader, nameBuilder, out isGeneric))
            {
                return false;
            }

            if (isGeneric)
            {
                if (!TryAddGenericParameters(reader, nameBuilder))
                {
                    return false;
                }
            }

            return TryConsumeRemainingInput(reader, nameEnd);
        }

        private static void SkipLeadingWhitespace(StringReader reader)
        {
            int current;

            while ((current = reader.Peek()) != -1)
            {
                if (!char.IsWhiteSpace((char)current))
                {
                    break;
                }
                reader.Read();  // discard whitespace
            }
        }

        private bool TryAddTypeName(StringReader reader, StringBuilder nameBuilder, out  bool isGeneric)
        {
            isGeneric = false;

            int startingIndex = nameBuilder.Length;
            int current;

            // fail if EOF
            if (reader.Peek() == -1)
            {
                return false;
            }

            // read the type name, discarding namespaces and converting '+' to '.'
            while ((current = reader.Peek()) != -1)
            {
                char currentChar = (char)current;

                if (currentChar == ',' || currentChar == ']')
                {
                    // this might be the end of non-assembly-qualified generic parameter name.
                    // leave the char and let the caller sort it out.
                    break;
                }

                reader.Read();  // consume the char

                if (currentChar == '.')
                {
                    // assume what we had before was a namespace; discard it.
                    nameBuilder.Remove(startingIndex, nameBuilder.Length - startingIndex);
                }
                else if (currentChar == '`')
                {
                    // it's a generic type; the name ends here.
                    isGeneric = true;
                    break;
                }
                else if (currentChar == '+')
                {
                    // deal with nested types
                    nameBuilder.Append('.');
                }
                else if (IsValidIdentifierChar(currentChar))
                {
                    nameBuilder.Append(currentChar);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidIdentifierChar(char currentChar)
        {
            return char.IsLetterOrDigit(currentChar) || currentChar == '_';
        }

        private bool TryAddGenericParameters(StringReader reader, StringBuilder nameBuilder)
        {
            int parameterCount;
            int current;
            bool isClosed = false;

            // fail if EOF - need at least the parameters count.
            if (reader.Peek() == -1)
            {
                return false;
            }

            nameBuilder.Append('<');

            #region grab the number of parameters

            string parametersCountString = "";      // don't bother with a string builder here.
            while ((current = reader.Peek()) != -1)
            {
                char currentChar = (char)current;

                if (char.IsDigit(currentChar))
                {
                    parametersCountString += currentChar;
                    reader.Read();
                }
                else if (currentChar == '[')
                {
                    isClosed = true;
                    reader.Read();      // consume the closed generic bracket
                    break;
                }
                else if (currentChar == ',')
                {
                    break;              // can be the end of a non-assembly-qualified name followed by another parameter
                }
                else
                {
                    // invalid type name
                    return false;
                }
            }
            if (!int.TryParse(parametersCountString, out parameterCount))
            {
                return false;
            }

            #endregion

            #region add the parameters of a closed generic, or placeholders

            if (isClosed)
            {
                for (int i = 0; i < parameterCount; i++)
                {
                    if (i > 0)
                    {
                        if ((current = reader.Read()) == -1 || ((char)current) != ',')
                        {
                            // this is not the first parameter, so a comma is expected.
                            return false;
                        }

                        nameBuilder.Append(", ");
                    }

                    if ((current = reader.Peek()) == -1)
                    {
                        return false;       // fail if EOF
                    }
                    if (((char)current) == '[')
                    {
                        // this is an assembly-qualified type name, which ends with a bracket
                        reader.Read();      // consume the peeked bracket
                        if (!TryCreateName(reader, nameBuilder, TypeNameKind.EndWithBracket))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // this is a non-assembly-qualified type name
                        if (!TryCreateName(reader, nameBuilder, TypeNameKind.NoAssemblyName))
                        {
                            return false;
                        }
                    }
                }

                // check there's a bracket after all the parameters are dealt with
                if ((current = reader.Read()) == -1 || ((char)current) != ']')
                {
                    return false;
                }
            }
            else
            {
                // open type - indicate the empty parameter positions
                nameBuilder.Append(new string(',', parameterCount - 1));
            }

            #endregion

            nameBuilder.Append('>');

            return true;
        }

        private bool TryConsumeRemainingInput(StringReader reader, TypeNameKind nameEnd)
        {
            int current;

            switch (nameEnd)
            {
                case TypeNameKind.Normal:
                    // just return, the rest of the string can be ignored, 
                    // as long as the current char is a comma or the EOF is reached
                    return ((current = reader.Read()) == -1) || (((char)current) == ',');

                case TypeNameKind.NoAssemblyName:
                    // should find a comma now, or an ending bracket owned by the parent type
                    current = reader.Peek();
                    if (current != -1)
                    {
                        char currentChar = (char)current;
                        if (currentChar == ',')
                        {
                            return true;
                        }
                        else if (currentChar == ']')
                        {
                            return true;    // let the caller handler the ending bracket
                        }
                    }
                    return false;

                case TypeNameKind.EndWithBracket:
                    // should consume everything up to a bracket
                    while ((current = reader.Read()) != -1)
                    {
                        if (((char)current) == ']')
                        {
                            return true;
                        }
                    }
                    return false;       // found the end of the string finding the ending bracket

                default:
                    return false;       // unknown situation
            }
        }

        private enum TypeNameKind
        {
            Normal,
            NoAssemblyName,
            EndWithBracket
        }
    }
}