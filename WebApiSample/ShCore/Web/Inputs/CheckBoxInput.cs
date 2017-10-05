using System;
using System.Web.UI.WebControls;
namespace ShCore.Web.Inputs
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckBoxInput : CheckBox, IInput
    {
        public CheckBoxInput()
        {
            CssClass = "chk";
        }

        public object GetValue()
        {
            return Checked;
        }

        public void SetValue(object value)
        {
            if (value == null || value.Equals(DBNull.Value)) Checked = false;
            else Checked = (bool)value;
        }

        public string FieldName
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            Checked = false;
        }


        public void SetEnabled(bool enable)
        {
            Enabled = enable;
        }
    }
}
