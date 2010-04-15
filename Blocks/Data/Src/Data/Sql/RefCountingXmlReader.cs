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
using System.Xml;
using System.Xml.Schema;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql
{
    /// <summary>
    /// Wrapper around <see cref="XmlReader"/> that ties into our
    /// reference counting on connections.
    /// </summary>
    internal class RefCountingXmlReader : XmlReader
    {
        private readonly DatabaseConnectionWrapper connection;
        private readonly XmlReader innerReader;

        public RefCountingXmlReader(DatabaseConnectionWrapper connection, XmlReader innerReader)
        {
            this.connection = connection;
            this.innerReader = innerReader;
            this.connection.AddRef();
        }

        public override int AttributeCount
        {
            get { return innerReader.AttributeCount; }
        }

        public override string BaseURI
        {
            get { return innerReader.BaseURI; }
        }

        public override bool CanReadBinaryContent
        {
            get { return innerReader.CanReadBinaryContent; }
        }

        public override bool CanReadValueChunk
        {
            get { return innerReader.CanReadValueChunk; }
        }

        public override bool CanResolveEntity
        {
            get { return innerReader.CanResolveEntity; }
        }

        public override int Depth
        {
            get { return innerReader.Depth; }
        }

        public override bool EOF
        {
            get { return innerReader.EOF; }
        }

        public override bool HasAttributes
        {
            get { return innerReader.HasAttributes; }
        }

        public override bool HasValue
        {
            get { return innerReader.HasValue; }
        }

        public override bool IsDefault
        {
            get { return innerReader.IsDefault; }
        }

        public override bool IsEmptyElement
        {
            get { return innerReader.IsEmptyElement; }
        }

        public override string this[string name]
        {
            get { return innerReader[name]; }
        }

        public override string this[string name, string namespaceURI]
        {
            get { return innerReader[name, namespaceURI]; }
        }

        public override string this[int i]
        {
            get { return innerReader[i]; }
        }

        public override string LocalName
        {
            get { return innerReader.LocalName; }
        }

        public override string Name
        {
            get { return innerReader.Name; }
        }

        public override string NamespaceURI
        {
            get { return innerReader.NamespaceURI; }
        }

        public override XmlNameTable NameTable
        {
            get { return innerReader.NameTable; }
        }

        public override XmlNodeType NodeType
        {
            get { return innerReader.NodeType; }
        }

        public override string Prefix
        {
            get { return innerReader.Prefix; }
        }

        public override char QuoteChar
        {
            get { return innerReader.QuoteChar; }
        }

        public override ReadState ReadState
        {
            get { return innerReader.ReadState; }
        }

        public override IXmlSchemaInfo SchemaInfo
        {
            get { return innerReader.SchemaInfo; }
        }

        public override XmlReaderSettings Settings
        {
            get { return innerReader.Settings; }
        }

        public override string Value
        {
            get { return innerReader.Value; }
        }

        public override Type ValueType
        {
            get { return innerReader.ValueType; }
        }

        public override string XmlLang
        {
            get { return innerReader.XmlLang; }
        }

        public override XmlSpace XmlSpace
        {
            get { return innerReader.XmlSpace; }
        }

        public override void Close()
        {
            innerReader.Close();
            connection.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            ((IDisposable) innerReader).Dispose();
            connection.Dispose();
        }

        public override string GetAttribute(int i)
        {
            return innerReader.GetAttribute(i);
        }

        public override string GetAttribute(string name)
        {
            return innerReader.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return innerReader.GetAttribute(name, namespaceURI);
        }

        public override bool IsStartElement()
        {
            return innerReader.IsStartElement();
        }

        public override bool IsStartElement(string name)
        {
            return innerReader.IsStartElement(name);
        }

        public override bool IsStartElement(string localname, string ns)
        {
            return innerReader.IsStartElement(localname, ns);
        }

        public override string LookupNamespace(string prefix)
        {
            return innerReader.LookupNamespace(prefix);
        }

        public override void MoveToAttribute(int i)
        {
            innerReader.MoveToAttribute(i);
        }

        public override bool MoveToAttribute(string name)
        {
            return innerReader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return innerReader.MoveToAttribute(name, ns);
        }

        public override XmlNodeType MoveToContent()
        {
            return innerReader.MoveToContent();
        }

        public override bool MoveToElement()
        {
            return innerReader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return innerReader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return innerReader.MoveToNextAttribute();
        }

        public override bool Read()
        {
            return innerReader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return innerReader.ReadAttributeValue();
        }

        public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
        {
            return innerReader.ReadContentAs(returnType, namespaceResolver);
        }

        public override int ReadContentAsBase64(byte[] buffer, int index, int count)
        {
            return innerReader.ReadContentAsBase64(buffer, index, count);
        }

        public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
        {
            return innerReader.ReadContentAsBinHex(buffer, index, count);
        }

        public override bool ReadContentAsBoolean()
        {
            return innerReader.ReadContentAsBoolean();
        }

        public override DateTime ReadContentAsDateTime()
        {
            return innerReader.ReadContentAsDateTime();
        }

        public override decimal ReadContentAsDecimal()
        {
            return innerReader.ReadContentAsDecimal();
        }

        public override double ReadContentAsDouble()
        {
            return innerReader.ReadContentAsDouble();
        }

        public override float ReadContentAsFloat()
        {
            return innerReader.ReadContentAsFloat();
        }

        public override int ReadContentAsInt()
        {
            return innerReader.ReadContentAsInt();
        }

        public override long ReadContentAsLong()
        {
            return innerReader.ReadContentAsLong();
        }

        public override object ReadContentAsObject()
        {
            return innerReader.ReadContentAsObject();
        }

        public override string ReadContentAsString()
        {
            return innerReader.ReadContentAsString();
        }

        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
        {
            return innerReader.ReadElementContentAs(returnType, namespaceResolver);
        }

        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver,
            string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAs(returnType, namespaceResolver, localName, namespaceURI);
        }

        public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
        {
            return innerReader.ReadElementContentAsBase64(buffer, index, count);
        }

        public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
        {
            return innerReader.ReadElementContentAsBinHex(buffer, index, count);
        }

        public override bool ReadElementContentAsBoolean()
        {
            return innerReader.ReadElementContentAsBoolean();
        }

        public override bool ReadElementContentAsBoolean(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsBoolean(localName, namespaceURI);
        }

        public override DateTime ReadElementContentAsDateTime()
        {
            return innerReader.ReadElementContentAsDateTime();
        }

        public override DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsDateTime(localName, namespaceURI);
        }

        public override decimal ReadElementContentAsDecimal()
        {
            return innerReader.ReadElementContentAsDecimal();
        }

        public override decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsDecimal(localName, namespaceURI);
        }

        public override double ReadElementContentAsDouble()
        {
            return innerReader.ReadElementContentAsDouble();
        }

        public override double ReadElementContentAsDouble(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsDouble(localName, namespaceURI);
        }

        public override float ReadElementContentAsFloat()
        {
            return innerReader.ReadElementContentAsFloat();
        }

        public override float ReadElementContentAsFloat(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsFloat(localName, namespaceURI);
        }

        public override int ReadElementContentAsInt()
        {
            return innerReader.ReadElementContentAsInt();
        }

        public override int ReadElementContentAsInt(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsInt(localName, namespaceURI);
        }

        public override long ReadElementContentAsLong()
        {
            return innerReader.ReadElementContentAsLong();
        }

        public override long ReadElementContentAsLong(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsLong(localName, namespaceURI);
        }

        public override object ReadElementContentAsObject()
        {
            return innerReader.ReadElementContentAsObject();
        }

        public override object ReadElementContentAsObject(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsObject(localName, namespaceURI);
        }

        public override string ReadElementContentAsString()
        {
            return innerReader.ReadElementContentAsString();
        }

        public override string ReadElementContentAsString(string localName, string namespaceURI)
        {
            return innerReader.ReadElementContentAsString(localName, namespaceURI);
        }

        public override string ReadElementString()
        {
            return innerReader.ReadElementString();
        }

        public override string ReadElementString(string name)
        {
            return innerReader.ReadElementString(name);
        }

        public override string ReadElementString(string localname, string ns)
        {
            return innerReader.ReadElementString(localname, ns);
        }

        public override void ReadEndElement()
        {
            innerReader.ReadEndElement();
        }

        public override string ReadInnerXml()
        {
            return innerReader.ReadInnerXml();
        }

        public override string ReadOuterXml()
        {
            return innerReader.ReadOuterXml();
        }

        public override void ReadStartElement()
        {
            innerReader.ReadStartElement();
        }

        public override void ReadStartElement(string name)
        {
            innerReader.ReadStartElement(name);
        }

        public override void ReadStartElement(string localname, string ns)
        {
            innerReader.ReadStartElement(localname, ns);
        }

        public override string ReadString()
        {
            return innerReader.ReadString();
        }

        public override XmlReader ReadSubtree()
        {
            return innerReader.ReadSubtree();
        }

        public override bool ReadToDescendant(string name)
        {
            return innerReader.ReadToDescendant(name);
        }

        public override bool ReadToDescendant(string localName, string namespaceURI)
        {
            return innerReader.ReadToDescendant(localName, namespaceURI);
        }

        public override bool ReadToFollowing(string name)
        {
            return ReadToFollowing(name);
        }

        public override bool ReadToFollowing(string localName, string namespaceURI)
        {
            return innerReader.ReadToFollowing(localName, namespaceURI);
        }

        public override bool ReadToNextSibling(string name)
        {
            return innerReader.ReadToNextSibling(name);
        }

        public override bool ReadToNextSibling(string localName, string namespaceURI)
        {
            return innerReader.ReadToNextSibling(localName, namespaceURI);
        }

        public override int ReadValueChunk(char[] buffer, int index, int count)
        {
            return innerReader.ReadValueChunk(buffer, index, count);
        }

        public override void ResolveEntity()
        {
            innerReader.ResolveEntity();
        }

        public override void Skip()
        {
            innerReader.Skip();
        }
    }
}
