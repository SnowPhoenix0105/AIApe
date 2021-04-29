using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Buaa.AIBot.Bot.Framework
{
    /// <summary>
    /// The container of bot's status.
    /// </summary>
    /// <remarks>
    /// Using key-value pair to store the satus of bot.
    /// Every value should be able to be dumped by <see cref="JsonSerializer"/>, and can be load from it.
    /// </remarks>
    /// <typeparam name="IdType">The type to mark a status, usually an enum.</typeparam>
    public interface IBotStatusContainer<IdType>
    {
        /// <summary>
        /// Store an boject.
        /// </summary>
        /// <remarks><paramref name="value"/> should be able to be dumped by <see cref="JsonSerializer"/>.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Put<T>(string key, T value);

        /// <summary>
        /// Get an object.
        /// </summary>
        /// <typeparam name="T">The Type of the value.</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
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

    /// <summary>
    /// The Status of a Bot.
    /// </summary>
    /// <typeparam name="IdType">The type to mark a status, usually an enum.</typeparam>
    public class BotStatus<IdType> : IBotStatusContainer<IdType>
    {
        /// <summary>
        /// The status of Bot.
        /// </summary>
        public IdType Status { get; set; }

        /// <summary>
        /// Stored key-values of this bot.
        /// </summary>
        /// <remarks>
        /// The values have already dumped to JSON, if you need clr object, using <see cref="BotStatus{IdType}.Get{T}(string)"/>
        /// </remarks>
        public Dictionary<string, string> Items { get; set; } = new Dictionary<string, string>();


        /// <summary>
        /// Store an boject.
        /// </summary>
        /// <remarks><paramref name="value"/> should be able to be dumped by <see cref="JsonSerializer"/>.</remarks>
        /// <seealso cref="IBotStatusContainer{IdType}.Get{T}(string)"/>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public T Get<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(Items[key]);
        }

        /// <summary>
        /// Get an object.
        /// </summary>
        /// <seealso cref="IBotStatusContainer{IdType}.Put{T}(string, T)"/>
        /// <typeparam name="T">The Type of the value.</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
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
