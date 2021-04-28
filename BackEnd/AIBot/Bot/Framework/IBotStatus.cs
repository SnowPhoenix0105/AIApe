using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Buaa.AIBot.Bot.Framework
{

    public interface IBotStatus<IdType>
    {
        void Put<T>(string key, T value);
        T Get<T>(string key);
        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key"></param>
        void Reserve(string key);
        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        void Reserve(string key1, string key2);
        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="key3"></param>
        void Reserve(string key1, string key2, string key3);
        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="key3"></param>
        /// <param name="keys"></param>
        void Reserve(string key1, string key2, string key3, params string[] keys);
    }

    public class BotStatus<IdType> : IBotStatus<IdType>
    {
        public IdType Status { get; set; }
        public Dictionary<string, string> Items { get; set; } = new Dictionary<string, string>();

        public T Get<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(Items[key]);
        }

        public void Put<T>(string key, T value)
        {
            Items[key] = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key"></param>
        public void Reserve(string key)
        {
            var tmp = new Dictionary<string, string>() 
            { 
                [key] = Items[key]
            };
            Items = tmp;
        }

        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        public void Reserve(string key1, string key2)
        {
            var tmp = new Dictionary<string, string>(2)
            {
                [key1] = Items[key1],
                [key2] = Items[key2]
            };
            Items = tmp;
        }

        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="key3"></param>
        public void Reserve(string key1, string key2, string key3)
        {
            var tmp = new Dictionary<string, string>(3)
            {
                [key1] = Items[key1],
                [key2] = Items[key2],
                [key3] = Items[key3]
            };
            Items = tmp;
        }

        /// <summary>
        /// Remove all other keys except given keys.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="key3"></param>
        /// <param name="keys"></param>
        public void Reserve(string key1, string key2, string key3, params string[] keys)
        {
            var tmp = new Dictionary<string, string>(keys.Length + 3)
            {
                [key1] = Items[key1],
                [key2] = Items[key2],
                [key3] = Items[key3]
            };
            foreach (var k in keys)
            {
                tmp[k] = Items[k];
            }
            Items = tmp;
        }
    }
}
