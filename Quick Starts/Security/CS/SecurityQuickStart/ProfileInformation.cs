//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.Serialization;

namespace SecurityQuickStart
{
	public enum ProfileTheme
	{
		Spring,
		Summer,
		Fall,
		Winter
	}
		
	/// <summary>
	/// Class to store common profile information.
	/// </summary>
	[Serializable()]        
	public class ProfileInformation 
	{
		private string userFirstName;
		private string userLastName;
		private ProfileTheme preferredTheme;
		
		public ProfileInformation()
		{
		}

		public ProfileInformation(string firstName, string lastName, ProfileTheme theme)
		{
			this.userFirstName = firstName;
			this.userLastName = lastName;
			this.preferredTheme = theme;
		}

		/// <summary>
		/// First name for the user.
		/// </summary>
		public string FirstName
		{
			get { return this.userFirstName; }
			set { this.userFirstName = value; }
		}

		/// <summary>
		/// Last name for the user.
		/// </summary>
		public string LastName
		{
			get { return this.userLastName; }
			set { this.userLastName = value; }
		}

		/// <summary>
		/// Preferred theme for a user.
		/// </summary>
		public ProfileTheme Theme
		{
			get { return this.preferredTheme; }
			set { this.preferredTheme = value; }
		}

		public override string ToString()
		{
			string result = string.Format(Properties.Resources.ProfileStringMessage, this.userFirstName, this.userLastName, this.preferredTheme);
				
			return result;
		}
	}
}
