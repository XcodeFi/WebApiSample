using ShCore.Attributes.Validators;
using ShCore.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShCore.Utility;
using ShCore.Types;
using System.ComponentModel;
using System.Collections;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Data;
using Newtonsoft.Json;
using System.Linq.Expressions;
namespace ShCore.Extensions
{
    /// <summary>
    /// Cung cấp phương thức mở rộng cho đối các đối tượng
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Chuyển đổi dữ liệu cho một đối tượng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static T To<T>(this object obj, T @default)
        {
            // Nếu bằng null thì return default
            if (obj == null) return @default;            

            // Lấy ra Converter của T
            var convert = SqlTypeDescriptor.Inst.GetConverter(typeof(T));

            // Thực hiện Convert
            return convert.CanConvertFrom(obj.GetType()) ? (T)convert.ConvertFrom(obj) : @default;
        }

        /// <summary>
        /// Convert đối tượng sang kiểu khác
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T To<T>(this object obj) 
        { 
            return obj.To<T>(default(T)); 
        }

        ///// <summary>
        ///// Thực hiện validate
        ///// </summary>
        ///// <param name="obj"></param>
        //public static ValidatorMessage Validate(this object obj)
        //{
        //    // Danh sách property có chứa attribute thực hiện validate
        //    var prs = obj.GetType().GetListPairPropertyListAttribute<ValidatorAttribute>();

        //    // 
        //    for (var i = 0; i < prs.Count; i++)
        //    {
        //        // Giá trị cần thực hiện validate
        //        var value = prs[i].T1.GetValue(obj, null);

        //        // Thực hiện validate với property đang xét
        //        var vm = DoValidate(prs[i], value);

        //        // Nếu có Message báo ko thỏa mãn thì return luôn
        //        if (vm != null) return vm;
        //    }

        //    // Message thực hiện validate            
        //    return ValidatorMessage.GetDefault();
        //}

        /// <summary>
        /// Thực hiện validate tập giá trị ứng với object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static ValidatorMessage Validate(this object obj, Dictionary<string, object> values)
        {
            // Danh sách property có chứa attribute thực hiện validate
            var prs = obj.GetType().GetListPairPropertyListAttribute<ValidatorAttribute>();

            // Join để thực hiện validate key trùng với tên property
            var list = values.Join(prs, v => v.Key, p => p.T1.Name, (v, p) => new { V = v, P = p }).ToList();

            for (var i = 0; i < list.Count; i++)
            {
                // Thực hiện validate với property đang xét
                var vm = DoValidate(list[i].P, list[i].V.Value, values, obj.GetType());

                // 
                if (vm != null) return vm;
            }

            // Message thực hiện validate
            return ValidatorMessage.GetDefault();
        }

        /// <summary>
        /// Thực hiện validate tập giá trị ứng với object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static ValidatorMessage Validate(this object obj, NameValueCollection values)
        {
            return obj.Validate(values.ToDic());
        }

