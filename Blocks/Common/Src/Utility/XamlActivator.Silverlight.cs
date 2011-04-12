using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    /// <summary>
    /// Creates instances of types from a partial type name using the <see cref="XamlReader"/>.
    /// </summary>
    public static class XamlActivator
    {
        /// <summary>
        /// Creates an instance of an object specified with an <see cref="XName"/>.
        /// </summary>
        /// <typeparam name="T">A type compatible with the element to create.</typeparam>
        /// <param name="typeXName">XName of the object to create.</param>
        /// <returns>The new instance, or <see langword="null"/> if the element cannot be created.</returns>
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
