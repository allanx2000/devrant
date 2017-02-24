using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.Dtos
{
    /// <summary>
    /// Basic data in all models
    /// </summary>
    public abstract class DataObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Parse<T>(JObject data)
        {
            Type t = typeof(T);

            if (!t.IsSubclassOf(typeof(DataObject)))
            {
                throw new System.Exception("Cannot parse: " + t.Name);
            }

            DataObject obj;

            switch (t.Name)
            {
                case "Rant":
                    obj = new Rant();
                    break;
                case "Collab":
                    obj = new Collab();
                    break;
                case "Notification":
                    obj = new Notification();
                    break;
                case "Comment":
                    obj = new Comment();
                    break;
                default:
                    throw new Exception(t.Name + " cannot be parsed.");
            }

            obj.AddValues(data);
            
            return (T)(object)obj;
        }

        /// <summary>
        /// Sets the property
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void Set(string key, object val)
        {
            kvs[key] = val;
        }

        /// <summary>
        /// Holds all the raw JSON data
        /// </summary>
        protected Dictionary<string, object> kvs = new Dictionary<string, object>();

        /// <summary>
        /// Adds all the values from a JSON object
        /// </summary>
        /// <param name="obj"></param>
        protected void AddValues(JObject obj)
        {
            foreach (JToken token in obj.Children())
            {
                try
                {
                    if (token.Type == JTokenType.Property)
                    {
                        var prop = (JProperty)token;
                        var val = Utilities.GetValue(prop.Value);
                        kvs.Add(prop.Name, val);
                    }
                    else
                    {

                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        /// <summary>
        /// Gets the property as the requested type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public T Get<T>(string property)
        {
            if (kvs.ContainsKey(property))
            {
                return (T)kvs[property];
            }
            else
                return default(T);
        }

        /// <summary>
        /// Returns the property as an object
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public object Get(string property)
        {
            if (kvs.ContainsKey(property))
            {
                return kvs[property];
            }
            else
                return null;
        }

        /// <summary>
        /// Create Time
        /// </summary>
        public long CreatedTime { get { return Get<int>("created_time"); } }

    }
}
