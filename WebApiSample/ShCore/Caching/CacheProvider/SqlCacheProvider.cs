using ShCore.Caching.CacheType.SqlDependency;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Linq;
namespace ShCore.Caching.CacheProvider
{
    public class SqlCacheProvider : WebCacheProvider
    {
                /// <summary>
        /// List các thông tin cho việc thực hiện cache sql dependency
        /// </summary>
        private List<CacheSqlInfoBase> dependencyItems = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listCacheSqlDependency"></param>
        public SqlCacheProvider(params CacheSqlInfoBase[] listCacheSqlDependency)
        {
            this.dependencyItems = listCacheSqlDependency.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="time"></param>
        public override void Set<T>(string name, T data, System.TimeSpan time)
        {
            if (HttpContext.Current == null) return;

            // Tổng hợp Dependency
            AggregateCacheDependency aggDep = new AggregateCacheDependency();

            // Tạo SqlCacheDependency Item
            dependencyItems.ForEach(c => c.Tables.ToList().ForEach(t => aggDep.Add(new SqlCacheDependency(c.CacheInfo.DataBase, t))));

            // Đưa vào cache
            HttpContext.Current.Cache.Insert(name, data, aggDep);
        }
    }
}
