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
    public class GroupsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                GroupsDLL objDLL = new GroupsDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }

        /// <summary>
        /// Generic Search from database
        /// </summary>
        /// <param name="condition">i.e. first_name="Ahsan"</param>
        /// <returns>DataTable</returns>
        public DataTable SearchRecord(String condition)
        {
            try
            {
                GroupsDLL objDLL = new GroupsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByGroupsID(int Groups_id)
        {
            try
            {
                GroupsDLL objDLL = new GroupsDLL();
                return objDLL.SearchRecordByGroupsID(Groups_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(GroupsModal obj)
        {
            try
            {
                GroupsDLL objDLL = new GroupsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(GroupsModal obj)
        {
            try
            {
                GroupsDLL objDLL = new GroupsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int GroupsId)
        {
            try
            {
                GroupsDLL objDLL = new GroupsDLL();
                return objDLL.Delete(GroupsId);
            }
            catch
            {

                throw;
            }
        }
    }
}
