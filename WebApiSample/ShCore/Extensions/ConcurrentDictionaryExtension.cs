using System.Linq;
using System.Collections.Concurrent;
namespace ShCore.Extensions
{
    public static class ConcurrentDictionaryExtension
    {
        /// <summary>
        /// Xóa theo value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static bool DoDeleteByValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TValue value)
        {
            var needRemoves = dic.Where(d => d.Value.Equals(value)).ToList();
            return needRemoves.Count == needRemoves.Where(kv =>
            {
                TValue v;
                return dic.TryRemove(kv.Key, out v);
            }).Count();
        }

        /// <summary>
        /// Xóa theo key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DoDeleteByKey<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key)
        {
            TValue value = default(TValue);
            return dic.TryRemove(key, out value);
        }
    }
}
