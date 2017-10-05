using ShCore.Utility;
using System.Collections.Generic;
using System;
using ShCore.Extensions;
using System.Linq;
using System.Reflection;
using ShCore.Reflectors;
namespace ShCore.Caching
{
    /// <summary>
    /// CacheManager
    /// </summary>
    public abstract class CacheManager<T> : Singleton<T> where T : new()
    {
        private List<Pair<Type, List<Pair<PropertyInfo, PropertyCacheAttribute>>>> typePropertyCacheInfo = null;
        /// <summary>
        /// Chứa thông tin type có những cặp propertyinfo và propertycache nào
        /// Được load theo thông tin Assembly chỉ định
        /// </summary>
        public List<Pair<Type, List<Pair<PropertyInfo, PropertyCacheAttribute>>>> TypePropertyCacheInfo
        {
            get 
            {
                if (typePropertyCacheInfo == null)
                {
                    // Khởi tạo
                    typePropertyCacheInfo = new List<Pair<Type, List<Pair<PropertyInfo, PropertyCacheAttribute>>>>();

                    // reflect assembly
                    var rfl = new ReflectAssemblyTypeListPropertyWithAttribute<PropertyCacheAttribute>();

                    // Tất cả các assemblies có chứa thông tin liên quan đến cache
                    var assemblies = SupportAssembly();

                    // Load theo từng Assembly để lấy được type có những cặp propertyinfo và propertycache nào
                    // đồng thời union vào biến typePropertyCacheInfo
                    for (int i = 0; i < assemblies.Length; i++)
                        typePropertyCacheInfo = typePropertyCacheInfo.Union(rfl[assemblies[i]]).ToList();
                }
                return typePropertyCacheInfo; 
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Assembly[] SupportAssembly()
        {
            return new Assembly[] { this.GetType().Assembly };
        }

        /// <summary>
        /// Gọi Action cập nhật
        /// </summary>
        /// <param name="action"></param>
        public void Clear(IEnumerable<PropertyCacheAttribute> pcs)
        {
            // Tổng số PropertyCache cần notify
            var total = pcs.Count();

            // Tìm Type có property cache là pcs
            foreach (var tpc in TypePropertyCacheInfo)
            {
                var jc = tpc.T2.Join(pcs, tpcItem => tpcItem.T2.TargetName, pcsItem => pcsItem.TargetName, (tpcItem, pcsItem) => new { TpcItem = tpcItem, PcsItem = pcsItem }).ToList();

                // Nếu Type không có đủ 2 PropertyCacheAttribute mà cần notify xóa cache thì thôi, tiếp tục tìm
                if (jc.Count != total) continue;

                // Khởi tạo đối tượng cacheEntry thỏa mãn có đủ PropertyCacheAttribute cần notify
                var cacheEntry = tpc.T1.CreateInstance<IClearCache>();

                // Thiết lập value cho Property mà Type có đủ PropertyCacheAttribute cần notify
                foreach (var jcItem in jc)
                    jcItem.TpcItem.T1.SetValue(cacheEntry, jcItem.PcsItem.Value, null);
                
                // Thực hiện clear
                cacheEntry.ClearWith(jc.Select(jcItem => jcItem.PcsItem).ToList());
            }
        }
    }

    /// <summary>
    /// Phương thức mở rộng cho PropertyCacheAttribute
    /// </summary>
    public static class PropertyCacheAttributeExtension
    {
        /// <summary>
        /// Khởi tạo danh sách PropertyCache cần notify
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="targetName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyCacheAttribute> Get(this IEnumerable<PropertyCacheAttribute> sources, string targetName, object value)
        {
            // return các phần tử cũ
            foreach (var s in sources) yield return s;

            // Add thêm phần tử mới
            yield return new PropertyCacheOptionalAttribute(targetName) { Value = value };
        }

        /// <summary>
        /// Tạo IEnumerable PropertyCacheAttribute từ string
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyCacheAttribute> Get(this string targetName, object value)
        {
            yield return new PropertyCacheOptionalAttribute(targetName) { Value = value };
        }
    }
}
