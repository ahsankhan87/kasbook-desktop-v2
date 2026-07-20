# Fixed Asset Register - Form Layout Architecture

## Visual Structure

```
┌─────────────────────────────────────────────────────────────────────────┐
│ Fixed Asset Register Form (1400 x 862)                                  │
│                                                                           │
│ ┌──────────────────────────────────────────────────────────────────┐   │
│ │ Summary Card (Blue Header) - Shows Selected Asset Info          │   │
│ │ Asset Name: [Large Text]  | Cost: PKR 0 | Book Value: PKR 0     │   │
│ └──────────────────────────────────────────────────────────────────┘   │
│                                                                           │
│ ┌─────────────────────────────────────── Split Container ──────────────┐│
│ │                                                                        ││
│ │ ┌──────────────────── Left Panel (65%) ──────┐ │ ┌─ Right Panel (35%)─┐│
│ │ │                                             │ │ │                   ││
│ │ │ [Add] [Import] [Categories] [Locations]    │ │ │  ┌─────────────┐ ││
│ │ │ [Edit]                                      │ │ │  │ Asset Info  │ ││
│ │ │                                             │ │ │  ├─────────────┤ ││
│ │ │ Category: [Dropdown] Status: [Dropdown]    │ │ │  │Dep.Setup    │ ││
│ │ │ Location: [Dropdown]                       │ │ │  ├─────────────┤ ││
│ │ │                                             │ │ │  │Dep.History  │ ││
│ │ │ Search: [TextBox                        ]  │ │ │  ├─────────────┤ ││
│ │ │                                             │ │ │  │Disposal/Rev │ ││
│ │ │ ┌─────────────────────────────────────┐    │ │ │  └─────────────┘ ││
│ │ │ │ Asset Grid - DataGridView           │    │ │ │                   ││
│ │ │ │ [AssetCode][Name][Category][Cost]...│    │ │ │  [Content Filled   ││
│ │ │ │ DESK-001   Desk   Furniture  50000  │    │ │ │   at Runtime]      ││
│ │ │ │ CHAIR-001  Chair  Furniture  25000  │    │ │ │                   ││
│ │ │ │ BLDG-001   Building Building 500000 │    │ │ │                   ││
│ │ │ │ ...                                 │    │ │ │                   ││
│ │ │ │                                     │    │ │ │                   ││
│ │ │ └─────────────────────────────────────┘    │ │ │                   ││
│ │ │                                             │ │ │                   ││
│ │ └─────────────────────────────────────────────┘ │ └───────────────────┘│
│ │                                                  │                      │
│ └──────────────────────────────────────────────────┴──────────────────────┘│
│                                                                           │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## Component Hierarchy

```
frm_fixed_asset_register
│
├─ mainPanel (DockStyle.Fill)
│  │
│  ├─ pnlSummaryCard (DockStyle.Top, Height: 98px)
│  │  ├─ lblAssetNameSummary (DockStyle.Top)
│  │  └─ lblCostSummary (DockStyle.Fill)
│  │
│  └─ splitContainer (DockStyle.Fill)
│     │
│     ├─ Panel1 - Left Panel (65%)
│     │  │
│     │  ├─ lblSearch (DockStyle.Top)
│     │  ├─ txtSearch (DockStyle.Top)
│     │  ├─ pnlFilters (DockStyle.Top, Height: 37px)
│     │  │  ├─ lblCategory + ddlCategory
│     │  │  ├─ lblStatus + ddlStatus
│     │  │  └─ lblLocation + ddlLocation
│     │  ├─ pnlToolbar (DockStyle.Top, Height: 41px)
│     │  │  ├─ btnAddAsset
│     │  │  ├─ btnImportAssets
│     │  │  ├─ btnManageCategories
│     │  │  ├─ btnManageLocations
│     │  │  └─ btnEditAsset
│     │  │
│     │  └─ dgvAssets (DockStyle.Fill)
│     │     Columns: [AssetCode, Name, Category, Cost, WDV, Status, Age]
│     │
│     └─ Panel2 - Right Panel (35%)
│        │
│        └─ tabControl (DockStyle.Fill)
│           │
│           ├─ tabAssetInfo (Tab 0 - Primary)
│           │  │ Size: (351, 711) ✓ FIXED
│           │  └─ TableLayoutPanel (2 columns, 12 rows)
│           │     ├─ Row 0: lblAssetCode + txtAssetCode
│           │     ├─ Row 1: lblAssetName + txtAssetName
│           │     ├─ Row 2: lblDescription + txtDescription
│           │     ├─ Row 3: lblCategory + ddlAssetCategory
│           │     ├─ Row 4: lblPurchaseDate + dtPurchaseDate
│           │     ├─ Row 5: lblSupplier + ddlSupplier
│           │     ├─ Row 6: lblInvoiceNo + txtInvoiceNo
│           │     ├─ Row 7: lblCost + txtCost
│           │     ├─ Row 8: lblLocation + ddlAssetLocation
│           │     ├─ Row 9: lblSerialNo + txtSerialNumber
│           │     ├─ Row 10: lblModelNo + txtModelNumber
│           │     └─ Row 11: lblStatus + ddlAssetStatus
│           │
│           ├─ tabDepreciationSetup (Tab 1)
│           │  │ Size: (351, 711) ✓ FIXED (was (21, 0) ✗)
│           │  └─ TableLayoutPanel (2 columns, 8 rows)
│           │     ├─ Row 0: lblDepMethod + ddlDepMethod
│           │     ├─ Row 1: lblUsefulLife + numUsefulLifeYears
│           │     ├─ Row 2: lblUsefulLifeMonths + numUsefulLifeMonths
│           │     ├─ Row 3: lblResidualValue + txtResidualValue
│           │     ├─ Row 4: lblDepRate + txtDepRate
│           │     ├─ Row 5: lblStartDepDate + dtStartDepreciationDate
│           │     ├─ Row 6: "Depreciation Schedule Preview"
│           │     └─ Row 7: dgvDepSchedule (grid)
│           │
│           ├─ tabDepreciationHistory (Tab 2)
│           │  │ Size: (351, 711) ✓ FIXED (was (21, 0) ✗)
│           │  └─ TableLayoutPanel
│           │     ├─ lblCurrentWDV (label showing current book value)
│           │     └─ dgvDepreciationHistory (grid with history data)
│           │
│           └─ tabDisposalRevaluation (Tab 3)
│              │ Size: (351, 711) ✓ FIXED (was (21, 0) ✗)
│              └─ TableLayoutPanel (2 columns, 8 rows)
│                 ├─ Row 0: lblDisposalDate + dtDisposalDate
│                 ├─ Row 1: lblDisposalProceeds + txtDisposalProceeds
│                 ├─ Row 2: lblGainLoss (display label)
│                 ├─ Row 3: lblRevaluationCost + txtRevaluationCost
│                 ├─ Row 4: lblSurcharge + txtRevaluationSurcharge
│                 ├─ Row 5: [Spacer]
│                 ├─ Row 6: btnRunDepreciation
│                 ├─ Row 7: btnPostDisposal + btnPostRevaluation
```

---

## Data Flow for Tab Population

### 1. Form Load Sequence

```
frm_fixed_asset_register_Load()
│
├─ AppTheme.Apply(this)          ← Apply KasBook styling
│
├─ LoadCategories()              ← Populate ddl dropdowns
├─ LoadLocations()
├─ LoadSuppliers()
├─ LoadDepreciationAccounts()
│
├─ LoadAssets()                  ← Load dgvAssets grid
│
├─ BuildAssetInfoTab(tabAssetInfo)              ← Create Tab 1 controls
├─ BuildDepreciationSetupTab(tabDepreciationSetup)   ← Create Tab 2 controls
├─ BuildDepreciationHistoryTab(tabDepreciationHistory) ← Create Tab 3 controls
├─ BuildDisposalRevaluationTab(tabDisposalRevaluation)  ← Create Tab 4 controls
│
├─ Wire Event Handlers
│  ├─ dgvAssets.SelectionChanged += DgvAssets_SelectionChanged
│  ├─ btnAddAsset.Click += BtnAddAsset_Click
│  ├─ btnRunDepreciation.Click += BtnRunDepreciation_Click
│  ├─ btnPostDisposal.Click += BtnPostDisposal_Click
│  └─ btnPostRevaluation.Click += BtnPostRevaluation_Click
│
└─ ApplyAssetListTheme()         ← Style the grid
```

### 2. Asset Selection Sequence

```
User clicks row in dgvAssets
	↓
