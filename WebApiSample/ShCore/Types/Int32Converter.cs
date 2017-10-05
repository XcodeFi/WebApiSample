using System.ComponentModel;
using System.Globalization;
using System;
using ShCore.Extensions;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(int))]
    public class Int32Converter : ShTypeConverter
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
                case ShTypeCode.String: return value.ToString().IsNull() ? 0 : Convert.ToInt32(value);

                case ShTypeCode.Int16:                
                case ShTypeCode.Byte:
                case ShTypeCode.Decimal:
                case ShTypeCode.Int64:
                case ShTypeCode.Double: return Convert.ToInt32(value);  
                
                case ShTypeCode.Int32: return value;
                case ShTypeCode.Boolean: return value.To<bool>() ? 1 : 0;
                
                case ShTypeCode.DBNull: return 0;
            }

            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Có thể Convert được từ những kiểu dữ liệu gì
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.String | ShTypeCode.Double | ShTypeCode.Int32 | ShTypeCode.Int16 | ShTypeCode.Byte | ShTypeCode.Boolean | ShTypeCode.Int64 | ShTypeCode.DBNull;
        }
    }
}
