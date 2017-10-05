using System;
using System.Linq;
using System.Collections.Generic;
namespace ShCore.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// Tìm kiếm những bản tin mới từ danh sách mói so với danh sách cũ
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="listNews"></param>
        /// <param name="listOlds"></param>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        /// <returns></returns>
        public static IEnumerable<T1> FindNewItems<T1, T2, T>(this IEnumerable<T1> listNews, IEnumerable<T2> listOlds, Func<T1, T> ex1, Func<T2, T> ex2)
        {
            return (from vi_new in listNews
                    join vi_old in listOlds on ex1(vi_new) equals ex2(vi_old) into vi_old_
                    from vi_old_item in vi_old_.DefaultIfEmpty()
                    select new { vi_new, vi_old_item }).Where(o => o.vi_old_item == null || o.vi_old_item.Equals(default(T))).Select(o => o.vi_new);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="action"></param>
        public static int ForEach<T>(this IEnumerable<T> data, Action<T> action)
        {
            var i = 0;
            foreach (var t in data)
            {
                i++;
                action(t);
            }
            return i;
        }

        /// <summary>
        /// Join một trường lại thành chuỗi cách nhau bởi sep
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string JoinString<T, T1>(this IEnumerable<T> list, Func<T, T1> action, string sep = ",", string noarry = "")
        {
            var array = list.Select(action).ToArray();
            return string.Join(sep, array) + (array.Length == 1 ? noarry : "");
        }

        /// <summary>
        /// Join một trường lại thành chuỗi cách nhau bởi sep
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string JoinString<T, T1>(this IEnumerable<T> list, Func<T, int, T1> action, string sep = ",")
        {
            return string.Join(sep, list.Select(action).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="listNews"></param>
        /// <param name="listOlds"></param>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        /// <param name="map"></param>
        /// <param name="notmap"></param>
        public static void SLeftJoin<T1, T2, T>(this IEnumerable<T1> listNews, IEnumerable<T2> listOlds, Func<T1, T> ex1, Func<T2, T> ex2, Action<T1, T2> map, Action<T1> notmap)
        {
            (from vi_new in listNews
             join vi_old in listOlds on ex1(vi_new) equals ex2(vi_old) into vi_old_
             from vi_old_item in vi_old_.DefaultIfEmpty()
             select new { vi_new, vi_old_item }).Where(o =>
             {
                 if (o.vi_old_item == null) return true;
                 else
                 {
                     map(o.vi_new, o.vi_old_item);
                     return false;
                 }
             }).ToList().Select(o =>
             {
                 notmap(o.vi_new);
                 return false;
             }).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="action"></param>
        public static int SJoin<T1, T2, TValue>(this IEnumerable<T1> t1, IEnumerable<T2> t2, Func<T1, TValue> f1, Func<T2, TValue> f2, Action<T1, T2> action)
        {
            return t1.Join(t2, f1, f2, (t1i, t2i) => { action(t1i, t2i); return false; }).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="action"></param>
        public static int SGroupJoin<T1, T2, TValue>(this IEnumerable<T1> t1, IEnumerable<T2> t2, Func<T1, TValue> f1, Func<T2, TValue> f2, Action<T1, IEnumerable<T2>> action)
        {
            return t1.GroupJoin(t2, f1, f2, (t1i, t2is) => { action(t1i, t2is); return false; }).Count();
        }

        /// <summary>
        /// Mục đích để so sánh các phần tử trong danh sách
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ItemSequent<T>(this List<T> list, Action<T,T> action)
        {
            if (list.Count <= 1) return;

            for (var i = 0; i < list.Count - 1; i++)
                for (var j = i + 1; j < list.Count; j++)
                {
                    action(list[i], list[j]);
                }
        }
    }
}
