using System.Reflection;
using ShCore.Attributes;
namespace ShCore.Reflectors
{
    /// <summary>
    /// ReflectMethodInfo MethodInfoAttribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class ReflectMethodInfo<TAttribute> : ReflectAttribute<MethodInfo, TAttribute> where TAttribute : MethodInfoAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override TAttribute GetValueForDic(MethodInfo key)
        {
            var att = base.GetValueForDic(key);
            if(att != null) att.MethodInfo = key;
            return att;
        }
    }
}
