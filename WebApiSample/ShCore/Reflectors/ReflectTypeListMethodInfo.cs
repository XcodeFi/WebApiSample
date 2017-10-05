using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using ShCore.Attributes;
using ShCore.Utility;
namespace ShCore.Reflectors
{
    /// <summary>
    /// Lấy ra danh sách MethodInfo với attribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectTypeListMethodInfo<TAttribute> : DictionaryCacheBase<Type, List<TAttribute>> where TAttribute : MethodInfoAttribute
    {
        protected override List<TAttribute> GetValueForDic(Type key)
        {
            // var abc = key.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(m => m.Name == "Lock").FirstOrDefault();

            return key.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).
                Select(m => Singleton<ReflectMethodInfo<TAttribute>>.Inst[m]).Where(t => t != null).ToList();
        }
    }
}
