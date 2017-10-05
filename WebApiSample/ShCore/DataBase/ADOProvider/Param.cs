using ShCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Data;
using System;
using System.Linq.Expressions;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// Chứa List Parameter
    /// </summary>
    public class Param
    {
        /// <summary>
        /// Danh sách Parameter
        /// </summary>
        private List<ParamInfoItem> @params = new List<ParamInfoItem>();

        /// <summary>
        /// Danh sách Parameter
        /// </summary>
        public List<ParamInfoItem> Items { get { return @params; } }

        /// <summary>
        /// Thêm Parameter theo Indexer
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name, string type = "", int? size = null]
        {
            set
            {
                @params.Add(new ParamInfoItem { Name = name, Size = size, Type = type, Value = value });
            }
            get
            {
                // Lấy ra param theo key
                var item = @params.FirstOrDefault(p => p.Name == name);

                // return
                return item.IsNull() ? null : item.Value;
            }
        }

        /// <summary>
        /// Đưa vào Command
        /// </summary>
        /// <param name="cmd"></param>
        public void ToCommand(DbCommand cmd, ParameterDirection direction = ParameterDirection.Input)
        {
            // Duyệt qua từng ParamItem
            this.Items.ForEach(p =>
            {
                // Tạo Parameter
                DbParameter pa = cmd.CreateParameter();

                // Tên
                pa.ParameterName = p.Name;

                // Nếu có kích thước của tham số
                if (p.Size.IsNotNull()) pa.Size = p.Size.Value;

                // Các trường hợp dữ liệu đặc biệt được thiết lập ở đây
                else switch (p.Type)
                    {
                        case "datetime": pa.DbType = DbType.DateTime; break;
                    }

                // Giá trị
                pa.Value = p.Value ?? DBNull.Value;

                // Direction
                pa.Direction = direction;

                // Đưa vào Command
                cmd.Parameters.Add(pa);
            });
        }

        /// <summary>
        /// Convert ra Dictionary<string, object>
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ConvertToDictionary()
        {
            // Khởi tạo dic để lưu trữ 
            var dic = new Dictionary<string, object>();

            // Tạo các giá trị cho dic
            @params.ForEach(p => dic[p.Name] = p.Value);

            // return
            return dic;
        }

        /// <summary>
        /// Thực hiện lấy Parameter từ LambdaExpression
        /// </summary>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        public static Param GetParam(LambdaExpression propertyLamda)
        {
            // Khởi tạo Param để return
            var sp = new Param();

            // Lấy dữ liệu từ biểu thức 
            var dic = propertyLamda.GetValues();

            // Thực hiện tạo Param
            dic.ToList().ForEach(d => sp[d.Key] = d.Value);

            // Trả ra
            return sp;
        }

        public void Join(Param param)
        {
            this.Items.SJoin(param.Items, t1 => t1.Name.ToLower(), t2 => t2.Name.ToLower(), (t1, t2) => t1.Value = t2.Value);
        }
    }

    /// <summary>
    /// Thông tin một Param
    /// </summary>
    public class ParamInfoItem
    {
        private string name;
        /// <summary>
        /// Tên Param
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int? size;
        /// <summary>
        /// Kích thước của Param
        /// </summary>
        public int? Size
        {
            get { return size; }
            set { size = value; }
        }

        private string type;
        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private object value;
        /// <summary>
        /// Giá trị
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
