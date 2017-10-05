using System.Collections.Generic;
namespace ShCore.Caching
{
    public interface IClearCache
    {
        void ClearWith(List<PropertyCacheAttribute> pcs);
    }
}
