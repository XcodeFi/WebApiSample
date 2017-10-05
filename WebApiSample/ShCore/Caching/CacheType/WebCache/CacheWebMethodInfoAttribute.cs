using System;
namespace ShCore.Caching.CacheType.WebCache
{
    /// <summary>
    /// Thông báo cho phương thức là thực hiện cache thông thường của dot net
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheWebMethodInfoAttribute : CacheMethodInfoBaseAttribute
    {

    }
}
