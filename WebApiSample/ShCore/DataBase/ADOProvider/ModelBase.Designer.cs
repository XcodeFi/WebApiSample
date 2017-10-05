using ShCore.DataBase.ADOProvider.ShSqlCommand;
using System.Data;
using System;
using ShCore.Extensions;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// Mở rộng thêm phương thức cho ModelBase
    /// </summary>
    public partial class ModelBase
    {
        private Type type = null;
        /// <summary>
        /// Type của đối tượng đang thực hiện
        /// </summary>
        protected Type Type
        {
            get
            {
                // Nếu chưa lấy Type lần nào thì lấy
                if (type.IsNull()) type = this.GetType();

                // return
                return type;
            }
        }

        /// <summary>
        /// Thực hiện cập nhật nội dung xuống cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        public virtual void Save()
        {
            this.ExeSaveQuery<ShSaveCommand>();
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <returns></returns>
        public virtual void Insert()
        {
            this.ExeSaveQuery<ShInsertCommand>();
        }

        /// <summary>
        /// Chỉnh sửa
        /// </summary>
        /// <returns></returns>
        public virtual void Update(params string[] fields)
        {
            this.ExeSaveQuery<ShUpdateCommand>(fields);
        }

        /// <summary>
        /// Xóa theo key
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            return this.ExeNoneQuery<ShDeleteCommand>();
        }

        /// <summary>
        /// Lấy theo khóa
        /// </summary>
        /// <returns></returns>
        public virtual bool GetByKey()
        {
            // Lấy ra theo Key
            var table = ExeQuery<ShGetByKeyCommand>();

            // Điền dữ liệu vào Model
            if (table != null && table.Rows.Count != 0) this.ParseFrom(table.Rows[0]);

            // trả ra cho biết là có dữ liệu với Key
            return table != null && table.Rows.Count != 0;
        }

        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetAll()
        {
            return ExeQuery<ShGetAllCommand>();
        }

        private TSqlBuilder builder = null;
        /// <summary>
        /// Lấy ra Builder xây dựng câu lệnh Sql cơ bản
        /// </summary>        
        protected TSqlBuilder Builder
        {
            get
            {
                // Kiểm tra xem Builder đã được khởi tạo chưa
                if (builder == null) builder = new TSqlBuilder(this.Type);

                // Trả ra Builder cần sử dụng
                return builder;
            }
        }

        /// <summary>
        /// Thực hiện câu lệnh Select
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns></returns>
        private DataTable ExeQuery<TCommand>() where TCommand : ShSelectCommand, new()
        {
            // Build câu lệnh
            var cmd = Builder.BuildCommand<TCommand>(this);

            // Thực thi
            return this.RunCommandToDataTable(cmd);
        }

        /// <summary>
        /// Thực hiện thực thi một câu lệnh NoneQuery
        /// </summary>        
        /// <typeparam name="TCommand"></typeparam>        
        /// <returns></returns>
        private int ExeNoneQuery<TCommand>() where TCommand : ShCommand, new()
        {
            // Build câu lệnh            
            var cmd = Builder.BuildCommand<TCommand>(this);

            // Thực thi câu lệnh
            return this.RunCommand(cmd);
        }

        /// <summary>
        /// Thực hiện thực thi một câu lệnh Cập nhật
        /// </summary>        
        /// <typeparam name="TCommand"></typeparam>        
        /// <returns></returns>
        private void ExeSaveQuery<TCommand>(params string[] fields) where TCommand : ShCommand, new()
        {
            // Build câu lệnh            
            var cmd = Builder.BuildCommand<TCommand>(this, fields);

            // Thực thi câu lệnh
            var table = this.RunCommandToDataTable(cmd);

            // Điền dữ liệu vào model
            if (table.Rows.Count != 0) this.ParseFrom(table.Rows[0]);
        }

        /// <summary>
        /// Thực thi câu lệnh bắn ra DataTable
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private DataTable RunCommandToDataTable(ShCommand cmd)
        {
            if (cmd == null) return null;

            // Thực thi
            return this.DataBaseService.ExeSql(cmd.Command, cmd.Parameter);
        }

        /// <summary>
        /// Thực thi câu lệnh None Query
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int RunCommand(ShCommand cmd)
        {
            if (cmd == null) return -1;

            // Thực thi
            return this.DataBaseService.ExeSqlNoneQuery(cmd.Command, cmd.Parameter);
        }
    }
}
