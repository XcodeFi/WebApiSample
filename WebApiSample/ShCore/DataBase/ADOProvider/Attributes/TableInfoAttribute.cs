using System;
namespace ShCore.DataBase.ADOProvider.Attributes
{
    /// <summary>
    /// Thông tin bảng
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableInfoAttribute : Attribute
    {
        private string tableName = string.Empty;
        /// <summary>
        /// Tên Table
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
    }
}
