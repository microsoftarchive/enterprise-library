using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    public static class XamlActivator
    {
        public static T CreateInstance<T>(string typeXName)
            where T : class
        {
            try
            {
                var xName = XName.Get(typeXName);
                var providerElement = new XElement(xName, new XAttribute(XNamespace.Xmlns + "temp", xName.NamespaceName));
                var provider = (T)XamlReader.Load(providerElement.ToString());

                return provider;
            }
            catch (XmlException)
            {
                return null;
            }
            catch (XamlParseException)
            {
                return null;
            }
        }
    }
}
