using System;

namespace ShCore.Caching.CacheType.SqlDependency
{
    /// <summary>
    /// Thông tin về Cơ sở dữ liệu
    /// Chỉ dùng khi sử dụng cache SqlDependency
    /// </summary>
    public class CacheDataBaseAttribute : Attribute
    {
        private string dataBase = string.Empty;
        public string DataBase
        {
            set { this.dataBase = value; }
            get { return this.dataBase; }
        }

        public CacheDataBaseAttribute(string dataBase)
        {
            this.dataBase = dataBase;
        }
    }
}
