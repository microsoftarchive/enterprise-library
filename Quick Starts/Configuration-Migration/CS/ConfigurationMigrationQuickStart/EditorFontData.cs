//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Text;
using System.Configuration;

namespace ConfigurationMigrationQuickStart
{
	/// <summary>
	/// Summary description for ConfigurationData.
	/// </summary>
	public class EditorFontData : ConfigurationSection 
	{		

		public EditorFontData()
		{          
		}

        [ConfigurationProperty("name")]
		public string Name 
		{
            get { return (string)this["name"]; }
			set{ this["name"] = value; }
		}

        [ConfigurationProperty("size")]
		public float Size 
		{
			get{ return (float)this["size"]; }
			set{ this["size"] = value; }
		}

        [ConfigurationProperty("style")]
		public int Style 
		{
            get { return (int)this["style"]; }
			set{ this["style"] = value; }
		} 

		public override string ToString() 
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Name = {0}; Size = {1}; Style = {2}", Name, Size.ToString(), Style.ToString());

			return sb.ToString();
		}
	}
}
