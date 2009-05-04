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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    [TestClass]
    public class SerializationUtilityFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void SerializationUtilityCanDeserializeNull()
        {
            Assert.IsNull(SerializationUtility.DeserializeFromString(null, typeof(string), ApplicationNode.Hierarchy));
        }

        [TestMethod]
        public void SerializationUtilityCanDeserializeConvertible()
        {
            Assert.AreEqual(123, (int)SerializationUtility.DeserializeFromString("123", typeof(int), ApplicationNode.Hierarchy));
        }

        [TestMethod]
        public void SerializationUtilityCanDeserializeCustomSerializable()
        {
            CustomSerializable deserializedInstance = SerializationUtility.DeserializeFromString("testcontents", typeof(CustomSerializable), ApplicationNode.Hierarchy) as CustomSerializable;

            Assert.IsNotNull(deserializedInstance);
            Assert.AreEqual("testcontents", deserializedInstance.Contents);
        }

        [TestMethod]
        public void SerializationUtilityCanDeserializeConfigurationNode()
        {
            InstrumentationNode instrumentationNode = new InstrumentationNode();
            ApplicationNode.AddNode(instrumentationNode);

            string relativePathToInstrumentationNode = SerializationUtility.CreatePathRelativeToRootNode(instrumentationNode.Path, ApplicationNode.Hierarchy);
            Assert.IsNotNull(relativePathToInstrumentationNode);

            InstrumentationNode deserializedInstance = SerializationUtility.DeserializeFromString(relativePathToInstrumentationNode, typeof(InstrumentationNode), ApplicationNode.Hierarchy) as InstrumentationNode;

            Assert.AreEqual(instrumentationNode, deserializedInstance);
        }

        [TestMethod]
        public void SerializationUtilityDeserializesToNullOnFailure()
        {
            string relativePathToInstrumentationNode = "/non extisting path";

            InstrumentationNode deserializedInstance = SerializationUtility.DeserializeFromString(relativePathToInstrumentationNode, typeof(InstrumentationNode), ApplicationNode.Hierarchy) as InstrumentationNode;

            Assert.IsNull(deserializedInstance);
        }

        [TestMethod]
        public void SerializationUtilityCanSerializeNull()
        {
            Assert.IsNull(SerializationUtility.SerializeToString(null, ApplicationNode.Hierarchy));
        }

        [TestMethod]
        public void SerializationUtilityCanSerializeConvertible()
        {
            Assert.AreEqual("123", SerializationUtility.SerializeToString(123, ApplicationNode.Hierarchy));
        }

        [TestMethod]
        public void SerializationUtilityCanSerializeCustomSerializable()
        {
            CustomSerializable serializable = new CustomSerializable("testcontents");
            Assert.AreEqual("testcontents", SerializationUtility.SerializeToString(serializable, ApplicationNode.Hierarchy));
        }

        [TestMethod]
        public void SerializationUtilityCanSerializeConfigurationNode()
        {
            InstrumentationNode instrumentationNode = new InstrumentationNode();
            ApplicationNode.AddNode(instrumentationNode);

            string serializedNode = SerializationUtility.SerializeToString(instrumentationNode, instrumentationNode.Hierarchy);
            string relativeNodePath = SerializationUtility.CreatePathRelativeToRootNode(instrumentationNode.Path, instrumentationNode.Hierarchy);
            Assert.AreEqual(relativeNodePath, serializedNode);
        }

        public class CustomSerializable : IEnvironmentalOverridesSerializable
        {
            string contents;
            public CustomSerializable() { }

            public CustomSerializable(string contents)
            {
                this.contents = contents;
            }

            public string Contents
            {
                get { return contents; }
            }

            public void DesializeFromString(string serializedContents)
            {
                contents = serializedContents;
            }

            public string SerializeToString()
            {
                return contents;
            }
        }
    }
}
