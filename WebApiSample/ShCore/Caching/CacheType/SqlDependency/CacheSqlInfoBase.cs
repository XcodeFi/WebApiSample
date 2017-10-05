using ShCore.Extensions;
namespace ShCore.Caching.CacheType.SqlDependency
{
    /// <summary>
    /// Thông tin cơ bản về Cache SqlDepedency
    /// </summary>
    public abstract class CacheSqlInfoBase
    {
        private CacheDataBaseAttribute cacheInfo = null;
        /// <summary>
        /// Thông tin về cơ sở dữ liệu
        /// </summary>
        public CacheDataBaseAttribute CacheInfo
        {
            get
            {
                // Lấy thông tin về db cần thực hiện cache
                if (cacheInfo == null) cacheInfo = this.GetType().GetAttribute<CacheDataBaseAttribute>();

                // return cacheInfo
                return cacheInfo;
            }
        }

        private string[] tables = null;
        /// <summary>
        /// Các bảng liên quan đến cache
        /// </summary>
        public string[] Tables
        {
            get { return this.tables; }
            set { this.tables = value; }
        }
    }
}
