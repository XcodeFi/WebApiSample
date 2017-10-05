using ShCore.Extensions;
namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Nội dung một Command
    /// </summary>
    public abstract class ShCommand
    {
        private string command = string.Empty;
        /// <summary>
        /// Câu lệnh Command
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        private Param parameter = new Param();
        /// <summary>
        /// Parameter cho câu lệnh
        /// </summary>
        public Param Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        /// <summary>
        /// Build ra câu lệnh
        /// </summary>
        public abstract void Build(ModelBase t, TSqlBuilder builder, params string[] fields);

        /// <summary>
        /// Build mệnh đề Where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        protected void BuildWhere(ModelBase t, TSqlBuilder builder)
        {
            command += " WHERE";
            builder.FieldPKs.ForEach(f =>
            {
                command += " t.{0} = @{0}".Frmat(f.FieldName) + " AND";

                parameter[f.FieldName] = t.Eval(f.FieldName);
            });
            // Nhớ cắt cái AND đi thì mới được
            this.command = this.command.Substring(0, this.command.LastIndexOf("AND"));
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của builder
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public virtual bool IsValid(TSqlBuilder builder)
        {
            return !builder.TableInfo.IsNull() && !builder.FieldPKs.IsNull() && builder.FieldPKs.Count != 0;
        }
    }

    /// <summary>
    /// Lệnh Select
    /// </summary>
    abstract class ShSelectCommand : ShCommand
    {
        /// <summary>
        /// Thực hiện ren ra câu lệnh Select tất cả các Field
        /// </summary>
        /// <param name="builder"></param>
        protected void BuildSelectAllField(TSqlBuilder builder)
        {
            // Select
            Command += " SELECT";

            // Các fields
            builder.AllProperties.ForEach(p => Command += " t.{0},".Frmat(p.Name));

            // Build lệnh
            this.Command = this.Command.TrimEnd(',') + " FROM {0} t".Frmat(builder.TableInfo.TableName);
        }
    }
}
