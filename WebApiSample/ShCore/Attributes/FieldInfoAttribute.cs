using System;
using System.Reflection;
namespace ShCore.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldInfoAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public FieldInfo FieldInfo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public object RawValue { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public object FieldValue { set; get; }

        /// <summary>
        /// Tên
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Thứ tự hiển thị lên view
        /// </summary>
        public int Index { set; get; }
        /// <summary>
        /// Ẩn hiện trên view
        /// </summary>
        public bool Visable { set; get; }
    }
}
