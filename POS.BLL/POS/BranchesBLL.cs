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
    public class BranchesBLL
    {
        public DataTable GetAll()
        {
            try
            {
                BranchesDLL objDLL = new BranchesDLL();
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
                BranchesDLL objDLL = new BranchesDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByBranchesID(int Branches_id)
        {
            try
            {
                BranchesDLL objDLL = new BranchesDLL();
                return objDLL.SearchRecordByBranchesID(Branches_id);
            }
            catch
            {

                throw;
            }
        }
        public string GetBranchNameByID(int branchId)
        {
            try
            {
                BranchesDLL objDLL = new BranchesDLL();
                return objDLL.GetBranchNameByID(branchId);
            }
            catch
            {

                throw;
            }
        }
        public int Insert(BranchesModal obj)
        {
            try
            {
                BranchesDLL objDLL = new BranchesDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(BranchesModal obj)
        {
            try
            {
                BranchesDLL objDLL = new BranchesDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int BranchesId)
        {
            try
            {
                BranchesDLL objDLL = new BranchesDLL();
                return objDLL.Delete(BranchesId);
            }
            catch
            {

                throw;
            }
        }
    }
}
