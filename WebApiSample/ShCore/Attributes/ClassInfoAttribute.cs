using System;
namespace ShCore.Attributes
{
    /// <summary>
    /// ClassInfoAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]    
    public class ClassInfoAttribute : Attribute
    {
        /// <summary>
        /// Type của class có Attribute ClassInfo
        /// </summary>
        public Type Type { set; get; }
    }
}
