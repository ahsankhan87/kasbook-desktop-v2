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
    public class UnitsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                UnitsDLL objDLL = new UnitsDLL();
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
                UnitsDLL objDLL = new UnitsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByUnitsID(int Units_id)
        {
            try
            {
                UnitsDLL objDLL = new UnitsDLL();
                return objDLL.SearchRecordByUnitsID(Units_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(UnitsModal obj)
        {
            try
            {
                UnitsDLL objDLL = new UnitsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(UnitsModal obj)
        {
            try
            {
                UnitsDLL objDLL = new UnitsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int UnitsId)
        {
            try
            {
                UnitsDLL objDLL = new UnitsDLL();
                return objDLL.Delete(UnitsId);
            }
            catch
            {

                throw;
            }
        }
    }
}
