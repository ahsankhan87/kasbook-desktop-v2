# Stock Suppression - Quick Reference for Users

## How to Use the Enhanced Stock Suppression Form

### ✨ New Features
1. **Type Directly** - Enter item codes directly in the textboxes
2. **Auto-Search** - System automatically searches 500ms after you stop typing
3. **Smart Search** - Tries item_number first, then product code
4. **Visual Feedback** - Colors indicate success (green), errors (red), or warnings (orange)

---

## Step-by-Step Workflow

### Finding the Old Part (Item to be Superseded)

**Option A: Type to Auto-Search**
1. Click in **"Old Part Code"** textbox
2. Type the item code or part number (e.g., `ABC-123` or `OLD-SKU`)
3. **Stop typing** → Wait 500ms
4. System searches and shows product name (e.g., "ABC-123 (Old Widget)")
5. ✅ If found: Label shows green with product details
6. ❌ If not found: Label shows red "Product not found"

**Option B: Browse & Search**
1. Click **"Search..."** button next to Old Part Code
2. Search dialog opens
3. Type your search term in the dialog
4. Select the product from the grid
5. Click **OK** - Product code auto-fills in main form

**Option C: Pre-Typed Search**
1. Type partial code in textbox (e.g., "OLD")
2. Click **"Search..."** button
3. Dialog opens with your typed text pre-filled
4. Browse results and select desired product

### Checking Supersession Status

After selecting an old part, you'll see one of:
- **"Not superseded"** (green) - Good to proceed
- **"Already superseded to: XYZ-789 (New Part)"** (orange) - Item already linked
- **Error message** (red) - Try a different product

ℹ️ If already superseded, you can still create a chain supersession (old → intermediate → new)

### Selecting Options

Configure what should happen during supersession:

| Option | Meaning | Default |
|--------|---------|---------|
| ✓ **Transfer Stock** | Move inventory from old to new item | ✅ ON |
| ✓ **Zero Out Demand** | Clear demand quantities on old item | ✅ ON |
| ✓ **Transfer Description** | Copy old item's description to new item (if new is empty) | ⏳ OFF |
| ✓ **Reset Reorder Level** | Set reorder level to 0 on new item | ✅ ON |

### Selecting Branches

1. Click **"Select Company/Branch"** button
2. Dialog shows all available branches
3. Check boxes next to branches where stock should transfer
4. Click **OK**
5. Label shows "X branch(es) selected"

### Finding the New Part (Replacement Item)

1. Repeat same process as **Old Part**:
   - Type directly and wait for auto-search, OR
   - Click Search to browse

2. New Part textbox will auto-populate when found

### Executing the Supersession

1. **Review all settings:**
   - Old Part: ✓ Populated
   - New Part: ✓ Populated
   - Branches: ✓ Selected (1+)
   - Options: ✓ Configured

2. Click **"Execute Supersession"** button

3. Confirmation dialog appears showing:
   - What will happen (stock transfer, history preservation)
   - Click **"Yes"** to proceed
   - Click **"No"** to cancel

4. **Processing...** indicator appears
   - System transfers stock
   - Creates supersession link
   - Preserves history

5. Success message confirms:
   - "Old item preserved with history"
   - "New item ready for future transactions"
   - "Supersession link established"

---

## Color Codes & Meanings

| Color | Meaning | Action |
|-------|---------|--------|
| 🟢 Green | Success / Ready | Proceed |
| 🔴 Red | Error / Not Found | Fix issue |
| 🟠 Orange | Warning / Already Superseded | Review |
| ⚫ Black | Normal status | No action needed |

---

## Tips & Tricks

### Speed Tips
- **Fastest:** Type 2-3 characters → Wait 500ms → Auto-search finds it
- Use **distinctive codes** (e.g., first 3 letters of part name)
- If unsure, use **Search button** to browse options

### Accuracy Tips
- **Verify product names** match what you expect
- **Check stock quantity** (shown in grid when browsing)
- **Review supersession status** before proceeding
- **Test with 1 branch first** if unsure

### Undo Options
- ❌ **Cannot undo** supersession directly
- ✅ **Workaround:** Create reverse link (New → Old) to revert relationship
- 📋 **Track changes:** Check audit log for supersession history

---

## Common Scenarios

### Scenario 1: Replace Worn-Out Widget
```
Old Part: WIDGET-OLD-123 (300 units in stock)
New Part: WIDGET-NEW-456 (replacement model)
Branches: Main store only
Options: ✓Transfer Stock, ✓Zero Demand

Result: 300 units moved to new part, old part marked as obsolete
```

### Scenario 2: Consolidate Stock
```
Old Part: PART-A-DUP (50 units)
Old Part: PART-B-DUP (75 units) ← Already superseded to PART-MAIN
New Part: PART-MAIN (consolidated)
Branches: All branches
Options: ✓Transfer Stock, ✓Transfer Description

Result: Creates chain link: A→MAIN, B→MAIN for full traceability
```

### Scenario 3: Manufacturer Recall
```
Old Part: RECALL-123 (all units recalled)
New Part: RECALL-123-FIXED (safe replacement)
Options: ✓Transfer Stock, ✓Zero Demand, ✓Reset Reorder

Result: Old item marked unsafe, demand zeroed, new ready for orders
```

---

## Troubleshooting

### "Product not found" after typing
- Check spelling of item code
- Item code might be different than expected
- Use Search button to browse available products
- Contact inventory manager if product doesn't exist

### Search not working
- Ensure you've typed at least 1 character
- Wait 500ms after typing stops
- Try Search button for manual search
- Check database connectivity

### Can't proceed to execution
- Verify **both** Old and New parts selected
- Verify **at least one** branch selected
- Ensure parts are **different** (can't supersede item to itself)
- Check for error messages (red text)

### Already superseded warning
- Item already has a supersession link
- You can still create a chain (old→intermediate→new) if needed
- Or select a different old part
- Click **Help** for chain supersession info

---

## Need Help?

Click **"Help"** button in the form for:
- Detailed workflow explanation
- Feature descriptions
- Best practices
- Contact information

**Also available in Arabic (العربية)** - Bilingual interface
