using System;
using System.Collections.Generic;
namespace ShCore.Caching
{
    /// <summary>
    /// ICacheProvider
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Thiết lập Cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        void Set<T>(string key, T value, TimeSpan time) where T : class;

        /// <summary>
        /// Lấy dữ liệu từ Cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// Thực hiện Clear cache theo Pattern Key
        /// </summary>
        /// <param name="pattern"></param>
        void Clear(List<string> patternKey);
    }
}
