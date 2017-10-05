using System.Web;
using ShCore.Extensions;
using ShCore.Utility;
namespace ShCore.Web.Extensions
{
    /// <summary>
    /// Cung cấp phương thức mở rộng cho HttpRequest
    /// </summary>
    public static class HttpRequestExtension
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static T Query<T>(this HttpRequest request, string key, T @default = default(T))
        {
            return request.Params[key].To(@default);
        }

        /// <summary>
        /// Lọc ra đối tượng mà Client request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Get<T>(this HttpRequest request, bool ignoreCase) where T : new()
        {
            return Model<T>.Parse(request.Params, ignoreCase);
        }

        /// <summary>
        /// Điền dữ liệu client request vào một đối tượng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="obj"></param>
        /// <param name="ignoreCase"></param>
        public static void ParseTo(this HttpRequest request, object obj, bool ignoreCase)
        {
            obj.Parse(request.Params, ignoreCase);
        }
    }
}