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

        public int InsertHeader(JournalsModal obj)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.InsertHeader(obj);
            }
            catch
            {

                throw;
            }
        }

        public int InsertVoucher(JournalsModal header, List<JournalsModal> lines)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.InsertVoucher(header, lines);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetVoucherHeaders(DateTime fromDate, DateTime toDate, string voucherType, string status, string search)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetVoucherHeaders(fromDate, toDate, voucherType, status, search);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetVoucherLines(string invoiceNo)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetVoucherLines(invoiceNo);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetVoucherHeaderById(int headerId)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetVoucherHeaderById(headerId);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetVoucherHeaderByInvoiceNo(string invoiceNo)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetVoucherHeaderByInvoiceNo(invoiceNo);
            }
            catch
            {

                throw;
            }
        }

        public int PostDraftVouchers(List<int> headerIds)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.PostDraftVouchers(headerIds);
            }
            catch
            {

                throw;
            }
        }

        public int DeleteDraftVouchers(List<int> headerIds)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.DeleteDraftVouchers(headerIds);
            }
            catch
            {

                throw;
            }
        }

        public int DeleteDraftVoucherByInvoiceNo(string invoiceNo)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.DeleteDraftVoucherByInvoiceNo(invoiceNo);
            }
            catch
            {

                throw;
            }
        }

        public int CreateReversalVoucher(int originalHeaderId, DateTime reversalDate, string reason)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.CreateReversalVoucher(originalHeaderId, reversalDate, reason);
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

        public List<ValidationError> ValidateJournalLines(List<JVLineModel> lines)
        {
            return new JournalsDLL().ValidateJournalLines(lines);
        }

        public PostResult PostJournalVoucher(JVHeaderModel header, List<JVLineModel> lines, int userId)
        {
            return new JournalsDLL().PostJournalVoucher(header, lines, userId);
        }

        public PostResult ReverseJournalVoucher(int voucherId, DateTime reversalDate, string reason, int userId)
        {
            return new JournalsDLL().ReverseJournalVoucher(voucherId, reversalDate, reason, userId);
        }

        public PostResult PostAutoJournalEntry(AutoJVModel model, int userId)
        {
            return new JournalsDLL().PostAutoJournalEntry(model, userId);
        }

        public (JVHeaderModel Header, List<JVLineModel> Lines) GetVoucherWithLines(int voucherId)
        {
            return new JournalsDLL().GetVoucherWithLines(voucherId);
        }

        public BatchPostResult BatchPostVouchers(List<int> voucherIds, int userId)
        {
            return new JournalsDLL().BatchPostVouchers(voucherIds, userId);
        }

        /// <summary>
        /// Get journal entries within a date range for reconciliation
        /// </summary>
        public DataTable GetJournalEntriesByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetJournalEntriesByDateRange(fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get unreconciled entries
        /// </summary>
        public DataTable GetUnreconciledEntries(DateTime fromDate, DateTime toDate)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetUnreconciledEntries(fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get voucher details by invoice number
        /// </summary>
        public DataTable GetVoucherDetailsByInvoiceNo(string invoiceNo)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetVoucherDetailsByInvoiceNo(invoiceNo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update reconciliation status for a journal entry
        /// </summary>
        public bool UpdateReconciliationStatus(string invoiceNo, bool isReconciled, int userId, DateTime reconcileDate)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.UpdateReconciliationStatus(invoiceNo, isReconciled, userId, reconcileDate);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get reconciliation history for audit trail
        /// </summary>
        public DataTable GetReconciliationHistory(string invoiceNo)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.GetReconciliationHistory(invoiceNo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Batch reconcile multiple entries
        /// </summary>
        public bool BatchReconcile(List<string> invoiceNos, int userId, DateTime reconcileDate)
        {
            try
            {
                JournalsDLL objDLL = new JournalsDLL();
                return objDLL.BatchReconcile(invoiceNos, userId, reconcileDate);
            }
            catch
            {
                throw;
            }
        }
    }
}
