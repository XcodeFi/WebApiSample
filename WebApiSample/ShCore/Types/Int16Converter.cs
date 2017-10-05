using System.ComponentModel;
using System.Globalization;
using System;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(short))]
    public class Int16Converter : ShTypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, ShTypeCode typeCode)
        {
            switch (typeCode)
            {
                case ShTypeCode.String:
                case ShTypeCode.Byte: return Convert.ToInt16(value);
                case ShTypeCode.Int16: return value;
                case ShTypeCode.DBNull: return 0;
            }
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Có thể Convert được từ những TypeCode nào
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.Int16 | ShTypeCode.String | ShTypeCode.Byte | ShTypeCode.DBNull;
        }
    }
}
