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
        public DataTable GetAccountsWithAccountType()
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetAccountsWithAccountType();
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

        public DataTable GetProfitLossHierarchy()
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetProfitLossHierarchy();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetProfitLossBalances(DateTime fromDate, DateTime toDate, DateTime? previousFromDate, DateTime? previousToDate, int? costCenterId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetProfitLossBalances(fromDate, toDate, previousFromDate, previousToDate, costCenterId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetProfitLossCostCenters()
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetProfitLossCostCenters();
            }
            catch
            {
                throw;
            }
        }

        public decimal GetInventoryValueAsOf(DateTime asOfDate)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetInventoryValueAsOf(asOfDate);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetNetProfitBetween(DateTime fromDate, DateTime toDate)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetNetProfitBetween(fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetBalanceByAccountNamePatternsAsOf(DateTime asOfDate, params string[] namePatterns)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetBalanceByAccountNamePatternsAsOf(asOfDate, namePatterns);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetLedgerAccounts()
        {
            try
            {
                LedgerDAL dal = new LedgerDAL();
                return dal.GetLedgerAccounts();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetAccountLedgerSummary(int accId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                LedgerDAL dal = new LedgerDAL();
                return dal.GetAccountLedgerSummary(accId, fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public Tuple<DataTable, int, decimal> GetAccountLedgerPage(int accId, DateTime fromDate, DateTime toDate, int page, int pageSize, string showFilter)
        {
            try
            {
                LedgerDAL dal = new LedgerDAL();
                return dal.GetAccountLedger(accId, fromDate, toDate, page, pageSize, showFilter);
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

        public DataTable GetCustomerSubLedger(int customerId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetCustomerSubLedger(customerId, fromDate, toDate, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerSubLedgerAging(int customerId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetCustomerSubLedgerAging(customerId, fromDate, toDate, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierSubLedger(int supplierId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetSupplierSubLedger(supplierId, fromDate, toDate, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierSubLedgerAging(int supplierId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetSupplierSubLedgerAging(supplierId, fromDate, toDate, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCashBook(int? cashAccountId = null, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetCashBook(cashAccountId, fromDate, toDate, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCashBookDailyTotals(int? cashAccountId = null, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetCashBookDailyTotals(cashAccountId, fromDate, toDate, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCashFlowStatement(DateTime fromDate, DateTime toDate)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetCashFlowStatement(fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetCashAndBankBalanceAsOf(DateTime asOfDate)
        {
            try
            {
                AccountsDLL objDLL = new AccountsDLL();
                return objDLL.GetCashAndBankBalanceAsOf(asOfDate);
            }
            catch
            {
                throw;
            }
        }
    }
}
