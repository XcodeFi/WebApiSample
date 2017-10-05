using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using ShCore.Utility;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// Lớp truy xuất cơ sở dữ liệu
    /// </summary>
    public abstract partial class MainDBBase : IDisposable
    {
        #region Các phương thức thao tác với cơ sở dữ liệu
        /// <summary>
        /// Thực hiện một câu lệnh sql none query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteSqlNoneQuery(string sql, Param param = null)
        {
            return ExecuteNoneQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Thực hiện một thủ tục none query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteStoreNoneQuery(string sql, Param param = null)
        {
            return ExecuteNoneQuery(CommandType.StoredProcedure, sql, param);
        }

        /// <summary>
        /// Thực hiện ExecuteStoreNoneQuery nhưng có tham số parameter là Output
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<int, Param> ExecuteStoreNoneQuery(string sql, Param paramInputs, Param paramOutputs)
        {
            return Execute(sql, paramInputs, paramOutputs, (cmd) => ShExeNoneQuery(cmd));
        }

        /// <summary>
        /// Thực hiện ExecuteStoreToTable có parameter output
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<DataTable, Param> ExecuteStoreToTable(string sql, Param paramInputs, Param paramOutputs)
        {
            return Execute(sql, paramInputs, paramOutputs, (cmd) =>
            {
                // Lấy ra DataSet
                var ds = CmdFillDataSet(cmd);

                // return DataTable
                return ds.Tables.Count == 0 ? new DataTable() : ds.Tables[0];
            });
        }

        /// <summary>
        /// Thực hiện câu lệnh thủ tục xử lý trên DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<int, Param> ExecuteToReader(string sql, Param paramInputs, Param paramOutputs, Action<IDataReader> action)
        {
            // Return
            return Execute(sql, paramInputs, paramOutputs, (cmd) =>
            {
                // ExecuteReader
                var reader = ShExeReader(cmd);

                // Biến đếm số bản ghi
                int i = 0;

                // Thực hiện đọc từ Reader
                while (reader.Read())
                {
                    // Thực hiện một action với bản ghi đang đọc
                    action(reader);

                    // cộng thêm 1
                    i++;
                }

                // return 
                return i;
            });
        }

        /// <summary>
        /// Thực hiện một command none query
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <returns></returns>
        private int ExecuteNoneQuery(CommandType type, string sql, Param paramInputs, Param paramOutputs = null)
        {
            return Execute(sql, paramInputs, paramOutputs, cmd => ShExeNoneQuery(cmd), type).T1;
        }

        /// <summary>
        /// Thực hiện một câu lệnh sql trả ra DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteSqlToTable(string sql, Param param = null)
        {
            var ds = ExecuteSql(sql, param);
            return ds.Tables.Count == 0 ? new DataTable() : ds.Tables[0];
        }

        /// <summary>
        /// Thực hiện một thủ tục trả ra DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteStoreToTable(string sql, Param param = null)
        {
            var ds = ExecuteStore(sql, param);
            return ds.Tables.Count == 0 ? new DataTable() : ds.Tables[0];
        }

        /// <summary>
        /// Thực hiện một câu lệnh sql trả ra DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet ExecuteSql(string sql, Param param = null)
        {
            return Execute(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Thực hiện một thủ tục trả ra DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet ExecuteStore(string sql, Param param = null)
        {
            return Execute(CommandType.StoredProcedure, sql, param);
        }

        /// <summary>
        /// BatchUpdate. Chỉ sử dụng với mục đích để đẩy dữ liệu vào Db với một câu lệnh thủ tục
        /// </summary>
        /// <param name="table"></param>
        /// <param name="store"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public int BatchUpdate(DataTable table, string store, string[] @params)
        {
            // Tạo DataAdapter
            DbDataAdapter dap = Factory.CreateDataAdapter();

            connection = this.Db.CreateConnection();

            // Tạo Command Insert, Update
            var cmdInsert = connection.CreateCommand();
            var cmdUpdate = connection.CreateCommand();

            // Parameter
            IDbDataParameter pa = null;

            // Tạo Parameter
            @params.ToList().ForEach(
                p =>
                {
                    pa = cmdInsert.CreateParameter(); pa.SourceColumn = pa.ParameterName = p; cmdInsert.Parameters.Add(pa);
                    pa = cmdUpdate.CreateParameter(); pa.SourceColumn = pa.ParameterName = p; cmdUpdate.Parameters.Add(pa);
                });

            dap.UpdateCommand = cmdUpdate;
            dap.UpdateCommand.CommandText = store;
            dap.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            dap.UpdateCommand.CommandType = CommandType.StoredProcedure;

            dap.InsertCommand = cmdInsert;
            dap.InsertCommand.CommandText = store;
            dap.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            dap.InsertCommand.CommandType = CommandType.StoredProcedure;

            dap.MissingMappingAction = MissingMappingAction.Passthrough;
            dap.MissingSchemaAction = MissingSchemaAction.Ignore;
            dap.ContinueUpdateOnError = true;

            // dap.UpdateBatchSize = 3000;
            return dap.Update(table);
        }
        #endregion
    }
}
