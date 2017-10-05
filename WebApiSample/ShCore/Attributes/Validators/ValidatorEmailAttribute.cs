using System.Text.RegularExpressions;
using ShCore.Extensions;
namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// 
    /// </summary>
    public  class ValidatorEmailAttribute : ValidatorAttribute
    {
        private static Regex regex = new Regex(@"^[a-z0-9,!#\$%&'\*\+/=\?\^_`\{\|}~-]+(\.[a-z0-9,!#\$%&'\*\+/=\?\^_`\{\|}~-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,})$");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if(this.Value == null) return false;
            return regex.Match(this.Value.ToString()).Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return "{0} không đúng định dạng Email".Frmat(this.FieldName);
        }
    }
}
