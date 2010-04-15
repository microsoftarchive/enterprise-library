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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Interface used to interpret parameters passed to an <see cref="DataAccessor{TResult}.Execute(object[])"/> method
    /// and assign them to the <see cref="DbCommand"/> that will be executed.
    /// </summary>
    public interface IParameterMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterValues"></param>
        void AssignParameters(DbCommand command, object[] parameterValues);
    }
}
