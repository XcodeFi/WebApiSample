using System.ComponentModel;
using System.Globalization;
using System;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(long))]
    public class Int64Converter : ShTypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, ShTypeCode typeCode)
        {
            switch (typeCode)
            {
                case ShTypeCode.String:                  
                case ShTypeCode.Int16:                
                case ShTypeCode.Int32:                
                case ShTypeCode.Decimal:
                case ShTypeCode.Byte: return Convert.ToInt64(value);                
                case ShTypeCode.Int64: return value;
                case ShTypeCode.DBNull: return 0;
            }
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.Int64 | ShTypeCode.String | ShTypeCode.Int16 | ShTypeCode.Int32 | ShTypeCode.Byte | ShTypeCode.Decimal | ShTypeCode.DBNull;
        }
    }
}
