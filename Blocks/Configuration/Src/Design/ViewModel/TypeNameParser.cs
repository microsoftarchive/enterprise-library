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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="TypeNameParserHelper"/> attempts to parse a string version of a type name 
    /// to get the short name of the type.
    /// </summary>
    /// <remarks>
    /// This tries to avoid using <see cref="Type.GetType()"/> for calculating this in case
    /// the type is not available at design-timel.<br/>
    /// If <see cref="TypeNameParserHelper"/> cannot parse the type name, it will attempt to load
    /// the type using <see cref="Type.GetType()"/>. </remarks>
    /// 
    public static class TypeNameParserHelper
    {
        ///<summary>
        /// Parses the string version of the type name to get the short type name.
        ///</summary>
        ///<param name="typeName"></param>
        ///<returns></returns>
        public static string ParseTypeName(string typeName)
        {
            var result = TypeNameParser.Parse(typeName);

            if (result != null)
            {
                return result.Name;
            }

            var type = Type.GetType(typeName);

            if (type != null)
            {
                return GetTypeName(type);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetTypeName(Type type)
        {
            string result;

            if (!type.IsGenericType)
            {
                result = type.Name;
            }
            else
            {
                result = type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.OrdinalIgnoreCase));
                result += "<";

                foreach (var arg in type.GetGenericArguments())
                {
                    result += GetTypeName(arg);
                    result += ", ";
                }

                result = result.Substring(0, result.Length - 2);

                result += ">";
            }

            return result;
        }
    }

    /// <summary>
    /// Class that tracks the current input state of the parser.
    /// </summary>
    internal class InputStream
    {
        private readonly string input;
        private int currentPosition;

        public InputStream(string input)
        {
            this.input = input ?? string.Empty;
        }

        public bool AtEnd
        {
            get { return currentPosition == input.Length; }
        }

        public char CurrentChar
        {
            get { return input[currentPosition]; }
        }

        public int CurrentPosition
        {
            get { return currentPosition; }
        }

        public void Consume(int numChars)
        {
            currentPosition = Clamp(currentPosition + numChars);
        }

        public void BackupTo(int bookmark)
        {
            currentPosition = Clamp(bookmark);
        }

        private int Clamp(int newPosition)
        {
            if (newPosition < 0) newPosition = 0;
            if (newPosition > input.Length) newPosition = input.Length;
            return newPosition;
        }
    }

    /// <summary>
    /// Class containing information about a type name.
    /// </summary>
    internal class TypeNameInfo
    {
        /// <summary>
        /// The base name of the class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Namespace if any
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Assembly name, if any
        /// </summary>
        public string AssemblyName { get; set; }

        public bool IsGenericType { get; set; }

        public bool IsOpenGeneric { get; set; }

        public int NumGenericParameters { get { return GenericParameters.Count; } }

        public List<TypeNameInfo> GenericParameters { get; private set; }

        public TypeNameInfo()
        {
            GenericParameters = new List<TypeNameInfo>();
        }

        public string FullName
        {
            get
            {
                string name = Name;
                if (IsGenericType)
                {
                    name += '`' + NumGenericParameters.ToString(CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(Namespace))
                {
                    name = Namespace + '.' + name;
                }
                if (!string.IsNullOrEmpty(AssemblyName))
                {
                    name = name + ", " + AssemblyName;
                }
                return name;
            }
        }
    }

    /// <summary>
    /// Object containing the result of attempting to match a PEG rule.
    /// This object is the return type for all parsing methods.
    /// </summary>
    internal class ParseResult
    {
        public ParseResult(bool matched)
        {
            Matched = matched;
            MatchedString = string.Empty;
            ResultData = null;
        }

        public ParseResult(string matchedCharacters)
            : this(matchedCharacters, null)
        {
            Matched = true;
            MatchedString = matchedCharacters ?? string.Empty;
        }

        public ParseResult(string matchedCharacters, object resultData)
        {
            Matched = true;
            MatchedString = matchedCharacters ?? string.Empty;
            ResultData = resultData;
        }

        /// <summary>
        /// Did the rule match?
        /// </summary>
        public bool Matched { get; private set; }

        /// <summary>
        /// The characters that were matched (if any)
        /// </summary>
        public string MatchedString { get; private set; }

        /// <summary>
        /// Any extra information provided by the parsing expression
        /// (only set if the parse matched). The nature
        /// of the data varies depending on the parsing expression.
        /// </summary>
        public object ResultData { get; private set; }
    }

    /// <summary>
    /// Helper methods to make it easier to pull the data
    /// out of the result of a sequence expression.
    /// </summary>
    internal class SequenceResult : IList<ParseResult>
    {
        private readonly IList<ParseResult> resultData;

        public SequenceResult(ParseResult result)
        {
            resultData = (IList<ParseResult>)result.ResultData;
        }

        #region IList<ParseResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return resultData.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<ParseResult> GetEnumerator()
        {
            return resultData.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        public void Add(ParseResult item)
        {
            resultData.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. 
        ///                 </exception>
        public void Clear()
        {
            resultData.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param>
        public bool Contains(ParseResult item)
        {
            return resultData.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        ///                 </param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.
        ///                 </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.
        ///                 </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.
        ///                 </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
        ///                     -or-
        ///                 <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///                     -or-
        ///                     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        ///                 </exception>
        public void CopyTo(ParseResult[] array, int arrayIndex)
        {
            resultData.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        public bool Remove(ParseResult item)
        {
            return resultData.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return resultData.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return resultData.IsReadOnly; }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///                 </param>
        public int IndexOf(ParseResult item)
        {
            return resultData.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.
        ///                 </param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///                 </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///                 </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///                 </exception>
        public void Insert(int index, ParseResult item)
        {
            resultData.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.
        ///                 </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///                 </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///                 </exception>
        public void RemoveAt(int index)
        {
            resultData.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.
        ///                 </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///                 </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///                 </exception>
        public ParseResult this[int index]
        {
            get { return resultData[index]; }
            set { resultData[index] = value; }
        }

        #endregion
    }

    internal class TypeNameParser : ParseBuilder
    {
        public static TypeNameInfo Parse(string typeName)
        {
            var input = new InputStream(typeName);

            ParseResult result = Sequence(Match_TypeName, EOF)(input);
            if (!result.Matched) return null;

            var typeNameResult = new SequenceResult(result)[0];
            return (TypeNameInfo)typeNameResult.ResultData;
        }

        private class ParsedUnqualifiedName
        {
            public string Namespace;
            public string Rootname;
            public GenericParameters GenericParameters;
        }

        private class GenericParameters
        {
            public bool IsOpenGeneric;
            public int Count { get { return Parameters.Count; } }
            public readonly List<TypeNameInfo> Parameters = new List<TypeNameInfo>();
        }

        private static void InitializeTypeNameInfo(ParsedUnqualifiedName from, TypeNameInfo to)
        {
            to.Name = from.Rootname;
            to.Namespace = from.Namespace;
            to.IsGenericType = from.GenericParameters != null;

            if (to.IsGenericType)
            {
                to.IsOpenGeneric = from.GenericParameters.IsOpenGeneric;
                if (to.IsOpenGeneric)
                {
                    for (int i = 0; i < from.GenericParameters.Count; ++i)
                    {
                        to.GenericParameters.Add(null);
                    }
                }
                else
                {
                    foreach (var genericParam in from.GenericParameters.Parameters)
                    {
                        to.GenericParameters.Add(genericParam);
                    }
                }
            }
        }

        // Parsing expressions from our grammar.
        private static ParseResult Match_TypeName(InputStream input)
        {
            var resultData = new TypeNameInfo();

            ParseResult result = Sequence(
                WithAction(Match_UnqualifiedName, r => InitializeTypeNameInfo((ParsedUnqualifiedName)r.ResultData, resultData)),
                ZeroOrOne(Sequence(Match_Comma, WithAction(Match_AssemblyName, r => resultData.AssemblyName = r.MatchedString))))(input);

            if (!result.Matched) return result;
            return new ParseResult(result.MatchedString, resultData);
        }

        private static ParseResult Match_AssemblyName(InputStream input)
        {
            return Sequence(Match_SimpleName, ZeroOrMore(Sequence(Match_Comma, Match_AssemblyNamePart)))(input);
        }

        private static ParseResult Match_UnqualifiedName(InputStream input)
        {
            var resultData = new ParsedUnqualifiedName();

            var result =
                Sequence(
                    WithAction(ZeroOrOne(Match_Namespace), r => resultData.Namespace = (string)r.ResultData),
                    WithAction(Match_RootName, r => resultData.Rootname = r.MatchedString),
                    WithAction(ZeroOrOne(Match_GenericParameters), r => resultData.GenericParameters = (GenericParameters)r.ResultData)
                    )(input);

            if (result.Matched)
            {
                return new ParseResult(result.MatchedString, resultData);
            }
            return result;
        }

        private static ParseResult Match_Namespace(InputStream input)
        {
            var ids = new List<string>();
            ParseResult result = OneOrMore(
                WithAction(Sequence(Match_Id, Match_Dot),
                    r => ids.Add(new SequenceResult(r)[0].MatchedString)))(input);

            if (result.Matched)
            {
                var ns = string.Join(".", ids.ToArray());
                return new ParseResult(result.MatchedString, ns);
            }

            return result;
        }

        private static ParseResult Match_RootName(InputStream input)
        {
            return Sequence(Match_Id, ZeroOrMore(Match_NestedName))(input);
        }

        private static ParseResult Match_NestedName(InputStream input)
        {
            return Sequence(Match_Plus, Match_Id)(input);
        }

        private static ParseResult Match_GenericParameters(InputStream input)
        {
            return FirstOf(Match_ClosedGeneric, Match_OpenGeneric)(input);
        }

        private static ParseResult Match_OpenGeneric(InputStream input)
        {
            return FirstOf(Match_CLRSyntax, Match_EmptyBrackets)(input);
        }

        private static ParseResult Match_CLRSyntax(InputStream input)
        {
            var resultData = new GenericParameters();
            ParseResult result = Sequence(Match_Backquote,
                WithAction(OneOrMore(Match_Digit),
                    r =>
                    {
                        resultData.IsOpenGeneric = true;
                        int numParameters = int.Parse(r.MatchedString, CultureInfo.InvariantCulture);
                        for (int i = 0; i < numParameters; ++i)
                        {
                            resultData.Parameters.Add(null);
                        }
                    }))(input);
            if (result.Matched)
            {
                return new ParseResult(result.MatchedString, resultData);
            }
            return result;
        }

        private static ParseResult Match_EmptyBrackets(InputStream input)
        {
            var resultData = new GenericParameters();
            ParseResult result = Sequence(Match_LeftBracket,
                WithAction(ZeroOrMore(Match_Comma), r =>
                {
                    int numParameters = r.MatchedString.Length + 1;
                    resultData.IsOpenGeneric = true;
                    for (int i = 0; i < numParameters; ++i)
                    {
                        resultData.Parameters.Add(null);
                    }
                }),
                Match_RightBracket)(input);
            if (result.Matched)
            {
                return new ParseResult(result.MatchedString, resultData);
            }
            return result;
        }

        private static ParseResult Match_ClosedGeneric(InputStream input)
        {
            var result = Sequence(ZeroOrOne(Match_CLRSyntax), Match_TypeParameters)(input);
            if (result.Matched)
            {
                var genericParameters = new GenericParameters();
                genericParameters.IsOpenGeneric = false;
                genericParameters.Parameters.AddRange((List<TypeNameInfo>)(new SequenceResult(result)[1].ResultData));
                return new ParseResult(result.MatchedString, genericParameters);
            }
            return result;
        }

        private static ParseResult Match_TypeParameters(InputStream input)
        {
            var typeParameters = new List<TypeNameInfo>();
            var result =
                Sequence(Match_LeftBracket,
                    WithAction(Match_GenericTypeParameter, StoreTypeParameter(typeParameters)),
                    ZeroOrMore(Sequence(Match_Comma, WithAction(Match_GenericTypeParameter, StoreTypeParameter(typeParameters)))), Match_RightBracket)(input);

            if (result.Matched)
            {
                return new ParseResult(result.MatchedString, typeParameters);
            }
            return result;
        }

        private static Action<ParseResult> StoreTypeParameter(ICollection<TypeNameInfo> l)
        {
            return r => l.Add((TypeNameInfo)r.ResultData);
        }

        private static ParseResult Match_GenericTypeParameter(InputStream input)
        {

            return FirstOf(
                WithAction(Match_UnqualifiedName, r =>
                {
                    var result = new TypeNameInfo();
                    InitializeTypeNameInfo((ParsedUnqualifiedName)r.ResultData, result);
                    return new ParseResult(r.MatchedString, result);
                }),
                WithAction(Sequence(Match_LeftBracket, Match_TypeName, Match_RightBracket), r => new SequenceResult(r)[1]))
                (input);
        }

        private static ParseResult Match_SimpleName(InputStream input)
        {
            return Sequence(Match_AssemblyNameStart, ZeroOrMore(Match_AssemblyNameContinuation), Match_Spacing)(input);
        }

        private static ParseResult Match_AssemblyNamePart(InputStream input)
        {
            return FirstOf(Match_Culture, Match_Version, Match_PublicKey, Match_PublicKeyToken)(input);
        }

        private static ParseResult Match_Culture(InputStream input)
        {
            return Sequence(Match("Culture"), Match_Spacing, Match_Equals, Match_LanguageTag)(input);
        }

        private static ParseResult Match_LanguageTag(InputStream input)
        {
            return Sequence(Match_LangTagPart, ZeroOrOne(Sequence(Match('-'), Match_LangTagPart)), Match_Spacing)(input);
        }

        private static ParseResult Match_LangTagPart(InputStream input)
        {
            var isAlpha = Match(ch => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(ch));
            var optAlpha = ZeroOrOne(isAlpha);
            return Sequence(isAlpha, optAlpha, optAlpha, optAlpha, optAlpha, optAlpha, optAlpha, optAlpha,
                optAlpha, optAlpha, optAlpha, optAlpha, optAlpha, optAlpha, optAlpha, optAlpha)(input);
        }

        private static ParseResult Match_Version(InputStream input)
        {
            return Sequence(Match("Version"), Match_Spacing, Match_Equals, Match_VersionNumber, Match_Spacing)(input);
        }

        private static ParseResult Match_VersionNumber(InputStream input)
        {
            return
                Sequence(Match_Int, Match_Dot, Match_Int, Match_Dot, Match_Int, Match_Dot, Match_Int)(input);
        }

        private static ParseResult Match_PublicKey(InputStream input)
        {
            return
                Sequence(Match("PublicKey"), Match_Spacing, Match_Equals, OneOrMore(Match_HexDigit), Match_Spacing)(
                    input);
        }

        private static ParseResult Match_PublicKeyToken(InputStream input)
        {
            return Sequence(Match("PublicKeyToken"), Match_Spacing, Match_Equals, Match_PublicKeyValue)(input);
        }

        private static ParseResult Match_PublicKeyValue(InputStream input)
        {
            var seq = Enumerable.Repeat<Func<InputStream, ParseResult>>(Match_HexDigit, 16)
                .Concat(new Func<InputStream, ParseResult>[] { Match_Spacing });

            return Sequence(seq.ToArray())(input);
        }

        private static ParseResult Match_AssemblyNameStart(InputStream input)
        {
            return Sequence(Not(Match_Dot), Match_AssemblyNameChar)(input);
        }

        private static ParseResult Match_AssemblyNameContinuation(InputStream input)
        {
            return Match_AssemblyNameChar(input);
        }

        private static ParseResult Match_AssemblyNameChar(InputStream input)
        {
            return FirstOf(Match_QuotedChar, Match(ch => !"^/\\:?\"<>|,[]".Contains(ch)))(input);
        }

        private static ParseResult Match_Id(InputStream input)
        {
            return Sequence(Match_IdStart, ZeroOrMore(Match_IdContinuation))(input);
        }

        private static ParseResult Match_IdStart(InputStream input)
        {
            return FirstOf(Match_IdNonAlpha, Match_IdAlpha)(input);
        }

        private static ParseResult Match_IdContinuation(InputStream input)
        {
            return FirstOf(Match_IdNonAlpha, Match_IdAlphanumeric)(input);
        }

        private static ParseResult Match_IdAlpha(InputStream input)
        {
            return FirstOf(Match_QuotedChar, Match(ch => char.IsLetter(ch)))(input);
        }

        private static ParseResult Match_IdAlphanumeric(InputStream input)
        {
            return FirstOf(Match_QuotedChar, Match(ch => char.IsLetterOrDigit(ch)))(input);
        }

        private static ParseResult Match_QuotedChar(InputStream input)
        {
            return WithAction(
                Sequence(Match_Backslash, Any),
                result =>
                {
                    string ch = new SequenceResult(result)[1].MatchedString;
                    return new ParseResult(ch);
                })(input);
        }

        private static ParseResult Match_IdNonAlpha(InputStream input)
        {
            return Match(ch => "_$@?".Contains(ch))(input);
        }

        private static ParseResult Match_Digit(InputStream input)
        {
            return Match(ch => char.IsDigit(ch))(input);
        }

        private static ParseResult Match_HexDigit(InputStream input)
        {
            return Match(ch => "0123456789ABCDEFabcdef".Contains(ch))(input);
        }

        private static ParseResult Match_Int(InputStream input)
        {
            return WithAction(Sequence(Match_Digit, ZeroOrMore(Match_Digit)),
                r => new ParseResult(r.MatchedString, int.Parse(r.MatchedString, CultureInfo.InvariantCulture)))(input);
        }

        private static ParseResult Match_LeftBracket(InputStream input)
        {
            return Sequence(Match('['), Match_Spacing)(input);
        }

        private static ParseResult Match_RightBracket(InputStream input)
        {
            return Sequence(Match(']'), Match_Spacing)(input);
        }

        private static ParseResult Match_Dot(InputStream input)
        {
            return Match('.')(input);
        }

        private static ParseResult Match_Backquote(InputStream input)
        {
            return Match('`')(input);
        }

        private static ParseResult Match_Plus(InputStream input)
        {
            return Match('+')(input);
        }

        private static ParseResult Match_Comma(InputStream input)
        {
            return Sequence(Match(','), Match_Spacing)(input);
        }

        private static ParseResult Match_Backslash(InputStream input)
        {
            return Match('\\')(input);
        }

        private static ParseResult Match_Equals(InputStream input)
        {
            return Sequence(Match('='), Match_Spacing)(input);
        }

        private static ParseResult Match_Spacing(InputStream input)
        {
            return ZeroOrOne(Match_Space)(input);
        }

        private static ParseResult Match_Space(InputStream input)
        {
            return FirstOf(Match(' '), Match('\t'), Match_Eol)(input);
        }

        private static ParseResult Match_Eol(InputStream input)
        {
            return FirstOf(Match("\r\n"), Match('\r'), Match('\n'))(input);
        }
    }

    /// <summary>
    /// A simple implementing of the rules for a Parsing Expression Grammar
    /// parsing algorithm. This supplies basic methods to do the primitives
    /// of the PEG, and combinators to create larger rules.
    /// </summary>
    internal class ParseBuilder
    {
        private static readonly ParseResult matchFailed = new ParseResult(false);

        /// <summary>
        /// The PEG "dot" operator that matches and consumes one character.
        /// </summary>
        /// <param name="input">Input to the parser.</param>
        /// <returns>The parse result.</returns>
        public static ParseResult Any(InputStream input)
        {
            if (input.AtEnd)
            {
                return matchFailed;
            }

            var result = new ParseResult(input.CurrentChar.ToString());

            input.Consume(1);
            return result;
        }

        /// <summary>
        /// Parse function generator that returns a method to match a single,
        /// specific character.
        /// </summary>
        /// <param name="charToMatch">Character to match.</param>
        /// <returns>The generated parsing function.</returns>
        public static Func<InputStream, ParseResult> Match(char charToMatch)
        {
            return input =>
            {
                if (!input.AtEnd && input.CurrentChar == charToMatch)
                {
                    return MatchAndConsumeCurrentChar(input);
                }
                return matchFailed;
            };
        }

        public static Func<InputStream, ParseResult> Match(string s)
        {
            return input =>
            {
                int bookmark = input.CurrentPosition;
                foreach (char ch in s)
                {
                    if (!input.AtEnd && input.CurrentChar == ch)
                    {
                        input.Consume(1);
                    }
                    else
                    {
                        input.BackupTo(bookmark);
                        return matchFailed;
                    }
                }
                return new ParseResult(s);
            };
        }

        /// <summary>
        /// Parse function generator that checks if the current character matches
        /// the predicate supplied.
        /// </summary>
        /// <param name="predicate">Predicate used to determine if the character is in
        /// the given range.</param>
        /// <returns>The generated parsing function.</returns>
        public static Func<InputStream, ParseResult> Match(Func<char, bool> predicate)
        {
            return input =>
            {
                if (!input.AtEnd && predicate(input.CurrentChar))
                {
                    return MatchAndConsumeCurrentChar(input);
                }
                return matchFailed;
            };
        }

        /// <summary>
        /// The "*" operator - match zero or more of the inner parse expressions.
        /// </summary>
        /// <param name="inner">Parse method to repeat matching.</param>
        /// <returns>The generated parsing function.</returns>
        public static Func<InputStream, ParseResult> ZeroOrMore(Func<InputStream, ParseResult> inner)
        {
            return input =>
            {
                var results = new List<ParseResult>();
                ParseResult result = inner(input);
                if (!result.Matched)
                {
                    return new ParseResult(true);
                }

                results.Add(result);
                string matchedString = result.MatchedString;
                result = inner(input);
                while (result.Matched)
                {
                    matchedString += result.MatchedString;
                    results.Add(result);
                    result = inner(input);
                }
                return new ParseResult(matchedString, results);
            };
        }

        public static Func<InputStream, ParseResult> ZeroOrOne(Func<InputStream, ParseResult> expression)
        {
            return input =>
            {
                var result = expression(input);
                if (result.Matched)
                {
                    return result;
                }
                return new ParseResult(true);
            };
        }

        public static Func<InputStream, ParseResult> OneOrMore(Func<InputStream, ParseResult> expression)
        {
            return input =>
            {
                int bookmark = input.CurrentPosition;
                var results = new List<ParseResult>();
                ParseResult result = expression(input);
                if (!result.Matched)
                {
                    input.BackupTo(bookmark);
                    return matchFailed;
                }
                string matchedString = "";
                while (result.Matched)
                {
                    results.Add(result);
                    matchedString += result.MatchedString;
                    result = expression(input);
                }
                return new ParseResult(matchedString, results);
            };
        }

        /// <summary>
        /// Parsing combinator that matches all of the given expressions in
        /// order, or matches none of them.
        /// </summary>
        /// <param name="expressions">Expressions that form the sequence to match.</param>
        /// <returns>The combined sequence.</returns>
        public static Func<InputStream, ParseResult> Sequence(params Func<InputStream, ParseResult>[] expressions)
        {
            return input =>
            {
                int bookmark = input.CurrentPosition;
                var results = new List<ParseResult>(expressions.Length);
                var matchedString = "";

                foreach (var expression in expressions)
                {
                    var result = expression(input);
                    if (!result.Matched)
                    {
                        input.BackupTo(bookmark);
                        return matchFailed;
                    }

                    results.Add(result);
                    matchedString += result.MatchedString;
                }

                return new ParseResult(matchedString, results);
            };
        }

        /// <summary>
        /// Parsing combinator that implements the PEG prioritized choice operator. Basically,
        /// try each of the expressions in order, and match if any of them match, stopping on the
        /// first match.
        /// </summary>
        /// <param name="expressions">Expressions that form the set of alternatives.</param>
        /// <returns>The combined parsing method.</returns>
        public static Func<InputStream, ParseResult> FirstOf(params Func<InputStream, ParseResult>[] expressions)
        {
            return input =>
            {
                foreach (var expression in expressions)
                {
                    ParseResult result = expression(input);
                    if (result.Matched)
                    {
                        return result;
                    }
                }
                return matchFailed;
            };
        }

        /// <summary>
        /// Parsing combinator implementing the "not" predicate. This wraps
        /// the given <paramref name="expression"/> parsing method with a check
        /// to see if it matched. If it matched, then the Not fails, and vice
        /// versa. The result consumes no input.
        /// </summary>
        /// <param name="expression">The parse method to wrap.</param>
        /// <returns>The generated parsing function.</returns>
        public static Func<InputStream, ParseResult> Not(Func<InputStream, ParseResult> expression)
        {
            return input =>
            {
                int bookmark = input.CurrentPosition;
                ParseResult innerResult = expression(input);
                input.BackupTo(bookmark);

                return new ParseResult(!innerResult.Matched);
            };
        }

        /// <summary>
        /// Parsing expression that matches End of input.
        /// </summary>
        /// <param name="input">Parser input.</param>
        /// <returns>Parse result</returns>
        public static ParseResult EOF(InputStream input)
        {
            return new ParseResult(input.AtEnd);
        }

        /// <summary>
        /// Combinator that executes an action if the given expression matched.
        /// </summary>
        /// <param name="expression">Parsing expression to match.</param>
        /// <param name="onMatched">Action to execute if <paramref name="expression"/>
        /// matched. Input is the matched text from <paramref name="expression"/>.</param>
        /// <returns>The result of <paramref name="expression"/>.</returns>
        public static Func<InputStream, ParseResult> WithAction(Func<InputStream, ParseResult> expression, Action<ParseResult> onMatched)
        {
            return input =>
            {
                ParseResult result = expression(input);
                if (result.Matched)
                {
                    onMatched(result);
                }
                return result;
            };
        }

        /// <summary>
        /// Combinator that executes an action if the given expression matched.
        /// </summary>
        /// <param name="expression">parsing expression to match.</param>
        /// <param name="onMatched">Method to execute if a match happens. This method returns
        /// the <see cref="ParseResult"/> that will be returned from the combined expression.</param>
        /// <returns>The result of <paramref name="onMatched"/> if expression matched, else
        /// whatever <paramref name="expression"/> returned.</returns>
        public static Func<InputStream, ParseResult> WithAction(Func<InputStream, ParseResult> expression, Func<ParseResult, ParseResult> onMatched)
        {
            return input =>
            {
                ParseResult result = expression(input);
                if (result.Matched)
                {
                    return onMatched(result);
                }
                return result;
            };
        }

        private static ParseResult MatchAndConsumeCurrentChar(InputStream input)
        {
            var result = new ParseResult(input.CurrentChar.ToString());
            input.Consume(1);
            return result;
        }
    }
}
