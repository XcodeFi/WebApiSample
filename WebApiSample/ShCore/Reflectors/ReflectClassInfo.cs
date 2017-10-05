using System;
using ShCore.Attributes;
using ShCore.Utility;
namespace ShCore.Reflectors
{
    /// <summary>
    /// ReflectClassInfo ClassInfoAttribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectClassInfo<TAttribute> : ReflectAttribute<Type, TAttribute>  where TAttribute : ClassInfoAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override TAttribute GetValueForDic(Type key)
        {
            var att = base.GetValueForDic(key);
            if (att != null) att.Type = key;
            return att;
        }
    }
}
