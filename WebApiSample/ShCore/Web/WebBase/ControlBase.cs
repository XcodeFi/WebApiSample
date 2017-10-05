using System.Web.UI;
using ShCore.Web.Extensions;
using ShCore.Extensions;
using PageWeb = System.Web.UI.Page;
using System;
using System.Web;
using ShCore.Utility;
namespace ShCore.Web.WebBase
{
    /// <summary>
    /// ControlBase
    /// </summary>
    public class ControlBase : UserControl
    {
        /// <summary>
        /// QueryString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public T GetParam<T>(string key, T @default = default(T))
        {
            return CurrentContext.Request.Query(key, @default);
        }

        /// <summary>
        /// QueryString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetParam<T>(string key)
        {
            return CurrentContext.Request.Query(key, default(T));
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpContext CurrentContext
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        /// Phương thức khởi tạo Control
        /// Có thể kế thừa lại được
        /// </summary>
        public virtual void InitData() { }

        /// <summary>
        /// Lấy ra Html nội dung của Control
        /// </summary>
        public string Html 
        {
            get { return this.GetHtml(); } 
        }

        /// <summary>
        /// Thực hiện LoadControl
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ControlBase DoLoad(Type type, PageWeb page = null)
        {
            page = page ?? new PageWeb();

            // AssemblyName
            var assemblyName = type.Assembly.FullName.Split(',')[0];

            // LoadControl            
            return page.LoadControl("/{0}.ascx".Frmat(type.FullName.Replace("{0}.".Frmat(assemblyName), string.Empty).Replace(".", "/").TrimStart('_'))) as ControlBase;
        }
    }

    /// <summary>
    /// Class để khởi tạo Control
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Control<T> where T : ControlBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static T Create(PageWeb page)
        {
            return ControlBase.DoLoad(typeof(T), page) as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T Create()
        {
            return Create(new PageWeb());
        }
    }
}