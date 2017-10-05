using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document.Entities
{
    /// </summary>
    [TableInfo(TableName = "[Admin.Districts]")]
    public class District : MainDbEntityBase
    {
        #region Properties

        [Field(IsKey = true, IsIdentity = true)]
        [DataValueField]
        public int PK_DistrictID { set; get; }

        [Field(Name = "Tên", Important = true)]
        [DataTextField]

        public string Name { set; get; }

        [Field(Name = "Loại")]
        public string DistrictType { set; get; }

        [Field(Name = "Ghi chú")]
        public string Note { set; get; }
        [Field(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }
        [Field(Name = "Người tạo")]
        public Guid? CreatedByUser { get; set; }
        [Field(Name = "Ngày sửa")]
        public Guid? UpdatedDate { get; set; }

        #endregion

        public static List<District> GetAllData()
        {
            return Singleton<District>.Inst.GetAll().ToList<District>(false).OrderBy(x => x.PK_DistrictID).ToList(); ;
        }

    }
}