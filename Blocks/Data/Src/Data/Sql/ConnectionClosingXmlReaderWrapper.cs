//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql
{
    /// <summary>
    /// XmlReader wrapper that closes a DbConnection when it's closed or disposed.
    /// </summary>
    internal class ConnectionClosingXmlReaderWrapper : XmlReader
    {
        private readonly XmlReader innerReader;
        private readonly DbConnection connection;

        public ConnectionClosingXmlReaderWrapper(XmlReader xmlReader, DbConnection connection)
        {
            if (xmlReader == null)
            {
                throw new ArgumentNullException("xmlReader");
            }
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.innerReader = xmlReader;
            this.connection = connection;
        }

        public override void Close()
        {
            this.innerReader.Close();
            this.connection.Close();
        }

        protected override void Dispose(bool disposing)
        {
            ((IDisposable)this.innerReader).Dispose();
            this.connection.Dispose();
        }

        public override string GetAttribute(int i)
        {
            return this.innerReader.GetAttribute(i);
        }

        public override string GetAttribute(string name)
        {
            return this.innerReader.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return this.innerReader.GetAttribute(name, namespaceURI);
        }

        public override bool IsStartElement()
        {
            return this.innerReader.IsStartElement();
        }

        public override bool IsStartElement(string name)
        {
            return this.innerReader.IsStartElement(name);
        }

        public override bool IsStartElement(string localname, string ns)
        {
            return this.innerReader.IsStartElement(localname, ns);
        }

        public override string LookupNamespace(string prefix)
        {
            return this.innerReader.LookupNamespace(prefix);
        }

        public override void MoveToAttribute(int i)
        {
            this.innerReader.MoveToAttribute(i);
        }

        public override bool MoveToAttribute(string name)
        {
            return this.innerReader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return this.innerReader.MoveToAttribute(name, ns);
        }

        public override XmlNodeType MoveToContent()
        {
            return this.innerReader.MoveToContent();
        }

        public override bool MoveToElement()
        {
            return this.innerReader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return this.innerReader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return this.innerReader.MoveToNextAttribute();
        }

        public override bool Read()
        {
            return this.innerReader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return this.innerReader.ReadAttributeValue();
        }

        public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
        {
            return this.innerReader.ReadContentAs(returnType, namespaceResolver);
        }

        public override int ReadContentAsBase64(byte[] buffer, int index, int count)
        {
            return this.innerReader.ReadContentAsBase64(buffer, index, count);
        }

        public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
        {
            return this.innerReader.ReadContentAsBinHex(buffer, index, count);
        }

        public override bool ReadContentAsBoolean()
        {
            return this.innerReader.ReadContentAsBoolean();
        }

        public override DateTime ReadContentAsDateTime()
        {
            return this.innerReader.ReadContentAsDateTime();
        }

        public override decimal ReadContentAsDecimal()
        {
            return this.innerReader.ReadContentAsDecimal();
        }

        public override double ReadContentAsDouble()
        {
            return this.innerReader.ReadContentAsDouble();
        }

        public override float ReadContentAsFloat()
        {
            return this.innerReader.ReadContentAsFloat();
        }

        public override int ReadContentAsInt()
        {
            return this.innerReader.ReadContentAsInt();
        }

        public override long ReadContentAsLong()
        {
            return this.innerReader.ReadContentAsLong();
        }

        public override object ReadContentAsObject()
        {
            return this.innerReader.ReadContentAsObject();
        }

        public override string ReadContentAsString()
        {
            return this.innerReader.ReadContentAsString();
        }

        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
        {
            return this.innerReader.ReadElementContentAs(returnType, namespaceResolver);
        }

        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAs(returnType, namespaceResolver, localName, namespaceURI);
        }

        public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
        {
            return this.innerReader.ReadElementContentAsBase64(buffer, index, count);
        }

        public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
        {
            return this.innerReader.ReadElementContentAsBinHex(buffer, index, count);
        }

        public override bool ReadElementContentAsBoolean()
        {
            return this.innerReader.ReadElementContentAsBoolean();
        }

        public override bool ReadElementContentAsBoolean(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsBoolean(localName, namespaceURI);
        }

        public override DateTime ReadElementContentAsDateTime()
        {
            return this.innerReader.ReadElementContentAsDateTime();
        }

        public override DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsDateTime(localName, namespaceURI);
        }

        public override decimal ReadElementContentAsDecimal()
        {
            return this.innerReader.ReadElementContentAsDecimal();
        }

        public override decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsDecimal(localName, namespaceURI);
        }

        public override double ReadElementContentAsDouble()
        {
            return this.innerReader.ReadElementContentAsDouble();
        }

        public override double ReadElementContentAsDouble(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsDouble(localName, namespaceURI);
        }

        public override float ReadElementContentAsFloat()
        {
            return this.innerReader.ReadElementContentAsFloat();
        }

        public override float ReadElementContentAsFloat(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsFloat(localName, namespaceURI);
        }

        public override int ReadElementContentAsInt()
        {
            return this.innerReader.ReadElementContentAsInt();
        }

        public override int ReadElementContentAsInt(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsInt(localName, namespaceURI);
        }

        public override long ReadElementContentAsLong()
        {
            return this.innerReader.ReadElementContentAsLong();
        }

        public override long ReadElementContentAsLong(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsLong(localName, namespaceURI);
        }

        public override object ReadElementContentAsObject()
        {
            return this.innerReader.ReadElementContentAsObject();
        }

        public override object ReadElementContentAsObject(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsObject(localName, namespaceURI);
        }

        public override string ReadElementContentAsString()
        {
            return this.innerReader.ReadElementContentAsString();
        }

        public override string ReadElementContentAsString(string localName, string namespaceURI)
        {
            return this.innerReader.ReadElementContentAsString(localName, namespaceURI);
        }

        public override string ReadElementString()
        {
            return this.innerReader.ReadElementString();
        }

        public override string ReadElementString(string name)
        {
            return this.innerReader.ReadElementString(name);
        }

        public override string ReadElementString(string localname, string ns)
        {
            return this.innerReader.ReadElementString(localname, ns);
        }

        public override void ReadEndElement()
        {
            this.innerReader.ReadEndElement();
        }

        public override string ReadInnerXml()
        {
            return this.innerReader.ReadInnerXml();
        }

        public override string ReadOuterXml()
        {
            return this.innerReader.ReadOuterXml();
        }

        public override void ReadStartElement()
        {
            this.innerReader.ReadStartElement();
        }

        public override void ReadStartElement(string name)
        {
            this.innerReader.ReadStartElement(name);
        }

        public override void ReadStartElement(string localname, string ns)
        {
            this.innerReader.ReadStartElement(localname, ns);
        }

        public override string ReadString()
        {
            return this.innerReader.ReadString();
        }

        public override XmlReader ReadSubtree()
        {
            return this.innerReader.ReadSubtree();
        }

        public override bool ReadToDescendant(string name)
        {
            return this.innerReader.ReadToDescendant(name);
        }

        public override bool ReadToDescendant(string localName, string namespaceURI)
        {
            return this.innerReader.ReadToDescendant(localName, namespaceURI);
        }

        public override bool ReadToFollowing(string name)
        {
            return this.ReadToFollowing(name);
        }

        public override bool ReadToFollowing(string localName, string namespaceURI)
        {
            return this.innerReader.ReadToFollowing(localName, namespaceURI);
        }

        public override bool ReadToNextSibling(string name)
        {
            return this.innerReader.ReadToNextSibling(name);
        }

        public override bool ReadToNextSibling(string localName, string namespaceURI)
        {
            return this.innerReader.ReadToNextSibling(localName, namespaceURI);
        }

        public override int ReadValueChunk(char[] buffer, int index, int count)
        {
            return this.innerReader.ReadValueChunk(buffer, index, count);
        }

        public override void ResolveEntity()
        {
            this.innerReader.ResolveEntity();
        }

        public override void Skip()
        {
            this.innerReader.Skip();
        }

        public override int AttributeCount
        {
            get
            {
                return this.innerReader.AttributeCount;
            }
        }

        public override string BaseURI
        {
            get
            {
                return this.innerReader.BaseURI;
            }
        }

        public override bool CanReadBinaryContent
        {
            get
            {
                return this.innerReader.CanReadBinaryContent;
            }
        }

        public override bool CanReadValueChunk
        {
            get
            {
                return this.innerReader.CanReadValueChunk;
            }
        }

        public override bool CanResolveEntity
        {
            get
            {
                return this.innerReader.CanResolveEntity;
            }
        }

        public override int Depth
        {
            get
            {
                return this.innerReader.Depth;
            }
        }

        public override bool EOF
        {
            get
            {
                return this.innerReader.EOF;
            }
        }

        public override bool HasAttributes
        {
            get
            {
                return this.innerReader.HasAttributes;
            }
        }

        public override bool HasValue
        {
            get
            {
                return this.innerReader.HasValue;
            }
        }

        public override bool IsDefault
        {
            get
            {
                return this.innerReader.IsDefault;
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                return this.innerReader.IsEmptyElement;
            }
        }

        public override string this[string name]
        {
            get
            {
                return this.innerReader[name];
            }
        }

        public override string this[string name, string namespaceURI]
        {
            get
            {
                return this.innerReader[name, namespaceURI];
            }
        }

        public override string this[int i]
        {
            get
            {
                return this.innerReader[i];
            }
        }

        public override string LocalName
        {
            get
            {
                return this.innerReader.LocalName;
            }
        }

        public override string Name
        {
            get
            {
                return this.innerReader.Name;
            }
        }

        public override string NamespaceURI
        {
            get
            {
                return this.innerReader.NamespaceURI;
            }
        }

        public override XmlNameTable NameTable
        {
            get
            {
                return this.innerReader.NameTable;
            }
        }

        public override XmlNodeType NodeType
        {
            get
            {
                return this.innerReader.NodeType;
            }
        }

        public override string Prefix
        {
            get
            {
                return this.innerReader.Prefix;
            }
        }

        public override char QuoteChar
        {
            get
            {
                return this.innerReader.QuoteChar;
            }
        }

        public override ReadState ReadState
        {
            get
            {
                return this.innerReader.ReadState;
            }
        }

        public override IXmlSchemaInfo SchemaInfo
        {
            get
            {
                return this.innerReader.SchemaInfo;
            }
        }

        public override XmlReaderSettings Settings
        {
            get
            {
                return this.innerReader.Settings;
            }
        }

        public override string Value
        {
            get
            {
                return this.innerReader.Value;
            }
        }

        public override Type ValueType
        {
            get
            {
                return this.innerReader.ValueType;
            }
        }

        public override string XmlLang
        {
            get
            {
                return this.innerReader.XmlLang;
            }
        }

        public override XmlSpace XmlSpace
        {
            get
            {
                return this.innerReader.XmlSpace;
            }
        }
    }
}
