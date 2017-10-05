using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using ShCore.Extensions;
namespace ShCore.Types
{
    /// <summary>
    /// Định nghĩa Descriptor cho SqlTypes để lấy Converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SqlTypeDescriptor
    {
        /// <summary>
        /// private Constructor để không cho khởi tạo, chỉ khởi tạo một lần
        /// </summary>
        private SqlTypeDescriptor() { }

        /// <summary>
        /// Khởi tạo một Instance duy nhất
        /// </summary>
        private static SqlTypeDescriptor inst = new SqlTypeDescriptor();
        public static SqlTypeDescriptor Inst
        {
            get { return inst; }
        }

        private Dictionary<Type, TypeConverter> dicTypeConverter = null;
        /// <summary>
        /// Thư viện TypeConverter của SqlTypes
        /// </summary>
        private Dictionary<Type, TypeConverter> DicTypeConverter
        {
            get
            {
                if (dicTypeConverter == null)
                {
                    lock (obj)
                    {
                        // Khởi tạo Dictionary lưu TypeConverter
                        dicTypeConverter = new Dictionary<Type, TypeConverter>();

                        // Duyệt qua từng Types trong Assembly
                        ConverterOfAttribute coa = null; TypeConverter tc = null;
                        this.GetType().Assembly.GetTypes().ToList().ForEach(t =>
                        {
                            // Kiểm tra xem class này có định nghĩa attribute type muốn convert                             
                            // Nếu có thì kiểm tra xem kiểu dữ liệu này có phải là Converter hay không
                            if ((coa = t.GetAttribute<ConverterOfAttribute>()) != null && (tc = t.CreateInstance<TypeConverter>()) != null)
                                dicTypeConverter[coa.Target] = tc;
                        });
                    }
                }
                return dicTypeConverter;
            }
        }

        private Dictionary<Type, ShTypeCode> dicTypeCode = null;
        private Dictionary<Type, ShTypeCode> DicTypeCode
        {
            get
            {
                if (dicTypeCode == null)
                {
                    lock (obj)
                    {
                        dicTypeCode = new Dictionary<Type, ShTypeCode>();
                        // Lấy ra các giá trị enum của ShTypeCode, tìm ra từng Attribute quy định kiểu dữ liệu
                        // Xong sau đó đưa vào Dictionary

                        var fields = typeof(ShTypeCode).GetFields();

                        ShTypeCodeOfAttribute attr = null;
                        foreach (var f in fields)
                            if ((attr = f.GetAttribute<ShTypeCodeOfAttribute>()) != null)
                                dicTypeCode[attr.Type] = (ShTypeCode)f.GetRawConstantValue();
                    }
                }
                return dicTypeCode;
            }
        }

        private static object obj = new object();

        /// <summary>
        /// Thực hiện lấy Converter
        /// </summary>
        /// <returns></returns>
        public TypeConverter GetConverter(Type type)
        {
            // Viết lại Converter tại đây
            TypeConverter typeConverter = null;

            // Nếu thực hiện lấy ở dic không thành công thì tạo mới
            return DicTypeConverter.TryGetValue(type, out typeConverter) ? typeConverter : TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Lấy ra ShTypeCode của kiểu dữ liệu
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ShTypeCode GetShTypeCode(Type type)
        {
            // Mặc định là không biết TypeCode là gì
            ShTypeCode typeCode;

            // Lấy ra Type Code của từng loại
            if (!DicTypeCode.TryGetValue(type, out typeCode)) typeCode = ShTypeCode.UnKnown;

            // return
            return typeCode;
        }
    }
}
