using System;
using System.ComponentModel;
using System.Globalization;
namespace ShCore.Types
{
    [ConverterOf(Target = typeof(Guid))]
    public class GuidConverter : ShTypeConverter
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
                case ShTypeCode.String: return new Guid(value.ToString());
                case ShTypeCode.Guid: return value;
                case ShTypeCode.DBNull: return Guid.Empty;
            }
            return base.ConvertFrom(context, culture, value, typeCode);
        }

        /// <summary>
        /// Có thể convert được từ những type nào
        /// </summary>
        /// <returns></returns>
        public override ShTypeCode GetTypeCodeCanConvert()
        {
            return ShTypeCode.String | ShTypeCode.Guid | ShTypeCode.DBNull;
        }
    }
}
