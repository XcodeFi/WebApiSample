using ShCore.Attributes;
namespace ShCore.Web.WebBase
{
    /// <summary>
    /// quy định method cần điều kiện gì mới được thực hiện
    /// </summary>
    public abstract class AjaxRequestConditionAttribute : MethodInfoAttribute
    {
        /// <summary>
        /// Điều kiện để thực hiện request  
        /// </summary>
        public abstract bool Condition { get; }

        /// <summary>
        /// Message thông báo khi không thỏa mãn điều kiện
        /// </summary>
        public abstract string Msg { get; }

        /// <summary>
        /// Thứ tự Validate
        /// </summary>
        public int Stt { set; get; }
    }
}
