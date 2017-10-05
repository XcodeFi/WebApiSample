using System;
using System.ComponentModel;
using System.Globalization;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(DateTime))]
    public class DateTimeConverter : ShTypeConverter
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
                case ShTypeCode.String: return Convert.ToDateTime(value, CultureInfo.GetCultureInfo("vi-VN"));
                case ShTypeCode.DateTime: return value;                
                case ShTypeCode.DBNull: return null;
            }
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Các type code có thể Convert được
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.DateTime | ShTypeCode.String | ShTypeCode.DBNull;
        }
    }
}
