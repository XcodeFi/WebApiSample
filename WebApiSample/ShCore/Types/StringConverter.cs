using System.ComponentModel;
using System.Globalization;
using System;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(string))]
    public class StringConverter : ShTypeConverter
    {
        /// <summary>
        /// Thực hiện Convert
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, ShTypeCode typeCode)
        {
            switch (typeCode)
            {                
                default: return value.ToString();
                case ShTypeCode.DBNull: return string.Empty;
            }
        }

        /// <summary>
        /// Các kiểu dữ liệu có thể Convert được
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.UnKnown;
        }

        /// <summary>
        /// Có thể  Convert được từ Type nào
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }
    }
}
