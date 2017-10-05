using System.Web.UI.WebControls;
using System.Linq;
using ShCore.Extensions;
using System;
using ShCore.Web.WebBase;
using ShCore.DataBase.ADOProvider;
using System.Collections.Generic;
using ShCore.Utility;
using ShCore.Attributes;
using System.Text;

namespace ShCore.Web.Inputs
{
    /// <summary>
    /// 
    /// </summary>
    public class DropDownListInput : DropDownList, IInput
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual object GetValue()
        {
            //return SelectedIndex > 0 ? null : SelectedValue;
            return SelectedValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetValue(object value)
        {
            SelectedValue = value == null || value.Equals(DBNull.Value) ? string.Empty : value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            SelectedIndex = 0;
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
        /// <param name="enable"></param>
        public void SetEnabled(bool enable)
        {
            Enabled = enable;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChosenInput : DropDownListInput
    {
        /// <summary>
        /// Tìm các giá trị được lựa chọn trong dropdownlist
        /// </summary>
        public string ListSelecteds
        {
            get
            {
                var ss = Page.Request.Form[UniqueID];
                return string.IsNullOrEmpty(ss) ? _valueSeted : string.Join(",", ss.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray());
            }
        }

        public string ListSelectedText
        {
            get
            {
                var selectedIDs = "," + ListSelecteds + ",";
                var selectedTexts = new List<string>();
                foreach(ListItem item in Items)
                {
                    if (selectedIDs.Contains("," + item.Value + ","))
                    {
                        selectedTexts.Add(item.Text);
                    }
                }
                return string.Join(", ", selectedTexts);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Multiple
        {
            get { return base.ViewState["Multiple"].To<bool>(); }
            set { base.ViewState["Multiple"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return Multiple ? ListSelecteds : SelectedValue;
        }

        /// <summary>
        /// Thiết lập giá trị
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object value)
        {
            SetListValue(value.IsNull() ? string.Empty : value.ToString());
        }

        private string _valueSeted = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void SetListValue(string values)
        {
            _valueSeted = values;
            if (!Multiple) base.SetValue(values);
        }

        /// <summary>
        /// Có thiết lập tìm kiếm các giá trị không
        /// </summary>
        public bool DisableSearch
        {
            set { base.ViewState["DisableSearch"] = value; }
            get { return base.ViewState["DisableSearch"].To<bool>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PlaceholderTextMultiple //placeholder_text_multiple
        {
            set { base.ViewState["PlaceholderTextMultiple"] = value; }
            get { return base.ViewState["PlaceholderTextMultiple"].To<string>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowImage { set; get; }

        /// <summary>
        /// Thực hiện Bind Html xuống client
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            // Chon nhieu
            if (Multiple) this.Attributes["multiple"] = "multiple";

            // Tìm các giá trị mặc định
            var values = string.IsNullOrEmpty(_valueSeted) ? Page.Request.Form[UniqueID] : _valueSeted;

            // Bỏ chọn ở tất cả item bằng js
            ResponseMessage.Current.JavaScript = "$('#" + ClientID + " option').attr('selected', false)";

            if (values.IsNull() && !Multiple)
                values = this.SelectedValue;

            // Thực hiện selected các giá trị được thiết lập
            if (!string.IsNullOrEmpty(values))
            {
                values.Split(',').Join(Items.Cast<ListItem>(), v => v, li => li.Value, (v, li) =>
                {
                    ResponseMessage.Current.JavaScript = "$('#" + ClientID + " option[value=\"" + li.Value + "\"]').attr('selected', true)";
                    return false;
                }).Count();
            }

            // Khởi tạo chosen
            ResponseMessage.Current.JavaScript = "$('#" + ClientID + "')." + (AllowImage ? "chosenImage" : "chosen") + "({ search_contains: true, placeholder_text_multiple: '" + (PlaceholderTextMultiple.IsNull() ? "Chọn" : PlaceholderTextMultiple) + "', disable_search: " + (DisableSearch ? "true" : "false") + " })";
            base.OnPreRender(e);
        }
    }

    /// <summary>
    /// Select chọn một đối tượng danh mục
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SelectInput<T> : ChosenInput, IOnLoad where T : ModelBase, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public void RefreshItems()
        {
            var typeT = typeof(T);

            // Tìm xem property nào đóng vai trò là value
            if (this.DataValueField.IsNull())
            {
                var dvf = typeT.GetListPairPropertyAttribute<DataValueFieldAttribute>().FirstOrDefault();
                if (dvf != null) this.DataValueField = dvf.T1.Name;
            }

            // Tìm xem property nào đóng vai trò là text
            if (this.DataTextField.IsNull())
            {
                var dtf = typeT.GetListPairPropertyAttribute<DataTextFieldAttribute>().FirstOrDefault();
                if (dtf != null) this.DataTextField = dtf.T1.Name;
            }

            this.DataSource = ResponseMessage.CacheContext(typeT.FullName + Key, () => GetData());
            this.DataBind();

            if (TextDefault.IsNotNull() || ValueDefault.IsNotNull())
                this.Items.Insert(0, new ListItem { Text = TextDefault, Value = ValueDefault });

            if (Items.Count > 0) Items[0].Selected = true;
        }

        public bool IsSelectedSpecifiedItem()
        {
            if (string.IsNullOrEmpty(ValueDefault)) return true;
            if (string.IsNullOrEmpty(ListSelecteds) || string.IsNullOrEmpty(SelectedValue) || ListSelecteds == "0") return false;
            return !ListSelecteds.StartsWith(ValueDefault);
        }

        protected virtual string Key { get { return string.Empty; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new protected virtual List<T> GetData()
        {
            return Singleton<T>.Inst.GetAll().ToList<T>(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public string TextDefault { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string ValueDefault { set; get; }

        /// <summary>
        /// Thuc hien bind du lieu khi tai trang
        /// </summary>
        public void BindOnLoad()
        {
            RefreshItems();
        }
    }

    /// <summary>
    /// SelectEnum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SelectEnumInput<T> : ChosenInput, IOnLoad
    {
        /// <summary>
        /// 
        /// </summary>
        public void BindOnLoad()
        {
            this.DataValueField = "RawValue";
            this.DataTextField = "Name";
            this.DataSource = GetData();
            this.DataBind();

            if (TextDefault.IsNotNull() || ValueDefault.IsNotNull())
                this.Items.Insert(0, new ListItem { Text = TextDefault, Value = ValueDefault });

            if (Items.Count > 0) Items[0].Selected = true;
        }

        public bool IsSelectedSpecifiedItem()
        {
            if (string.IsNullOrEmpty(ValueDefault)) return true;
            if (string.IsNullOrEmpty(ListSelecteds) || string.IsNullOrEmpty(SelectedValue)) return false;
            return !ListSelecteds.StartsWith(ValueDefault);
        }

        /// <summary>
        /// 
        /// </summary>
        public string TextDefault { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string ValueDefault { set; get; }

        /// <summary>
        /// Data
        /// </summary>
        /// <returns></returns>
        new protected virtual List<FieldInfoAttribute> GetData()
        {
            return EnumHelper<T>.Inst.GetAttributes<FieldInfoAttribute>();
        }
    }

    /// <summary>
    /// class quy định đâu là DataVlueField
    /// </summary>
    public class DataValueFieldAttribute : Attribute { }

    /// <summary>
    /// class quy định đâu là DataTextField
    /// </summary>
    public class DataTextFieldAttribute : Attribute { }
}
