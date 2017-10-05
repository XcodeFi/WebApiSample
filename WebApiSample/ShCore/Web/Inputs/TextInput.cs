using System.Web.UI.WebControls;
namespace ShCore.Web.Inputs
{
    /// <summary>
    /// TextInput
    /// </summary>
    public class TextInput : TextBox, IInput
    {
        private bool hasTrim = true;
        /// <summary>
        /// 
        /// </summary>
        public bool HasTrim
        {
            get { return hasTrim; }
            set { hasTrim = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual object GetValue()
        {
            return HasTrim ? Text.Trim() : Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetValue(object value)
        {
            Text = value == null ? string.Empty : value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string FieldName
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() { Text = string.Empty; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnabled(bool enable)
        {
            base.Enabled = enable;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PlaceHolder
        {
            set { Attributes["PlaceHolder"] = value; }
            get { return Attributes["PlaceHolder"]; }
        }
    }
}
