using System;
using System.Reflection;
namespace ShCore.Attributes
{
    /// <summary>
    /// Bound một phương thức
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodBoundaryAttribute : Attribute
    {
        private MethodInfo methodInfo = null;
        /// <summary>
        /// Thông tin phương thức
        /// </summary>
        public MethodInfo MethodInfo
        {
            get { return methodInfo; }
            set { methodInfo = value; }
        }

        /// <summary>
        /// Trước khi thực hiện phương thức
        /// </summary>
        public virtual void Before(MethodBoundaryArgs args) { }

        /// <summary>
        /// Sau khi thực hiện phương thức
        /// </summary>
        public virtual void After(MethodBoundaryArgs args) { }

        /// <summary>
        /// Run khi có lỗi xảy ra
        /// </summary>
        public virtual void OnException() { }
    }

    /// <summary>
    /// MethodBoundaryArgs
    /// </summary>
    public class MethodBoundaryArgs
    {
        private object valueReturn = null;
        /// <summary>
        /// Value return
        /// </summary>
        public object ValueReturn
        {
            get { return valueReturn; }
            set { valueReturn = value; }
        }

        private object[] arguments = null;
        /// <summary>
        /// Giá trị truyền cho phương thức
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
            set { arguments = value; }
        }
    }
}
