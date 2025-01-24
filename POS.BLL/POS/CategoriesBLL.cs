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
    public class CategoriesBLL
    {
        public DataTable GetAll()
        {
            try
            {
                CategoriesDLL objDLL = new CategoriesDLL();
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
                CategoriesDLL objDLL = new CategoriesDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByCategoriesID(int Categories_id)
        {
            try
            {
                CategoriesDLL objDLL = new CategoriesDLL();
                return objDLL.SearchRecordByCategoriesID(Categories_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(CategoriesModal obj)
        {
            try
            {
                CategoriesDLL objDLL = new CategoriesDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(CategoriesModal obj)
        {
            try
            {
                CategoriesDLL objDLL = new CategoriesDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int CategoriesId)
        {
            try
            {
                CategoriesDLL objDLL = new CategoriesDLL();
                return objDLL.Delete(CategoriesId);
            }
            catch
            {

                throw;
            }
        }
    }
}
