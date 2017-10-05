using System;
namespace ShCore.Attributes.Validators
{
    public class ValidatorDateGreaterToDay : ValidatorAttribute
    {
        public override bool Validate()
        {
            if (this.Value == null) return true;
            if (this.Value is DateTime && Convert.ToDateTime(this.Value).Date < DateTime.Today) return false;
            return true;
        }

        public override string GetMessage()
        {
            return this.FieldName + " phải lớn hơn ngày hiện tại";
        }
    }
}
