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
    public class OriginBLL
    {
        public DataTable GetAll()
        {
            try
            {
                OriginDLL objDLL = new OriginDLL();
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
                OriginDLL objDLL = new OriginDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByOriginID(int Origin_id)
        {
            try
            {
                OriginDLL objDLL = new OriginDLL();
                return objDLL.SearchRecordByOriginID(Origin_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(OriginModal obj)
        {
            try
            {
                OriginDLL objDLL = new OriginDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(OriginModal obj)
        {
            try
            {
                OriginDLL objDLL = new OriginDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int OriginId)
        {
            try
            {
                OriginDLL objDLL = new OriginDLL();
                return objDLL.Delete(OriginId);
            }
            catch
            {

                throw;
            }
        }
    }
}
