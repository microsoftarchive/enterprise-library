#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Quick Start
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.ComponentModel.DataAnnotations;

namespace SlabReconfigurationWebRole.Models
{
    public class MessageModel
    {
        [Required]
        [MaxLength(20)]
        public string Recipient { get; set; }

        [Required]
        [MaxLength(200)]
        public string Message { get; set; }
    }
}