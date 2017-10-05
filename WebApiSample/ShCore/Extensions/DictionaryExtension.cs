using System;
using System.Collections.Generic;
namespace ShCore.Extensions
{
    /// <summary>
    /// Phương thức mở rộng cho Dictionary
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// Get giá trị từ Dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="whenNull"></param>
        /// <returns></returns>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TValue> whenNull)
        {
            TValue value;
            if(!dic.TryGetValue(key, out value)) dic[key] = value = whenNull();
            return value;
        }

        /// <summary>
        /// TryGetValue
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            return dic.TryGetValue(key, out value) ? value : default(TValue);
        }
    }
}
