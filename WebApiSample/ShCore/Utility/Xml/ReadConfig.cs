using System.Xml.Serialization;
using System.IO;
using System.Xml;
namespace ShCore.Utility.Xml
{
    /// <summary>
    /// Đọc file Config và đưa dữ liệu đọc được vào một biến kiểu dữ liệu là T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadConfig<T> where T : ConfigBase, new()
    {
        /// <summary>
        /// Đóng Constructor để cho khởi tạo một Instance duy nhất
        /// </summary>
        private ReadConfig() { }

        private static ReadConfig<T> inst = new ReadConfig<T>();
        /// <summary>
        /// Khởi tạo một instance duy nhất để sử dụng
        /// </summary>
        public static ReadConfig<T> Inst
        {
            get { return inst; }            
        }

        /// <summary>
        /// Đối tượng để Serializer hoặc Deserializer đối tượng ra file hoặc ngược lại từ file ra đối tượng
        /// </summary>
        private XmlSerializer xmlData = new XmlSerializer(typeof(T));

        /// <summary>
        /// Thực hiện đọc file setting và đưa thông tin vào đối tượng
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            // Khởi tạo đối tượng T
            T t = new T();

            // Lấy đường dẫn chưa file Settings
            string settingsPath = t.GetPath();

            // Nếu đường dẫn đến file setting không tồn tại thì trả ra đối tượng rỗng
            if (!File.Exists(settingsPath)) return t;

            // Đối tượng để đọc file Xml
            XmlTextReader xmlReader = null;
            try
            {
                // Khởi tạo đối tượng đọc file setting
                // Tham số của constructor là đường dẫn file cần độc
                xmlReader = new XmlTextReader(settingsPath);

                // Đọc file và Deserialize ra đối tượng
                t = (T)xmlData.Deserialize(xmlReader);
            }
            finally
            {
                // Đóng lại reader mà dùng để đọc file xml setting
                if (xmlReader != null) xmlReader.Close();
            }

            // return 
            return t;
        }
    }
}
