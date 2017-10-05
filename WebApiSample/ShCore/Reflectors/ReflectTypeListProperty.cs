using ShCore.Utility;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
namespace ShCore.Reflectors
{
    /// <summary>
    /// Reflect để lấy được danh sách PropertyInfo của một Type
    /// </summary>
    public class ReflectTypeListProperty : DictionaryCacheBase<Type, List<PropertyInfo>>
    {
        protected override List<PropertyInfo> GetValueForDic(Type key)
        {
            return key.GetProperties().ToList();
        }
    }
}
