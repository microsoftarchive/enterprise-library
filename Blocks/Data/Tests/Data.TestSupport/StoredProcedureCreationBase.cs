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


namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public abstract class StoredProcedureCreationBase
    {
        protected Database db;
        protected StoredProcedureCreatingFixture baseFixture;

        protected abstract void CreateStoredProcedure();
        protected abstract void DeleteStoredProcedure();

        protected void CompleteSetup(Database db)
        {
            this.db = db;
            baseFixture = new StoredProcedureCreatingFixture(db);

            Database.ClearParameterCache();
            CreateStoredProcedure();
        }

        protected void Cleanup()
        {
            DeleteStoredProcedure();
            Database.ClearParameterCache();
        }
    }
}
