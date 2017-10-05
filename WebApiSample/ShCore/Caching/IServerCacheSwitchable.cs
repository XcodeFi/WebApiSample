using System.Collections.Generic;
namespace ShCore.Caching
{
    /// <summary>
    /// Chỉ định cho một CacheProvider có thể lựa chọn một Server làm cache
    /// </summary>
    public interface IServerCacheSwitchable
    {
        /// <summary>
        /// Switchor dùng để lựa chọn server cache
        /// </summary>
        IServerCacheSwitchor Switchor { set; get; }
    }

    /// <summary>
    /// IServerCacheSwitchor dùng để lựa chọn Server Cache
    /// </summary>
    public interface IServerCacheSwitchor
    {
        /// <summary>
        /// Lấy thông tin Server Cache
        /// </summary>
        /// <returns></returns>
        ServerCache GetServer();
    }
}
