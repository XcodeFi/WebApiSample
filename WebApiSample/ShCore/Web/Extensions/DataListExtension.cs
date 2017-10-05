using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShCore.Extensions;
using ShCore.Reflectors;
using ShCore.Utility;
namespace ShCore.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataListExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dl"></param>
        /// <param name="obj"></param>
        public static void Bind(this DataList dl, object obj)
        {
            dl.DataSource = obj;
            dl.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsItem(this DataListItemEventArgs e)
        {
            return e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem;
        }

        /// <summary>
        /// Tìm Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="controlId"></param>
        /// <returns></returns>
        public static T Find<T>(this DataListItemEventArgs e, string controlId) where T : Control
        {
            return e.Item.FindControl(controlId) as T;
        }

        /// <summary>
        /// As
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T As<T>(this DataListItemEventArgs e)
        {
            return e.Item.DataItem.As<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Pair<PropertyInfo, TAttribute>> GetListPairPropertyAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return new ReflectTypeListPropertyWithAttribute<TAttribute>()[type];
        }
    }
}
