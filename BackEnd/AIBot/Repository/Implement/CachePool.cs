using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Repository.Implement
{
    public enum CacheId: int
    {
        None = 0,
        Tag = 1,
        Question = 2,
    }

    public static class CacheIdTools
    {
        public static void Set(this ICachePool<int> pool, CacheId id, object value)
        {
            pool.Set((int)id, value);
        }

        public static void Get<T>(this ICachePool<int> pool, CacheId id)
        {
            pool.Get<T>((int)id);
        }

        public static T GetOrDefault<T>(this ICachePool<int> pool, CacheId id)
        {
            return pool.GetOrDefault<T>((int)id);
        }
    }

    public interface ICachePool<TK>
    {
        TV Get<TV>(TK key);
        TV GetOrDefault<TV>(TK key);
        void Set(TK key, object value);
    }

    public class CachePool<TK> : ICachePool<TK>
    {
        private Dictionary<TK, object> dic = new Dictionary<TK, object>();

        public object this[TK key]
        {
            get
            {
                return dic[key];
            }
            set
            {
                dic[key] = value;
            }
        }

        public void Set(TK key, object value)
        {
            dic[key] = value;
        }

        public TV GetOrDefault<TV>(TK key)
        {
            if (dic.TryGetValue(key, out object res))
            {
                return (TV)res;
            }
            return default(TV);
        }

        public TV Get<TV>(TK key)
        {
            return (TV)dic[key];
        }
    }
}
