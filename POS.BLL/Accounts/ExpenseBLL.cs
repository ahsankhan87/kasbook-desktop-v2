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
    }
}
