using System;
namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// Validate DateTime đúng định dạng
    /// </summary>
    public class ValidatorDateTimeFormat : ValidatorAttribute
    {
        public override bool Validate()
        {
            // null 
            if (this.Value == null) return false;

            if (Value is DateTime && Convert.ToDateTime(Value) == new DateTime(1900, 1, 2)) return false;

            // string empty
            if (string.IsNullOrEmpty(this.Value.ToString().Trim())) return false;

            // 
            return true;
        }

        /// <summary>
        /// Ghi ra thông báo
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return string.Format("{0} không được nhập sai định dạng.", this.FieldName);
        }
    }

    /// <summary>
    /// Validate DateTime đúng định dạng cho phép null
    /// </summary>
    public class ValidatorDateTimeFormatNullAllow : ValidatorAttribute
    {
        public override bool Validate()
        {
            if (Value is DateTime && Convert.ToDateTime(Value) == new DateTime(1900, 1, 2)) return false;

            return true;
        }

        /// <summary>
        /// Ghi ra thông báo
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return string.Format("{0} không tồn tại, kiểm tra lại giá trị nhập vào.", this.FieldName);
        }
    }
}
