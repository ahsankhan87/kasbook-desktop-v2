using System;
using POS.DLL;

namespace POS.BLL
{
    public class BackupBLL
    {
        public bool HasBackupForToday()
        {
            var dll = new BackupDLL();
            return dll.HasBackupForDate(DateTime.Now);
        }

        public bool HasBackupInLastDays(int days)
        {
            if (days < 0) days = 0;
            var from = DateTime.Now.Date.AddDays(-days);
            var dll = new BackupDLL();
            return dll.HasBackupSince(from);
        }

        public bool HasBackupForDate(DateTime date)
        {
            var dll = new BackupDLL();
            return dll.HasBackupForDate(date);
        }

        public bool HasBackupSince(DateTime fromDate)
        {
            var dll = new BackupDLL();
            return dll.HasBackupSince(fromDate);
        }
    }
}
