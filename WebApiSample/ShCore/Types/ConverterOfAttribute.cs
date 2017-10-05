using ShCore.Attributes;
using System;
namespace ShCore.Types
{
    /// <summary>
    /// Chỉ định class là Convert cho kiểu dữ liệu nào
    /// </summary>
    public class ConverterOfAttribute : ClassInfoAttribute
    {
        /// <summary>
        /// Target => Kiểu dữ liệu mong muốn được thực hiện Convert
        /// </summary>
        public Type Target { set; get; }
    }
}
