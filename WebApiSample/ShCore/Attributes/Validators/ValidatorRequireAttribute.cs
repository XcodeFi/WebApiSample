namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// Validate không được để trống
    /// </summary>
    public class ValidatorRequireAttribute : ValidatorAttribute
    {
        public override bool Validate()
        {
            // null 
            if (this.Value == null) return false;

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
            return string.Format("{0} không được để trống", this.FieldName);
        }
    }
}
