using System.ComponentModel;
using System.Globalization;
namespace ShCore.Types
{
    /// <summary>
    /// Convert dữ liệu sang kiểu bool
    /// </summary>
    [ConverterOf(Target = typeof(bool))]
    public class BooleanConverter : ShTypeConverter
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
                case ShTypeCode.String: return value.ToString() == "true" || value.ToString() == "1";
                case ShTypeCode.Int32: return value.ToString() == "1";
                case ShTypeCode.Boolean: return value;
                case ShTypeCode.DBNull: return false;
            }

            // Base
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Các kiểu dữ liệu có thể Convert được
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.Int32 | ShTypeCode.Boolean | ShTypeCode.String | ShTypeCode.DBNull;
        }
    }
}
