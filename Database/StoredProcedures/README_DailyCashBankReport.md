# Daily Cash & Bank Opening/Closing Report

## Overview
This report provides a comprehensive view of daily cash and bank balances, showing opening balances, receipts, payments, and closing balances. It includes a variance tracking feature to compare physical cash/bank counts against system balances.

## Features
- **Single Day Mode**: View detailed cash and bank movements for a specific day
- **Date Range Mode**: Analyze trends across multiple days
- **Consolidated View**: See cash and bank totals in separate columns
- **By-Account View**: Detailed breakdown per cash/bank account
- **Variance Tracking**: Compare actual physical counts vs system closing balances
- **Export to CSV**: Export report data for further analysis
- **Branch Filtering**: Automatically filters by user's branch

## Installation

### Step 1: Deploy the Stored Procedure
Run the SQL script to create the stored procedure in your database:

```sql
-- File: Database/StoredProcedures/sp_DailyCashBankReport.sql
-- Execute this script in your SQL Server database
```

The stored procedure `sp_DailyCashBankReport` accepts the following parameters:
- `@FromDate` (DATE, optional): Start date for the report (defaults to today)
- `@ToDate` (DATE, optional): End date for the report (defaults to today)
- `@BranchId` (INT, optional): Filter by specific branch
- `@AccountId` (INT, optional): Filter by specific cash/bank account

### Step 2: Access the Report
After deployment, the report is accessible from the main menu:

**Finance → Cash Management → Daily Cash & Bank Report**

## How to Use

### Single Day Mode
1. Select "Single Day" mode
2. Choose the date
3. Click "Load Report"
4. The variance panel shows:
   - **System Closing**: Balance according to software
   - **Actual**: Enter your physical cash/bank count
   - **Variance**: Automatically calculated difference (green = surplus, red = shortage)

### Date Range Mode
1. Select "Date Range" mode
2. Choose from and to dates
3. Click "Load Report"
4. View daily trends in consolidated or by-account format

### View Options
- **Consolidated**: Shows daily totals with separate Cash and Bank columns
- **By Account**: Shows detailed breakdown for each cash/bank account

### Columns Explained

**Consolidated View:**
- **Date**: Transaction day
- **Cash Opening Balance**: Cash balance at start of day
- **Cash Receipts**: Total cash received during the day
- **Cash Payments**: Total cash paid out during the day
- **Cash Closing Balance**: Cash balance at end of day
- **Bank Opening Balance**: Bank balance at start of day
- **Bank Receipts**: Total bank deposits during the day
- **Bank Payments**: Total bank withdrawals during the day
- **Bank Closing Balance**: Bank balance at end of day
- **Total Opening/Closing/Receipts/Payments**: Combined cash + bank totals

**By-Account View:**
- All above columns plus:
- **Account Code**: Account identification code
- **Account Name**: Full account name
- **Type**: Cash or Bank classification

## Daily Tally Workflow (Single Day Mode)

This report is designed for end-of-day reconciliation:

1. **At End of Day**: Load today's report in single-day mode
2. **Count Physical Cash**: Count cash in drawer/safe
3. **Verify Bank Balance**: Check actual bank account balance
4. **Enter Actual Amounts**: Input physical counts in the variance panel
5. **Review Variance**:
   - Green (positive) = You have more than the system shows
   - Red (negative) = You have less than the system shows (shortage)
6. **Investigate Variances**: If variance is significant, review transactions for:
   - Missing entries
   - Incorrect amounts
   - Unrecorded cash/bank movements

## Technical Details

### Data Source
- **Tables**: `acc_entries`, `acc_accounts`
- **Classification**: Accounts marked with `is_cash = 1` or `is_bank = 1`
- **Calculation**: Opening balance = cumulative balance before start date; movements = entries within date range

### Architecture
- **BLL**: `POS.BLL.Accounts.AccountsBLL.GetDailyCashBankReport(...)`
- **DLL**: `POS.DLL.Accounts.AccountsDLL.GetDailyCashBankReport(...)`
- **Stored Procedure**: `sp_DailyCashBankReport`
- **UI Form**: `pos.Reports.Accounts.FrmDailyCashBankReport`

### Performance Considerations
- Report uses window functions for running balance calculations
- Large date ranges may take longer to compute
- Consider indexing `acc_entries.entry_date` and `acc_entries.account_id` if not already indexed

## Permissions
If your system uses role-based permissions, assign the appropriate permission tag to the menu item in `Main.Designer.cs`.

## Troubleshooting

**Problem**: "No data found for the selected period"
- **Solution**: Ensure you have cash/bank accounts properly classified with `is_cash` or `is_bank` flags

**Problem**: Opening balance seems incorrect
- **Solution**: Check transactions before the start date; opening balance is cumulative from all prior transactions

**Problem**: Variance panel not showing
- **Solution**: Variance panel only appears in Single Day mode; switch from Date Range mode

**Problem**: Account not appearing in report
- **Solution**: Verify the account has `is_cash = 1` or `is_bank = 1` in the `acc_accounts` table

## Support
For issues or questions, refer to the codebase documentation or contact your system administrator.
