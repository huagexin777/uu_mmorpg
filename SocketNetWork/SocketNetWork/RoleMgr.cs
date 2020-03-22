using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketNetWork
{
    class RoleMgr
    {

        #region 单例

        private static object obj = new object();

        private static RoleMgr _instance;
        public static RoleMgr Instance
        {
            get
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = new RoleMgr();
                    }
                    return _instance;
                }
               
            }
        }

        #endregion


        private List<Role> roleList = new List<Role>();
        internal List<Role> RoleList { get => roleList; set => roleList = value; }
    }
}
