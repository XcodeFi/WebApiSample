using System.ComponentModel;
using System.Data.SqlTypes;
using System;
using System.Globalization;
namespace ShCore.Types
{
    /// <summary>
    /// TypeConvert của Sơn
    /// </summary>
    public abstract class ShTypeConverter : TypeConverter
    {
        /// <summary>
        /// Xem có thể Convert được từ những kiểu dữ liệu nào
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {                  
            return GetTypeCodeCanConvert().IsSet(SqlTypeDescriptor.Inst.GetShTypeCode(sourceType));
        }

        /// <summary>
        /// Thực hiện Convert
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return ConvertFrom(context, culture, value, value == null ? ShTypeCode.DBNull : SqlTypeDescriptor.Inst.GetShTypeCode(value.GetType()));
        }

        /// <summary>
        /// Thực hiện Convert thông qua typeCode
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public virtual object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, ShTypeCode typeCode)
        {
            // Mặc định là Convert theo class Base
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// return những Type Code có thể Convert được
        /// </summary>
        /// <returns></returns>
        public abstract ShTypeCode GetTypeCodeCanConvert();
    }
}
