using ShCore.Extensions;
namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// Validate chiều dài một ký string
    /// </summary>
    public class ValidatorLengthAttribute : ValidatorAttribute
    {
        public int Min { set; get; }
        public int Max { set; get; }

        public override bool Validate()
        {
            // 
            if (this.Value.IsNull() || this.Value.ToString().Length == 0) return true;

            var length = this.Value == null ? 0 : this.Value.ToString().Length;

            var isMinValid = Min == 0 || length >= Min;
            var isMaxValid = Max == 0 || length <= Max;

            return isMaxValid && isMinValid;
        }

        /// <summary>
        /// Thông báo
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return "{0} phải {1} {2} ký tự".Frmat(this.FieldName, this.Min == 0 ? "" : "từ {0}".Frmat(this.Min), this.Max == 0 ? "" : " đến {0}".Frmat(this.Max));
        }
    }
}
