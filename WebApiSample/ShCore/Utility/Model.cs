using System.Collections.Generic;
using ShCore.Extensions;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Data;
using ShCore.Attributes.Validators;
namespace ShCore.Utility
{
    /// <summary>
    /// Model T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Model<T> where T : new()
    {
        /// <summary>
        /// Điền dữ liệu từ Dictionary vào T
        /// </summary>
        /// <param name="dicValues"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Parse(Dictionary<string, object> dicValues, bool ignoreCase)
        {
            return New(t => t.Parse(dicValues, ignoreCase));            
        }

        /// <summary>
        /// Parse đồng thời thực hiện validate
        /// </summary>
        /// <param name="dicValues"></param>
        /// <returns></returns>
        public static T ParseWithValidate(Dictionary<string, object> dicValues)
        {
            return New(t => 
            {
                // 
                var vm = t.Validate(dicValues);

                // 
                if (!vm.IsValid) throw new ValidatorException(vm);

                // 
                t.Parse(dicValues, false);
            });
        }

        /// <summary>
        /// Parse đồng thời thực hiện validate
        /// </summary>
        /// <param name="dicValues"></param>
        /// <returns></returns>
        public static T ParseWithValidate(NameValueCollection values)
        {
            return ParseWithValidate(values.ToDic());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Parse(NameValueCollection values, bool ignoreCase)
        {
            return New(t => t.Parse(values, ignoreCase));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Parse(DataRow dr, bool ignoreCase)
        {
            return New(t => t.Parse(dr, ignoreCase));
        }

        /// <summary>
        /// Parse DataTable thành List T
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<T> ParseToList(DataTable table, bool ignoreCase, Action<DataRow, T> action = null)
        {
            return Cast(table, ignoreCase, action).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> Cast(DataTable table, bool ignoreCase, Action<DataRow, T> action = null)
        {
            return table.AsEnumerable().Select(dr =>
            {
                var t = Parse(dr, ignoreCase);
                if (action != null) action(dr,t);
                return t;
            });
        }

        /// <summary>
        /// Khởi tạo T
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T New(Action<T> func = null)
        {
            // Khởi tạo T
            T t = new T();

            // 
            if (func != null) func(t);

            //
            return t;
        }
    }
}
