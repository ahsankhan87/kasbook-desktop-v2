using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace POS.BLL
{
    public partial class JournalsBLL
    {
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

        public JVVoucherModel GetVoucherWithLines(int voucherId)
        {
            return new JournalsDLL().GetVoucherWithLines(voucherId);
        }

        public BatchPostResult BatchPostVouchers(List<int> voucherIds, int userId)
        {
            return new JournalsDLL().BatchPostVouchers(voucherIds, userId);
        }
    }
}
