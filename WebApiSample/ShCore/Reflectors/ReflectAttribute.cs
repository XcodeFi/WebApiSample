using System;
using System.Reflection;
using ShCore.Utility;
namespace ShCore.Reflectors
{
    /// <summary>
    /// ReflectAttribute
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectAttribute<TKey, TAttribute> : DictionaryCacheBase<TKey, TAttribute> where TKey : MemberInfo where TAttribute : Attribute
    {
        /// <summary>
        /// GetAttribute
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override TAttribute GetValueForDic(TKey key)
        {
            return MemberInfoHelper.Inst.GetAttribute<TAttribute>(key, true);
        }
    }
}
