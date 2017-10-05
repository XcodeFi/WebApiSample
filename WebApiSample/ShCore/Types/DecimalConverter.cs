using System.ComponentModel;
using System.Globalization;
using System;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(decimal))]
    public class DecimalConverter : ShTypeConverter
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
                case ShTypeCode.String:                    
                case ShTypeCode.Int64:                
                case ShTypeCode.Int32:                
                case ShTypeCode.Int16:
                case ShTypeCode.Double: return Convert.ToDecimal(value);
                case ShTypeCode.Decimal: return value;
                case ShTypeCode.DBNull: return new Decimal(0);
            }
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Các kiểu dữ liệu có thể Convert được
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.Decimal | ShTypeCode.String | ShTypeCode.Int64 | ShTypeCode.Int32 | ShTypeCode.Int16 | ShTypeCode.Double | ShTypeCode.DBNull;
        }
    }
}
