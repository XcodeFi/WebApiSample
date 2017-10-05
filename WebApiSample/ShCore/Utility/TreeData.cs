using System.Collections.Generic;
using System.Data;
using System.Linq;
using ShCore.Extensions;
namespace ShCore.Utility
{
    /// <summary>
    /// Xử lý TreeData cho một DataTable có cấu trúc self join
    /// </summary>
    public class TreeData
    {
        /// <summary>
        /// 
        /// </summary>
        public string FieldID { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string FieldName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string FieldParent { set; get; }

        /// <summary>
        /// Source cần Format
        /// </summary>
        public DataTable Source { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public DataTable GetInTreeView(object parent, string prefix = "")
        {
            var table = Source.Clone();
            table.Columns.Add("Level");
            GetInTreeView(parent, string.Empty, table, 0, prefix.IsNull() ? Prefix : prefix, true);
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        public const string Prefix = "→";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="ch"></param>
        /// <param name="table"></param>
        /// <param name="level"></param>
        /// <param name="prefix"></param>
        /// <param name="isFirst"></param>
        private void GetInTreeView(object parent, string ch, DataTable table, int level, string prefix, bool isFirst)
        {
            if (!isFirst)
            {
                ch = ch + prefix;
                level++;
            }

            DataRow[] drs = Source.AsEnumerable().Where(row => row[FieldParent].Equals(parent)).ToArray();

            DataRow dr = null;
            for (int i = 0; i < drs.Length; i++)
            {
                dr = table.NewRow();
                for (int j = 0; j < Source.Columns.Count; j++)
                {
                    dr[Source.Columns[j].ColumnName] = drs[i][Source.Columns[j].ColumnName];
                }
                dr[FieldName] = ch + drs[i][FieldName];

                dr["Level"] = level;
                table.Rows.Add(dr);
                GetInTreeView(drs[i][FieldID], ch, table, level, prefix, false);
            }
        }

        /// <summary>
        /// Tìm các chuyên mục con
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private IEnumerable<DataRow> DoFindChild(object parent)
        {
            var childs = Source.AsEnumerable().Where(dr => dr[FieldParent].Equals(parent));
            return childs.Concat(childs.SelectMany(c => DoFindChild(c[FieldID])));
        }

        /// <summary>
        /// Tìm các chuyên mục con
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public DataTable FindChild(object parent)
        {
            var childs = DoFindChild(parent).ToList();
            return childs.Count == 0 ? Source.Clone() : childs.CopyToDataTable();
        }
    }
}
