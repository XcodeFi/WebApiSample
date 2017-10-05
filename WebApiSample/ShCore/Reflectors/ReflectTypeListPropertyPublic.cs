using ShCore.Utility;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
namespace ShCore.Reflectors
{
    /// <summary>
    /// Type với các property public
    /// </summary
    public class ReflectTypeListPropertyPublic : DictionaryCacheBase<Type, List<PropertyInfo>>
    {
        /// <summary>
        /// Lấy các thuộc tính public
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override List<PropertyInfo> GetValueForDic(Type key)
        {
            return key.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        }
    }
}
