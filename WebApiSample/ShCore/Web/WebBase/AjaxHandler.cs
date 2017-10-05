using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using ShCore.Extensions;
using ShCore.Reflectors;
using ShCore.Utility;
using ShCore.Web.Extensions;
namespace ShCore.Web.WebBase
{
    /// <summary>
    /// Thực hiện xử lý một Request Ajax
    /// </summary>
    public class AjaxHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Xử lý Request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            // Request client  đang thực hiện
            var request = context.Request;

            // Type của đối tượng chứa phương thức Ajax mà client đang yêu cầu
            var typeAjaxable = "{0}.{1},{0}".Frmat(request.QueryString["_n"], request.QueryString["_o"]);

            try
            {
                // 
                var typeAjax = Type.GetType(typeAjaxable);

                // Lấy phương thức cần thực hiện
                var method = typeAjax.GetMethod(request.QueryString["_m"]);

                // Lấy ra điều kiện gọi phương thức và kiểm tra có được phép gọi phương thức hay không
                var listAca = method.GetAttributes<AjaxRequestConditionAttribute>().OrderBy(a => a.Stt).ToList();
                for (var i = 0; i < listAca.Count; i++)
                    if (!listAca[i].Condition) throw new Exception(listAca[i].Msg);

                // Tìm attribute bound method
                var abr = new ReflectMethodInfo<AjaxBoundRequestAttribute>()[method];

                // Trước khi thực hiện gọi phương thức ajax
                if (abr != null) abr.Before();

                // Gọi phương thức
                method.Invoke(Activator.CreateInstance(typeAjax) as IAjax, new object[] { });

                // Sau khi thực hiện phương thức ajax
                if (abr != null) abr.After();
            }
            catch (Exception ex)
            {
                // Lấy ra Exception ở bên trong
                var exInner = ex.InnerException;

                // Chừng nào mà Exception bên trong không phải null thì kiểm tra lấy tiếp
                Exception exTemp = null;
                while (exInner != null)
                {
                    exTemp = exInner;
                    exInner = exInner.InnerException;
                }

                // Lấy ra Exeption 
                exTemp = exTemp ?? ex;

                // Thông báo
                ResponseMessage.Current.Data["MessageError"] = exTemp.Message;
            }
            finally
            {   
                context.Response.Write(JsonConvert.SerializeObject(ResponseMessage.Current));
                EndRequest();
            }
        }

        /// <summary>
        /// Kết thúc Request
        /// </summary>
        protected virtual void EndRequest() { }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }
    }

    public interface IAjax { }

    public static class IAjaxExtension
    {
        /// <summary>
        /// Truy vấn param
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ajax"></param>
        /// <param name="key"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static T Query<T>(this IAjax ajax, string key, T @default = default(T))
        {
            return HttpContext.Current.Request.Query(key, @default);
        }

        /// <summary>
        /// Thiết lập Data gửi xuống client
        /// </summary>
        /// <param name="ajax"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public static void SetData(this IAjax ajax, string name, object data)
        {
            ResponseMessage.Current.Data[name] = data;
        }

        /// <summary>
        /// ParseParamTo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ajax"></param>
        /// <returns></returns>
        public static T ParseParamTo<T>(this IAjax ajax) where T : new()
        {
            return Model<T>.ParseWithValidate(HttpContext.Current.Request.Params);
        }

        /// <summary>
        /// Gửi lệnh reload tới client
        /// </summary>
        /// <param name="ajax"></param>
        public static void Reload(this IAjax ajax)
        {
            ResponseMessage.Current.JavaScript = "window.location.reload()";
        }

        /// <summary>
        /// ReloadPath
        /// </summary>
        /// <param name="ajax"></param>
        public static void ReloadPath(this IAjax ajax)
        {
            ResponseMessage.Current.JavaScript = "window.location.href = window.location.pathname";
        }

        /// <summary>
        /// Alert
        /// </summary>
        /// <param name="ajax"></param>
        /// <param name="msg"></param>
        public static void Alert(this IAjax ajax, string msg)
        {
            ResponseMessage.Current.JavaScript = "ShCore.alert(" + JsonConvert.SerializeObject(msg) + ")";
        }
    }
}
