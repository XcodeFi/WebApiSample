using System;
using System.Globalization;
using ShCore.Web.WebBase;

namespace ShCore.Web.Inputs
{
    /// <summary>
    /// TextBox nhập định dạng tiền
    /// </summary>
    public class MoneyInput : TextInput
    {
        /// <summary>
        /// Pre render
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Thực hiện gọi hàm js tạo Mask             
            ResponseMessage.Current.JavaScript = "$('#" + ClientID + "').maskMoney({ allowZero: false, allowNegative: false, defaultZero: false, thousands: '" + Bp + "', precision: " + Precision + " })";
        }

        public string Precision
        {
            set { base.ViewState["Precision"] = value; }
            get { return base.ViewState["Precision"] == null ? "0" : base.ViewState["Precision"].ToString(); }
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
                    base.SetValue(string.Format(new CultureInfo("en-US"), "{0:#,##0}", Convert.ToDecimal(value)));
                }
                catch
                {
                    base.SetValue("");
                }
            }
        }
    }
}
