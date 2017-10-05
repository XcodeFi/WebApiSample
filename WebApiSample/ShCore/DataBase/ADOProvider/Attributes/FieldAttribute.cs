using ShCore.Attributes;

namespace ShCore.DataBase.ADOProvider.Attributes
{
    /// <summary>
    /// Thông tin Field dữ liệu tương ứng cột trong cơ sở dữ liệu
    /// </summary>
    public class FieldAttribute : PropertyInfoAttribute
    {
        private bool isKey = false;
        /// <summary>
        /// Có phải là khóa
        /// </summary>
        public bool IsKey
        {
            get { return isKey; }
            set { isKey = value; }
        }

        private bool isIdentity = false;
        /// <summary>
        /// Có phải là tự tăng hay không
        /// </summary>
        public bool IsIdentity
        {
            get { return isIdentity; }
            set { isIdentity = value; }
        }

        private string fieldName;
        /// <summary>
        /// FieldName
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// Trường này có quan trọng ?
        /// Nếu có chỉnh sửa thì sẽ ghi log
        /// </summary>
        public bool Important { set; get; }
    }
}
