using ShCore.Attributes;
using ShCore.Extensions;
using System;
namespace ShCore.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheFactoryAttribute : ClassInfoAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type TypeFactory { set; get; }

        /// <summary>
        /// Lấy ra CacheProvider
        /// </summary>
        /// <returns></returns>
        public ICacheProvider GetCacheProvider()
        {
            return TypeFactory.CreateInstance<ICacheFactory>().Provider;
        }
    }
}
