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
    public class EmployeeBLL
    {
        public DataTable GetAll()
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }

        public DataTable SearchRecordByID(int employee_id)
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.SearchRecordByID(employee_id);
            }
            catch
            {

                throw;
            }
        }

        public long GetEmpCommissionBalance(int employee_id)
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.GetEmpCommissionBalance(employee_id);
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
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(EmployeeModal obj)
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int InsertEmpCommission(JournalsModal obj)
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.InsertEmpCommission(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(EmployeeModal obj)
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int EmployeeId)
        {
            try
            {
                EmployeeDLL objDLL = new EmployeeDLL();
                return objDLL.Delete(EmployeeId);
            }
            catch
            {

                throw;
            }
        }
    }
}
