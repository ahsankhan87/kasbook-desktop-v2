using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BLL
{
    public class GeneralBLL
    {
        /// <summary>
        /// Generic function to get all record from database with condition
        /// </summary>
        /// <param name="keyword">i.e. id,name,address</param>
        /// <param name="table">database table name</param>
        /// <returns>DataTable</returns>
        public DataTable GetRecord(String keyword,String table)
        {
            try
            {
                GeneralDLL objDLL = new GeneralDLL();
                return objDLL.GetRecord(keyword, table);
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetProductsList()
        {
            try
            {
                GeneralDLL objDLL = new GeneralDLL();
                return objDLL.GetProductsList();
            }
            catch
            {

                throw;
            }
        }

    }
}
