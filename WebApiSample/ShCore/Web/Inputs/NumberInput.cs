using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.Adapters;
using ShCore.Web.WebBase;

namespace ShCore.Web.Inputs
{
    public class NumberInput : TextInput, IInput
    {
        public NumberInput()
        {
           // this.Attributes.Add("OnKeyPress", "return NumberInput.NumberFilter()");
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Thực hiện gọi hàm js tạo Mask             
            ResponseMessage.Current.JavaScript = "NumberInput.NumberFilterFloat('#" + ClientID + "')";
        }
        protected string Bp
        {
            get
            {
                return ",";
            }
        }

        /// <summary>
        /// Viết lại hàm GetValue
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return base.GetValue().ToString().Replace(Bp, string.Empty);
        }

        /// <summary>
        /// Viết lại hàm SetValue
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) base.SetValue("");

            else
            {
                try
                {
                    base.SetValue(string.Format(new CultureInfo("en-US"), "{0:#,##0.##}", Convert.ToDecimal(value)));
                }
                catch
                {
                    base.SetValue("0");
                }
            }
        }

    }
}
