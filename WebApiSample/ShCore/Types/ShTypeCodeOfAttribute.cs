using ShCore.Attributes;
using System;
namespace ShCore.Types
{
    /// <summary>
    /// Mở rộng cho TypeCode của dữ liệu gì
    /// </summary>    
    public class ShTypeCodeOfAttribute : FieldInfoAttribute
    {
        private Type type = null;
        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get { return type; }            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        public ShTypeCodeOfAttribute(Type type)
        {
            this.type = type;
        }
    }
}
