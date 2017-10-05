using System;
using System.Collections.Generic;
namespace ShCore.Extensions
{
    public static class DateTimeExtension
    {
        public static IEnumerable<DateTime> RangeTo(this DateTime from, DateTime to)
        {
            var totalDays = (to - from).TotalDays;

            yield return from;

            for (int i = 1; i <= totalDays; i++)
                yield return from.AddDays(i);
        }

        private static DateTime MinTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double GetTotalMilliseconds(this DateTime date)
        {
            return (date - MinTime).TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this double milliseconds)
        {
            return MinTime.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static FromTo<DateTime> GetRangeDateInWeek(this DateTime date)
        {
            var dow = (int)date.DayOfWeek;
            if (dow == 0) dow = 7;

            return new FromTo<DateTime>
            {
                From = date.AddDays(1 - dow).Date,
                To = date.AddDays(7 - dow).Date.AddDays(1).AddSeconds(-1)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static FromTo<DateTime> GetRangeDateInMonth(this DateTime date)
        {
            return new FromTo<DateTime>
            {
                From = new DateTime(date.Year, date.Month, 1),
                To = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddSeconds(-1)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TimeSpan TryParseTimeAndThrowException(this string data)
        {
            TimeSpan ts;
            if (TimeSpan.TryParse(data, out ts)) return ts;
            else throw new Exception(data + " không đúng định dạng HH:mm");
        }

        /// <summary>
        /// DayOfWeek chuẩn bus
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetDayOfWeek(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Sunday ? 8 : ((int)dateTime.DayOfWeek + 1);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FromTo<T>
    {
        public T From { set; get; }
        public T To { set; get; }
    }
}
