using System.Web.UI;
using System.Text;
using System.IO;
using ShCore.Attributes.Validators;
using System.Data;
using System.Collections.Generic;

using System.Linq;
using ShCore.Extensions;
namespace ShCore.Web.Extensions
{
    public static class ControlExtension
    {
        /// <summary>
        /// Render một Control thành String
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetHtml(this Control ctrl)
        {
            var sb = new StringBuilder();
            using (var tw = new StringWriter(sb))
            {
                using (var hw = new HtmlTextWriter(tw))
                    ctrl.RenderControl(hw);

                return sb.ToString();
            }
        }

        public static TControl Find<TControl>(this Control control, string id) where TControl : Control
        {
            return control.FindControl(id).As<TControl>();
        }

        /// <summary>
        /// Thực hiện Bind dữ liệu của các input
        /// </summary>
        /// <param name="control"></param>
        public static void InputBinding(this Control control)
        {
            control.FindAllChildrenByType<IOnLoad>().Select(i => { i.BindOnLoad(); return true; }).Count();
        }

        /// <summary>
        /// Lấy ra tất cả Controls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindAllChildrenByType<T>(this Control control)
        {
            //
            var controls = control.Controls.Cast<Control>();

            //
            var enumerable = controls as Control[] ?? controls.ToArray();
            return enumerable.OfType<T>().Concat(enumerable.SelectMany(ctrl => ctrl.FindAllChildrenByType<T>()));
        }

        /// <summary>
        /// Thực hiện tìm các IShInput
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<IInput> FindIInputs(this Control control)
        {
            return control.FindAllChildrenByType<IInput>().Where(c => !string.IsNullOrEmpty(c.FieldName)).ToList();
        }

        /// <summary>
        /// Tìm IControl
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<IControl> FindIControls(this Control control)
        {
            return control.FindAllChildrenByType<IControl>().Where(c => !string.IsNullOrEmpty(c.FieldName)).ToList();
        }

        /// <summary>
        /// Thực hiện điền tất cả nội dung của obj vào control chứa các IInput
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        public static void FillForm(this Control control, object obj)
        {
            control.FindIControls().ForEach(i => i.SetValue(DataBinder.Eval(obj, i.FieldName)));
        }

        /// <summary>
        /// FillForm từ DataRow
        /// </summary>
        /// <param name="control"></param>
        /// <param name="dr"></param>
        public static void FillForm(this Control control, DataRow dr)
        {
            control.FillForm(dr.Table.DefaultView[dr.Table.Rows.IndexOf(dr)]);
        }

        /// <summary>
        /// Thực hiện điền tất cả nội dung của obj vào control chứa các IInput
        /// </summary>
        /// <param name="control"></param>
        /// <param name="values"></param>
        public static void FillForm(this Control control, Dictionary<string, object> values)
        {
            // lấy ra các IInput
            var inputs = control.FindIInputs();

            // Điền dữ liệu lên Form
            foreach (var input in inputs)
            {
                if (values.ContainsKey(input.FieldName))
                    input.SetValue(values[input.FieldName]);
            }
        }

        /// <summary>
        /// Thực hiện Clear
        /// </summary>
        /// <param name="control"></param>
        public static void ClearForm(this Control control)
        {
            control.FindIInputs().ForEach(i => i.Clear());
        }

        /// <summary>
        /// Tìm 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IEnumerable<IInput> FindInputByFieldName(this Control control, params string[] fieldNames)
        {
            return control.FindIInputs().Join(fieldNames, c => c.FieldName, f => f, (c, f) => c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetInputValue(this Control control, string fieldName, object value)
        {
            control.FindInputByFieldName(fieldName).ForEach(c => c.SetValue(value));
        }

        /// <summary>
        /// Thực hiện lấy ra đối tượng T với các giá trị từ IInput và control chứa nó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public static T ParseTo<T>(this Control control, bool validate = true) where T : new()
        {
            // 
            var t = new T();

            // 
            control.ParseTo(t, validate);

            // 
            return t;
        }

        /// <summary>
        /// Điền từ dữ liệu một Control vào một đối tượng
        /// </summary>
        /// <param name="control"></param>
        /// <param name="t"></param>
        /// <param name="validate"></param>
        public static void ParseTo(this Control control, object t, bool validate = true)
        {
            // lấy ra các IInput
            var inputs = control.FindIInputs();

            // Lấy ra dictionary với cặp fieldname và giá trị từ input
            var dic = inputs.ToDictionary(i => i.FieldName, i => i.GetValue());



            // Thực hiện validate
            if (validate)
            {
                var result = t.Validate(dic);
                // 
                if (!result.IsValid) throw new ValidatorException(result);
            }

            // 
            t.Parse(dic, false);
        }
    }
}
