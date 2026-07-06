// ============================================================================
// USAGE EXAMPLES: Chart of Accounts Code Generation
// ============================================================================

// Example 1: Generate Codes for All Existing Data (One-Time Setup)
// ============================================================================

public void SetupCodesForExistingData()
{
    using (CodesUpdateHelper helper = new CodesUpdateHelper())
    {
        // Get current statistics
        var stats = helper.GetCodeAssignmentStats();

        Console.WriteLine("=== Current Code Coverage ===");
        Console.WriteLine($"Level-1 Groups: {stats.Level1GroupsWithCodes}/{stats.Level1GroupsTotal} ({stats.Level1GroupsCoverage:F2}%)");
        Console.WriteLine($"Level-2 Groups: {stats.Level2GroupsWithCodes}/{stats.Level2GroupsTotal} ({stats.Level2GroupsCoverage:F2}%)");
        Console.WriteLine($"Accounts: {stats.AccountsWithCodes}/{stats.AccountsTotal} ({stats.AccountsCoverage:F2}%)");

        if (stats.Level1GroupsMissing > 0 || stats.Level2GroupsMissing > 0 || stats.AccountsMissing > 0)
        {
            // Generate codes for missing records
            var result = helper.UpdateAllCodes();

            if (result.IsSuccess)
            {
                MessageBox.Show($"Success!\n\n{result.Message}", "Code Generation Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error: {result.Message}", "Code Generation Failed", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("All groups and accounts already have codes.", "No Action Needed");
        }
    }
}

// ============================================================================
// Example 2: Add Menu Item to Admin Form
// ============================================================================

private void AddMaintenanceMenuItems()
{
    // In your main admin menu or settings form:

    ToolStripMenuItem maintenanceItem = new ToolStripMenuItem("Maintenance");
    ToolStripMenuItem codesItem = new ToolStripMenuItem("Generate Account Codes");
    codesItem.Click += (s, e) => 
    {
        var form = new frm_codes_maintenance();
        form.ShowDialog(this);
    };

    maintenanceItem.DropDownItems.Add(codesItem);

    // Add to your menu strip:
    // menuStrip1.Items.Add(maintenanceItem);
}

// ============================================================================
// Example 3: Automatic Code Generation During New Group Addition
// ============================================================================

public class frm_addGroup_Example
{
    private ChartOfAccountsBLL _coaBll = new ChartOfAccountsBLL();

    public void AddNewGroupWithAutoCode()
    {
        try
        {
            // When user clicks "Save" for a new group:

            GroupsModal groupInfo = new GroupsModal
            {
                name = txt_name.Text,
                name_2 = txt_name_2.Text,
                parent_id = Convert.ToInt32(cmb_parent_id.SelectedValue),
                account_type_id = Convert.ToInt32(cmb_account_type.SelectedValue),
                description = txt_description.Text
            };

            // If code field is empty, auto-generate
            if (string.IsNullOrWhiteSpace(txt_group_code.Text))
            {
                groupInfo.code = _coaBll.GenerateAccountCode(groupInfo.parent_id);
                txt_group_code.Text = groupInfo.code;
            }
            else
            {
                groupInfo.code = txt_group_code.Text;
            }

            // Save to database
            GroupsBLL bll = new GroupsBLL();
            int result = bll.Insert(groupInfo);

            if (result > 0)
            {
                MessageBox.Show($"Group created with code: {groupInfo.code}", "Success");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error");
        }
    }
}

// ============================================================================
// Example 4: Scheduled Background Update (Optional)
// ============================================================================

public class ScheduledCodeGeneration
{
    private Timer _dailyCheckTimer;

    public void StartScheduledCodeGeneration()
    {
        // Check daily at 2 AM for any records missing codes
        _dailyCheckTimer = new Timer();
        _dailyCheckTimer.Interval = 24 * 60 * 60 * 1000; // 24 hours
        _dailyCheckTimer.Tick += (s, e) => CheckAndGenerateMissingCodes();
        _dailyCheckTimer.Start();
    }

    private void CheckAndGenerateMissingCodes()
    {
        try
        {
            var helper = new CodesUpdateHelper();
            var stats = helper.GetCodeAssignmentStats();

            // If there are missing codes, auto-generate them
            if (stats.Level1GroupsMissing > 0 || stats.Level2GroupsMissing > 0 || stats.AccountsMissing > 0)
            {
                var result = helper.UpdateAllCodes();

                // Log the result
                if (result.IsSuccess)
                {
                    LogAction($"Scheduled Code Generation: {result.Message}");
                }
                else
                {
                    LogAction($"Scheduled Code Generation Failed: {result.Message}", isError: true);
                }
            }
        }
        catch (Exception ex)
        {
            LogAction($"Error in scheduled code generation: {ex.Message}", isError: true);
        }
    }

    private void LogAction(string message, bool isError = false)
    {
        // Implement your logging logic
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {(isError ? "ERROR" : "INFO")}: {message}");
    }
}

// ============================================================================
// Example 5: Export Codes Report
// ============================================================================

public void ExportCodesReport()
{
    try
    {
        var helper = new CodesUpdateHelper();
        var stats = helper.GetCodeAssignmentStats();

        string report = $@"
=================================================================
        CHART OF ACCOUNTS - CODE GENERATION REPORT
=================================================================

Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}

LEVEL-1 GROUPS (Root Groups)
  Total Records:       {stats.Level1GroupsTotal}
  With Codes:          {stats.Level1GroupsWithCodes}
  Missing Codes:       {stats.Level1GroupsMissing}
  Coverage:            {stats.Level1GroupsCoverage:F2}%

LEVEL-2 GROUPS (Sub-groups)
  Total Records:       {stats.Level2GroupsTotal}
  With Codes:          {stats.Level2GroupsWithCodes}
  Missing Codes:       {stats.Level2GroupsMissing}
  Coverage:            {stats.Level2GroupsCoverage:F2}%

ACCOUNTS (Level-3)
  Total Records:       {stats.AccountsTotal}
  With Codes:          {stats.AccountsWithCodes}
  Missing Codes:       {stats.AccountsMissing}
  Coverage:            {stats.AccountsCoverage:F2}%

=================================================================
SUMMARY
  Overall Coverage:    {((stats.Level1GroupsWithCodes + stats.Level2GroupsWithCodes + stats.AccountsWithCodes) * 100.0 / (stats.Level1GroupsTotal + stats.Level2GroupsTotal + stats.AccountsTotal)):F2}%
  Status:              {(stats.Level1GroupsMissing == 0 && stats.Level2GroupsMissing == 0 && stats.AccountsMissing == 0 ? "✓ COMPLETE" : "⚠ INCOMPLETE")}
=================================================================
";

        // Save to file or display
        string fileName = $"COA_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        File.WriteAllText(fileName, report);

        MessageBox.Show($"Report exported to: {fileName}", "Success");
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error: {ex.Message}", "Error");
    }
}

// ============================================================================
// Example 6: Validate Codes After Import/Migration
// ============================================================================

public void ValidateCodesAfterImport()
{
    try
    {
        var helper = new CodesUpdateHelper();
        var stats = helper.GetCodeAssignmentStats();

        if (!string.IsNullOrEmpty(stats.Error))
        {
            MessageBox.Show($"Error retrieving stats: {stats.Error}", "Error");
            return;
        }

        var issues = new List<string>();

        // Check for incomplete assignments
        if (stats.Level1GroupsCoverage < 100)
            issues.Add($"Level-1 Groups: {stats.Level1GroupsMissing} missing codes");

        if (stats.Level2GroupsCoverage < 100)
            issues.Add($"Level-2 Groups: {stats.Level2GroupsMissing} missing codes");

        if (stats.AccountsCoverage < 100)
            issues.Add($"Accounts: {stats.AccountsMissing} missing codes");

        if (issues.Count > 0)
        {
            string message = "Found issues:\n\n" + string.Join("\n", issues) + 
                           "\n\nWould you like to generate missing codes now?";

            if (MessageBox.Show(message, "Import Validation", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var result = helper.UpdateAllCodes();
                MessageBox.Show(result.Message, result.IsSuccess ? "Success" : "Error");
            }
        }
        else
        {
            MessageBox.Show("✓ All codes are valid and complete!", "Validation Passed");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error: {ex.Message}", "Error");
    }
}

// ============================================================================
// Example 7: User-Friendly Statistics Display
// ============================================================================

public void DisplayCodeStatistics()
{
    var helper = new CodesUpdateHelper();
    var stats = helper.GetCodeAssignmentStats();

    // Create a summary for display
    string summary = $@"
Chart of Accounts Code Summary:

Level-1 Groups:  {stats.Level1GroupsWithCodes:D3}/{stats.Level1GroupsTotal:D3}  [{stats.Level1GroupsCoverage,6:F1}%]  {GetProgressBar(stats.Level1GroupsCoverage)}

Level-2 Groups:  {stats.Level2GroupsWithCodes:D3}/{stats.Level2GroupsTotal:D3}  [{stats.Level2GroupsCoverage,6:F1}%]  {GetProgressBar(stats.Level2GroupsCoverage)}

Accounts:        {stats.AccountsWithCodes:D6}/{stats.AccountsTotal:D6}  [{stats.AccountsCoverage,6:F1}%]  {GetProgressBar(stats.AccountsCoverage)}
";

    Console.WriteLine(summary);
}

private string GetProgressBar(double percentage, int barWidth = 20)
{
    int filled = (int)((percentage / 100) * barWidth);
    return "[" + new string('█', filled) + new string('░', barWidth - filled) + "]";
}

// ============================================================================
// Example 8: Integration with Existing AccountsBLL
// ============================================================================

public class AccountsBLLExtensions
{
    private ChartOfAccountsBLL _coaBll = new ChartOfAccountsBLL();

    public int InsertAccountWithAutoCode(AccountsModal account, int groupId)
    {
        // Auto-generate code if not provided
        if (string.IsNullOrWhiteSpace(account.code))
        {
            account.code = _coaBll.GenerateAccountCode(groupId);
        }

        // Validate code uniqueness
        if (!_coaBll.IsCodeUnique(account.code, excludeAccountId: account.id))
        {
            throw new InvalidOperationException($"Account code '{account.code}' already exists.");
        }

        // Set group_id if not set
        if (account.group_id == 0)
        {
            account.group_id = groupId;
        }

        // Insert via existing AccountsBLL
        var bll = new AccountsBLL();
        return bll.Insert(account);
    }
}

// ============================================================================
// Example 9: Verify Codes Before and After
// ============================================================================

public void VerifyCodeGeneration()
{
    var helper = new CodesUpdateHelper();

    // Before
    Console.WriteLine("Before code generation:");
    var statsBefore = helper.GetCodeAssignmentStats();
    PrintStats(statsBefore);

    // Generate
    Console.WriteLine("\nGenerating codes...");
    var result = helper.UpdateAllCodes();
    Console.WriteLine(result.Message);

    // After
    Console.WriteLine("\nAfter code generation:");
    var statsAfter = helper.GetCodeAssignmentStats();
    PrintStats(statsAfter);

    // Summary
    Console.WriteLine("\nChanges:");
    Console.WriteLine($"  Level-1 Groups: {statsBefore.Level1GroupsMissing} → {statsAfter.Level1GroupsMissing}");
    Console.WriteLine($"  Level-2 Groups: {statsBefore.Level2GroupsMissing} → {statsAfter.Level2GroupsMissing}");
    Console.WriteLine($"  Accounts: {statsBefore.AccountsMissing} → {statsAfter.AccountsMissing}");
}

private void PrintStats(CodeAssignmentStats stats)
{
    Console.WriteLine($"  Level-1: {stats.Level1GroupsWithCodes}/{stats.Level1GroupsTotal} ({stats.Level1GroupsCoverage:F1}%)");
    Console.WriteLine($"  Level-2: {stats.Level2GroupsWithCodes}/{stats.Level2GroupsTotal} ({stats.Level2GroupsCoverage:F1}%)");
    Console.WriteLine($"  Accounts: {stats.AccountsWithCodes}/{stats.AccountsTotal} ({stats.AccountsCoverage:F1}%)");
}

