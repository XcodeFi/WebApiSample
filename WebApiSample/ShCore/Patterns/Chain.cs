namespace ShCore.Patterns
{
    /// <summary>
    /// Pattern Chain Generic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Chain<T>
    {
        private T handler;
        /// <summary>
        /// Xử lý
        /// </summary>
        public T Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        /// <summary>
        /// Thiết lập Handler 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        public void SetHandler<THandler>() where THandler : T, new()
        {
            // Khởi tạo Handler xử lý
            var handler = new THandler();

            // Nếu xử lý hiện thời chưa có thì gán
            if (this.handler == null) this.handler = handler;

            // Nếu có rồi thì gán cho hàng kế tiếp
            else (this.handler as Chain<T>).Handler = handler;
        }
    }
}
