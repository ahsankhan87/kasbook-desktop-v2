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
    public class CompaniesBLL
    {
        public DataTable GetAll()
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }

        public DataTable GetCompany()
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.GetCompany();
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
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByCompaniesID(int Companies_id)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.SearchRecordByCompaniesID(Companies_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(CompaniesModal obj)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }
        public int Register(CompaniesModal obj)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.Register(obj);
            }
            catch
            {

                throw;
            }
        }


        public int Update(CompaniesModal obj)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }
        
        public int UpdateSubscriptionKeyToDatabase(int companyId, string key)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.UpdateSubscriptionKeyToDatabase(companyId, key);
            }
            catch
            {

                throw;
            }
        }
        public int updateAppLock(int companyId, bool locked)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.updateAppLock(companyId, locked);
            }
            catch
            {

                throw;
            }
        }
        public int Delete(int CompaniesId)
        {
            try
            {
                CompaniesDLL objDLL = new CompaniesDLL();
                return objDLL.Delete(CompaniesId);
            }
            catch
            {

                throw;
            }
        }
    }
}
