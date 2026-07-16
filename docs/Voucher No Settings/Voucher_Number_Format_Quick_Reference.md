# Voucher Number Format - Quick Reference Guide

## What's New?
The accounting settings now support advanced voucher numbering that automatically includes:
1. **Prefix** - Custom letter code (e.g., S, JV, CR)
2. **Branch ID** - Auto-populated from your login session
3. **Date** - Optional date in your chosen format
4. **Counter** - Sequential number that resets based on your policy

## Voucher Number Examples

### Sales Invoices
```
Configuration:
- Prefix: S
- Date Format: YYYYMMDD
- Number Format: YYYY-NNNN
- Starting Number: 0001

Example Generated: S1-20260713-2026-0001
				   │ │ ││││││|| ││││ ████
				   │ │ ││││││|| ││││ └─ Counter (0001)
				   │ │ ││││││|| └───── Year (2026)
				   │ │ └─────────────── Date (20260713 = July 13, 2026)
				   │ └─ Branch ID (1)
				   └─ Prefix (S)
```

### Cash Receipts
```
Configuration:
- Prefix: CR
- Date Format: (leave blank)
- Number Format: NNNN
- Starting Number: 1000

Example Generated: CR1-1000
				   │││ ████
				   │││ └─ Counter (1000)
				   │└── Branch ID (1)
				   └─ Prefix (CR)
```

### Journal Vouchers with Detailed Date
```
Configuration:
- Prefix: JV
- Date Format: DD/MM/YYYY
- Number Format: NNNN
- Starting Number: 0001

Example Generated: JV1-13/07/2026-0001
				   ││ └──────────── ────
				   ││ Date Format  Counter
				   ││ (DD/MM/YYYY)
				   └─ Branch ID (1)
```

## Date Format Placeholders

| Placeholder | Example | Meaning |
|-----------|---------|---------|
| `YYYY` | 2026 | 4-digit year |
| `YY` | 26 | 2-digit year |
| `MM` | 07 | 2-digit month (with leading zero) |
| `DD` | 13 | 2-digit day (with leading zero) |

## Complete Date Format Combinations

| Format | Result | Use Case |
|--------|--------|----------|
| `YYYYMMDD` | 20260713 | Compact, sortable (Recommended) |
| `YYYY-MM-DD` | 2026-07-13 | ISO standard, readable |
| `YYMMDD` | 260713 | Short, saves space |
| `DD/MM/YYYY` | 13/07/2026 | Regional format |
| `YYYY/MM/DD` | 2026/07/13 | Alternative format |
| (blank) | (none) | Omit date from number |

## Number Format Options

| Format | Result | Notes |
|--------|--------|-------|
| `YYYY-NNNN` | 2026-0001 | Year included in counter |
| `YY-NNNN` | 26-0001 | 2-digit year in counter |
| `NNNN` | 0001 | Counter only (4 digits) |

## Reset Policies

| Policy | Behavior | Example |
|--------|----------|---------|
| **Daily** | Counter resets to starting number each day | Aug 1: 0001, Aug 2: 0001 |
| **Annually** | Counter resets on Jan 1 each year | Dec 2025: 9999, Jan 2026: 0001 |
| **Never** | Counter continues indefinitely | 0001, 0002, ... 9999, 10000, ... |
| **Per Financial Year** | Resets on FY start date (configured in settings) | Depends on FY setup |

## Step-by-Step Configuration

### To Set Up Sales Invoices

1. **Open** Settings → Accounting Settings → Voucher Configuration tab
2. **Find** the "SALES" or "JV" row (or appropriate voucher type)
3. **Set Prefix**: Enter `S` (or your preferred code)
4. **Branch ID**: Already filled with your branch number (read-only)
5. **Select Number Format**: Choose `YYYY-NNNN` (includes year in counter)
6. **Set Date Format**: Enter `YYYYMMDD` (for compact date like 20260713)
7. **Select Reset**: Choose `Annually`
8. **Starting Number**: Enter `1`
9. **Preview**: Should show something like `S1-20260713-2026-0001`
10. **Save**: Click "Save Settings" button

### To Omit Date from Voucher Number

- Leave the **Date Format** field empty
- Example result: `S1-2026-0001` (no date component)

### To Use Only a Counter

- Leave the **Date Format** field empty
- Set **Number Format** to `NNNN`
- Example result: `S1-0001` (just prefix, branch, and counter)

## Common Configurations by Industry

### Manufacturing / Inventory
```
Date Format: YYYYMMDD
Number Format: YYYY-NNNN
Reset: Per Financial Year
Example: MFG1-20260713-2026-0001
```

### Retail / POS
```
Date Format: DDMMYY
Number Format: NNNN
Reset: Daily
Example: SALE1-130726-0001
```

### Accounting / Government Compliance
```
Date Format: YYYY-MM-DD
Number Format: NNNN
Reset: Annually
Example: JV1-2026-07-13-0001
```

### Simple Sequential (Legacy)
```
Date Format: (blank)
Number Format: NNNN
Reset: Never
Example: CR1-0001, CR1-0002, ...
```

## Important Notes

⚠️ **Branch ID is Required**
- Branch ID is automatically populated from your login
- Each branch can have different voucher numbering
- Cannot be edited (read-only field)

⚠️ **Date Format is Case-Sensitive**
- Use uppercase: `YYYY`, `MM`, `DD`
- Lowercase will NOT be replaced
- Any other characters pass through unchanged

⚠️ **Preview Updates Live**
- The Preview column shows today's date
- Once you save and use it, actual vouchers will use the current date

⚠️ **Counter Format**
- Counters are zero-padded to 4 digits
- Examples: 0001, 0010, 0100, 1000
- After 9999, it wraps to 10000 (5 digits)

## Troubleshooting

| Problem | Cause | Solution |
|---------|-------|----------|
| Date not appearing | Date Format field is empty | Enter a date format (e.g., YYYYMMDD) |
| Wrong date in voucher | Format uses lowercase | Use uppercase: YYYY, MM, DD |
| Gibberish in number | Invalid placeholder | Check spelling: YYYY (not yyyy), MM (not mm), DD (not dd) |
| Preview doesn't match what I set | Cache not refreshed | Click elsewhere in grid, preview updates automatically |
| Settings not saved | Didn't click Save button | Click "Save Settings" button after changes |

## Support

For questions about:
- **Accounting Configuration**: Contact your System Administrator or CFO
- **Technical Issues**: Check the Accounting Settings form help or contact IT Support
- **Voucher Numbering Policy**: Consult your company's accounting procedures

---
*Last Updated: 2026-01-01*
*Version: 1.0*
