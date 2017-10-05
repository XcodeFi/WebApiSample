using System;
using ShCore.Utility;
using System.Collections.Generic;
using System.Reflection;
namespace ShCore.Reflectors
{
    /// <summary>
    /// Reflect để lấy xem property có Attribute gì
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectTypeListPropertyWithAttribute<TAttribute> : DictionaryCacheBase<Type, List<Pair<PropertyInfo, TAttribute>>> where TAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override List<Pair<PropertyInfo, TAttribute>> GetValueForDic(Type key)
        {
            return MemberInfoHelper.Inst.GetListPairPropertyAttribute<TAttribute>(key, true);
        }
    }
}
