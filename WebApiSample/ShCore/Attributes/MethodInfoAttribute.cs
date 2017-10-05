using System;
using System.Reflection;
namespace ShCore.Attributes
{
    /// <summary>
    /// MethodInfoAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodInfoAttribute : Attribute
    {
        /// <summary>
        /// Thông tin phương thức mà đặt Attribute MethodInfo
        /// </summary>
        public MethodInfo MethodInfo { set; get; }
    }
}
