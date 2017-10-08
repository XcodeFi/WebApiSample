using ShCore.DataBase.ADOProvider.Attributes;
using ShCore.Extensions;
using ShCore.Utility;
using ShCore.Web.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Document.Entities
{
    [TableInfo(TableName = "[Device]")]
    public class Device : MainDbEntityBase
    {
        [Field(IsKey = true, IsIdentity = true)]
        public int ID { set; get; }

        [Field]
        public string Token { get; set; }
        [Field]
        public string PhoneNumber { get; set; }

        public static List<Device> GetAllData()
        {
            return Singleton<Device>.Inst.GetAll().ToList<Device>(false).ToList();
        }
        public override void Save()
        {
            if (ID>0)
            {
                base.Save();
            }
            else
            {
                base.Insert();
            }
        }

        public Device GetByID(int id)
        {
            return GetAllData().Find(d=>d.ID==id);
        }

        public override int Delete()
        {
            return base.Delete();
        }
    }
}
