using System.Linq;
using ShCore.DataBase.ADOProvider.Attributes;
using ShCore.Extensions;
namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Câu lệnh thực hiện Update
    /// </summary>
    class ShUpdateCommand : ShCommand
    {
        public override void Build(ModelBase t, TSqlBuilder builder, params string[] fields)
        {
            // Build Where
            this.BuildWhere(t, builder);
            var f1 = string.Empty;
            
            builder.AllProperties.ForEach(p =>
            {
                var fn = builder.FieldPKs.FirstOrDefault(f => f.FieldName == p.Name); // Kiểm tra xem có phải là key không
                if ((fn == null || !fn.IsKey) && (fields.Length == 0 || fields.Contains(p.Name))) // Nếu không phải key thì mới build câu lệnh
                {
                    f1 += "t.[{0}]= @{0},".Frmat(p.Name);
                    this.Parameter[p.Name] = t.Eval(p.Name);
                }
            });
            f1 = f1.TrimEnd(',');

            // Thực hiện build command
            this.Command = "UPDATE t SET {0} FROM {1} t ".Frmat(f1, builder.TableInfo.TableName) + this.Command;
        }
    }
}
