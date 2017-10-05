using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShCore.Attributes.Validators
{
    /// <summary>
    /// Validate sử dụng regex
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// trungtq  14/09/2015   created
    /// </Modified>
    public class ValidatorRegex : ValidatorAttribute
    {
        protected string Pattern { get; set; }

        public override bool Validate()
        {
            return Regex.Match(this.Value.ToString(), Pattern).Success;
        }

        public override string GetMessage()
        {
            return this.FieldName + " không đúng định dạng!";
        }

        public ValidatorRegex(string pattern)
        {
            this.Pattern = pattern;
        }
    }
}
