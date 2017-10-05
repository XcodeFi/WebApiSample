using System.ComponentModel;
using System.Globalization;
using System;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(double))]
    public class DoubleConverter : ShTypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, ShTypeCode typeCode)
        {
            switch (typeCode)
            {
                case ShTypeCode.String:
                case ShTypeCode.Single: return value.GetType()==typeof(string)&& string.IsNullOrEmpty(value.ToString())? 0: Convert.ToDouble(value);                   
                case ShTypeCode.Double: return value;
                case ShTypeCode.DBNull: return 0;
            }
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Các type code có thể thực hiện convert được
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.Double | ShTypeCode.String | ShTypeCode.DBNull;
        }
    }
}
