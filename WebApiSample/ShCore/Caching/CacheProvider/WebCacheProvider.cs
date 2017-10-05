using System;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
namespace ShCore.Caching.CacheProvider
{
    /// <summary>
    /// WebCacheProvider
    /// </summary>
    public class WebCacheProvider : ICacheProvider
    {
        /// <summary>
        /// Set Cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public virtual void Set<T>(string key, T value, TimeSpan time) where T : class
        {
            HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddTicks(time.Ticks), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return HttpRuntime.Cache[key] as T;
        }

        /// <summary>
        /// Clear cache thỏa mãn cùng lúc một list pattern key
        /// </summary>
        /// <param name="patternKey"></param>
        public void Clear(List<string> patternKey)
        {
            // nếu ko có key nào thì thoát luôn
            if (patternKey.Count == 0) return;

            // Danh sách tất cả các Keys
            var listKeys = HttpRuntime.Cache.Cast<DictionaryEntry>().Select(d => d.Key.ToString()).ToList();

            // Thực hiện Remove Cache có Key được tìm thấy mà chứa tất cả các patternKey
            listKeys.ForEach(key =>
            {                
                if (patternKey.Count(pk => key.Contains(pk)) == patternKey.Count)
                    HttpRuntime.Cache.Remove(key);
            });
        }
    }
}
