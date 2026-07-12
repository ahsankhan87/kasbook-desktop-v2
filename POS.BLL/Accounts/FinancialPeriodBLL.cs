using POS.Core;
using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class FinancialPeriodBLL
    {
        private readonly PeriodDAL _periodDal;

        public FinancialPeriodBLL()
        {
            _periodDal = new PeriodDAL();
        }

        public DataTable GetPeriods(int yearId)
        {
            return _periodDal.GetAllPeriods(yearId);
        }

        public int OpenNewPeriod(int yearId)
        {
            return _periodDal.CreateNextMonthPeriod(yearId);
        }

        public DataTable GetCloseChecklist(int periodId)
        {
            return _periodDal.GetPeriodCloseChecklist(periodId);
        }

        public DataTable GetPeriodSummary(int periodId)
        {
            return _periodDal.GetPeriodSummary(periodId);
        }

        public DataTable GetPeriodTransactions(int periodId)
        {
            return _periodDal.GetPeriodTransactions(periodId);
        }

        public FinancialPeriodCloseResultModal SoftClosePeriod(FinancialPeriodCloseOptionsModal options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.close_type = "Soft";
            return _periodDal.ClosePeriod(options);
        }

        public FinancialPeriodCloseResultModal HardLockPeriod(int periodId)
        {
            var options = new FinancialPeriodCloseOptionsModal
            {
                period_id = periodId,
                user_id = UsersModal.logged_in_userid,
                close_type = "Hard",
                auto_post_depreciation = false,
                reverse_prior_accruals = false,
                confirmation_text = "Hard lock confirmed"
            };

            return _periodDal.ClosePeriod(options);
        }

        public FinancialPeriodCloseResultModal ReopenPeriod(int periodId, string reason, string adminPassword)
        {
            if (!IsCurrentUserAdmin())
            {
                return new FinancialPeriodCloseResultModal
                {
                    success = false,
                    message = "Only administrators can reopen a soft-closed period."
                };
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                return new FinancialPeriodCloseResultModal
                {
                    success = false,
                    message = "Reopen reason is required."
                };
            }

            if (!ValidateCurrentUserPassword(adminPassword))
            {
                return new FinancialPeriodCloseResultModal
                {
                    success = false,
                    message = "Invalid admin password/PIN."
                };
            }

            int affected = _periodDal.ReopenSoftClosedPeriod(periodId, UsersModal.logged_in_userid, reason);
            return new FinancialPeriodCloseResultModal
            {
                success = affected > 0,
                message = affected > 0 ? "Period reopened successfully." : "Only soft-closed periods can be reopened.",
                affected_rows = affected
            };
        }

        public event EventHandler<YearEndCloseProgressEventArgs> YearEndCloseProgressChanged;

        public DataTable GetYearEndPreCloseValidationReport(int yearId)
        {
            return _periodDal.GetYearEndPreCloseValidationReport(yearId);
        }

        public YearEndCloseResultModal ExecuteYearEndClose(int yearId, int userId)
        {
            if (yearId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(yearId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            YearEndCloseOptionsModal options = new YearEndCloseOptionsModal
            {
                year_id = yearId,
                user_id = userId,
                branch_id = UsersModal.logged_in_branch_id > 0 ? (int?)UsersModal.logged_in_branch_id : null,
                income_summary_account_id = null,
                retained_earnings_account_id = null
            };

            NotifyYearEndProgress("Starting year-end close...");
            YearEndCloseResultModal result = _periodDal.ExecuteYearEndClose(options, NotifyYearEndProgress);

            if (result.success)
            {
                Log.LogAction("Year-End Close", $"Year {yearId} closed. RunId: {result.run_id}, Net P&L: {result.net_profit_loss}", userId, UsersModal.logged_in_branch_id);
                NotifyYearEndProgress("Year-end close completed.");
            }
            else
            {
                NotifyYearEndProgress("Year-end close finished with validation issues.");
            }

            return result;
        }

        public YearEndRollbackResultModal RollbackYearEndClose(int yearId)
        {
            return RollbackYearEndClose(yearId, "Rollback requested by administrator.");
        }

        public YearEndRollbackResultModal RollbackYearEndClose(int yearId, string reason)
        {
            if (yearId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(yearId));
            }

            if (!IsCurrentUserAdmin())
            {
                return new YearEndRollbackResultModal
                {
                    success = false,
                    message = "Only administrators can roll back year-end close."
                };
            }

            YearEndRollbackResultModal result = _periodDal.RollbackYearEndClose(yearId, UsersModal.logged_in_userid, reason);
            if (result.success)
            {
                Log.LogAction("Year-End Close Rollback", $"Year {yearId} rollback completed.", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
            }

            return result;
        }

        private void NotifyYearEndProgress(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            YearEndCloseProgressChanged?.Invoke(this, new YearEndCloseProgressEventArgs(message));
        }

        private static bool IsCurrentUserAdmin()
        {
            string role = (UsersModal.logged_in_user_role ?? string.Empty).Trim();
            if (role.Equals("Administrator", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("Owner", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return UsersModal.logged_in_user_level >= 1;
        }

        private static bool ValidateCurrentUserPassword(string passwordOrPin)
        {
            if (string.IsNullOrWhiteSpace(passwordOrPin) || string.IsNullOrWhiteSpace(UsersModal.logged_in_username))
            {
                return false;
            }

            UsersModal login = new UsersModal
            {
                username = UsersModal.logged_in_username,
                password = passwordOrPin.Trim()
            };

            UsersDLL userDal = new UsersDLL();
            int userId = userDal.Login(login);
            return userId == UsersModal.logged_in_userid;
        }
    }
}
