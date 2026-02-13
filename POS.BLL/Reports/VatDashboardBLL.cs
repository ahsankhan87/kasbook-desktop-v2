using System;
using System.Data;
using POS.DLL;
using POS.Core;

namespace POS.BLL
{
    public class VatDashboardBLL
    {
        public DataTable GetCompanySummary(DateTime from, DateTime to)
        {
            var dll = new VatDashboardDLL();
            return dll.GetCompanySummary(from, to);
        }

        public DataTable GetBranchSummary(DateTime from, DateTime to)
        {
            var dll = new VatDashboardDLL();
            return dll.GetBranchSummary(from, to, UsersModal.logged_in_branch_id);
        }

        public DataTable GetBranchMovement(DateTime from, DateTime to)
        {
            var dll = new VatDashboardDLL();
            return dll.GetBranchMovement(from, to);
        }
    }
}
