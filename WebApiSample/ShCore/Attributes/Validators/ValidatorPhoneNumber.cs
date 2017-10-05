namespace ShCore.Attributes.Validators
{
    public class ValidatorPhoneNumber : ValidatorAttribute
    {
        private int i = 0;

        public override bool Validate()
        {
            if (this.Value == null || this.Value.ToString().Equals(string.Empty)) return true;

            var phone = this.Value.ToString();

            var splitChar = ';';
            if (phone.Contains(",")) splitChar = ',';

            foreach (string s in phone.Split(splitChar))
            {
                if (s.Length > 11 || s.Length < 10)
                {
                    i = 1;
                    return false;
                }
                int outNumber = 0;
                string sTG = s.Replace("0", "");

                if (!int.TryParse(sTG, out outNumber))
                {
                    i = 2;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            if (i == 1)
                return this.FieldName + " phải có từ 10 hoặc 11 chữ số  ";
            else
                return this.FieldName + " không được nhập chữ hoặc ký tự lạ ";
        }
    }
}
