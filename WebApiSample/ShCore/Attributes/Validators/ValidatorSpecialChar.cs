using System.Linq;
namespace ShCore.Attributes.Validators
{
    public class ValidatorSpecialChar : ValidatorAttribute
    {
        private static string specials = "!@#$%^&*()";

        /// <summary>
        /// Kiểm tra xem có chứa ký tự đặc biệt không
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return !(this.Value != null && this.Value.ToString().Join(specials, v => v, c => c, (v, c) => true).Count() != 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return this.FieldName + " không được chứa ký tự đặc biệt";
        }
    }
}
