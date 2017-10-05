using System;
namespace ShCore.Attributes
{
    /// <summary>
    /// PropertyInfoAttribute
    /// </summary>
    public class PropertyInfoAttribute : Attribute
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { set; get; }
    }
}
