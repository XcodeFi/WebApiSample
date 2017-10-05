using ShCore.Extensions;
using System;
namespace ShCore.Attributes.Validators
{
    public class ValidatorIsDateAttribute : ValidatorAttribute
    {
        /// <summary>
        /// Xem có đúng định dạng ngày hay không
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            try
            {
                if (this.Value != null) this.Value.To<DateTime>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string GetMessage()
        {
            return "{0} không đúng định dạng ngày".Frmat(this.FieldName);
        }
    }
}
