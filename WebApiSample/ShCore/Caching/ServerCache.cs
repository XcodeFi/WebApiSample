using ShCore.Extensions;
using System.Collections.Generic;
using System.Linq;
namespace ShCore.Caching
{
    /// <summary>
    /// Thông tin của một ServerCache
    /// </summary>
    public class ServerCache
    {
        /// <summary>
        /// Địa chỉ IP hoặc Domain
        /// </summary>
        public string IpAddress { set; get; }

        /// <summary>
        /// Cổng Cache
        /// </summary>
        public string Port { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{0}:{1}".Frmat(IpAddress, Port);
        }
    }
}
