namespace ShCore.Attributes.Validators
{
    public class ValidatorDenyValue : ValidatorAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public object ValueDeny { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (ValueDeny == null || this.Value == null) return true;

            return !this.Value.Equals(ValueDeny);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            if (this.FieldName == "Giá vé thường")
            {
                return "Vui lòng nhập " + this.FieldName;
            }
            else
            {
                return "Vui lòng chọn " + this.FieldName;
            }
        }
    }
}
