using Microsoft.Practices.EnterpriseLibrary.Data;
using ShCore.DataBase.ADOProvider;
using ShCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document
{
    public class MainDb : MainDBBase
    {
        /// <summary>
        /// Một Instance Database của EnterpriseLibrary
        /// </summary>
        private static Database InstDb = DatabaseFactory.CreateDatabase("NotifyDb");

        /// <summary>
        /// 
        /// </summary>
        protected override Database Db
        {
            get { return InstDb; }
        }
    }

    /// <summary>
    /// Service dành cho MainDb
    /// </summary>
    public class MainDbService : DataBaseService<MainDb> { }

    /// <summary>
    /// 
    /// </summary>
    public class MainDbEntityBase : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IDataBaseService GetDataBaseService()
        {
            return Singleton<MainDbService>.Inst;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MainDbEntityBase<T> : ModelBase<T> where T : new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IDataBaseService GetDataBaseService()
        {
            return Singleton<MainDbService>.Inst;
        }
    }
}