        /// <summary>
        /// Thực hiện validate với một giá trị
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ValidatorMessage DoValidate(Pair<PropertyInfo, List<ValidatorAttribute>> pr, object value, Dictionary<string, object> data, Type typeObject)
        { 
            // Property quy định alias tên thuộc tính
            var pif = pr.T1.GetAttribute<PropertyInfoAttribute>();

            // Tên alias
            var name = pif == null ? pr.T1.Name : pif.Name;

            // sắp xếp thứ tự các validator
            var list = pr.T2.OrderBy(v => v.Stt).ToList();

            // Duyệt qua các validator của property
            for (var j = 0; j < list.Count; j++)
            {
                var validator = list[j];

                // Thiết lập giá trị cần thực hiện validate
                validator.SetData(value, name);
                validator.Data = data;
                validator.ObjectType = typeObject;

                // Thực hiện valiate
                if (!validator.Validate()) return new ValidatorMessage
                {
                    Status = ValidatorStatus.InValid,
                    Message = validator.GetMessage(),
                    FieldName = pr.T1.Name
                };
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra xem obj có phải là T hay không
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Is<T>(this object obj) 
        { 
            return obj is T; 
        }

        /// <summary>
        /// Cast object dang list ra list object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<object> CastToList(this object obj)
        {
            if (obj.Is<string>()) return null;

            // Nếu là IListSource 
            if (obj.Is<IListSource>()) return obj.As<IListSource>().GetList().Cast<object>().ToList();

            // Nếu là IEnumerable thì lấy object
            if (obj.Is<IEnumerable>()) return obj.As<IEnumerable>().Cast<object>().ToList();

            // mặc định là return null
            return null;
        }

        /// <summary>
        /// Kiểm tra xem một đối tượng có Null hay không
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj) 
        { 
            return obj == null; 
        }

        /// <summary>
        /// Kiểm tra xem một đối tượng có Not Null hay không
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Cast object to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T As<T>(this object obj) 
        { 
            return obj.IsNull() ? default(T) : (T)obj; 
        }

        /// <summary>
        /// Phân tích giá trị thuộc tính cho đối tượng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        /// <param name="ignoreCase"></param>
        public static void Parse(this object obj, NameValueCollection values, bool ignoreCase)
        {
            obj.Parse(values.ToDic(), ignoreCase);
        }

        /// <summary>
        /// Phân tích giá trị thuộc tính cho đối tượng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        /// <param name="ignoreCase"></param>
        public static void Parse(this object obj, DataRow dr, bool ignoreCase)
        {
            // Dùng dic để lưu các giá trị
            var dic = new Dictionary<string, object>();

            // Duyệt qua từng Column
            dr.Table.Columns.Cast<DataColumn>().ToList().ForEach(c => dic[c.ColumnName] = dr[c]);

            // 
            obj.Parse(dic, ignoreCase);
        }

        /// <summary>
        /// Phân tích giá trị thuộc tính cho đối tượng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        /// <param name="ignoreCase"></param>
        public static void Parse(this object obj, Dictionary<string, object> dic, bool ignoreCase)
        {
            // Không phân biệt hoa thường ở tên property
            if (ignoreCase) obj.Parse(dic, p => p.Name.ToLower(), name => name.ToLower());

            // Có phân biệt hoa thường ở tên property
            else obj.Parse(dic, p => p.Name, name => name);
        }

        /// <summary>
        /// Phân tích giá trị thuộc tính cho đối tượng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        private static void Parse(this object obj, Dictionary<string, object> dic, Func<PropertyInfo, string> getPropertyName, Func<string,string> getKeyName)
        {
            // TypeOfObject
            var type = obj.GetType();

            // danh sách Property của obj
            var pis = type.GetListProperties().Where(p => p.CanWrite);

            // Join tìm thuộc tính phù hợp để điền giá trị
            pis.Join(dic, p => getPropertyName(p), d => getKeyName(d.Key), (p, d) => 
            {
                // Lấy ra Converter phù hợp
                var converter = SqlTypeDescriptor.Inst.GetConverter(p.PropertyType);

                // Nếu convert được thì gán giá trị bằng giá trị được convert
                if (converter.CanConvertFrom(d.Value.IsNull() ? typeof(DBNull) : d.Value.GetType()))
                    p.SetValue(obj, converter.ConvertFrom(d.Value), null);

                else if (converter.GetType().Name == typeof(EnumConverter).Name && d.Value.IsNotNull())
                    p.SetValue(obj, converter.ConvertFrom(d.Value.ToString()), null);

                return false;
            }).Count();
        }

        /// <summary>
        /// Hàm thiết lập giá trị cho propertyName của đối tượng mà không có validate
        /// Chỉ dùng khi chắc chắn propertyName là có và value là giá trị gán được
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetValueToProperty(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="igonreCase"></param>
        public static void SetValueToProperty(this object obj, string propertyName, object value, bool igonreCase)
        {
            var dic = new Dictionary<string, object>();
            dic[propertyName] = value;
            obj.Parse(dic, igonreCase);
        }

        /// <summary>
        /// Lựa chọn giá trị khi Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static T WhenNull<T>(this T t, Func<T> action)
        {
            return t.IsNull() ? action() : t;
        }

        /// <summary>
        /// Lựa chọn switch value khi t bằng một giá trị nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="when"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T WhenValue<T>(this T t, T when, Func<T> action)
        {
            return t.Equals(when) ? action() : t;
        }

        /// <summary>
        /// Thực hiện Eval
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static object Eval(this object obj, string member)
        {
            return DataBinder.Eval(obj, member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static object TryEval(this object obj, string member)
        {
            try
            {
                return obj.Eval(member);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Thực hiện gọi đến một phương thức và Boundary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static T Call<T>(this object obj, Expression<Func<T>> exp)
        {
            // Nếu không phải là một phương thức thì trả ra mặc định của T luôn
            if (!(exp.Body is MethodCallExpression)) return default(T);

            // MethodCallExpression
            var mce = exp.Body as MethodCallExpression;

            // Lấy ra MethodBoundaryAttribute bao phương thức
            var mbi = mce.Method.GetAttribute<MethodBoundaryAttribute>(false);

            // Nếu không tồn tại attribute bao phương thức thì thực hiện và trả ra luôn kết quả
            if (mbi.IsNull()) return (T)Expression.Lambda(mce).Compile().DynamicInvoke();

            // Tham số cho sự kiện before, after của method
            var args = new MethodBoundaryArgs { Arguments = mce.GetValueOfParameter() };

            // Lưu giá trị cần trả ra
            T t = default(T);

            // Thực hiện phương thức và bao gồm các sự kiện bao quanh phương thức
            try
            {
                // MethodInfo
                mbi.MethodInfo = mce.Method;

                // Trước khi thực hiện phương thức
                mbi.Before(args);

                // Thực hiện phương thức nếu chưa có dữ liệu trả về
                if (args.ValueReturn.IsNull()) args.ValueReturn = t = (T)Expression.Lambda(mce).Compile().DynamicInvoke();

                // Lay gia tri tra ve tu Before xu ly
                else t = args.ValueReturn.As<T>();
            }
            catch (Exception ex)
            {
                // Khi có Exception xảy ra
                mbi.OnException();

                // Throw lại ex
                throw ex;
            }
            finally
            {
                // Sau khi thực hiện phương thức thành công
                mbi.After(args);
            }

            // trả ra kết quả
            return t;
        }
    }
}
