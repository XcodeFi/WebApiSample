using ShCore.Extensions;

namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Câu lệnh thực hiện lấy một bản ghi theo Key
    /// </summary>
    class ShGetByKeyCommand : ShSelectCommand
    {
        /// <summary>
        /// Get By Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="builder"></param>
        public override void Build(ModelBase t, TSqlBuilder builder, params string[] fields)
        {
            this.BuildSelectAllField(builder);
            this.BuildWhere(t, builder);
        }
    }
}
