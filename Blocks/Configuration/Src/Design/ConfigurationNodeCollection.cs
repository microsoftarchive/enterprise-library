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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    sealed class ConfigurationNodeCollection : IReadOnlyCollection<ConfigurationNode>
    {
        private ConfigurationNode owner;

        internal ConfigurationNodeCollection(ConfigurationNode owner)
        {
            this.owner = owner;
        }

        
        public int Count
        {
            get { return owner.ChildCount; }
        }

		public ConfigurationNode this[int index]
        {
            get
            {
                if (index < 0 || index >= owner.ChildCount) throw new ArgumentOutOfRangeException("index");

                return owner.ChildNodes[index];
            }            
        }

		public ConfigurationNode this[string name]
		{
			get 
            { 
                if (owner.childNodeLookup.ContainsKey(name)) return owner.childNodeLookup[name];
                return null;
            }
		}
        
		public bool Contains(ConfigurationNode node)
        {
			if (null == node) throw new ArgumentNullException("node");

			return Contains(node.Name);
        }
        
        public bool Contains(string nodeName)
        {
            return owner.childNodeLookup.ContainsKey(nodeName);
        }

		public void ForEach(Action<ConfigurationNode> action)
		{
			for (int index = 0; index < Count; index++)
			{
				action(owner.ChildNodes[index]);
			}
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ConfigurationNodeCollectionEnumerator(this);
		}

		public IEnumerator<ConfigurationNode> GetEnumerator()
        {
            return new ConfigurationNodeCollectionEnumerator(this);
        }       

        private struct ConfigurationNodeCollectionEnumerator : IEnumerator<ConfigurationNode>, IEnumerator
        {
			private IReadOnlyCollection<ConfigurationNode> list;
            private int index;
            private int total;

			public ConfigurationNodeCollectionEnumerator(IReadOnlyCollection<ConfigurationNode> list)
            {
                this.list = list;                
                total = list.Count;
                index = -1;
            }

            public void Dispose()
            {
            }

			public ConfigurationNode Current
            {
                get
                {
                    return list[index];
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (index < total - 1)
                {
                    index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            void IEnumerator.Reset()
            {
                index = -1;                
            }
        }
    }
}
