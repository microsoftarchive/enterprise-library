//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.IO;
using System.IO.IsolatedStorage;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    /// <summary>
    /// Defines the mechanism to store and read individual fields from IsolatedStorage. This class maintains no
    /// state with respect to the data read from IsolatedStorage, so it may be reused to reread or rewrite the same field
    /// repeatedly.
    /// </summary>
    internal class IsolatedStorageCacheItemField
    {
        private string fieldName;
        private string fileSystemLocation;
        private IsolatedStorageFile storage;
        private IStorageEncryptionProvider encryptionProvider;

        /// <summary>
        /// Instance constructor
        /// </summary>
        /// <param name="storage">IsolatedStorage area to use. May not be null.</param>
        /// <param name="fieldName">Name of the file in which the field value is stored. May not be null.</param>
        /// <param name="fileSystemLocation">Complete path to directory where file specified in fieldName is to be found. May not be null.</param>
        /// <param name="encryptionProvider">Encryption provider</param>
        public IsolatedStorageCacheItemField(IsolatedStorageFile storage, string fieldName,
                                             string fileSystemLocation, IStorageEncryptionProvider encryptionProvider)
        {
            this.fieldName = fieldName;
            this.fileSystemLocation = fileSystemLocation;
            this.storage = storage;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Writes value to specified location in IsolatedStorage
        /// </summary>
        /// <param name="itemToWrite">Object to write into Isolated Storage</param>
        /// <param name="encrypted">True if item written is to be encrypted</param>
        public void Write(object itemToWrite, bool encrypted)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation + @"\" + fieldName, FileMode.CreateNew, FileAccess.Write, FileShare.None, storage))
            {
                WriteField(itemToWrite, fileStream, encrypted);
            }
        }

        /// <summary>
        /// Overwrites given field in Isolated Storage. Item will not be encrypted
        /// </summary>
        /// <param name="itemToWrite">Object to write into Isolated Storage</param>
        public void Overwrite(object itemToWrite)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation + @"\" + fieldName, FileMode.Truncate, FileAccess.Write, FileShare.None, storage))
            {
                WriteField(itemToWrite, fileStream, false);
            }
        }

        /// <summary>
        /// Reads value from specified location in IsolatedStorage
        /// </summary>
        /// <param name="encrypted">True if field is stored as encrypted</param>
        /// <returns>Value read from IsolatedStorage. This value may be null if the value stored is null.</returns>
        public object Read(bool encrypted)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation + @"\" + fieldName, FileMode.Open, FileAccess.Read, FileShare.None, storage))
            {
                return ReadField(fileStream, encrypted);
            }
        }

        /// <summary>
        /// Responsible for writing value to IsolatedStorage using given IsolatedStorageFileStream reference. Subclasses
        /// may override this method to provide different implementations of writing to Isolated Storage.
        /// </summary>
        /// <param name="itemToWrite">Value to write. May be null.</param>
        /// <param name="fileStream">Stream to which value should be written. May not be null.</param>
        /// <param name="encrypted">True if item is to be encrypted</param>
        protected virtual void WriteField(object itemToWrite, IsolatedStorageFileStream fileStream, bool encrypted)
        {
            byte[] serializedKey = SerializationUtility.ToBytes(itemToWrite);
            if (encrypted)
            {
                serializedKey = EncryptValue(serializedKey);
            }

            if (serializedKey != null)
            {
                fileStream.Write(serializedKey, 0, serializedKey.Length);
            }
        }

        /// <summary>
        /// Responsible for reading value from IsolatedStorage using given IsolatedStorageFileStream reference. Subclasses
        /// may override this method to provide different implementations of reading from IsolatedStorage.
        /// </summary>
        /// <param name="fileStream">Stream from which value should be written. May not be null.</param>
        /// <param name="encrypted">True if item is stored encrypted</param>
        /// <returns>Value read from Isolated Storage. May be null if value stored is null</returns>
        protected virtual object ReadField(IsolatedStorageFileStream fileStream, bool encrypted)
        {
            if (fileStream.Length == 0)
            {
                return null;
            }

            byte[] fieldBytes = new byte[fileStream.Length];
            fileStream.Read(fieldBytes, 0, fieldBytes.Length);
            if (encrypted)
            {
                fieldBytes = DecryptValue(fieldBytes);
            }
            object fieldValue = SerializationUtility.ToObject(fieldBytes);
            return fieldValue;
        }

        private byte[] EncryptValue(byte[] valueBytes)
        {
            if (encryptionProvider != null)
            {
                valueBytes = encryptionProvider.Encrypt(valueBytes);
            }

            return valueBytes;
        }

        private byte[] DecryptValue(byte[] fieldBytes)
        {
            if (encryptionProvider != null)
            {
                fieldBytes = encryptionProvider.Decrypt(fieldBytes);
            }
            return fieldBytes;
        }
    }
}