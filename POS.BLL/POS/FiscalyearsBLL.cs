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
    public class FiscalYearBLL
    {
        public DataTable GetAll()
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }
        public DataTable GetActiveFiscalYear()
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.GetActiveFiscalYear();
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecord(String condition)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByFiscalYearID(int FiscalYear_id)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.SearchRecordByFiscalYearID(FiscalYear_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(FiscalYearModal obj)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(FiscalYearModal obj)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int FiscalYearId)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.Delete(FiscalYearId);
            }
            catch
            {

                throw;
            }
        }


        public int SetAllStatusZero(int FiscalYearId)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.SetAllStatusZero(FiscalYearId);
            }
            catch
            {

                throw;
            }
        }


        public int UpdateStatus(int FiscalYearId, bool status)
        {
            try
            {
                FiscalYearDLL objDLL = new FiscalYearDLL();
                return objDLL.UpdateStatus(FiscalYearId, status);
            }
            catch
            {

                throw;
            }
        }
    }
}
