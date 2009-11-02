using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class KeyedHashAlgorithmProviderAddCommand : TypePickingCollectionElementAddCommand
    {
        ProtectedKeySettings keySettings;

        public KeyedHashAlgorithmProviderAddCommand(TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(commandAttribute, configurationElementType, elementCollectionModel)
        {
        }

        protected override bool AfterSelectType(Type selectedType)
        {
            CryptographicKeyWizard keyManager = new CryptographicKeyWizard(new KeyedHashAlgorithmKeyCreator(selectedType));
            DialogResult keyResult = keyManager.ShowDialog();

            keySettings = keyManager.KeySettings;
            return keyResult == DialogResult.OK;
        }

        protected override void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            base.SetProperties(createdElement, selectedType);

            createdElement.Property("Key").Value = keySettings;
        }
    }
}
