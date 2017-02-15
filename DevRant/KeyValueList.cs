using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant
{
    /// <summary>
    /// List for key values
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }

    /// <summary>
    /// Used for Post requests
    /// </summary>
    public class Parameters : KeyValueList<string, string>
    {
        /// <summary>
        /// Returns the parameters as a query string
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            List<string> kvs = new List<string>();

            foreach (var kv in this)
            {
                kvs.Add(kv.Key + "=" + kv.Value);
            }

            return string.Join("&", kvs);
        }
    }
}
