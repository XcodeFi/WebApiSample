using ShCore.Attributes;
using ShCore.Extensions;

namespace ShCore.Caching
{
    /// <summary>
    /// Dùng để Thông báo một phương thức có sử dụng cache
    /// </summary>
    public class CacheAttribute : MethodBoundaryAttribute
    {
        private CacheMethodInfoBaseAttribute cif = null;
        /// <summary>
        /// Thông tin cache
        /// </summary>
        private CacheMethodInfoBaseAttribute Cif
        {
            get
            {
                if (cif == null)
                {
                    cif = this.MethodInfo.GetAttribute<CacheMethodInfoBaseAttribute>(false); ;
                    if (cif != null) cif.MethodInfo = this.MethodInfo;
                }

                return cif;
            }
        }

        /// <summary>
        /// Thông báo xem phương thức này trước đó dữ liệu đã được lưu trong cache hay chưa
        /// </summary>
        private bool beforeHasCache = false;

        /// <summary>
        /// Trước khi thực hiện phương thức
        /// </summary>
        /// <param name="arg"></param>
        public override void Before(MethodBoundaryArgs arg)
        {
            // Không có thông tin cache thì thoát khỏi phương thức luôn
            if (Cif == null) return;

            // Nếu có thông tin cache thì thực hiện tiếp
            // Lấy tên cache
            Cif.CacheName = Cif.BuildCacheName(arg.Arguments);

            // Lấy dữ liệu từ cache
            object value = Cif.GetCache();

            // nếu có dữ liệu từ Cache thì override giá trị trả ra của phương thức
            if (value != null)
            {
                beforeHasCache = true;
                arg.ValueReturn = value;
            }
        }

        /// <summary>
        /// Sau khi thực hiện phương thức
        /// </summary>
        /// <param name="arg"></param>
        public override void After(MethodBoundaryArgs arg)
        {
            // Thoát khỏi sự kiện như không có thông tin cache
            if (Cif == null) return;

            // Nếu trước đó chưa được lưu trong cache thì lưu dữ liệu vừa được thực hiện bởi phương thức vào cache
            if (!beforeHasCache && arg.ValueReturn != null) Cif.SetCache(arg.ValueReturn);
        }
    }
}
