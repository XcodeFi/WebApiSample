using ShCore.Extensions;

namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Lệnh Delete
    /// </summary>
    class ShDeleteCommand : ShCommand
    {
        /// <summary>
        /// Thực hiện Build ra câu lệnh
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public override void Build(ModelBase t, TSqlBuilder builder, params string[] fields)
        {
            this.BuildWhere(t, builder);
            this.Command = "DELETE t FROM {0} t ".Frmat(builder.TableInfo.TableName) + this.Command;
        }
    }
}
