using System.Text.RegularExpressions;
using ShCore.Extensions;
namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// Không được phép nhiều khoảng trắng liên tiếp nhau
    /// </summary>
    public class ValidatorNotMultipleWhiteSpaceAttribute : ValidatorAttribute
    {
        /// <summary>
        /// regex kiểm tra có 2 ký tự trắng liên tiếp hay không
        /// </summary>
        private static Regex regex = new Regex("\\s\\s");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (this.Value == null) return true;

            return !regex.Match(this.Value.ToString()).Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return "{0} không được chứa các khoảng trắng liên tiếp".Frmat(this.FieldName);
        }
    }
}
