using System;
using ShCore.Extensions;
using System.Linq;
using ShCore.Caching.CacheProvider;
namespace ShCore.Caching.CacheType.SqlDependency
{
    /// <summary>
    /// Thông báo cho một phương thức là cần thực hiện cache sql
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheSqlMethodInfoAttribute : CacheMethodInfoBaseAttribute
    {
        /// <summary>
        /// Khởi tạo CacheProvider
        /// </summary>
        /// <returns></returns>
        protected override ICacheProvider CreateCacheProvider()
        {
            // Khởi tạo cache provider là CacheSql
            var list = this.MethodInfo.GetAttributes<CacheSqlInfoAttribute>();

            // Khởi tạo CacheSql
            return new SqlCacheProvider(list.Select(cs => cs.CacheSql).ToArray());
        }
    }
}
