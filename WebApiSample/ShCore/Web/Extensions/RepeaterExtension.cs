using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShCore.Extensions;
using System.Linq;
namespace ShCore.Web.Extensions
{
    /// <summary>
    /// Phương thức mở rộng cho Repeater
    /// </summary>
    public static class RepeaterExtension
    {
        /// <summary>
        /// Thực hiện bind dữ liệu
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="source"></param>
        public static void DoBind(this GridView grid, object source)
        {
            grid.DataSource = source;
            grid.DataBind();
        }

        /// <summary>
        /// Thực hiện Bind dữ liệu tới GridView
        /// </summary>
        /// <param name="source"></param>
        /// <param name="grid"></param>
        public static void BindTo(this object source, GridView grid)
        {
            grid.DoBind(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static List<T> Cast<T>(this GridView grid) where T : new()
        {
            return grid.Rows.Cast<GridViewRow>().Select(r => r.ParseTo<T>(false)).ToList();
        }

        /// <summary>
        /// Thực hiện bind dữ liệu
        /// </summary>
        /// <param name="rp"></param>
        /// <param name="source"></param>
        public static void DoBind(this Repeater rp, object source)
        {
            rp.DataSource = source;
            rp.DataBind();
        }

        /// <summary>
        /// Bind dữ liệu tới Repeater
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rp"></param>
        public static void BindTo(this object source, Repeater rp)
        {
            rp.DoBind(source);
        }

        /// <summary>
        /// Bind dữ liệu tới các Repeater
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rps"></param>
        public static void BindTo(this object source, params Repeater[] rps)
        {
            foreach (var rp in rps) rp.DoBind(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsItem(this RepeaterItemEventArgs e)
        {
            return e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item;
        }

        /// <summary>
        /// Tìm Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="controlId"></param>
        /// <returns></returns>
        public static T Find<T>(this RepeaterItemEventArgs e, string controlId) where T : Control
        {
            return e.Item.FindControl(controlId) as T;
        }

        /// <summary>
        /// As
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T As<T>(this RepeaterItemEventArgs e)
        {
            return e.Item.DataItem.As<T>();
        }
    }
}
