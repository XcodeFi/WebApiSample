using System;
using System.Linq;
using ShCore.DataBase.ADOProvider.Attributes;
using ShCore.Extensions;
using System.Collections.Generic;
using ShCore.Reflectors;
using System.Reflection;
namespace ShCore.DataBase.ADOProvider.ShSqlCommand
{
    /// <summary>
    /// Build Command từ ModelBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TSqlBuilder
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public TSqlBuilder(Type typeOfT) { this.typeOfT = typeOfT; }

        /// <summary>
        /// typeOfT
        /// </summary>
        private Type typeOfT = null;
        public Type TypeOfT
        {
            get { return typeOfT; }
        }

        private TableInfoAttribute tableInfo = null;
        /// <summary>
        /// Thông tin về bảng trong cơ sở dữ liệu
        /// </summary>
        public TableInfoAttribute TableInfo
        {
            get
            {
                // Nếu thông tin bảng chưa có thì lấy ra
                if (tableInfo == null)
                    tableInfo = typeOfT.GetAttribute<TableInfoAttribute>();
                return tableInfo;
            }
        }

        private List<FieldAttribute> fieldPKs = null;
        /// <summary>
        /// Danh sách Primary Key
        /// </summary>
        public List<FieldAttribute> FieldPKs
        {
            get
            {
                // Nếu chưa thông tin khóa chính thì lấy ra
                if (fieldPKs == null)
                    // Chỉ lấy những Field là khóa chính                    
                    fieldPKs = new ReflectTypeListPropertyWithAttribute<FieldAttribute>()[typeOfT].Where(f => f.T2.IsKey).Select(f => 
                    {
                        f.T2.FieldName = f.T1.Name;
                        return f.T2;
                    }).ToList();
                // trả ra danh sách khóa chinh
                return fieldPKs;
            }
        }

        private List<PropertyInfo> allProperties = null;
        /// <summary>
        /// Tất cả Property có FieldAttribute
        /// </summary>
        public List<PropertyInfo> AllProperties
        {
            get
            {
                if (allProperties == null)
                    allProperties = new ReflectTypeListPropertyWithAttribute<FieldAttribute>()[typeOfT].Select(f => f.T1).ToList();
                return allProperties;
            }
        }

        /// <summary>
        /// Build ra câu lệnh Command tương ứng
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ShCommand BuildCommand<TCommand>(ModelBase t, params string[] fields) where TCommand : ShCommand, new()
        {
            // Build ra câu lệnh Command
            ShCommand cmd = new TCommand();

            // Nếu như Builder hiện tại không hợp lệ thì trả ra command null
            if (!cmd.IsValid(this)) return null;

            cmd.Build(t, this, fields);
            // Trả ra câu lệnh
            return cmd;
        }

        /// <summary>
        /// Build select với tất cả các field
        /// </summary>
        /// <returns></returns>
        public string BuildSelectAllField()
        {
            // Select
            string str = " SELECT";

            // Các fields
            this.AllProperties.ForEach(p => str += " t.{0},".Frmat(p.Name));

            // Build lệnh
            return str.TrimEnd(',') + " FROM {0} t".Frmat(this.TableInfo.TableName);
        }
    }
}
