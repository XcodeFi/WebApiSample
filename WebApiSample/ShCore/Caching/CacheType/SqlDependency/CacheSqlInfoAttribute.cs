using System;
namespace ShCore.Caching.CacheType.SqlDependency
{
    /// <summary>
    /// Đưa ra các thông tin về SqlCacheDepedency như là Db, bảng nào
    /// Attribute này chỉ được dùng khi mà phương thức có Attribute CacheSqlMethodInfoAttribute để thông báo cho phương thức là sử dụng cache sql
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CacheSqlInfoAttribute : Attribute
    {
        /// <summary>
        /// Sử dụng cache trên cơ sở dữ liệu nào
        /// </summary>
        private CacheSqlInfoBase cacheSqlBase = null;
        public CacheSqlInfoBase CacheSql
        {
            get { return cacheSqlBase; }
        }
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tables"></param>
        public CacheSqlInfoAttribute(Type type, params string[] tables)
        {
            // cache trên cơ sở dữ liệu nào
            cacheSqlBase = Activator.CreateInstance(type) as CacheSqlInfoBase;

            // Cache được notify trên bảng nào
            cacheSqlBase.Tables = tables;
        }
    }
}
