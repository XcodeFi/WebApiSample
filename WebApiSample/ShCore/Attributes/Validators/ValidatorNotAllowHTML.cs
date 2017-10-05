namespace ShCore.Attributes.Validators
{
    public class ValidatorNotAllowHTML : ValidatorAttribute
    {
        private int i = 0;

        public override bool Validate()
        {
            if (this.Value == null || this.Value.ToString().Equals(string.Empty)) return true;
                
            if (this.Value.ToString().Contains("<") || this.Value.ToString().Contains(">"))
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
                return this.FieldName + " không được nhập ký tự HTML  ";
        }
    }
}
