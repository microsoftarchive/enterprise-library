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
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    class FormSerializer
    {
        private Form form;
        private string storageArea;
        private IsolatedStorageFile store;

        private Rectangle dimensions;
        private FormWindowState windowState;

        public FormSerializer(Form form)
        {
            this.form = form;
            if (InitializeStore())
            {
                this.form.FormClosed += new FormClosedEventHandler(OnClosed);
                this.form.Resize += new EventHandler(OnResize);
                this.form.Move += new EventHandler(OnMove);
                this.form.Load += new EventHandler(OnLoad);
                this.dimensions = Rectangle.Empty;
            }
        }

        [SuppressMessage(
            "Microsoft.Design", 
            "CA1031", 
            Justification = "Form serialization is a side feature that can be safely ignored should an error occur.")]
        private bool InitializeStore()
        {
            try
            {
                store = IsolatedStorageFile.GetUserStoreForDomain();
                store.CreateDirectory("Enterprise Library");
                storageArea = Path.Combine("Enterprise Library", this.form.GetType().Name);
                return true;
            }
            catch (Exception)
            {
                if (store != null)
                {
                    store.Dispose();
                }
            }

            return false;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            string[] files = store.GetFileNames(storageArea);
            if (files.Length == 0) return;

            IsolatedStorageFormState item = new IsolatedStorageFormState(store, storageArea);
            FormState state = item.Load();
            if (null == state) return;

            form.Bounds = state.Dimension;
            form.WindowState = state.WindowState;
        }

        private void OnMove(object sender, System.EventArgs e)
        {
            if (form.WindowState == FormWindowState.Normal)
                dimensions.Location = form.Location;

            windowState = form.WindowState;
        }

        private void OnResize(object sender, System.EventArgs e)
        {
            if (form.WindowState == FormWindowState.Normal)
                dimensions.Size = form.Size;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (windowState == FormWindowState.Minimized)
                windowState = FormWindowState.Normal;

            IsolatedStorageFormState item = new IsolatedStorageFormState(store, storageArea);
            item.Store(new FormState(dimensions, windowState));
            store.Dispose();
        }
    }

    static class SerializationUtility
    {
        public static byte[] ToBytes(object value)
        {
            if (value == null)
            {
                return null;
            }

            byte[] inMemoryBytes;
            using (MemoryStream inMemoryData = new MemoryStream())
            {
                new BinaryFormatter().Serialize(inMemoryData, value);
                inMemoryBytes = inMemoryData.ToArray();
            }

            return inMemoryBytes;
        }

        public static object ToObject(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return null;
            }

            using (MemoryStream dataInMemory = new MemoryStream(serializedObject))
            {
                return new BinaryFormatter().Deserialize(dataInMemory);
            }
        }
    }

    class IsolatedStorageFormStateField
    {
        private string fieldName;
        private string fileSystemLocation;
        private IsolatedStorageFile storage;

        public IsolatedStorageFormStateField(IsolatedStorageFile storage, string fieldName,
                                             string fileSystemLocation)
        {
            this.fieldName = fieldName;
            this.fileSystemLocation = fileSystemLocation;
            this.storage = storage;
        }

        public void Write(object itemToWrite)
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, storage))
            {
                WriteField(itemToWrite, fileStream);
            }
        }

        public object Read()
        {
            using (IsolatedStorageFileStream fileStream =
                new IsolatedStorageFileStream(fileSystemLocation, FileMode.Open, FileAccess.Read, FileShare.None, storage))
            {
                return ReadField(fileStream);
            }
        }

        private void WriteField(object itemToWrite, IsolatedStorageFileStream fileStream)
        {
            byte[] serializedKey = SerializationUtility.ToBytes(itemToWrite);

            if (serializedKey != null)
            {
                fileStream.Write(serializedKey, 0, serializedKey.Length);
            }
        }

        private object ReadField(IsolatedStorageFileStream fileStream)
        {
            if (fileStream.Length == 0)
            {
                return null;
            }

            byte[] fieldBytes = new byte[fileStream.Length];
            fileStream.Read(fieldBytes, 0, fieldBytes.Length);
            object fieldValue = SerializationUtility.ToObject(fieldBytes);
            return fieldValue;
        }
    }

    [Serializable]
    class FormState
    {
        private readonly Rectangle dimension;
        private readonly FormWindowState windowState;

        public FormState(Rectangle dimension, FormWindowState windowState)
        {
            this.dimension = dimension;
            this.windowState = windowState;
        }

        public Rectangle Dimension
        {
            get { return dimension; }
        }

        public FormWindowState WindowState
        {
            get { return windowState; }
        }
    }

    class IsolatedStorageFormState
    {
        private IsolatedStorageFormStateField formStateField;

        public IsolatedStorageFormState(IsolatedStorageFile storage, string storageArea)
        {
            formStateField = new IsolatedStorageFormStateField(storage, "FormState", storageArea);
        }

        public void Store(FormState state)
        {
            formStateField.Write(state);
        }

        public FormState Load()
        {
            FormState formState = formStateField.Read() as FormState;
            if (null == formState) return null;

            return formState;
        }
    }
}
