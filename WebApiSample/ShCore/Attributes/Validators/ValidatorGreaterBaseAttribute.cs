using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShCore.DataBase.ADOProvider.Attributes;
using ShCore.Utility;

namespace ShCore.Attributes.Validators
{
    public abstract class ValidatorGreaterBaseAttribute : ValidatorAttribute
    {
        /// <summary>
        /// Field cần được so sánh giá trị
        /// </summary>
        private readonly string _fieldCompare = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldCompare"></param>
        public ValidatorGreaterBaseAttribute(string fieldCompare)
        {
            _fieldCompare = fieldCompare;
        }

        /// <summary>
        /// Thực hiện lấy message khi dữ liệu không đúng
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            string nameAlias = string.Empty;

            var pi = ObjectType.GetProperty(_fieldCompare);
            if (pi != null)
            {
                var fa = MemberInfoHelper.Inst.GetAttribute<FieldAttribute>(pi, true);
                if (fa != null)
                    nameAlias = fa.Name;
            }


            // Lấy thông tin tên trường
            // var fd = MemberInfoHelper.Inst.GetAttribute ;//this.ObjectValidate.GetType().GetProperty(this.fieldCompare).GetAttribute<FieldAttribute>();

            // Đợi làm nhé
            return FieldName + " phải lớn hơn " + (nameAlias ?? _fieldCompare);
        }

        /// <summary>
        /// Thực hiện validate
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            // Lấy giá trị cần so sánh
            object valueCompare; // this.ObjectValidate.GetPropertyValue(this.fieldCompare);

            if (!Data.TryGetValue(_fieldCompare, out valueCompare)) return true;

            // Nếu 1 trong 2 là Null thì không thực hiện validate
            if (valueCompare == null || Value == null) return true;

            // Đúng khi mà lớn hơn
            return IsValueGreater(valueCompare);
        }

        /// <summary>
        /// Kiểm tra xem giá trị cần thực hiện validate có lớn hơn
        /// </summary>
        /// <param name="valueCompare"></param>
        /// <returns></returns>
        protected abstract bool IsValueGreater(object valueCompare);
    }

    /// <summary>
    /// Validate giá trị lớn hơn với kiểu Double
    /// </summary>
    public class ValidatorGreaterAttribute : ValidatorGreaterBaseAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldName"></param>
        public ValidatorGreaterAttribute(string fieldName) : base(fieldName) { }

        /// <summary>
        /// Thực hiện so sánh
        /// </summary>
        /// <param name="valueCompare"></param>
        /// <returns></returns>
        protected override bool IsValueGreater(object valueCompare)
        {
            if (Value == null || valueCompare == null || string.IsNullOrEmpty(Value.ToString().Trim())
                || string.IsNullOrEmpty(valueCompare.ToString().Trim())) return true;
            return Convert.ToDouble(Value) >= Convert.ToDouble(valueCompare);
        }
    }

    /// <summary>
    /// Validate giá trị lớn hơn kiểu DateTime
    /// </summary>
    public class ValidatorDateGreaterAttribute : ValidatorGreaterBaseAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldName"></param>
        public ValidatorDateGreaterAttribute(string fieldName) : base(fieldName) { }

        /// <summary>
        /// Thực hiện so sánh
        /// </summary>
        /// <param name="valueCompare"></param>
        /// <returns></returns>
        protected override bool IsValueGreater(object valueCompare)
        {
            if (Value == null || string.IsNullOrEmpty(Value.ToString().Trim())) return true;
            if (valueCompare == null || string.IsNullOrEmpty(valueCompare.ToString().Trim())) return true;

            var to = Convert.ToDateTime(Value);
            if (to == DateTime.MinValue) return true;

            var from = Convert.ToDateTime(valueCompare);
            if (from == DateTime.MinValue) return true;

            return to >= from;
        }
    }
}
