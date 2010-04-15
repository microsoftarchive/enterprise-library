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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.RuleEditor;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents a customized <see cref="RichTextBox"/> 
    /// control that adds syntax highlighting for
    /// identity role rule expressions.
    /// </summary>
    internal class ExpressionTextBox : RichTextBox
    {
        private const int UnderlineType = 0x800000;
        private const int GetCharFormat = 0x043A;
        private const int SelectionCharacterFormatting = 0x01;
        private const int SetCharFormat = 0x0444;
        private const int SetRedraw = 0x0B;
        private const int SetEventMask = 0x0431;

        private bool parsed;
        private Parser parser;
        private string parseStatus = String.Empty;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExpressionTextBox"/> class.
        /// </summary>
        public ExpressionTextBox()
            : base()
        {
            parser = new Parser();
        }

        /// <summary>
        /// Gets the current parse status message.
        /// </summary>
        public string ParseStatus
        {
            get { return parseStatus; }
        }

        /// <summary>
        /// Gets whether or not the expression is valid.
        /// </summary>
        public bool ExpressionIsValid
        {
            get
            {
                if (this.Text.Length == 0)
                {
                    return true;
                }

                return parsed;
            }
        }

        /// <summary> 
        /// Gets or sets the underline style to apply to the current selection or insertion point. 
        /// </summary> 
        /// <value>A <see cref="UnderlineStyle"/> that represents the underline style to 
        /// apply to the current text selection or to text entered after the insertion point.</value> 
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UnderlineStyle SelectionUnderlineStyle
        {
            [method: System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode = true)]
            get
            {
                NativeMethods.CHARFORMAT2 fmt = new NativeMethods.CHARFORMAT2();
                fmt.cbSize = Marshal.SizeOf(fmt);

                // Get the underline style 
                NativeMethods.SendMessage(new HandleRef(this, Handle), GetCharFormat, new IntPtr(SelectionCharacterFormatting), ref fmt);
                if ((fmt.dwMask & UnderlineType) == 0)
                {
                    return UnderlineStyle.None;
                }
                else
                {
                    byte style = (byte)(fmt.bUnderlineType & 0x0F);
                    return (UnderlineStyle)style;
                }
            }
            [method: System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode = true)]
            set
            {
                // Ensure we don't alter the color 
                UnderlineColor color = this.SelectionUnderlineColor;

                // Ensure we don't show it if it shouldn't be shown 
                if (value == UnderlineStyle.None)
                {
                    color = UnderlineColor.Black;
                }

                // Set the underline type 
                NativeMethods.CHARFORMAT2 fmt = new NativeMethods.CHARFORMAT2();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.dwMask = UnderlineType;
                fmt.bUnderlineType = (byte)((byte)value | (byte)color);
                NativeMethods.SendMessage(new HandleRef(this, Handle), SetCharFormat, new IntPtr(SelectionCharacterFormatting), ref fmt);
            }
        }

        /// <summary> 
        /// Gets or sets the underline color to apply to the current selection or insertion point. 
        /// </summary> 
        /// <value>A <see cref="UnderlineColor"/> that represents the underline color to 
        /// apply to the current text selection or to text entered after the insertion point.</value> 
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UnderlineColor SelectionUnderlineColor
        {
            [method: System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode = true)]
            get
            {
                NativeMethods.CHARFORMAT2 fmt = new NativeMethods.CHARFORMAT2();
                fmt.cbSize = Marshal.SizeOf(fmt);

                // Get the underline color 
                NativeMethods.SendMessage(new HandleRef(this, Handle), GetCharFormat, new IntPtr(SelectionCharacterFormatting), ref fmt);
                if ((fmt.dwMask & UnderlineType) == 0)
                {
                    return UnderlineColor.None;
                }
                else
                {
                    byte style = (byte)(fmt.bUnderlineType & 0xF0);
                    return (UnderlineColor)style;
                }
            }
            [method: System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode = true)]
            set
            {
                // If the an underline color of "None" is specified, remove underline effect 
                if (value == UnderlineColor.None)
                {
                    this.SelectionUnderlineStyle = UnderlineStyle.None;
                }
                else
                {
                    // Ensure we don't alter the style 
                    UnderlineStyle style = this.SelectionUnderlineStyle;

                    // Ensure we don't show it if it shouldn't be shown 
                    if (style == UnderlineStyle.None)
                    {
                        value = UnderlineColor.Black;
                    }

                    // Set the underline color 
                    NativeMethods.CHARFORMAT2 fmt = new NativeMethods.CHARFORMAT2();
                    fmt.cbSize = Marshal.SizeOf(fmt);
                    fmt.dwMask = UnderlineType;
                    fmt.bUnderlineType = (byte)((byte)style | (byte)value);
                    NativeMethods.SendMessage(new HandleRef(this, Handle), SetCharFormat, new IntPtr(SelectionCharacterFormatting), ref fmt);
                }
            }
        }

        internal void HighlightText()
        {
            int selectionStart = this.SelectionStart;
            int selectionLength = this.SelectionLength;

            HighlightTokens();
            ParseText();

            this.SelectionStart = selectionStart;
            this.SelectionLength = selectionLength;
        }

        /// <summary>
        /// Adds syntax highlighting to the current text.
        /// </summary>
        /// <param name="e">The arguments for the event.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            this.HighlightText();
            base.OnTextChanged(e);
        }

        /// <devDoc>
        /// Initializes Parser to improve first time text input performance.
        /// </devDoc>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal void InitParser()
        {
            try
            {
                parser.Parse(String.Empty);
            }
            catch
            { /* suppress */
            }
        }

        private void HighlightTokens()
        {
            this.SelectionStart = 0;
            this.SelectionLength = this.Text.Length;

            this.SelectionUnderlineColor = UnderlineColor.None;
            this.SelectionUnderlineStyle = UnderlineStyle.None;

            LexicalAnalyzer lexer = new LexicalAnalyzer(this.Text);

            TokenType lastToken = TokenType.Any;

            for (TokenType tokenType = lexer.MoveNext(); tokenType != TokenType.EndOfFile; tokenType = lexer.MoveNext())
            {
                this.SelectionStart = lexer.CurrentMatch.Index;
                this.SelectionLength = lexer.CurrentMatch.Length;

                switch (tokenType)
                {
                    case TokenType.And:
                    case TokenType.Or:
                    case TokenType.Not:
                        this.SelectionColor = Color.Blue;
                        break;
                    case TokenType.Identity:
                    case TokenType.Role:
                        this.SelectionColor = Color.Navy;
                        break;
                    case TokenType.Word:
                    case TokenType.QuotedString:
                        if (lastToken == TokenType.Identity || lastToken == TokenType.Role)
                        {
                            this.SelectionColor = Color.Navy;
                        }
                        else
                        {
                            this.SelectionColor = Color.Black;
                        }
                        break;
                    default:
                        this.SelectionColor = Color.Black;
                        break;
                }

                lastToken = tokenType;
            }
        }

        private void ParseText()
        {
            try
            {
                parsed = true;
                parser.Parse(this.Text);
                parseStatus = Resources.ParseSucceededMessageBox;
            }
            catch (SyntaxException ex)
            {
                parsed = false;
                parseStatus = string.Format(CultureInfo.CurrentCulture, Resources.ParseFailedMessage, ex.Message);
                int position = ex.Index;
                if (position < this.Text.Length - 1)
                {
                    position++;
                }

                this.SelectionStart = position;
                this.SelectionLength = this.Text.Length == 0 ? 0 : this.Text.Length - position - 1;
                this.SelectionUnderlineStyle = UnderlineStyle.Wave;
                this.SelectionUnderlineColor = UnderlineColor.Red;
            }
        }
    }
}
