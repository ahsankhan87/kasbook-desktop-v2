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
    public class JournalsBLL
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
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetMaxInvoiceNo();
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

        public int Insert(JournalsModal obj)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.Insert(obj);
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
