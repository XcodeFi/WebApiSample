using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
using ShCore.Utility;
using ShCore.Extensions;
using ShCore.Attributes;
namespace ShCore.Reflectors
{
    /// <summary>
    /// Reflect một Assembly để có được danh sách Type mà Property có TAttribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectAssemblyTypeListPropertyWithAttribute<TAttribute> : DictionaryCacheBase<Assembly, List<Pair<Type, List<Pair<PropertyInfo, TAttribute>>>>> where TAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override List<Pair<Type, List<Pair<PropertyInfo, TAttribute>>>> GetValueForDic(Assembly key)
        {
            return key.GetTypes().Select(t => new Pair<Type, List<Pair<PropertyInfo, TAttribute>>>
            {
                T1 = t,
                T2 = t.GetListPairPropertyAttribute<TAttribute>()
            }).Where(r => r.T2.Count != 0).ToList();
        }
    }

    /// <summary>
    /// Lấy danh sách ClassInfoAttribute của một Assembly
    /// </summary>
    public class ReflectAssemblyClassInfoAttribute<T> : DictionaryCacheBase<Assembly, List<T>> where T : ClassInfoAttribute
    {
        /// <summary>
        /// Thực hiện lấy List ClassInfoAttribute theo Assembly
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override List<T> GetValueForDic(Assembly key)
        {
            return key.GetTypes().Select(t => Singleton<ReflectClassInfo<T>>.Inst[t]).Where(t => t.IsNotNull()).ToList();
        }
    }
}
