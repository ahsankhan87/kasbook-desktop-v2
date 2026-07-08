# Comprehensive Discount Module — Implementation Plan

## 0. What Already Exists (Baseline)

| Artifact | Location | Status |
|---|---|---|
| `max_discount_percent` / `max_discount_amount` on `UsersModal` | `POS.Core` | ✅ Model done |
| `logged_in_max_discount_percent/amount` session statics | `POS.Core` | ✅ Done |
| `DiscountValidator.IsDiscountValid()` | `pos/Sales/DiscountValidator.cs` | ✅ Done |
| `txt_max_discount_percent/amount` on `frm_adduser` | `pos/Master/Users/frm_adduser.cs` | ✅ UI/save done |
| `frm_PromotionsDiscountsReport` stub | `pos/Reports/Sales/` | ⚠️ Stub only |
| `pos_users_rights` table columns for discount | SQL (inferred) | ✅ Already added |

Everything below is **net-new work**.

---

## 1. Database Schema — New SQL Tables

All tables follow existing naming convention (`pos_*`). All will be added via a **single migration SQL script** (`discount_module_migration.sql`).

### 1.1 `pos_discount_schemes`
Core scheme/promotion table — the root of every discount type.

```
id, name, name_ar, discount_type
  (ITEM | INVOICE | PROMOTIONAL | TIME_BASED | MEMBERSHIP | BXGY | SLAB | COUPON),
calc_type (PERCENT | AMOUNT),
value (discount value / percent),
is_active, priority (lower = higher priority),
start_date, end_date (NULL = no expiry),
max_uses (NULL = unlimited), uses_count,
branch_id, company_id,
created_by, created_at, updated_at
```

### 1.2 `pos_discount_conditions`
Flexible condition rows linked to a scheme (EAV-style, matches existing SP pattern).

```
id, scheme_id (FK → pos_discount_schemes),
condition_type
  (MIN_QTY | MIN_AMOUNT | CUSTOMER_GROUP | CUSTOMER_ID |
   PRODUCT_ID | CATEGORY_ID | TIME_RANGE | DAY_OF_WEEK | MEMBERSHIP_TIER),
condition_value (varchar, cast by app)
```

### 1.3 `pos_discount_customer_prices`
Customer-specific pricing (overrides unit price per product per customer).

```
id, customer_id, product_id (item_number),
special_price, start_date, end_date,
branch_id, is_active, created_by, created_at
```

### 1.4 `pos_discount_quantity_slabs`
Slab (tiered quantity) pricing per product.

```
id, scheme_id, product_id,
min_qty, max_qty (NULL = open-ended),
discount_percent, discount_amount,
is_active
```

### 1.5 `pos_discount_bxgy`
Buy X Get Y rules.

```
id, scheme_id,
buy_product_id, buy_qty,
get_product_id, get_qty,
get_discount_percent (100 = free),
is_active
```

### 1.6 `pos_coupons`
Coupons / vouchers.

```
id, code (UNIQUE), scheme_id (FK),
coupon_type (PERCENT | AMOUNT | FREE_ITEM),
value, min_invoice_amount,
max_uses, uses_count,
customer_id (NULL = any),
valid_from, valid_to,
is_active, branch_id, company_id,
created_by, created_at
```

### 1.7 `pos_discount_approvals`
Approval workflow for discounts exceeding the user limit.

```
id, invoice_ref (temp reference before save),
discount_type, requested_by, requested_discount_value,
approved_by, approved_at,
status (PENDING | APPROVED | REJECTED),
reason, branch_id, created_at
```

### 1.8 `pos_discount_audit_log`
Immutable audit record of every discount applied.

```
id, invoice_no, invoice_type (SALES | POS),
discount_scheme_id (nullable), coupon_code (nullable),
discount_type, calc_type, discount_value,
product_id (nullable — NULL = invoice-level),
applied_by (user_id), approved_by (nullable),
branch_id, created_at
```

