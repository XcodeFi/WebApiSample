using ShCore.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace ShCore.Reflectors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectTypeListPropertyWithListAttribute<TAttribute> : DictionaryCacheBase<Type, List<Pair<PropertyInfo, List<TAttribute>>>> where TAttribute : Attribute
    {
        protected override List<Pair<PropertyInfo, List<TAttribute>>> GetValueForDic(Type key)
        {
            return MemberInfoHelper.Inst.GetListPairPropertyListAttribute<TAttribute>(key, true);
        }
    }
}
