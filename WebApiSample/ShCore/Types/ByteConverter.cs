using System.ComponentModel;
using System.Globalization;
using System;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(byte))]
    public class ByteConverter : ShTypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, ShTypeCode typeCode)
        {
            switch (typeCode)
            {
                case ShTypeCode.Int32:
                case ShTypeCode.String: return Convert.ToByte(value);                
                case ShTypeCode.Byte: return value;
                case ShTypeCode.DBNull: return 0;
            }

            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Các kiểu dữ liệu có thể Convert sang byte
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.Byte | ShTypeCode.String | ShTypeCode.Int32 | ShTypeCode.DBNull;
        }
    }
}
