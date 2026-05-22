using System;
using POS.DLL;
using System.Data;
using POS.Core;
using System.Collections.Generic;

namespace POS.BLL
{
    public class ExpenseBLL
    {
        public DataTable GetAll()
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }
        public String GetMaxInvoiceNo()
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetMaxInvoiceNo();
            }
            catch
            {

                throw;
            }
        }

        public int Insert(List<ExpenseModal_Header> sales)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.Insert(sales);
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
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByJournalsID(int Journals_id)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.SearchRecordByJournalsID(Journals_id);
            }
            catch
            {

                throw;
            }
        }

        public int Update(JournalsModal obj)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int JournalsId)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.Delete(JournalsId);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetExpenseTrackerList(DateTime fromDate, DateTime toDate, int expenseAccountId = 0, string paymentMode = "", string searchText = "")
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetExpenseTrackerList(fromDate, toDate, expenseAccountId, paymentMode, searchText);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetExpenseByVoucher(string voucherNo)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetExpenseByVoucher(voucherNo);
            }
            catch
            {
                throw;
            }
        }

        public int DeleteByVoucher(string voucherNo)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.DeleteByVoucher(voucherNo);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetExpenseTotal(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetExpenseTotal(fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetPendingExpenseTotal()
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetPendingExpenseTotal();
            }
            catch
            {
                throw;
            }
        }

        public int GetPendingExpenseCount()
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetPendingExpenseCount();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetExpenseMonthlyComparison(int year)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetExpenseMonthlyComparison(year);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetExpenseBreakdown(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetExpenseBreakdown(fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetRecentExpenses(int top)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetRecentExpenses(top);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetTopExpenseAccounts(DateTime fromDate, DateTime toDate, int top)
        {
            try
            {
                ExpenseDLL objDLL = new ExpenseDLL();
                return objDLL.GetTopExpenseAccounts(fromDate, toDate, top);
            }
            catch
            {
                throw;
            }
        }
    }
}
