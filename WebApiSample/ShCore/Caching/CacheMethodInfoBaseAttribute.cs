using ShCore.Caching.CacheProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShCore.Caching
{
    /// <summary>
    /// Lớp cơ sở để thông báo cho một phương thức cần thực hiện cache
    /// </summary>
    public abstract class CacheMethodInfoBaseAttribute : Attribute
    {
        private ICacheProvider cacheProvider = null;
        /// <summary>
        /// Phương thức cần dùng cache provider nào để thực hiện set và get data
        /// </summary>
        protected ICacheProvider CacheProvider
        {
            get
            {
                // Nếu chưa có provider thì khởi tạo
                if (cacheProvider == null) cacheProvider = this.CreateCacheProvider();

                // return
                return cacheProvider;
            }
        }

        private string cacheName = string.Empty;
        /// <summary>
        /// Tên cache 
        /// </summary>
        public string CacheName
        {
            set { this.cacheName = value; }
            get { return this.cacheName; }
        }

        /// <summary>
        /// Tạo Cache Provider
        /// </summary>
        /// <returns></returns>
        protected virtual ICacheProvider CreateCacheProvider()
        {
            return new WebCacheProvider();
        }

        private MemberInfo methodInfo = null;
        /// <summary>
        /// Thông tin về phương thức mà gọi để lấy dữ liệu
        /// </summary>
        public MemberInfo MethodInfo
        {
            set { this.methodInfo = value; }
            get { return this.methodInfo; }
        }

        /// <summary>
        /// Tạo tên cache
        /// </summary>
        /// <param name="action"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public string BuildCacheName(object[] @params)
        {
            string cacheName = methodInfo.DeclaringType + "." + methodInfo.Name;
            @params.ToList().ForEach(p => cacheName += "_" + p);
            return cacheName;
        }

        /// <summary>
        /// Thực hiện lấy cache
        /// </summary>
        /// <returns></returns>
        public object GetCache()
        {
            return this.CacheProvider.Get<object>(cacheName);
        }

        /// <summary>
        /// Đưa dữ liệu vào cache
        /// </summary>
        /// <param name="data"></param>
        public void SetCache(object data)
        {
            this.CacheProvider.Set(cacheName, data, new TimeSpan(0, 30, 0));
        }
    }
}
