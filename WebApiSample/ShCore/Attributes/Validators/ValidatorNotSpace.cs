namespace ShCore.Attributes.Validators
{
    public class ValidatorNotSpace : ValidatorAttribute
    {
        public override bool Validate()
        {
            if (this.Value == null) return true;
            return !(this.Value.ToString().IndexOf(" ") >= 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return this.FieldName + " không được chứa khoảng trắng";
        }
    }
}
