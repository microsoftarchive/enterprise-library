using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class NameValueCollectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            if (collection == null)
            {
                return null;
            }

            return collection.AllKeys.ToDictionary(k => k, k => collection[k]);
        }
    }
}
