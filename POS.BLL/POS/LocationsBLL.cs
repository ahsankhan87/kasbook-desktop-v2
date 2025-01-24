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
    public class LocationsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                LocationsDLL objDLL = new LocationsDLL();
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
                LocationsDLL objDLL = new LocationsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByLocationsID(int Locations_id)
        {
            try
            {
                LocationsDLL objDLL = new LocationsDLL();
                return objDLL.SearchRecordByLocationsID(Locations_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(LocationsModal obj)
        {
            try
            {
                LocationsDLL objDLL = new LocationsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(LocationsModal obj)
        {
            try
            {
                LocationsDLL objDLL = new LocationsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int LocationsId)
        {
            try
            {
                LocationsDLL objDLL = new LocationsDLL();
                return objDLL.Delete(LocationsId);
            }
            catch
            {

                throw;
            }
        }
    }
}
