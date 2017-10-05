using System;
using System.Linq;
using ShCore.Attributes;
using ShCore.Utility;
using System.Collections.Generic;
using System.ComponentModel;
namespace ShCore.Reflectors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectFieldInfo<TAttribute> : DictionaryCacheBase<Type, List<TAttribute>> where TAttribute : FieldInfoAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override List<TAttribute> GetValueForDic(Type key)
        {
            return key.GetFields().Select(fi =>
            {
                var arr = fi.GetCustomAttributes(typeof(TAttribute), true);
                return new { Fi = fi, Attr = arr.Length == 0 ? null : arr[0] as TAttribute };
            }).
            Where(fia => fia.Attr != null).
            Select(fia =>
            {
                fia.Attr.FieldInfo = fia.Fi;
                fia.Attr.RawValue = fia.Fi.GetRawConstantValue();
                fia.Attr.FieldValue = TypeDescriptor.GetConverter(fia.Fi.FieldType).ConvertFromString(fia.Attr.RawValue.ToString());
                return fia.Attr;
            }).ToList();
        }
    }
}
