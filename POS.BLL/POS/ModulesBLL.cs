using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.DLL;
using System.Data;
using POS.Core;

namespace POS.BLL
{
    public class ModulesBLL
    {

        public DataTable GetAllModules()
        {
            try
            {
                ModulesDLL objDLL = new ModulesDLL();
                return objDLL.GetAllModules();
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetParentModules(int parent_id)
        {
            try
            {
                ModulesDLL objDLL = new ModulesDLL();
                return objDLL.GetModules_ByParent(parent_id);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetUsersModules_ByParent(int parent_id, int user_id)
        {
            try
            {
                ModulesDLL objDLL = new ModulesDLL();
                return objDLL.GetUsersModules_ByParent(parent_id, user_id);
            }
            catch
            {

                throw;
            }
        }

    }
}