### 1.9 Alter Existing Tables
- `pos_sales`: add `coupon_code VARCHAR(50)`, `discount_scheme_id INT`, `discount_approval_id INT`
- `pos_sales_items`: add `discount_scheme_id INT`, `discount_type VARCHAR(30)`, `bxgy_free_item BIT DEFAULT 0`
- `pos_users_rights`: already has `max_discount_percent` / `max_discount_amount` ✅ (no change)

### 1.10 Stored Procedures
One SP per table following the project pattern (`sp_DiscountSchemesCrud`, `sp_CouponsCrud`, `sp_DiscountApprovalCrud`, `sp_DiscountAuditLog`) each with `@OperationType` = 1 (Insert), 2 (Update), 3 (Delete), 4 (Get by ID), 5 (Get All).

---

## 2. POS.Core — New Model Classes

**New file:** `POS.Core/Discounts/DiscountSchemeModal.cs`
Fields mirror `pos_discount_schemes` + child collections.

**New file:** `POS.Core/Discounts/CouponModal.cs`

**New file:** `POS.Core/Discounts/DiscountResultModal.cs`
Returned by the discount engine — carries final resolved discount per line and per invoice.

```csharp
public class DiscountResultModal
{
    public int? SchemeId { get; set; }
    public string DiscountType { get; set; }   // enum-as-string
    public string CalcType { get; set; }        // PERCENT / AMOUNT
    public double DiscountValue { get; set; }   // applied amount (not %)
    public bool RequiresApproval { get; set; }
    public string CouponCode { get; set; }
    public bool IsBxGyFreeItem { get; set; }
    public int? FreeProductId { get; set; }
}
```

**New file:** `POS.Core/Discounts/DiscountApprovalModal.cs`

---

## 3. POS.DLL — New Data Layer Classes

**`POS.DLL/Discounts/DiscountSchemesDLL.cs`**
- `GetAll()`, `GetById(int id)`, `Insert(DiscountSchemeModal)`, `Update(...)`, `Delete(int id)`
- `GetActiveSchemes(DateTime now, int branchId)` — used by engine at runtime

**`POS.DLL/Discounts/CouponsDLL.cs`**
- `GetByCode(string code, int branchId)`, `IncrementUsage(string code)`, `Insert`, `Update`, `Delete`

**`POS.DLL/Discounts/CustomerPriceDLL.cs`**
- `GetPriceForCustomerProduct(int customerId, string itemNumber)` → returns `double?`

**`POS.DLL/Discounts/QuantitySlabDLL.cs`**
- `GetSlabDiscount(int schemeId, string productId, double qty)` → returns `DiscountResultModal`

**`POS.DLL/Discounts/BxGyDLL.cs`**
- `GetBxGyRules(int schemeId)` → returns list of buy-x-get-y rules

**`POS.DLL/Discounts/DiscountApprovalDLL.cs`**
- `Insert(DiscountApprovalModal)`, `UpdateStatus(int id, string status, int approvedBy)`, `GetPending(int branchId)`

**`POS.DLL/Discounts/DiscountAuditDLL.cs`**
- `LogDiscount(DiscountAuditModal)` — called by engine after every save

---

## 4. POS.BLL — New Business Layer Classes

### 4.1 `POS.BLL/Discounts/DiscountEngineBLL.cs` ⭐ (Core of Module)

This is the central **discount resolution engine**. Called from the sales form before saving an invoice.

**Key method signatures:**

```csharp
// Called per line item
DiscountResultModal ResolveItemDiscount(
    string itemNumber, double qty, double unitPrice,
    int customerId, DateTime saleDateTime, int branchId);

// Called for invoice-level (after line totals are known)
DiscountResultModal ResolveInvoiceDiscount(
    double invoiceTotal, int customerId,
    DateTime saleDateTime, int branchId, string couponCode = null);

// Called for BxGy resolution — returns list of free items to add
List<DiscountResultModal> ResolveBxGy(
    List<(string itemNumber, double qty)> cartLines,
    int branchId);

// Called for quantity slab
DiscountResultModal ResolveSlabDiscount(
    string itemNumber, double qty, int branchId);

// Validate coupon
CouponModal ValidateCoupon(string code, double invoiceTotal, int customerId, int branchId);
```

