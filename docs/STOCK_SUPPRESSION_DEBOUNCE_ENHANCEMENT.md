# Stock Suppression UX Enhancement - Debounced Search

## Overview
Enhanced the stock suppression workflow with editable part-code textboxes and debounced auto-search functionality, allowing users to quickly find products by typing item codes directly.

## Features Implemented

### 1. **Editable Textboxes**
- `txtOldPartCode` and `txtNewPartCode` are now fully editable
- Users can type item codes or part numbers directly
- No need to click a button to start searching (though search buttons still available)

### 2. **Debounced Auto-Search (500ms Delay)**
- As users type in the code textboxes, a 500ms debounce timer is triggered
- After the timer expires, the system automatically searches for matching products
- Prevents excessive database queries while typing
- Uses `System.Windows.Forms.Timer` for WinForms compatibility with C# 7.3

### 3. **Intelligent Search Resolution**
- First searches by `item_number` (full match priority)
- Falls back to search by product `code` if item number not found
- Displays product name and code in formatted label (e.g., "ABC-123 (Product Name)")
- Color-coded feedback:
  - **Green** (TextPrimary): Success
  - **Red**: Product not found
  - **Orange**: Item already superseded

### 4. **Search Dialog Integration**
- When clicking the Search button, the dialog pre-fills with any typed code
- Dialog automatically loads search results if initial term is provided
- Auto-selects single-result items for faster workflow
- User can still browse multiple results if needed

### 5. **Session State Tracking**
- Tracks last searched terms to avoid redundant queries
- Maintains search state across textbox edits
- Properly cleans up timers on form closure

## Code Architecture

### Timer Management
```csharp
private System.Windows.Forms.Timer _oldPartDebounceTimer;
private System.Windows.Forms.Timer _newPartDebounceTimer;
private const int DEBOUNCE_DELAY_MS = 500;

private void InitializeDebounceTimers()
{
    // Each timer triggers its corresponding search method
    // Timer is stopped and restarted on each TextChanged event
}
```

### Event-Driven Search
```csharp
private void TxtOldPartCode_TextChanged(object sender, EventArgs e)
{
    // Stop any existing timer
    _oldPartDebounceTimer.Stop();
    // Restart with new input
    _oldPartDebounceTimer.Start();
}
```

### Search Implementation
```csharp
private void PerformOldPartSearch()
{
    // 1. Trim and validate input
    // 2. Search by item_number first
    // 3. Fallback to search by code
    // 4. Update label and load supersession info
    // 5. Color-code feedback
}
```

## User Workflow

### Scenario 1: Quick Entry (Debounce)
1. User types "ABC-123" in Old Part Code textbox
2. After 500ms of no typing, auto-search triggers
3. Product found and populated
4. Label shows "ABC-123 (Part Name)"
5. User can proceed to new part

### Scenario 2: Browse & Select
1. User types partial code "AB" in textbox
2. After 500ms, system attempts search (may or may not find)
3. User clicks Search button with "AB" pre-filled
4. Dialog opens with search results
5. User selects desired product from grid

### Scenario 3: Manual Dialog Entry
1. User clicks Search button without typing
2. Dialog opens empty (user can search from scratch)
3. User types code/name and sees results
4. Selection made and returned to main form

## Configuration

### Debounce Delay
```csharp
private const int DEBOUNCE_DELAY_MS = 500; // Milliseconds
```
**Adjustable:** Change this constant to modify delay (e.g., 300 for faster, 800 for slower)

### Search Columns (ProductsDLL)
The underlying search queries use these columns (in priority order):
1. `part_number` - Primary search
2. `code` - Secondary search
3. `name` - Text search
4. `description` - Fallback search

## Performance Implications

### Positive
- **Reduced DB Load:** Debounce prevents query spam during rapid typing
- **Faster User Interaction:** No dialog lag; immediate visual feedback
- **Better UX:** Auto-completion reduces clicks

### Neutral
- **Timer Overhead:** Minimal; only 2 timers per form instance
- **Memory:** Negligible addition (timers + string tracking)

## Technical Details

### Event Handling
- Named event handlers (not lambdas) prevent variable shadowing in C# 7.3
- `OnFormClosing` override ensures proper timer disposal
- TextChanged detection uses `String.Equals` with case-insensitive comparison

### UI Feedback
- Labels use `AppTheme.TextPrimary` color for consistency
- Error states displayed in red (`System.Drawing.Color.Red`)
- Warnings in orange (`System.Drawing.Color.Orange`)
- Integrates with existing `UiMessages` for bilingual support

### Database Calls
- Reuses existing `ProductBLL` methods:
  - `SearchRecordByProductNumber(string condition)` - Instance method
  - `SearchProductByBrandAndCategory(...)` - Static method
- No new database schema required
- Leverages existing `superseded_to_item_code` field for status checks

## Backwards Compatibility

✅ **Fully compatible** with existing code:
- No breaking changes to ProductDLL or ProductBLL signatures
- Dialog still accepts standard parameters
- Existing button/menu integration unchanged
- Supersession logic preserved from previous implementation

## Files Modified

1. **pos/Products/Suppression/frm_stock_suppression.cs**
   - Added debounce timers and initialization
   - Wired TextChanged events
   - Implemented PerformOldPartSearch() and PerformNewPartSearch()
   - Enhanced help text with feature description

2. **pos/Products/Suppression/frm_stock_suppression_stock_records.cs**
   - Added optional initialSearchTerm parameter to constructor
   - Pre-fills txtSearch if term provided
   - Auto-loads results on form load
   - Auto-selects single results

## Future Enhancement Opportunities

1. **Search History:** Cache recent searches to offer suggestions
2. **Barcode Support:** Direct barcode scanner integration
3. **Multi-Field Search:** Search across multiple product attributes simultaneously
4. **Advanced Filtering:** Add filters for stock level, category, brand
5. **Keyboard Shortcuts:** E.g., Ctrl+S to focus search, Enter to select first result

## Testing Checklist

- [ ] Type valid item code → Auto-search finds product → Label populates
- [ ] Type invalid code → "Product not found" error after 500ms
- [ ] Rapidly change textbox content → Only final search executes
- [ ] Click Search with pre-filled code → Dialog opens with results
- [ ] Dialog returns selection → Main form updates correctly
- [ ] Form closes → Timers disposed without exceptions
- [ ] Already-superseded item shows orange warning label
- [ ] Chain supersession scenario works (old → intermediate → new)

## Troubleshooting

| Issue | Cause | Solution |
|-------|-------|----------|
| Search not triggering | Text length < 1 | Ensure at least 1 character typed |
| Slow search response | Debounce too long or slow DB | Reduce DEBOUNCE_DELAY_MS or index database |
| Duplicate searches | Timer not stopping properly | Check timer.Stop() call in TextChanged |
| Form won't close | Timers not disposed | Verify OnFormClosing cleanup code |

## Conclusion

The debounce enhancement provides a modern, responsive UX for the stock suppression workflow while maintaining code quality, performance, and compatibility with the existing application architecture.
