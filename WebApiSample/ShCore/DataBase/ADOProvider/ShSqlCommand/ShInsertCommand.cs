using ShCore.Extensions;
using System.Linq;
using ShCore.DataBase.ADOProvider.Attributes;
namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Câu lệnh thực hiện Insert
    /// </summary>
    class ShInsertCommand : ShCommand
    {
        public override void Build(ModelBase t, TSqlBuilder builder, params string[] fields)
        {
            var f1 = string.Empty; // Các field cần insert
            var f2 = string.Empty; // Tham số của các field cần insert
            FieldAttribute fn = null; // 

            // Danh sách các thuộc tính của đối tượng
            var ps = builder.AllProperties;

            // Duyệt qua từng thuộc tính
            ps.ForEach(p =>
            {
                fn = builder.FieldPKs.FirstOrDefault(f => f.FieldName == p.Name); // Kiểm tra xem có phải là tự tăng không
                if (fn == null || !fn.IsIdentity) // Nếu không phải là tự tăng mới build câu lệnh
                {
                    f1 += "[" + p.Name + "],";
                    f2 += "@" + p.Name + ",";
                    this.Parameter[p.Name] = t.Eval(p.Name);
                }
            });

            // Xem có trường tự tăng thì lấy ra số tự tăng
            fn = builder.FieldPKs.FirstOrDefault(f => f != null && f.IsIdentity);

            // Tạo identity theo key tự tăng
            var @identity = fn != null ? "SELECT @@IDENTITY {0}".Frmat(fn.FieldName) : "SELECT 0";

            f1 = f1.TrimEnd(',');
            f2 = f2.TrimEnd(',');

            // Build câu lệnh
            this.Command = "BEGIN INSERT INTO {0} ({1}) VALUES ({2}) {3} END".Frmat(builder.TableInfo.TableName, f1, f2, @identity);
        }
    }
}
