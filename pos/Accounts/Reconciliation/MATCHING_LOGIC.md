# Advanced Reconciliation Matcher - Matching Logic

## Overview
The Advanced Reconciliation Matcher uses a two-tier matching strategy to link journal entries with sales/purchase invoices:

1. **Reference Number Matching** (Priority 1) - Most accurate
2. **Amount-based Matching** (Priority 2) - Fallback

## Database Schema Context

### Journal Entries (acc_entries_header)
- **invoice_no**: Contains the sales/purchase invoice number (when linked)
- **reference_no**: Alternative field for storing reference number

### Journal Entry Details (acc_entries)
- **payment_ref_invoice_no**: Payment reference storing sales/purchase invoice number

### Sales/Purchase Data
- **invoice_no**: The sales or purchase invoice number to be matched

## Matching Strategy

### Phase 1: Reference Number Matching (Exact Match)
This is the PRIMARY matching method because it's based on explicit relationships stored in the database.

**Process:**
```
For each unreconciled journal entry:
  1. Extract invoice_no from the journal entry
  2. Search for an exact match in Sales entries where invoice_no matches
  3. If found → Create reconciliation match with REFERENCE_BASED flag
  4. If NOT found → Try purchases by reference number
  5. If still NOT found → Proceed to Phase 2 (Amount-based matching)
```

**Why this works:**
- When a sale/purchase is recorded in the journal, its invoice number is stored in `acc_entries_header.invoice_no`
- An exact match is more reliable than amount matching
- This prevents mismatches where similar amounts exist for different invoices

### Phase 2: Amount-based Matching (Fallback)
Used when reference numbers don't match or journal entry has no invoice reference.

**Process:**
```
For each unmatched journal entry:
  1. Calculate journal entry amount (debit or credit)
  2. Search for sales entries where:
	 ABS(sales_total_amount - journal_amount) < tolerance
  3. If found → Create reconciliation match with AMOUNT_BASED flag
  4. If NOT found → Try purchases with same logic
  5. If nothing found → Entry remains unreconciled
```

**Tolerance Settings:**
- User can set tolerance amount via `numTolerance` spinner
- Default: 1 (SAR/currency unit)
- Used to handle rounding differences or minor discrepancies

## Auto-Match Algorithm

### Logic Flow
```
AutoMatchEntries()
├─ For each unreconciled journal entry
│  ├─ Try Sales Reference Match
│  │  └─ Compare invoice_no exactly
│  ├─ If not found → Try Sales Amount Match
│  │  └─ Compare total_amount ± tolerance
│  ├─ If still not found → Try Purchase Reference Match
│  │  └─ Compare invoice_no exactly
│  └─ If still not found → Try Purchase Amount Match
│     └─ Compare total_amount ± tolerance
└─ Display all matches found with Match Score
```

### Match Score Calculation
```csharp
MatchScore = (1 - ABS(journalAmount - matchAmount) / journalAmount) * 100
```

**Examples:**
- Exact match (10,000 = 10,000) → Score: 100%
- 1% difference (10,000 ≠ 10,100) → Score: 99%
- 5% difference (10,000 ≠ 10,500) → Score: 95%
- 0 journal amount → Score: 0% (undefined)

## Manual Matching

Users can override automatic matching by:
1. Selecting a journal entry in the left panel
2. Clicking a sales/purchase entry in the tabs
3. Clicking "Add Manual Match"
4. Clicking "Apply Matches"

## Important Notes

### Reference Number Linking
- **Sales invoices**: When creating a sale, the system stores the invoice_no in:
  - `acc_entries_header.invoice_no` (if journal created immediately)
  - `acc_entries.payment_ref_invoice_no` (optional reference field)

- **Purchase invoices**: Similar to sales, stored in:
  - `acc_entries_header.invoice_no`
  - `acc_entries.payment_ref_invoice_no`

### Column Mappings
The matcher searches these columns:

| Type | Table | Column | Purpose |
|------|-------|--------|---------|
| Sales | acc_entries_header | invoice_no | Primary key matching |
| Sales | tbl_sales | invoice_no | Sales invoice reference |
| Purchase | acc_entries_header | invoice_no | Primary key matching |
| Purchase | tbl_purchases | invoice_no | Purchase invoice reference |

### Data Validation
- `is_reconciled` is stored as SQL `bit` (0 = unreconciled, 1 = reconciled)
- Amount fields are cast to `double` with null safety (`?? 0`)
- Invoice numbers are case-insensitive string comparisons

## Reconciliation Status

Once matches are applied:
- Journal entry `is_reconciled` is set to 1
- `reconcile_date` is set to current date/time
- `reconciled_by_user_id` is recorded for audit trail

## Example Scenario

### Scenario: Multiple Sales in One Day
```
Sales Invoice #INV001: 5,000 SAR
Sales Invoice #INV002: 5,000 SAR
Journal Entry #JE001: 5,000 SAR (reference = INV001)
Journal Entry #JE002: 5,000 SAR (reference = INV002)
```

**Without Reference Matching (broken):**
- JE001 might match INV002 or vice versa (arbitrary)

**With Reference Matching (working):**
- JE001 explicitly matches INV001 ✓
- JE002 explicitly matches INV002 ✓

## Performance Considerations

- Reference matching is O(n) per journal entry
- Amount matching also O(n) with tolerance comparison
- Large datasets (1000+ entries) may take 1-2 seconds for auto-match
- UI shows progress via cursor changes (WaitCursor)

## Future Enhancements

1. **Multi-match Detection**: Warn if one journal entry matches multiple sales/purchases
2. **Partial Matching**: Allow matching of partial amounts
3. **Date Validation**: Add date range validation for matches
4. **Audit Trail**: Log all manual match overrides
5. **Batch Operations**: Allow reconciling multiple entries at once
