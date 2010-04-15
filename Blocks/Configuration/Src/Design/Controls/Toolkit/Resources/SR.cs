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

//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Resources;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit.Windows.Controls
{
    /// <summary>
    ///     Retrieves exception strings and other localized strings.
    /// </summary>
    internal static class SR
    {
         internal static string Get(SRID id)
         {
             return _resourceManager.GetString(id.String);
         }

         internal static string Get(SRID id, params object[] args)
         {
             string message = _resourceManager.GetString(id.String);
             if (message != null)
             {
                 // Apply arguments to formatted string (if applicable)
                 if (args != null && args.Length > 0)
                 {
                     message = String.Format(CultureInfo.CurrentCulture, message, args);
                 }
             }

             return message;
         }

         // Get exception string resources for current locale
         private static ResourceManager _resourceManager = new ResourceManager("ExceptionStringTable", typeof(SR).Assembly);
    }
}
