using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShCore.Attributes.Validators
{
    public class ValidatorNumeric : ValidatorAttribute
    {

        public override bool Validate()
        {
            return new Regex(@"^\d+$").Match(this.Value.ToString()).Success;
        }

        public override string GetMessage()
        {
            return this.FieldName + " phải là kiểu số!";
        }
    }
}
