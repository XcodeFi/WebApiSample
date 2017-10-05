using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// Validator chỉ cho phép nhập số và chữ cái.
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// trungtq  14/09/2015   created
    /// </Modified>
    public class ValidatorOnlyNumberAndCharacter : ValidatorRegex
    {
        public ValidatorOnlyNumberAndCharacter()
            : base(@"^[a-zA-Z0-9]*$")
        {
        }

        public override string GetMessage()
        {
            return this.FieldName + " phải nhập dạng số hoặc chữ cái!";
        }
    }
}
