using System.Data;
using ShCore.Utility;
using System.Collections.Generic;
using System;
using ShCore.Extensions;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbTable<T> where T : ModelBase, new()
    {
        /// <summary>
        /// Lấy tất cả ra List
        /// </summary>
        /// <returns></returns>
        public static List<T> GetAllToList(Action<DataRow, T> afterParse = null)
        {
            // Select All
            var table = GetAll();

            // Trả ra dữ liệu
            return table.IsNull() ? new List<T>() : Model<T>.ParseToList(table, false, afterParse);
        }

        /// <summary>
        /// Lấy tất cả dữ liệu và trả ra Table
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {
            return Singleton<T>.Inst.GetAll();
        }
    }
}
