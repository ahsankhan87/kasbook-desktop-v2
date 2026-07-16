# Accounting Import Engine - Error Threshold Validation

## Overview
The import engine implements a **50% error threshold** rule to prevent importing data with excessive validation errors. This ensures data quality and prevents partially-valid imports that could corrupt the accounting data.

## Implementation

### Core Logic
Located in `POS.Core/Accounts/ImportModels.cs`:

```csharp
public class ImportValidationResult
{
	public int TotalRows { get; set; }
	public int ValidRows { get; set; }
	public int InvalidRows { get; set; }
	public decimal ErrorRate => TotalRows > 0 ? (decimal)InvalidRows / TotalRows : 0;
	public bool ExceedsErrorThreshold => ErrorRate > 0.5m; // 50% threshold
}
```

### Validation Flow

#### Chart of Accounts Import
**File**: `POS.BLL/Accounts/ImportBLL.cs` → `ExecuteCOAImport`

```csharp
var validationResult = validator.Validate(rows);

if (validationResult.ExceedsErrorThreshold)
{
	result.Success = false;
	result.Message = $"Validation failed: {validationResult.ErrorRate:P0} error rate exceeds 50% threshold";
	result.Errors = validationResult.Errors;
	return result;
}
```

#### Opening Balance Import
**File**: `POS.BLL/Accounts/ImportBLL.cs` → `ExecuteOpeningBalanceImport`

```csharp
var validationResult = validator.ValidateWithAnalysis(rows);

if (!validationResult.IsValid || validationResult.ExceedsErrorThreshold)
{
	result.Success = false;
	result.Message = "Validation failed: " + (validationResult.ExceedsErrorThreshold
		? $"{validationResult.ErrorRate:P0} error rate exceeds 50% threshold"
		: "Trial balance is not balanced");
	result.Errors = validationResult.Errors;
	return result;
}
```

#### Journal History Import
**File**: `POS.BLL/Accounts/ImportBLL.cs` → `ExecuteJournalImport`

```csharp
var validationResult = validator.ValidateWithAnalysis(rows);

if (!validationResult.IsValid || validationResult.ExceedsErrorThreshold)
{
	result.Success = false;
	result.Message = "Validation failed: " + (validationResult.ExceedsErrorThreshold
		? $"{validationResult.ErrorRate:P0} error rate exceeds 50% threshold"
		: "One or more vouchers are not balanced");
	result.Errors = validationResult.Errors;
	return result;
}
```

## Decision Logic

### Threshold Check
```
ErrorRate = InvalidRows / TotalRows
ExceedsThreshold = ErrorRate > 0.50 (50%)
```

### Import Decisions

| Error Rate | Action | Reason |
|------------|--------|--------|
| 0% - 50% | **Allow import** | Valid rows imported, errors logged | 
| > 50% | **Abort import** | Too many errors, user must fix file |

### Examples

#### Example 1: Allow Import
- Total Rows: 100
- Invalid Rows: 40
- Error Rate: 40%
- **Result**: Import proceeds with 60 valid rows

#### Example 2: Abort Import
- Total Rows: 100
- Invalid Rows: 55
- Error Rate: 55%
- **Result**: Import aborted, user must fix file

#### Example 3: Edge Case (Exactly 50%)
- Total Rows: 100
- Invalid Rows: 50
- Error Rate: 50%
- **Result**: Import proceeds (threshold is `> 0.5`, not `>= 0.5`)

## User Experience

### Success Flow (< 50% errors)
1. User uploads file with validation errors
2. System shows: "40% of rows have errors. Do you want to import the 60 valid rows?"
3. User confirms
4. Valid rows imported
5. Error report generated with failed rows

### Abort Flow (> 50% errors)
1. User uploads file with too many errors
2. System shows: "Validation failed: 55% error rate exceeds 50% threshold"
3. Import aborted immediately
4. Error report shown with all validation issues
5. User must fix file and re-upload

## Error Reporting

All validators provide detailed error lists in `ImportValidationResult.Errors`:

```csharp
public class ImportErrorModal
{
	public int RowNumber { get; set; }
	public string ErrorType { get; set; }
	public string ErrorMessage { get; set; }
	public string RowData { get; set; }
}
```

### Error Types
- **VALIDATION**: Field-level validation error
- **TRIAL_BALANCE**: Opening balance not balanced
- **UNBALANCED_VOUCHER**: Journal voucher Dr ≠ Cr
- **CIRCULAR_REFERENCE**: Account hierarchy loop
- **DUPLICATE_VOUCHER**: Duplicate voucher number
- **NO_DATA**: Empty file
- **NO_VALID_DATA**: All rows failed validation

## Integration with UI

The error threshold is checked **before** any data is written to the database, ensuring:
- No partial imports when data quality is poor
- Clear error messages to users
- Detailed error logs for troubleshooting
- Safe rollback if import is allowed but fails

## Configuration

The threshold is currently hardcoded to **50%** in `ImportModels.cs`:

```csharp
public bool ExceedsErrorThreshold => ErrorRate > 0.5m; // 50% threshold
```

To change the threshold, modify this line. Examples:
- **30% threshold**: `ErrorRate > 0.3m`
- **70% threshold**: `ErrorRate > 0.7m`

## Testing Checklist

- [x] Threshold calculation correct (InvalidRows / TotalRows)
- [x] Abort when error rate > 50%
- [x] Allow when error rate ≤ 50%
- [x] Edge case: exactly 50% errors (should allow)
- [x] Error message includes percentage
- [x] Detailed error list provided
- [x] No database writes when threshold exceeded
- [x] Import session not created when aborted early

## Benefits

1. **Data Quality**: Prevents importing heavily corrupted data
2. **User Feedback**: Clear messaging about what went wrong
3. **Safety**: No partial imports with poor data quality
4. **Efficiency**: Catches bad files early, before DB operations
5. **Auditability**: All errors logged for review