DgvAssets_SelectionChanged() fires
	↓
_currentAsset = _filteredAssets[selectedRow]
	↓
LoadAssetDetail()
	├─ txtAssetCode.Text = _currentAsset.AssetCode
	├─ txtAssetName.Text = _currentAsset.AssetName
	├─ ddlAssetCategory.SelectedItem = _currentAsset.CategoryName
	├─ dtPurchaseDate.Value = _currentAsset.PurchaseDate
	├─ ... (other Asset Info tab fields)
	│
	├─ ddlDepMethod.SelectedItem = _currentAsset.DepreciationMethod
	├─ numUsefulLifeYears.Value = _currentAsset.UsefulLifeYears
	├─ ... (other Depreciation Setup tab fields)
	│
	├─ LoadDepreciationSchedulePreview()
	│  └─ dgvDepSchedule.Rows.Add(...) for each month
	│
	├─ LoadDepreciationHistory()
	│  └─ dgvDepreciationHistory.Rows.Add(...) for each run
	│
	└─ UpdateSummaryCard()
	   └─ lblAssetNameSummary.Text = Asset Name + Cost Info
```

### 3. Tab Building Pattern (Example: BuildAssetInfoTab)

```
BuildAssetInfoTab(tabAssetInfo)
	↓
Create TableLayoutPanel with 2 columns, 12 rows
	├─ Column 0: Fixed width 150px (Labels)
	└─ Column 1: Percentage width 100% (Controls)
	↓
