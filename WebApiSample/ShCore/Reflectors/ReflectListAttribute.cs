using System.Collections.Generic;
using ShCore.Utility;
using System.Reflection;
using System;
using System.Linq;
namespace ShCore.Reflectors
{
    /// <summary>
    /// Reflect để lấy xem property có list Attribute gì
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectListAttribute<TKey, TAttribute> : DictionaryCacheBase<TKey, List<TAttribute>> where TKey : MemberInfo where TAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override List<TAttribute> GetValueForDic(TKey key)
        {
            return MemberInfoHelper.Inst.GetAttributes<TAttribute>(key, true);
        }
    }
}
