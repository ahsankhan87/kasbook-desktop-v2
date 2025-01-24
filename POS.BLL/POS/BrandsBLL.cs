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
    public class BrandsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                BrandsDLL objDLL = new BrandsDLL();
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
                BrandsDLL objDLL = new BrandsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByBrandsCode(String brandCode)
        {
            try
            {
                BrandsDLL objDLL = new BrandsDLL();
                return objDLL.SearchRecordByBrandsCode(brandCode);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByBrandsID(int Brands_id)
        {
            try
            {
                BrandsDLL objDLL = new BrandsDLL();
                return objDLL.SearchRecordByBrandsID(Brands_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(BrandsModal obj)
        {
            try
            {
                BrandsDLL objDLL = new BrandsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(BrandsModal obj)
        {
            try
            {
                BrandsDLL objDLL = new BrandsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int BrandsId)
        {
            try
            {
                BrandsDLL objDLL = new BrandsDLL();
                return objDLL.Delete(BrandsId);
            }
            catch
            {

                throw;
            }
        }
    }
}