Create controls and add to table at grid positions
	├─ table.Controls.Add(lblAssetCode, 0, 0)
	├─ table.Controls.Add(txtAssetCode, 1, 0)
	├─ table.Controls.Add(lblAssetName, 0, 1)
	├─ table.Controls.Add(txtAssetName, 1, 1)
	└─ ... (10 more rows)
	↓
Add table to tabAssetInfo.Controls
	└─ tabAssetInfo.Controls.Add(table)
	↓
All controls inherit DockStyle.Fill and resize with tab
```

---

## Key Layout Properties

### Tab Control
- **Dock:** Fill (fills right panel)
- **SelectedIndex:** 0 (Asset Info shown first)
- **Size:** (359, 740)

### Tab Pages (FIXED)
```csharp
// All 4 tabs now have identical size:
Size = (351, 711)
Padding = (12, 12, 12, 12)
```

### Content Panels (inside tabs)
```csharp
// All use TableLayoutPanel with DockStyle.Fill
table.Dock = DockStyle.Fill
table.ColumnCount = 2
table.RowCount = [varies per tab: 12, 8, variable, 8]
table.ColumnStyles[0] = 150px (fixed)
table.ColumnStyles[1] = 100% (flexible)
```

### Individual Controls
```csharp
// All docked to fill their grid cells
Dock = DockStyle.Fill  // or DockStyle.Top for read-only/label fields
```

---

## Fix Summary

| Aspect | Before | After | Status |
|--------|--------|-------|--------|
| Tab 1 Size | (351, 711) | (351, 711) | ✓ Correct |
| Tab 2 Size | (21, 0) | (351, 711) | ✓ **FIXED** |
| Tab 3 Size | (21, 0) | (351, 711) | ✓ **FIXED** |
| Tab 4 Size | (21, 0) | (351, 711) | ✓ **FIXED** |
| Tab Content Visibility | Hidden | Visible | ✓ **FIXED** |
| Build Status | N/A | 0 Errors | ✓ Success |

---

## Usage Notes

### When User Opens Form
1. Asset list grid populates with all active assets (left panel)
2. Summary card shows blue header with "No Asset Selected"
3. Right-side tabs are visible with empty content

### When User Selects an Asset
1. All 4 tabs populate with asset data
2. Tab 1 shows asset master information (read-only code, editable name/location)
3. Tab 2 shows depreciation setup and monthly preview schedule
4. Tab 3 shows depreciation history with current WDV
5. Tab 4 shows disposal/revaluation options with posting buttons
6. Summary card updates to show asset name and key metrics

### Tab Navigation
- User can click between tabs to view different aspects of the asset
- All data remains in memory; switching tabs doesn't reload
- Buttons trigger workflows (run depreciation, post disposal, etc.)

---

## Performance Notes
- Dynamic layout creation happens once at form load (not repeated)
- Tab content population happens only when asset is selected
- No repeated binding or reallocation during tab switches
- Ideal for UI responsiveness with dozens of assets in grid
