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
    public class ICTBLL
    {
       
        public DataTable GetAll_ict_releases()
        {
            try
            {
                ICTDLL objDLL = new ICTDLL();
                return objDLL.GetAll_ict_releases();
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAll_ict_transfer_transactions()
        {
            try
            {
                ICTDLL objDLL = new ICTDLL();
                return objDLL.GetAll_ict_transfer_transactions();
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAll_ict_requests()
        {
            try
            {
                ICTDLL objDLL = new ICTDLL();
                return objDLL.GetAll_ict_requests();
            }
            catch
            {

                throw;
            }
        }
        public int save_ict_release_qty(List<ICTModal> ict_detail)
        {
            try
            {
                ICTDLL objDLL = new ICTDLL();
                return objDLL.save_ict_release_qty(ict_detail);
            }
            catch
            {

                throw;
            }
        }
        public int save_ict_transfer_qty(List<ICTModal> ict_detail)
        {
            try
            {
                ICTDLL objDLL = new ICTDLL();
                return objDLL.save_ict_transfer_qty(ict_detail);
            }
            catch
            {

                throw;
            }
        }
    }
}
