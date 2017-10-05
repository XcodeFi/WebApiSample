using ShCore.Extensions;
using System.Data;
using System;
using ShCore.Utility;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace ShCore.DataBase.ADOProvider
{
    /// <summary>
    /// ModelBase
    /// </summary>
    public partial class ModelBase
    {
        /// <summary>
        /// Dùng chung cho các class để set css cho row
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// LuanBH  9/1/2016   created
        /// </Modified>
        public string CssClass { get; set; }

        #region IDataBaseService
        private IDataBaseService dataBaseService = null;

        /// <summary>
        /// Thiết lập DataBaseService
        /// </summary>
        /// <param name="dataBaseService"></param>
        public void SetDataBaseService(IDataBaseService dataBaseService)
        {
            this.dataBaseService = dataBaseService;
        }

        /// <summary>
        /// Cung cấp IDataBaseService mà model muốn sử dụng truy vấn vào db 
        /// </summary>
        /// <returns></returns>
        protected virtual IDataBaseService GetDataBaseService()
        {
            return null;
        }

        /// <summary>
        /// Một Service cần thực hiện xuống cơ sở dữ liệu
        /// </summary>
        protected IDataBaseService DataBaseService
        {
            get
            {
                // Lấy ra service cần dùng cho Model
                if (this.dataBaseService.IsNull()) this.dataBaseService = GetDataBaseService();

                // return
                return dataBaseService;
            }
        }
        #endregion

        #region protected
        /// <summary>
        /// Thực thi một thủ tục với việc lấy Param tự động
        /// </summary>
        /// <param name="store"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected DataTable ExeStore(string store, params object[] values)
        {
            return DoExeWith(inputs => this.DataBaseService.ExeStore(store, inputs.T1), store, values);
        }

        protected DataSet ExeDsStore(string store, params object[] values)
        {
            return DoExeWith(inputs => this.DataBaseService.ExeStoreToDataSet(store, inputs.T1), store, values);
        }
        /// <summary>
        /// Thực thi một thủ tục với việc lấy Param tự động
        /// </summary>
        /// <param name="store"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected int ExeStoreNoneQuery(string store, params object[] values)
        {
            return DoExeWith(inputs => this.DataBaseService.ExeStoreNoneQuery(store, inputs.T1), store, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        protected Pair<int, Param> ExeStoreNoneQuery(string store, Param inputs, Param outputs = null)
        {
            return this.DataBaseService.ExeStoreNoneQuery(store, inputs, outputs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        protected int ExeStoreNoneQuery(string store, Param inputs)
        {
            return this.DataBaseService.ExeStoreNoneQuery(store, inputs);
        }

        /// <summary>
        /// Vừa lấy tham số tự động từ đối tượng, vừa lấy tham số từ lamda
        /// </summary>
        /// <param name="store"></param>
        /// <param name="propertyLamdaInputs"></param>
        /// <param name="propertyLamdaOutPuts"></param>
        /// <returns></returns>
        protected Pair<int, Param> ExeStoreNoneQuery(string store, Expression<Action> propertyLamdaInputs, Expression<Action> propertyLamdaOutPuts = null)
        {
            var pp = this.GetParamsFromStore(store, propertyLamdaInputs, propertyLamdaOutPuts);
            var test_log = store + "-";
            foreach (var item in pp.T1.Items)
            {
                test_log += item.Name + ": " + item.Value + ";";
            }
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Debug store.txt", true))
            //{
            //    file.WriteLine(test_log);
            //}
            return ExeStoreNoneQuery(store, pp.T1, pp.T2);
        }

        /// <summary>
        /// Thực thi một thủ tục với việc lấy Param tự động
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        protected Pair<DataTable, Param> ExeStoreWithOutput(string store, params object[] values)
        {
            return DoExeWith(inputs => this.DataBaseService.ExeStoreToTable(store, inputs.T1, inputs.T2), store, values);
        }

        /// <summary>
        /// Thực thi một thủ tục với việc lấy Param tự động
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        protected Pair<int, Param> ExeStoreNoneQueryWithOutput(string store, params object[] values)
        {
            return DoExeWith(inputs => this.DataBaseService.ExeStoreNoneQuery(store, inputs.T1, inputs.T2), store, values);
        }

        /// <summary>
        ///  Điền dữ liệu từ DataRow vào đối tượng hiện tại
        /// </summary>
        /// <param name="dr"></param>
        public void ParseFrom(DataRow dr)
        {
            this.Parse(dr, false);
        }
        #endregion

        #region private static
        /// <summary>
        /// Thực thi với một thủ tục
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="store"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private T DoExeWith<T>(Func<Pair<Param, Param>, T> func, string store, params object[] values)
        {
            // Nếu không có service thì trả ra mặc định
            if (this.DataBaseService.IsNull()) return default(T);

            // Nếu mảng rỗng thì khởi tạo một phần tử rỗng
            if (values.IsNull()) values = new object[] { null };

            // Lấy ra Param
            var param = GetParamsFromStore(store, values);
            //var test_log = store + "-";
            //foreach (var item in param.T1.Items)
            //{
            //    test_log += item.Name + ": " + item.Value + ";";
            //}
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Debug store.txt", true))
            //{
            //    file.WriteLine(test_log);
            //}

            // Thực thi
            return func(param);
        }

        /// <summary>
        /// Khởi tạo Parameter
        /// </summary>
        /// <param name="store"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private Pair<Param, Param> GetParamsFromStore(string store, params object[] values)
        {
            // Nếu không truyền tập giá trị của param thì lấy tất cả giá trị thuộc tính của đối tượng
            if (values.Length == 0) return DoGetParamsFromStore(store, (s, i) => this.Eval(s));

            // Ngược lại tạo Param theo thứ tự như trong gọi thủ tục
            return DoGetParamsFromStore(store, (s, i) => values[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        protected Pair<Param, Param> GetParamsFromStore(string store, Expression<Action> propertyLamdaInputs = null, Expression<Action> propertyLamdaOutPuts = null)
        {
            var pp = DoGetParamsFromStore(store, (s, i) => this.TryEval(s));

            // Thêm parameter input
            if (propertyLamdaInputs != null) pp.T1.Join(Param.GetParam(propertyLamdaInputs));

            // Thêm parameter output
            if (propertyLamdaOutPuts != null) pp.T2.Join(Param.GetParam(propertyLamdaOutPuts));

            return pp;
        }

        /// <summary>
        /// Thực hiện Lấy tham số cho thủ tục từ tên thủ tục
        /// </summary>
        /// <param name="store"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private Pair<Param, Param> DoGetParamsFromStore(string store, Func<string, int, object> func)
        {
            // Lấy ra các Params từ Cache
            var autoParams = GetParamInfo(store);

            // Tạo ParamInputs tương ứng
            var paramInputs = new Param();

            // Tạo ParamOutputs tương ứng
            var paramOutputs = new Param();

            // Thiết lập Param
            for (int i = 0; i < autoParams.Rows.Count; i++)
                switch (autoParams.Rows[i][1].ToString())
                {
                    // Tạo Param Output
                    case "INOUT": DoCreateParamItem(paramOutputs, autoParams.Rows[i], i, (s, index) => func(s, index)); break;

                    // Tạo Param Input
                    default: DoCreateParamItem(paramInputs, autoParams.Rows[i], i, (s, index) => func(s, index)); break;
                }

            // return
            return new Pair<Param, Param> { T1 = paramInputs, T2 = paramOutputs };
        }

        /// <summary>
        /// Thực hiện khởi tạo ParamItem
        /// </summary>
        /// <param name="p"></param>
        /// <param name="row"></param>
        /// <param name="func"></param>
        private void DoCreateParamItem(Param p, DataRow row, int index, Func<string, int, object> func)
        {
            // name
            string s = row[0].ToString();

            // Biến lưu tên param
            string paramName = string.Empty;

            // Tạo Param
            p[paramName = s.TrimStart('@'), row[3].To<string>(), row[2].IsNull() ? null : new Nullable<int>(row[2].To<int>())] = func(paramName, index);
        }

        /// <summary>
        /// Lấy thông tin Param từ store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        private DataTable GetParamInfo(string store)
        {
            // Lấy ra các Params từ Cache
            var autoParams = this.DataBaseService.ExeSql("SELECT PARAMETER_NAME, PARAMETER_MODE, CHARACTER_MAXIMUM_LENGTH, DATA_TYPE FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = @Store", () => With(store));

            // return
            return autoParams;
        }

        /// <summary>
        /// Hàm rỗng
        /// Mục đích thông báo những Parameter cần khởi tạo cho câu lệnh truy xuất đến CSDL
        /// </summary>
        /// <param name="values"></param>
        protected void With(params object[] values) { }
        #endregion
    }

    /// <summary>
    /// ModelBase<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelBase<T> : ModelBase where T : new()
    {
        /// <summary>
        /// Khởi tạo một đối tượng T
        /// </summary>
        public static T Inst { get { return Singleton<T>.Inst; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllToList()
        {
            return this.GetAll().ToList<T>(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public List<T> ExeStoreToList(string store, params object[] values)
        {
            return this.ExeStore(store, values).ToList<T>(false);
        }

        /// <summary>
        /// Thực hiện truy vấn với mệnh đề where
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DataTable Select(Expression<Func<T, bool>> predicate)
        {
            // Biến lưu thứ tự của Param
            int i = 0;

            // Phân tích chuỗi mệnh đề where
            var pre = AnalyticQuery(predicate.Body.As<BinaryExpression>(), ref i);

            // command
            var command = "{0} {1}".Frmat(this.Builder.BuildSelectAllField(), pre.T1.IsNotNull() ? "WHERE {0}".Frmat(pre.T1) : string.Empty);

            // Thực thi vào trả ra kết quả
            return this.DataBaseService.ExeSql(command, pre.T2);
        }

        /// <summary>
        /// Select và bắn ra List
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> SelectToList(Expression<Func<T, bool>> predicate)
        {
            return Select(predicate).ToList<T>(false);
        }

        /// <summary>
        /// Lệnh xóa
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> predicate)
        {
            // Biến lưu thứ tự các param
            int i = 0;

            // Phân tích chuỗi mệnh đề where
            var pre = AnalyticQuery(predicate.Body.As<BinaryExpression>(), ref i);

            // Command
            var command = "DELETE t FROM {0} t {1}".Frmat(this.Builder.TableInfo.TableName, pre.T1.IsNotNull() ? "WHERE {0}".Frmat(pre.T1) : string.Empty);

            // Thực thi lệnh xóa
            return this.DataBaseService.ExeSqlNoneQuery(command, pre.T2);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <param name="predicateSet"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> predicateSet, Expression<Func<T, bool>> predicate)
        {
            // Biến lưu thứ tự các Param
            int i = 0;

            // Phân tích mệnh đề Set
            var set = AnalyticSet(predicateSet.Body.As<BinaryExpression>(), ref i);

            // Phân tích mệnh đề
            var where = AnalyticQuery(predicate.Body.As<BinaryExpression>(), ref i, set.T2);

            // Command Sql
            var command = "UPDATE t SET {0} FROM {1} t {2}".Frmat(set.T1, this.Builder.TableInfo.TableName, where.T1.IsNotNull() ? "WHERE {0}".Frmat(where.T1) : string.Empty);

            // return số bản ghi được cập nhật
            return this.DataBaseService.ExeSqlNoneQuery(command, set.T2);
        }

        /// <summary>
        /// Phân tích câu truy vấn
        /// </summary>
        /// <param name="predicate"></param>
        private Pair<string, Param> AnalyticQuery(BinaryExpression be, ref int i, Param param = null, string prefix = "t")
        {
            // Biến lưu kết quả gồm Key = Where, Value = Param
            var result = new Pair<string, Param> { T1 = string.Empty };

            // Khởi tạo biến lưu các Param nếu là lần đầu tiên phân tích BinaryExpression
            result.T2 = param.IsNull() ? new Param() : param;

            // Nếu biểu thức bên trái là MemberExpression
            if (be.Left.Is<MemberExpression>())
            {
                // Tên field
                string name = be.Left.As<MemberExpression>().Member.Name;

                // Nếu như bên phải là một biểu thức
                if (be.Right.Is<BinaryExpression>()) result.T1 = "{0}.{1} {3} {2}".Frmat(prefix, name, Analytic(be.Right.As<BinaryExpression>(), ref i, result.T2, prefix).T1, be.NodeType.GetExpression());

                else
                {
                    // Giá trị
                    object value = be.Right.GetValue();

                    // Khởi tạo Param
                    result.T2["{0}_{1}".Frmat(name, i)] = value;

                    // Tạo biểu thức cho mệnh đề where
                    result.T1 = "{3}.{0} {2} @{0}_{1}".Frmat(name, i, be.NodeType.GetExpression(), prefix);

                    // Tăng biến i
                    i++;
                }
            }
            else
            {
                // Biểu thức bên trái
                var rl = AnalyticQuery(be.Left.As<BinaryExpression>(), ref i, result.T2, prefix);

                // be.Left.As<UnaryExpression>().Operand is MemberExpression

                // Biểu thức bên phải
                var rr = AnalyticQuery(be.Right.As<BinaryExpression>(), ref i, result.T2, prefix);

                // Nối hai biểu thức với nhau
                result.T1 = "({0}) {2} ({1})".Frmat(rl.T1, rr.T1, be.NodeType.GetExpression());
            }

            // return kết quả
            return result;
        }

        /// <summary>
        /// Phân tích mệnh đề Set
        /// </summary>
        /// <param name="be"></param>
        /// <param name="i"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private Pair<string, Param> AnalyticSet(BinaryExpression be, ref int i, Param param = null, string prefix = "t")
        {
            // Biến lưu kết quả gồm Key = Mệnh đề Set, Value = Param
            var result = new Pair<string, Param> { T1 = string.Empty };

            // Khởi tạo biến lưu các Param nếu là lần đầu tiên phân tích BinaryExpression
            result.T2 = param.IsNull() ? new Param() : param;

            // Nếu biểu thức bên trái là MemberExpression
            if (be.Left.Is<MemberExpression>())
            {
                // Tên field
                string name = be.Left.As<MemberExpression>().Member.Name;

                // Nếu right là một biểu thức thì phân tích
                if (be.Right.Is<BinaryExpression>()) result.T1 = "{0}.{1} = {2}".Frmat(prefix, name, Analytic(be.Right.As<BinaryExpression>(), ref i, result.T2, prefix).T1);

                // Nếu là Constant
                else
                {
                    // Giá trị
                    object value = be.Right.GetValue();

                    // Khởi tạo Param
                    result.T2["{0}_{1}".Frmat(name, i)] = value;

                    // Thiết lập biểu thức
                    result.T1 = "{2}.{0} = @{0}_{1}".Frmat(name, i, prefix);

                    // Tăng i
                    i++;
                }
            }
            else
            {
                // Biểu thức bên trái
                var rl = AnalyticSet(be.Left.As<BinaryExpression>(), ref i, result.T2, prefix);

                // Biểu thức bên phải
                var rr = AnalyticSet(be.Right.As<BinaryExpression>(), ref i, result.T2, prefix);

                // Ghép các mệnh đề
                result.T1 += "{0},{1},".Frmat(rl.T1, rr.T1);
            }

            // Trim dấu , cuối cùng
            result.T1 = result.T1.TrimEnd(',');

            // return kết quả
            return result;
        }

        /// <summary>
        /// Phân tích biểu thức ví dụ: t.Abc + t.Xyx + 10 thành biểu thức và param trong Sql
        /// </summary>
        /// <param name="be"></param>
        /// <param name="i"></param>
        /// <param name="param"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private Pair<string, Param> Analytic(BinaryExpression be, ref int i, Param param = null, string prefix = "t")
        {
            // Biến lưu kết quả gồm Key = Mệnh đề Set, Value = Param
            var result = new Pair<string, Param> { T1 = string.Empty };

            // Khởi tạo biến lưu các Param nếu là lần đầu tiên phân tích BinaryExpression
            result.T2 = param.IsNull() ? new Param() : param;

            // Biểu thức trái
            var rl = AnalyticExpression(be.Left, ref i, result.T2, prefix);

            // Biểu thức phải
            var rr = AnalyticExpression(be.Right, ref i, result.T2, prefix);

            // Ghép biểu thức trái và phải với nhau
            result.T1 = "({0} {2} {1})".Frmat(rl.T1, rr.T1, be.NodeType.GetExpression());

            // return kết quả
            return result;
        }

        /// <summary>
        /// Phân tích một biểu thức
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="i"></param>
        /// <param name="param"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private Pair<string, Param> AnalyticExpression(Expression ex, ref int i, Param param = null, string prefix = "t")
        {
            // Biến lưu kết quả gồm Key = Mệnh đề Set, Value = Param
            var result = new Pair<string, Param> { T1 = string.Empty };

            // Khởi tạo biến lưu các Param nếu là lần đầu tiên phân tích BinaryExpression
            result.T2 = param.IsNull() ? new Param() : param;

            // Nếu là MemberExpression
            if (ex.Is<MemberExpression>())
            {
                // Gán thành MemberExpression
                var me = ex.As<MemberExpression>();

                // Nếu Base của me là một Constants thì tạo param
                if (me.Expression.Is<ConstantExpression>()) CreateParamFromExpression(me, result, ref i);

                // Ngược lại Format như một field của bảng
                else result.T1 = "{0}.{1}".Frmat(prefix, me.Member.Name);
            }
            // Nếu không thì lại phân tích cả biểu thức
            else if (ex.Is<BinaryExpression>()) result.T1 = Analytic(ex.As<BinaryExpression>(), ref i, result.T2, prefix).T1;

            // Ngược lại thì coi như một biểu thức trả ra hằng số
            else CreateParamFromExpression(ex, result, ref i);

            // return kết quả
            return result;
        }

        /// <summary>
        /// Tạo Parameter từ một Expression Member hoặc Constant
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="result"></param>
        /// <param name="i"></param>
        /// <param name="prefix"></param>
        private void CreateParamFromExpression(Expression ex, Pair<string, Param> result, ref int i)
        {
            // Giá trị của Parameter
            result.T2["ShP{0}".Frmat(i)] = ex.GetValue();

            // Tên Parameter
            result.T1 = "@ShP{0}".Frmat(i);

            // 
            i++;
        }
    }
}
