using System;
using System.Data;
using System.Linq.Expressions;
using ShCore.Utility;
using ShCore.Extensions;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// Class trung gian để gọi thủ tục với mọi loại MainDBBase
    /// Việc này sẽ giúp tránh phải sử dụng câu lệnh using một cách nhàm chán
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataBaseService<T> : IDataBaseService where T : MainDBBase, new()
    {
        /// <summary>
        /// DbType đang sử dụng
        /// </summary>
        public Type DbType { get { return typeof(T); } }

        /// <summary>
        /// Một mainDb được sử dụng nếu có transaction
        /// </summary>
        private T dbMain = null;

        /// <summary>
        /// Bắt đầu một Transaction
        /// </summary>
        public void BeginTransaction()
        {
            // Khởi tạo một Connection
            dbMain = new T();

            // Bắt đầu một transaction
            dbMain.BeginTransaction();
        }

        /// <summary>
        /// Dispose sẽ thực hiện hủy Connection
        /// </summary>
        public void Dispose()
        {
            // Nếu khác null thì Dispose
            if (dbMain != null)
            {
                // Dispose
                dbMain.Dispose();

                // Clear
                dbMain = null;
            }
        }

        /// <summary>
        /// Thực hiện lựa chọn thực thi Db nào
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        private TResult DoWithDb<TResult>(Func<T, TResult> func)
        {
            // Thực thi trong transaction nếu có
            if (dbMain != null) return func(dbMain);

            // Khởi tạo một Connection và thực thi
            using (T db = new T()) return func(db);
        }

        #region Execute Cmd Store
        /// <summary>
        /// Truy vấn thủ tục trả ra DataTable
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExeStore(string storeName, Param param = null)
        {
            return DoWithDb(db => db.ExecuteStoreToTable(storeName, param));
        }

        /// <summary>
        /// Thực thi thủ tục
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExeStoreNoneQuery(string storeName, Param param = null)
        {
            return DoWithDb(db => db.ExecuteStoreNoneQuery(storeName, param));
        }

        /// <summary>
        /// Thực thi thủ tục trả ra DataSet
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet ExeStoreToDataSet(string storeName, Param param = null)
        {
            return DoWithDb(db => db.ExecuteStore(storeName, param));
        }

        /// <summary>
        /// Thực thi thủ tục với việc tạo Param từ Lamda, trả ra DataTable
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        public DataTable ExeStore(string storeName, Expression<Action> propertyLamda)
        {
            return ExeStore(storeName, Param.GetParam(propertyLamda));
        }

        /// <summary>
        /// Thực thi thủ tục tạo Param bằng Lamda
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        public int ExeStoreNoneQuery(string storeName, Expression<Action> propertyLamda)
        {
            return ExeStoreNoneQuery(storeName, Param.GetParam(propertyLamda));
        }

        /// <summary>
        /// Thực thi thủ tục tạo Param bằng Lamda tả ra DataSet
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        public DataSet ExeStoreToDataSet(string storeName, Expression<Action> propertyLamda)
        {
            return ExeStoreToDataSet(storeName, Param.GetParam(propertyLamda));
        }

        /// <summary>
        /// Thực thi có trả ra Output Param
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<int, Param> ExeStoreNoneQuery(string sql, Param paramInputs, Param paramOutputs)
        {
            return DoWithDb(db => db.ExecuteStoreNoneQuery(sql, paramInputs, paramOutputs));
        }

        /// <summary>
        /// Thực thi có trả ra Output Param
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<DataTable, Param> ExeStoreToTable(string sql, Param paramInputs, Param paramOutputs)
        {
            return DoWithDb(db => db.ExecuteStoreToTable(sql, paramInputs, paramOutputs));
        }

        /// <summary>
        /// Thực thi với Param tạo bằng Expression
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<int, Param> ExeStoreNoneQuery(string sql, Expression<Action> paramInputs, Expression<Action> paramOutputs)
        {
            return ExeStoreNoneQuery(sql, Param.GetParam(paramInputs), Param.GetParam(paramOutputs));
        }

        /// <summary>
        /// Thực thi với Param tạo bằng Expression
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        public Pair<DataTable, Param> ExeStoreToTable(string sql, Expression<Action> paramInputs, Expression<Action> paramOutputs)
        {
            return ExeStoreToTable(sql, Param.GetParam(paramInputs), Param.GetParam(paramOutputs));
        }

        /// <summary>
        /// BatchUpdate
        /// </summary>
        /// <param name="table"></param>
        /// <param name="storeSave"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public int BatchUpdate(DataTable table, string storeSave, params string[] @params)
        {
            return DoWithDb(db => db.BatchUpdate(table, storeSave, @params));
        }
        #endregion

        #region Cmd Sql

        /// <summary>
        /// Thực thi câu lệnh Sql
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExeSql(string storeName, Param param = null)
        {
            return DoWithDb(db => db.ExecuteSqlToTable(storeName, param));
        }

        /// <summary>
        /// Thực thi câu lệnh Sql
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExeSqlNoneQuery(string storeName, Param param = null)
        {
            return DoWithDb(db => db.ExecuteSqlNoneQuery(storeName, param));
        }

        /// <summary>
        /// Thực thi Sql trả ra DataSet
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet ExeSqlToDataSet(string storeName, Param param = null)
        {
            return DoWithDb(db => db.ExecuteSql(storeName, param));
        }

        /// <summary>
        /// Thực thi câu lệnh Sql với propertyLamda
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        public DataTable ExeSql(string storeName, Expression<Action> propertyLamda)
        {
            return ExeSql(storeName, Param.GetParam(propertyLamda));
        }

        /// <summary>
        /// Thực thi Sql None Query
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        public int ExeSqlNoneQuery(string storeName, Expression<Action> propertyLamda)
        {
            return ExeSqlNoneQuery(storeName, Param.GetParam(propertyLamda));
        }
        #endregion

        /// <summary>
        /// Thực hiện Commit
        /// </summary>
        /// <param name="isDone"></param>
        public void Commit(bool isDone)
        {
            // Nếu khác null thì Dispose
            if (dbMain != null)
            {
                // Có hoàn thành hay không
                dbMain.IsError = !isDone;

                // Dispose
                dbMain.Dispose();

                // Clear
                dbMain = null;
            }
        }
    }

    /// <summary>
    /// Interface giao tiếp với DBBase
    /// </summary>
    public interface IDataBaseService : IDisposable
    {
        /// <summary>
        /// DbType
        /// </summary>
        Type DbType { get; }

        /// <summary>
        /// Thực hiện commit
        /// </summary>
        /// <param name="isDone"></param>
        void Commit(bool isDone);

        /// <summary>
        /// Bắt đầu transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// ExeStore
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataTable ExeStore(string storeName, Param param = null);

        /// <summary>
        /// ExeStore
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        DataTable ExeStore(string storeName, Expression<Action> propertyLamda);

        /// <summary>
        /// ExeStoreNoneQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExeStoreNoneQuery(string storeName, Param param = null);

        /// <summary>
        /// ExeStoreNoneQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        Pair<int, Param> ExeStoreNoneQuery(string sql, Param paramInputs, Param paramOutputs);

        /// <summary>
        /// ExeStoreNoneQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        Pair<int, Param> ExeStoreNoneQuery(string sql, Expression<Action> paramInputs, Expression<Action> paramOutputs);

        /// <summary>
        /// ExeStoreToTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        Pair<DataTable, Param> ExeStoreToTable(string sql, Param paramInputs, Param paramOutputs);

        /// <summary>
        /// ExeStoreToTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramInputs"></param>
        /// <param name="paramOutputs"></param>
        /// <returns></returns>
        Pair<DataTable, Param> ExeStoreToTable(string sql, Expression<Action> paramInputs, Expression<Action> paramOutputs);

        /// <summary>
        /// ExeStoreNoneQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        int ExeStoreNoneQuery(string storeName, Expression<Action> propertyLamda);

        /// <summary>
        /// ExeStoreToDataSet
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataSet ExeStoreToDataSet(string storeName, Param param = null);

        /// <summary>
        /// ExeStoreToDataSet
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        DataSet ExeStoreToDataSet(string storeName, Expression<Action> propertyLamda);

        /// <summary>
        /// ExeSql
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataTable ExeSql(string storeName, Param param = null);

        /// <summary>
        /// ExeSql
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        DataTable ExeSql(string storeName, Expression<Action> propertyLamda);

        /// <summary>
        /// ExeSqlNoneQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExeSqlNoneQuery(string storeName, Param param = null);

        /// <summary>
        /// ExeSqlNoneQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="propertyLamda"></param>
        /// <returns></returns>
        int ExeSqlNoneQuery(string storeName, Expression<Action> propertyLamda);

        /// <summary>
        /// ExeSqlToDataSet
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataSet ExeSqlToDataSet(string storeName, Param param = null);

        /// <summary>
        /// BatchUpdate
        /// </summary>
        /// <param name="table"></param>
        /// <param name="storeSave"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        int BatchUpdate(DataTable table, string storeSave, params string[] @params);
    }

    /// <summary>
    /// Mở rộng thương thức cho IDataBaseService
    /// </summary>
    public static class IDataBaseServiceExtender
    {
        /// <summary>
        /// Thực hiện action của TModel với DataBaseService xác định
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="service"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        public static void DoWithModel<TModel>(this IDataBaseService service, Action<TModel> action) where TModel : ModelBase, new()
        {
            // Khởi tạo model
            TModel model = new TModel();

            // Nếu như service khác null thì thiết lập service cho Model để thực hiện truy vấn tới db
            if (service.IsNotNull()) model.SetDataBaseService(service);

            // Thực hiện action
            action(model);
        }

        /// <summary>
        /// Thực hiện action của TModel với DataBaseService xác định
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="model"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult DoWithModel<TModel, TResult>(this IDataBaseService service, Func<TModel, TResult> action) where TModel : ModelBase, new()
        {
            // Biến lưu kết quả để return
            TResult result = default(TResult);

            // Thực hiện action từ model
            service.DoWithModel<TModel>(i => result = action(i));

            // return
            return result;
        }
    }
}
