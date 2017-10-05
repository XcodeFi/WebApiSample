using ShCore.Extensions;

namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Thực hiện lấy tất cả bản ghi
    /// </summary>
    class ShGetAllCommand : ShSelectCommand
    {
        /// <summary>
        /// Build câu lệnh Select All
        /// </summary>
        /// <param name="t"></param>
        /// <param name="builder"></param>
        public override void Build(ModelBase t, TSqlBuilder builder, params string[] fields)
        {
            this.BuildSelectAllField(builder);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của Builder
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public override bool IsValid(TSqlBuilder builder)
        {
            return !builder.TableInfo.IsNull();
        }
    }
}
