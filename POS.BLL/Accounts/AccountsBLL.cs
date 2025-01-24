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
    public class AccountsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }

        public DataTable GetGroupAccountByParent(int parent_id = 0)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetGroupAccountByParent(parent_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAccountByGroup(int group_id = 0)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetAccountByGroup(group_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable AccountReport(DateTime from_date, DateTime to_date, int account_id = 0)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.AccountReport(from_date, to_date, account_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetGroupsByAccountType(int account_type_id = 0)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetGroupsByAccountType(account_type_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GroupReport(DateTime from_date, DateTime to_date, int group_id = 0)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GroupAccountReport(from_date, to_date, group_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable TrialBalanceReport(DateTime from_date, DateTime to_date)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.TrialBalanceReport(from_date, to_date);
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
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByAccountsID(int Accounts_id)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.SearchRecordByAccountsID(Accounts_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(AccountsModal obj)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(AccountsModal obj)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int AccountsId)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.Delete(AccountsId);
            }
            catch
            {

                throw;
            }
        }
    }
}