**Resolution priority order** (configurable via `priority` field):
1. Customer-specific price override
2. Active promotional scheme (time-based / membership first)
3. Slab pricing
4. BxGy
5. Item-level scheme discount
6. Invoice-level coupon / scheme discount
7. Manual user discount (validated against `DiscountValidator`)

> **Rule:** Only one scheme applies per scope (item vs. invoice) unless a `stackable` flag is added later. Highest-priority scheme wins.

### 4.2 `POS.BLL/Discounts/DiscountSchemesBLL.cs`
CRUD pass-through + validation (date range, uniqueness of promo code).

### 4.3 `POS.BLL/Discounts/CouponsBLL.cs`
CRUD + `ValidateAndApply(string code, ...)` (calls `DiscountEngineBLL.ValidateCoupon` + increments usage atomically).

### 4.4 `POS.BLL/Discounts/DiscountApprovalBLL.cs`
- `RequestApproval(...)` — inserts pending row
- `Approve(int id, int approvedByUserId)` / `Reject(...)`
- `GetPendingApprovals(int branchId)`

### 4.5 `POS.BLL/Discounts/CustomerPriceBLL.cs`
CRUD for `pos_discount_customer_prices`.

---

## 5. pos (UI) — New & Modified Forms

### 5.1 New: `pos/Discounts/frm_discount_schemes.cs` (List Form)
- DataGridView listing all schemes
- Buttons: Add, Edit, Delete, Toggle Active
- Follows `AppTheme.ApplyListFormStyle(...)` pattern
- Permission tag: `"Discounts.ManageSchemes"`

### 5.2 New: `pos/Discounts/frm_add_discount_scheme.cs` (Detail / Add-Edit)
- Fields: Name, Name (AR), Type (ComboBox), Calc Type, Value, Start/End Date, Priority, Max Uses, Active checkbox
- Tab pages for: Conditions, Slab Tiers, BxGy Rules
- Saves via `DiscountSchemesBLL`

### 5.3 New: `pos/Discounts/frm_coupons.cs` (List)
- Coupon management grid
- Fields: Code, Scheme, Type, Value, Valid From/To, Uses Count/Max, Status

### 5.4 New: `pos/Discounts/frm_add_coupon.cs` (Detail)

### 5.5 New: `pos/Discounts/frm_customer_prices.cs`
- Search customer → list their product-specific prices
- Add / Edit / Delete

### 5.6 New: `pos/Discounts/frm_discount_approval.cs`
- Shows pending approval requests to manager/admin
- Approve / Reject buttons
- Uses `DiscountApprovalBLL.GetPendingApprovals(...)`
- Only accessible to users with level ≥ manager (`Permissions.Discounts_ApproveOverride`)

### 5.7 Modified: `pos/Sales/frm_sales.cs`
Key changes (minimal, localized):

| Where | Change |
|---|---|
| After product is added to grid | Call `DiscountEngineBLL.ResolveItemDiscount(...)` → auto-populate `discount` column |
| After customer is selected | Call `CustomerPriceBLL.GetPriceForCustomerProduct(...)` → override `unit_price` in grid |
| Coupon field (new `txt_coupon_code` + `btn_apply_coupon`) in footer | Calls `CouponsBLL.ValidateAndApply(...)` |
| Before `btn_save_Click` | Check `DiscountValidator.IsDiscountValid(...)` for manual discounts; if exceeds limit → call `DiscountApprovalBLL.RequestApproval(...)` and show dialog |
| After successful save | Call `DiscountAuditDLL.LogDiscount(...)` for each applied discount |
| BxGy auto-addition | After resolving BxGy, auto-add free rows to grid with price=0 and `bxgy_free_item=true` |

### 5.8 Modified: `pos/Sales/frm_pos_sale.cs`
Same discount engine calls as `frm_sales.cs` but for POS quick-sale workflow.

### 5.9 Modified: `pos/Master/Users/frm_adduser.cs`
Already has `txt_max_discount_percent` / `txt_max_discount_amount` fields and save logic. ✅ No change needed.

