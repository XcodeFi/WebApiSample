using System.Data.Common;
using System.Data;
using System;
using ShCore.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ShCore.Utility;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// MainDBBase
    /// </summary>
    public partial class MainDBBase
    {
        #region Properties
        DbConnection connection = null;

        /// <summary>
        /// Factory để tạo provider
        /// </summary>
        DbProviderFactory factory = null;

        /// <summary>
        /// Transaction
        /// </summary>
        DbTransaction transaction = null;

        /// <summary>
        /// Cờ để check lỗi
        /// </summary>
        private bool isError = false;

        /// <summary>
        /// Thiết lập Error
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set { isError = value; }
        }

        /// <summary>
        /// Db 
        /// </summary>
        protected abstract Database Db { get; }
        #endregion

        #region private method
        /// <summary>
        /// Tạo Command 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(CommandType type, string sql, Param paramInputs, Param paramOutputs = null)
        {
            // Khởi tạo commander
            DbCommand cmd = Factory.CreateCommand();

            // CommandType
            cmd.CommandType = type;

            // Sql Command
            cmd.CommandText = sql;

            // Khởi tạo các tham số để thực hiện truy vấn                
            // Thực hiện Tạo Parameter Input
            if (paramInputs.IsNotNull()) paramInputs.ToCommand(cmd);

            // Thực hiện Tạo Parameter Ouput
            if (paramOutputs.IsNotNull()) paramOutputs.ToCommand(cmd, ParameterDirection.Output);

            // return cmd
            return cmd;
        }

        /// <summary>
        /// Factory
        /// </summary>
        private DbProviderFactory Factory
        {
            get
            {
                // Khởi tạo Factory nếu chưa có
                if (factory.IsNull()) factory = Db.DbProviderFactory;

                // return
                return factory;
            }
        }

        /// <summary>
        /// ExecuteStore có parameter là output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private Pair<T, Param> Execute<T>(string sql, Param paramInputs, Param paramOutputs, Func<DbCommand, T> func, CommandType type = CommandType.StoredProcedure)
        {
            // biến lưu kết quả trả ra
            var keyValue = new Pair<T, Param>();

            DbCommand cmd = null;

            try
            {
                // Tạo Command
                cmd = this.CreateCommand(type, sql, paramInputs, paramOutputs);

                // Thực thi câu lệnh cmd để trả ra kết quả
                keyValue.T1 = func(cmd);

                // Lấy ra giá trị output
                if (paramOutputs.IsNotNull()) paramOutputs.Items.ForEach(p => p.Value = cmd.Parameters[p.Name].Value);

                // ParamOutput
                keyValue.T2 = paramOutputs;
            }
            catch (Exception ex)
            {
                // Xử lý Exception nếu có
                DoException(ex);
            }
            finally
            {
                if (cmd.IsNotNull()) cmd.Dispose();
            }

            // return kết quả
            return keyValue;
        }

        /// <summary>
        /// Thực hiện một câu lệnh Command trả tra DataSet
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <returns></returns>
        private DataSet Execute(CommandType type, string sql, Param paramInputs)
        {
            return Execute(sql, paramInputs, null, cmd => CmdFillDataSet(cmd), type).T1;
        }

        /// <summary>
        /// Fill Data Set từ Command
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private DataSet CmdFillDataSet(DbCommand cmd)
        {
            // Khởi tạo DataSet
            DataSet ds = new DataSet();

            // Nếu có transaction
            if (transaction.IsNotNull()) this.Db.LoadDataSet(cmd, ds, new string[] { "Sh" }, transaction);

            // Nếu không transaction
            else this.Db.LoadDataSet(cmd, ds, "Sh");

            // return ds
            return ds;
        }

        /// <summary>
        /// Viết riêng cho việc thực hiện câu lệnh không trả ra result set
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int ShExeNoneQuery(DbCommand cmd)
        {
            // Nếu có transaction
            if (transaction.IsNotNull()) return this.Db.ExecuteNonQuery(cmd, transaction);

            // Nếu không transaction
            else return this.Db.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// ShExeReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private IDataReader ShExeReader(DbCommand cmd)
        {
            // Nếu có transaction
            if (transaction.IsNotNull()) return this.Db.ExecuteReader(cmd, transaction);

            // Nếu không có transaction
            else return this.Db.ExecuteReader(cmd);
        }

        /// <summary>
        /// Thực hiện xử lý Exception
        /// </summary>
        /// <param name="ex"></param>
        private void DoException(Exception ex)
        {
            // Cờ thông báo là lỗi
            // isError = true;

            // Thực hiện Dispose luôn
            // this.Dispose();

            // Bắn ra Exception
            throw ex;
        }
        #endregion

        #region Other Method public
        /// <summary>
        /// Phương thức để bắt đầu transacion
        /// </summary>
        /// <param name="ill"></param>
        public void BeginTransaction(IsolationLevel ill = IsolationLevel.Unspecified)
        {
            // Khởi tạo Connection
            connection = Db.CreateConnection();

            // Mở connection
            connection.Open();

            // Begin transaction
            transaction = connection.BeginTransaction(ill);
        }

        /// <summary>
        /// Phương thức Dispose
        /// </summary>
        public void Dispose()
        {
            // Kết thúc transaction 
            if (transaction.IsNotNull())
            {
                try
                {
                    // Nếu lỗi thì Rollback
                    if (isError) transaction.Rollback();

                    // Không thì Commit
                    else transaction.Commit();
                }
                catch { }
                // Dispose transaction
                finally { transaction.Dispose(); }
            }

            // Đóng connection
            if (connection.IsNotNull())
            {
                // Đóng Connection
                try { connection.Close(); }
                catch { }
                // Dispose Connection
                finally { connection.Dispose(); }
            }
        }
        #endregion
    }
}
