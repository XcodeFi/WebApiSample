using System.Collections.Generic;
using System.Web;
using ShCore.Extensions;
using System;
namespace ShCore.Web.WebBase
{
    /// <summary>
    /// Message được dùng để truyền tải nội dung xuống client
    /// </summary>
    public class ResponseMessage
    {
        private string javaScript = string.Empty;
        /// <summary>
        /// JavaScript cần yêu cầu thực hiện xuống client
        /// </summary>
        public string JavaScript
        {
            get { return this.javaScript; }
            set { this.javaScript += value + ";"; }
        }

        private Dictionary<string, object> data = new Dictionary<string, object>();
        /// <summary>
        /// Data. Đối tượng kiểu mảng truyền xuống client
        /// </summary>
        public Dictionary<string, object> Data
        {
            get { return data; }
        }

        private string html = string.Empty;
        /// <summary>
        /// Nội dung Html muốn gửi xuống client
        /// </summary>
        public string Html
        { 
            set { this.html = value; } 
            get { return this.html; } 
        }

        /// <summary>
        /// Response mong muốn trả về tại thời điểm hiện thời
        /// </summary>
        public static ResponseMessage Current
        {
            get
            {
                // Lấy ra ResponseMessage ở trong Request hiện thời
                var response = HttpContext.Current.Items["ResponseMessage"] as ResponseMessage;

                // Nếu chưa có thì khởi tạo mời
                if (response == null) HttpContext.Current.Items["ResponseMessage"] = response = new ResponseMessage();

                // return
                return response;
            }
        }

        /// <summary>
        /// Gửi xuống một alert tới client
        /// </summary>
        /// <param name="msg"></param>
        public void Alert(string msg)
        {
            this.JavaScript = "ShCore.alert({0})".Frmat(msg.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void GotoUrl(string url)
        {
            this.JavaScript = "window.location.href = '" + url + "'";
        }

        /// <summary>
        /// Thiết lập một function chạy timeout
        /// </summary>
        /// <param name="func"></param>
        /// <param name="miliseconds"></param>
        public void SetTimeout(string func, int miliseconds)
        {
            this.JavaScript = "setTimeout(function() { " + func + ";}, " + miliseconds + ")";
        }

        /// <summary>
        /// Thực hiện cache trong context
        /// Chỉ ứng với một lần thực hiện request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T CacheContext<T>(string key, Func<T> func) where T : class
        {
            var t = HttpContext.Current.Items[key].As<T>();
            if (t == null)
                HttpContext.Current.Items[key] = t = func();
            return t;
        }
    }
}