### 5.10 New: `pos/Discounts/frm_discount_approval_request.cs` (Popup)
Modal dialog shown to sales user when their discount exceeds limit:
- Shows "Approval Required" message
- Option to call manager over (manager enters PIN/password inline) **OR** submit for async approval
- On approval: stores `approved_by`, stamps sale

---

## 6. Reports — Enhance Existing + New

### 6.1 Enhanced: `pos/Reports/Sales/frm_PromotionsDiscountsReport.cs`
Replace stub with real query joining `pos_discount_audit_log` → show scheme name, type, total discount given, per-product breakdown.

### 6.2 New: `pos/Reports/Discounts/frm_CouponUsageReport.cs`
- Coupon code, usage count, total discount given, date range filter

### 6.3 New: `pos/Reports/Discounts/frm_DiscountApprovalReport.cs`
- Who requested, who approved, discount amount, invoice reference

---

## 7. Navigation — `frm_main` / Menu

Add new menu group **"Discounts"** under Master or its own top-level item:
- Discount Schemes
- Coupons / Vouchers
- Customer Special Prices
- Pending Approvals *(badge count if pending > 0)*
- Reports → Promotions Report, Coupon Usage, Approval History

Gated by `AppTheme.Apply` and `FormSecurityExtensions.ApplyPermissions` using new permission tags.

---

## 8. Security / Permissions

Add new permission keys to `pos/Security/Authorization/Models.cs` (or the existing `Permissions` class):

```
Discounts_ManageSchemes
Discounts_ManageCoupons
Discounts_ManageCustomerPrices
Discounts_ApproveOverride
Discounts_ViewReports
```

Register these in `SqlRoleRepository` / `FrmPermissions`.

---

## 9. Audit Logging

- All CREATE / UPDATE / DELETE on discount entities call `Log.LogAction(...)` (existing `sp_LogAction`).
- Every discount **applied** during a sale writes to `pos_discount_audit_log` via `DiscountAuditDLL.LogDiscount(...)`.

---

## 10. Implementation Sequence (Phases)

| Phase | Scope | Estimated Risk |
|---|---|---|
| **Ph-1: Foundation** | SQL migration script, `POS.Core` models, `POS.DLL` CRUD classes | Low |
| **Ph-2: Engine** | `DiscountEngineBLL`, `CouponsBLL`, `DiscountApprovalBLL` | Medium |
| **Ph-3: UI — Management** | `frm_discount_schemes`, `frm_coupons`, `frm_customer_prices` | Low |
| **Ph-4: Sales Integration** | `frm_sales.cs` + `frm_pos_sale.cs` modifications | High (core path) |
| **Ph-5: Approval Workflow** | `frm_discount_approval`, `frm_discount_approval_request` popup | Medium |
| **Ph-6: Reports** | `frm_PromotionsDiscountsReport` (real), coupon/approval reports | Low |
| **Ph-7: Nav + Permissions** | Menu entries, permission keys, role assignments | Low |

---

## 11. Open Questions — Decide Before Implementation

1. **Stacking**: Should multiple discount schemes stack on one item (e.g., promo 10% + slab 5%), or is it *first match wins*?
2. **Approval flow**: Inline (manager enters password at terminal right now) **or** async (sale is held/draft until approved remotely)? Or both?
3. **Membership tier**: Is there already a `pos_customers` column for membership tier, or do we need a new `pos_membership_tiers` table?
4. **BxGy free items**: When a free item is added to the grid, should it appear as a zero-price row (visible on invoice) or as an invoice-level discount?
5. **Coupon scope**: Company-wide coupons or branch-specific? (Schema supports both via `branch_id = NULL` = company-wide)
6. **ZATCA compliance**: For e-invoice (ZATCA Phase 2), item-level discounts must appear in the XML `<cac:AllowanceCharge>` element. Should the engine tag which discounts need ZATCA propagation?
7. **Slab pricing**: Does it replace unit price, or is it expressed as a discount on top of the list price?
8. **frm_pos_sale**: The quick-sale POS form — should it support all discount types or only coupons + auto-schemes (no manual line discount)?
