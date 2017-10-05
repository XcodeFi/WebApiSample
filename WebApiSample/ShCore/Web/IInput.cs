namespace ShCore.Web
{
    /// <summary>
    /// IInput
    /// </summary>
    public interface IInput : IControl
    {
        /// <summary>
        /// Thực hiện Clear
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>
        void Focus();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        void SetEnabled(bool enable);
    }

    /// <summary>
    /// Định nghĩa thông tin về ảnh
    /// </summary>
    public interface IImageInfo
    {
        string Path { set; get; }
        string Note { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICommand : IControl
    {
        /// <summary>
        /// Thực hiện lệnh
        /// </summary>
        void DoCommand();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetMessage();
    }

    public interface IControl
    {
        /// <summary>
        /// Lấy ra giá trị của Input
        /// </summary>
        /// <returns></returns>
        object GetValue();

        /// <summary>
        /// Thiết lập giá trị của Input
        /// </summary>
        /// <param name="value"></param>
        void SetValue(object value);

        /// <summary>
        /// FieldName
        /// </summary>
        string FieldName { set; get; }
    }
}
