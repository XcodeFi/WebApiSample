using System.Collections.Generic;
using System.Collections.Concurrent;
using ShCore.Extensions;
namespace ShCore.Utility
{
    /// <summary>
    /// DictionaryBase dùng để cache
    /// </summary>
    public abstract class DictionaryCacheBase<TKey, TValue>
    {
        /// <summary>
        /// Dic để lưu trữ các giá trị
        /// </summary>
        private static ConcurrentDictionary<TKey, TValue> dic = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// indexer để lấy giá trị theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!dic.TryGetValue(key, out value))
                    dic[key] = value = GetValueForDic(key);

                return value;
            }
        }

        /// <summary>
        /// Phương thức nội bộ để lấy được giá trị với một Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract TValue GetValueForDic(TKey key);
    }
}
