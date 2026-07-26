using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using POS.Core;
using POS.DLL.Inventory;


namespace POS.DLL
{
    public class PurchasesDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private PurchasesModal info = new PurchasesModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }
            
        }

        public DataTable GetAllPurchases()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT TOP 10000 p.*, CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,4)) AS total, " +
                            " CAST(ISNULL(p.foreign_total_amount, 0) + ISNULL(p.foreign_total_tax, 0) - ISNULL(p.foreign_total_discount, 0) AS decimal(18,4)) AS foreign_net_amount," +
                            " CONCAT(sp.first_name,' ',sp.last_name) as supplier_name, " +
                            " ISNULL(c.code, 'SAR') as currency_code "+
                            " FROM pos_purchases p LEFT JOIN pos_suppliers sp ON p.supplier_id=sp.id"+
                            " LEFT JOIN pos_currencies c ON p.currency_id = c.id "+
                            " WHERE p.purchase_date BETWEEN @FY_from_date AND @FY_to_date AND p.branch_id = @branch_id order by p.id desc";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@FY_from_date", UsersModal.fy_from_date);
                        cmd.Parameters.AddWithValue("@FY_to_date", UsersModal.fy_to_date);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetAllPurchasesItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT PI.invoice_no,PI.id,PI.item_code,PI.item_number,PI.quantity,PI.cost_price,PI.unit_price,PI.discount_value," +
                            " (PI.cost_price*PI.quantity-ABS(PI.discount_value)) AS total,PI.loc_code," + 
                            " P.name AS product_name, " +
                            " ((PI.cost_price*PI.quantity-ABS(PI.discount_value))*PI.tax_rate/100) AS vat, " +
                            " (((PI.cost_price*PI.quantity-ABS(PI.discount_value))*PI.tax_rate/100) + (PI.cost_price*PI.quantity-ABS(PI.discount_value))) AS net_total, " +
                            " PI.exchange_rate, PI.currency_id, PI.foreign_unit_price, PI.foreign_cost_price, PI.foreign_discount_value" +
                            " FROM pos_purchases_items PI " +
                            " LEFT JOIN pos_products P ON P.item_number=PI.item_number " +
                            " WHERE PI.invoice_no = @invoice_no AND PI.branch_id = @branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetProductPurchaseHistory(string item_number)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable history = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        String query = "SELECT TOP 100 PI.id, P.name AS product_name, PI.item_code, PI.item_number, PI.quantity AS qty, " +
                            "PI.unit_price, PI.cost_price, PH.invoice_no, PH.description, PH.purchase_date AS trans_date, " +
                            "CONCAT(ISNULL(S.first_name, ''), ' - (', ISNULL(PH.supplier_invoice_no, ''),')') AS supplier " +
                            "FROM pos_purchases_items PI " +
                            "INNER JOIN pos_purchases PH ON PH.id = PI.purchase_id AND PH.branch_id = PI.branch_id " +
                            "LEFT JOIN pos_products P ON P.item_number = PI.item_number " +
                            "LEFT JOIN pos_suppliers S ON S.id = PH.supplier_id " +
                            "WHERE PI.item_number = @item_number AND PI.branch_id = @branch_id " +
                            "AND ISNULL(PH.account, '') <> 'Return' " +
                            "ORDER BY PH.purchase_date DESC, PI.id DESC";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@item_number", item_number);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(history);
                    return history;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable SearchRecord(String condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT p.id,p.invoice_no,purchase_type,purchase_date,total_amount,discount_value,total_tax, supplier_invoice_no,CONCAT(sp.first_name,' ',sp.last_name) as supplier_name "+
                            "FROM pos_purchases  p LEFT JOIN pos_suppliers sp ON p.supplier_id=sp.id WHERE p.invoice_no LIKE @invoice_no AND p.branch_id = @branch_id", cn);
                        //cmd.Parameters.AddWithValue("@invoice_no", condition);
                        cmd.Parameters.AddWithValue("@invoice_no", string.Format("%{0}%", condition));
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;

                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }
        
        public DataTable GetAllPurchaseByInvoice(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account,S.supplier_id," +
                            " S.supplier_invoice_no,S.employee_id,S.description,S.account,S.shipping_cost,S.payment_terms_id,S.payment_method_id,S.currency_id," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity,SI.unit_price,SI.cost_price,SI.serialnumber,SI.discount_percent," +
                            " SI.quantity AS qty,SI.cost_price AS avg_cost," + // this line is for print of build edit product page
                            " SI.discount_value,(SI.unit_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity*SI.tax_rate/100) AS vat," +
                            " P.name AS name,P.id,P.name_ar,P.code,P.location_code,P.item_type,P.barcode,P.description," +
                            " U.name AS unit," +
                            " CT.name AS category" +
                            " FROM pos_purchases S" +
                            " LEFT JOIN pos_purchases_items SI ON S.id=SI.purchase_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code=P.category_code" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id"+
                            " ORDER BY SI.serialnumber ASC";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public String GetMaxInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_purchases WHERE SUBSTRING(invoice_no, 1,1) = 'P' AND account <> 'Return' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());
                    
                        if(maxId == "")
                        {
                            return maxId = "P-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("P-{0:000000}", intval);
                            return maxId;
                        }
                    
                    }
                    return "";
                }
                catch
                {

                    throw;
                }
            }

        }

        public String GetMaxReturnInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_purchases WHERE SUBSTRING(invoice_no, 1,2) = 'PR'  AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "PR-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(3, 6));
                            intval++;
                            maxId = String.Format("PR-{00:000000}", intval);
                            return maxId;
                        }

                    }
                    return "";
                }
                catch
                {

                    throw;
                }
            }

        }
        // Add inside PurchasesDLL class (recommended near GetMaxInvoiceNo)
        public string GenerateDailyInvoiceNo(string tableName, string invoiceColumn, string prefix, int? branchId = null, DateTime? invoiceDate = null)
        {
            int bId = branchId ?? UsersModal.logged_in_branch_id;
            DateTime d = (invoiceDate ?? DateTime.Now).Date;

            string datePart = d.ToString("yyyyMMdd");
            string start = prefix + bId + "-" + datePart + "-"; // e.g. "P1-20260128-"
            string like = start + "%";

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand($@"
            SELECT MAX({invoiceColumn})
            FROM {tableName}
            WHERE branch_id = @branch_id
              AND {invoiceColumn} LIKE @like;", cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", bId);
                cmd.Parameters.AddWithValue("@like", like);

                cn.Open();
                string lastRef = Convert.ToString(cmd.ExecuteScalar());

                int newNum = 1;
                if (!string.IsNullOrWhiteSpace(lastRef) && lastRef.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                {
                    string tail = lastRef.Substring(start.Length); // "0001"
                    int lastNum;
                    if (int.TryParse(tail, out lastNum))
                        newNum = lastNum + 1;
                }

                return start + newNum.ToString("0000");
            }
        }

        public String GetMaxInvoiceNo_HOLD()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_hold_purchases WHERE branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "H-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("H-{0:000000}", intval);
                            return maxId;
                        }

                    }
                    return "";
                }
                catch
                {

                    throw;
                }
            }

        }

        public int Insertpurchases(List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
        {
            Int32 newPurchaseID = 0;
            bool isAutoPostPurchases = false;

            // Load inventory costing settings once (outside the transaction — read-only, no lock needed)
            // sp_Purchase_items is the WAC authority; we only need the method to decide FIFO layer creation.
            string costingMethod = "WAC";
            try
            {
                var _inventoryValuationDLL = new InventoryValuationDLL().GetSettings(UsersModal.logged_in_branch_id);
                if (!string.IsNullOrWhiteSpace(_inventoryValuationDLL?.ValuationMethod))
                    costingMethod = _inventoryValuationDLL.ValuationMethod.ToUpperInvariant();
            }
            catch { /* fall back to WAC — non-fatal */ }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        cmd = new SqlCommand("sp_Purchases", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@employee_id", purchase_header.employee_id);
                            cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_type", purchase_header.purchase_type);
                            cmd.Parameters.AddWithValue("@supplier_invoice_no", purchase_header.supplier_invoice_no);
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@total_amount", purchase_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", purchase_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", purchase_header.total_discount);
                            cmd.Parameters.AddWithValue("@discount_percent", purchase_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@purchase_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@account", purchase_header.account);
                            cmd.Parameters.AddWithValue("@PO_invoice_no", purchase_header.po_invoice_no);
                            cmd.Parameters.AddWithValue("@PO_status", 0);
                            cmd.Parameters.AddWithValue("@purchase_time", purchase_header.purchase_time);
                            cmd.Parameters.AddWithValue("@shipping_cost", purchase_header.shipping_cost);
                            cmd.Parameters.AddWithValue("@payment_method_id", purchase_header.payment_method_id);
                            cmd.Parameters.AddWithValue("@payment_terms_id", purchase_header.payment_terms_id);
                            cmd.Parameters.AddWithValue("@currency_id", purchase_header.currency_id);
                            cmd.Parameters.AddWithValue("@exchange_rate", purchase_header.exchange_rate > 0 ? purchase_header.exchange_rate : 1m);
                            cmd.Parameters.AddWithValue("@foreign_total_amount", purchase_header.foreign_total_amount);
                            cmd.Parameters.AddWithValue("@foreign_total_tax", purchase_header.foreign_total_tax);
                            cmd.Parameters.AddWithValue("@foreign_total_discount", purchase_header.foreign_total_discount);

                            cmd.Parameters.AddWithValue("@OperationType", "1");
                        }

                        newPurchaseID = Convert.ToInt32(cmd.ExecuteScalar());

                        //foreach (PurchaseModalHeader purchase_header in purchases)
                        //{
                        //    cmd = new SqlCommand(@"
                        //        IF COL_LENGTH('dbo.pos_purchases','currency_id') IS NOT NULL
                        //            UPDATE dbo.pos_purchases SET currency_id = @currency_id WHERE id = @id;
                        //        IF COL_LENGTH('dbo.pos_purchases','exchange_rate') IS NOT NULL
                        //            UPDATE dbo.pos_purchases SET exchange_rate = @exchange_rate WHERE id = @id;
                        //        IF COL_LENGTH('dbo.pos_purchases','foreign_total_amount') IS NOT NULL
                        //            UPDATE dbo.pos_purchases SET foreign_total_amount = @foreign_total_amount WHERE id = @id;
                        //        IF COL_LENGTH('dbo.pos_purchases','foreign_total_tax') IS NOT NULL
                        //            UPDATE dbo.pos_purchases SET foreign_total_tax = @foreign_total_tax WHERE id = @id;
                        //        IF COL_LENGTH('dbo.pos_purchases','foreign_total_discount') IS NOT NULL
                        //            UPDATE dbo.pos_purchases SET foreign_total_discount = @foreign_total_discount WHERE id = @id;", cn, transaction);
                        //    cmd.Parameters.AddWithValue("@id", newProdID);
                        //    cmd.ExecuteNonQuery();
                        //}

                        foreach (PurchasesModal detail in purchase_detail)
                        {
                            cmd = new SqlCommand("sp_Purchase_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@serialNo", detail.serialNo);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@item_id", detail.item_id);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@purchase_id", newPurchaseID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@quantity", detail.quantity);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@discount_percent", detail.line_discount_percent);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@supplier_id", detail.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_date", detail.purchase_date);
                            cmd.Parameters.AddWithValue("@PO_invoice_no", detail.po_invoice_no);
                            cmd.Parameters.AddWithValue("@PO_status", detail.po_status);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code.ToUpper());
                            cmd.Parameters.AddWithValue("@purchase_type", detail.purchase_type);
                            cmd.Parameters.AddWithValue("@currency_id", detail.currency_id);
                            cmd.Parameters.AddWithValue("@exchange_rate", detail.exchange_rate > 0 ? detail.exchange_rate : 1m);
                            cmd.Parameters.AddWithValue("@foreign_unit_price", detail.foreign_unit_price);
                            cmd.Parameters.AddWithValue("@foreign_cost_price", detail.foreign_cost_price);
                            cmd.Parameters.AddWithValue("@foreign_discount_value", detail.foreign_discount_value);

                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            var purchaseItemId = Convert.ToInt32(cmd.ExecuteScalar());

                            // ── FIFO layer creation ───────────────────────────────────────
                            // WAC is already updated inside sp_Purchase_items.
                            // For FIFO valuation we also record a cost layer so that the
                            // costing engine can consume layers on sale (FIFO depletion).
                            // This runs in the same transaction so it rolls back atomically.
                            if (costingMethod == "FIFO" && detail.item_type != "Service")
                            {
                                var _inventoryCostingEngine = new InventoryCostingEngineDLL();
                                _inventoryCostingEngine.InsertFIFOLayer(
                                    detail.item_id,
                                    detail.item_number,
                                    UsersModal.logged_in_branch_id,
                                    newPurchaseID,
                                    detail.purchase_date,
                                    detail.quantity,
                                    detail.cost_price,
                                    detail.currency_id,
                                    detail.exchange_rate > 0 ? detail.exchange_rate : 1m,
                                    cn, transaction);
                            }
                            // ── end FIFO layer ───────────────────────────────────────────

                            //cmd = new SqlCommand(@"
                            //    IF COL_LENGTH('dbo.pos_purchases_items','currency_id') IS NOT NULL
                            //        UPDATE dbo.pos_purchases_items SET currency_id = @currency_id WHERE id = @id;
                            //    IF COL_LENGTH('dbo.pos_purchases_items','exchange_rate') IS NOT NULL
                            //        UPDATE dbo.pos_purchases_items SET exchange_rate = @exchange_rate WHERE id = @id;
                            //    IF COL_LENGTH('dbo.pos_purchases_items','foreign_unit_price') IS NOT NULL
                            //        UPDATE dbo.pos_purchases_items SET foreign_unit_price = @foreign_unit_price WHERE id = @id;
                            //    IF COL_LENGTH('dbo.pos_purchases_items','foreign_cost_price') IS NOT NULL
                            //        UPDATE dbo.pos_purchases_items SET foreign_cost_price = @foreign_cost_price WHERE id = @id;
                            //    IF COL_LENGTH('dbo.pos_purchases_items','foreign_discount_value') IS NOT NULL
                            //        UPDATE dbo.pos_purchases_items SET foreign_discount_value = @foreign_discount_value WHERE id = @id;", cn, transaction);
                            //cmd.Parameters.AddWithValue("@id", purchaseItemId);
                            //cmd.ExecuteNonQuery();
                        }

                        isAutoPostPurchases = GetBoolSetting(cn, transaction, SettingKeys.AutoPostPurchases, false);

                        if (!isAutoPostPurchases)
                        {
                            foreach (PurchaseModalHeader purchase_header in purchases)
                            {
                                UpdatePurchasePostedFlag(cn, transaction, purchase_header.invoice_no, false);
                            }
                        }

                        transaction.Commit();

                        //insert log when trans commit
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            Log.LogAction("Add Purchase", $"InvoiceNo: {purchase_header.invoice_no}, Purchase Date: {purchase_header.purchase_date}, Total Amount: {((purchase_header.total_amount + purchase_header.total_tax) - purchase_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                    }
                    catch 
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                if (isAutoPostPurchases)
                {
                    PostPurchaseJournalsAndUpdatePostedFlag(purchases);
                }

                return newPurchaseID;

            }
        }

        private void PostPurchaseJournalsAndUpdatePostedFlag(List<PurchaseModalHeader> purchases)
        {
            if (purchases == null || purchases.Count == 0)
                return;

            JournalsDLL journalsDal = new JournalsDLL();

            foreach (PurchaseModalHeader purchaseHeader in purchases)
            {
                bool posted = false;

                try
                {
                    AutoJVModel model = BuildPurchaseAutoJournalModel(purchaseHeader);
                    if (model != null && model.Lines != null && model.Lines.Count > 0)
                    {
                        PostResult result = journalsDal.PostAutoJournalEntry(model, UsersModal.logged_in_userid);
                        posted = result != null && result.Success;
                    }
                }
                catch
                {
                    posted = false;
                }

                UpdatePurchasePostedFlag(purchaseHeader.invoice_no, posted);
            }
        }

        private AutoJVModel BuildPurchaseAutoJournalModel(PurchaseModalHeader purchaseHeader)
        {
            if (purchaseHeader == null)
                return null;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                int cashAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultCashAccount);
                int payableAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultApAccount);
                int inventoryAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultInventoryAccount);
                int purchaseDiscountAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultDiscountAccount);
                int taxInputAccountId = ResolveDefaultAccountId(cn, null, "ACC_DEFAULT_TAX_RECEIVABLE");
                if (taxInputAccountId <= 0)
                    taxInputAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultSalesTaxAccount);

                int settlementAccountId = ResolvePurchaseSettlementAccountId(cn, purchaseHeader, cashAccountId, payableAccountId);
                if (inventoryAccountId <= 0 || settlementAccountId <= 0)
                    return null;

                int? supplierId = string.Equals(purchaseHeader.purchase_type, "Credit", StringComparison.OrdinalIgnoreCase)
                    && purchaseHeader.supplier_id > 0
                    ? (int?)purchaseHeader.supplier_id
                    : null;

                int? bankId = IsBankPaymentMethod(purchaseHeader.payment_method_text) && purchaseHeader.bank_id > 0
                    ? (int?)purchaseHeader.bank_id
                    : null;

                string narration = purchaseHeader.description;
                decimal amount = purchaseHeader.total_amount;
                decimal discount = purchaseHeader.total_discount;
                decimal tax = purchaseHeader.total_tax;

                List<JVLineModel> lines = new List<JVLineModel>();

                if (amount > 0)
                {
                    AddPurchaseAutoLine(lines, inventoryAccountId, amount, 0m, narration, null, null);
                    AddPurchaseAutoLine(lines, settlementAccountId, 0m, amount, narration, supplierId, bankId);
                }

                if (discount > 0 && purchaseDiscountAccountId > 0)
                {
                    AddPurchaseAutoLine(lines, settlementAccountId, discount, 0m, narration, supplierId, bankId);
                    AddPurchaseAutoLine(lines, purchaseDiscountAccountId, 0m, discount, narration, null, null);
                }

                if (tax > 0 && taxInputAccountId > 0)
                {
                    AddPurchaseAutoLine(lines, taxInputAccountId, tax, 0m, narration, null, null);
                    AddPurchaseAutoLine(lines, settlementAccountId, 0m, tax, narration, supplierId, bankId);
                }

                if (lines.Count == 0)
                    return null;

                return new AutoJVModel
                {
                    ModuleName = "PURCHASES",
                    RefModule = "pos_purchases",
                    RefId = 0,
                    VoucherDate = purchaseHeader.purchase_date,
                    ReferenceNo = purchaseHeader.invoice_no,
                    Narration = narration,
                    IsAutoPosted = true,
                    Lines = lines
                };
            }
        }

        private void PostReturnPurchaseJournalsAndUpdatePostedFlag(List<PurchaseModalHeader> purchases)
        {
            if (purchases == null || purchases.Count == 0)
                return;

            JournalsDLL journalsDal = new JournalsDLL();

            foreach (PurchaseModalHeader purchaseHeader in purchases)
            {
                bool posted = false;

                try
                {
                    int originalVoucherId = GetPostedPurchaseVoucherIdByInvoiceNo(purchaseHeader.old_invoice_no);
                    if (originalVoucherId > 0)
                    {
                        decimal originalNet = GetPurchaseNetAmountByInvoiceNo(purchaseHeader.old_invoice_no);
                        decimal returnNet = Convert.ToDecimal(purchaseHeader.total_amount) + Convert.ToDecimal(purchaseHeader.total_tax) - Convert.ToDecimal(purchaseHeader.total_discount);

                        decimal ratio = 0m;
                        if (originalNet > 0m && returnNet > 0m)
                        {
                            ratio = returnNet / originalNet;
                            if (ratio > 1m)
                                ratio = 1m;
                        }

                        string reason = string.Format("Purchase return reversal. Return Invoice: {0}, Original Invoice: {1}", purchaseHeader.invoice_no, purchaseHeader.old_invoice_no);

                        if (ratio >= 1m)
                        {
                            PostResult result = journalsDal.ReverseJournalVoucher(originalVoucherId, purchaseHeader.purchase_date, reason, UsersModal.logged_in_userid);
                            posted = result != null && result.Success;
                        }
                        else if (ratio > 0m)
                        {
                            AutoJVModel partialModel = BuildPartialReturnPurchaseReversalModel(journalsDal, originalVoucherId, purchaseHeader, ratio, reason);
                            if (partialModel != null && partialModel.Lines != null && partialModel.Lines.Count > 0)
                            {
                                PostResult result = journalsDal.PostAutoJournalEntry(partialModel, UsersModal.logged_in_userid);
                                posted = result != null && result.Success;
                            }
                        }
                    }
                }
                catch
                {
                    posted = false;
                }

                UpdatePurchasePostedFlag(purchaseHeader.invoice_no, posted);
            }
        }

        private int GetPostedPurchaseVoucherIdByInvoiceNo(string invoiceNo)
        {
            if (string.IsNullOrWhiteSpace(invoiceNo))
                return 0;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                using (SqlCommand voucherCmd = new SqlCommand(@"
                    SELECT TOP 1 id
                    FROM acc_entries_header
                    WHERE branch_id = @branch_id
                      AND ISNULL(status, 'Posted') = 'Posted'
                      AND VoucherType = @voucher_type
                      AND ReferenceNo = @reference_no
                    ORDER BY id DESC", cn))
                {
                    voucherCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    voucherCmd.Parameters.AddWithValue("@voucher_type", "PURCHASES");
                    voucherCmd.Parameters.AddWithValue("@reference_no", invoiceNo.Trim());

                    object idObj = voucherCmd.ExecuteScalar();
                    if (idObj != null && idObj != DBNull.Value)
                    {
                        int voucherId;
                        if (int.TryParse(Convert.ToString(idObj), out voucherId) && voucherId > 0)
                            return voucherId;
                    }
                }
            }

            return 0;
        }

        private decimal GetPurchaseNetAmountByInvoiceNo(string invoiceNo)
        {
            if (string.IsNullOrWhiteSpace(invoiceNo))
                return 0m;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                using (SqlCommand cmdNet = new SqlCommand(@"
                    SELECT TOP 1
                        CAST(ISNULL(total_amount,0) + ISNULL(total_tax,0) - ISNULL(discount_value,0) AS DECIMAL(18,4))
                    FROM pos_purchases
                    WHERE branch_id = @branch_id AND invoice_no = @invoice_no
                    ORDER BY id DESC", cn))
                {
                    cmdNet.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmdNet.Parameters.AddWithValue("@invoice_no", invoiceNo.Trim());

                    object value = cmdNet.ExecuteScalar();
                    if (value != null && value != DBNull.Value)
                    {
                        decimal parsed;
                        if (decimal.TryParse(Convert.ToString(value), out parsed) && parsed > 0m)
                            return parsed;
                    }
                }
            }

            return 0m;
        }

        private AutoJVModel BuildPartialReturnPurchaseReversalModel(JournalsDLL journalsDal, int originalVoucherId, PurchaseModalHeader purchaseHeader, decimal ratio, string reason)
        {
            if (journalsDal == null || purchaseHeader == null || originalVoucherId <= 0 || ratio <= 0m)
                return null;

            var source = journalsDal.GetVoucherWithLines(originalVoucherId);
            if (source.Header == null || source.Lines == null || source.Lines.Count == 0)
                return null;

            List<JVLineModel> lines = new List<JVLineModel>();
            foreach (JVLineModel sourceLine in source.Lines)
            {
                decimal debit = Math.Round(sourceLine.Credit * ratio, 2, MidpointRounding.AwayFromZero);
                decimal credit = Math.Round(sourceLine.Debit * ratio, 2, MidpointRounding.AwayFromZero);
                if (debit <= 0m && credit <= 0m)
                    continue;

                lines.Add(new JVLineModel
                {
                    AccountId = sourceLine.AccountId,
                    Debit = debit,
                    Credit = credit,
                    Narration = string.IsNullOrWhiteSpace(reason) ? sourceLine.Narration : reason,
                    ModuleName = "PURCHASE_RETURN",
                    CustomerId = sourceLine.CustomerId,
                    SupplierId = sourceLine.SupplierId,
                    BankId = sourceLine.BankId,
                    PeriodId = sourceLine.PeriodId
                });
            }

            RebalancePurchaseAutoLines(lines);
            if (lines.Count == 0)
                return null;

            return new AutoJVModel
            {
                ModuleName = "PURCHASE_RETURN",
                RefModule = "pos_purchases",
                RefId = 0,
                VoucherDate = purchaseHeader.purchase_date,
                ReferenceNo = purchaseHeader.invoice_no,
                Narration = reason,
                IsAutoPosted = true,
                Lines = lines
            };
        }

        private void RebalancePurchaseAutoLines(List<JVLineModel> lines)
        {
            if (lines == null || lines.Count == 0)
                return;

            decimal totalDebit = lines.Sum(x => x.Debit);
            decimal totalCredit = lines.Sum(x => x.Credit);
            decimal diff = Math.Round(totalDebit - totalCredit, 2, MidpointRounding.AwayFromZero);

            if (diff == 0m)
                return;

            if (diff > 0m)
            {
                JVLineModel line = lines.LastOrDefault(x => x.Credit > 0m);
                if (line != null)
                    line.Credit = Math.Round(line.Credit + diff, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                JVLineModel line = lines.LastOrDefault(x => x.Debit > 0m);
                if (line != null)
                    line.Debit = Math.Round(line.Debit + Math.Abs(diff), 2, MidpointRounding.AwayFromZero);
            }
        }

        private AutoJVModel BuildReturnPurchaseAutoJournalModel(PurchaseModalHeader purchaseHeader)
        {
            if (purchaseHeader == null)
                return null;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                int cashAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultCashAccount);
                int payableAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultApAccount);
                int inventoryAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultInventoryAccount);
                int purchaseDiscountAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultDiscountAccount);
                int taxInputAccountId = ResolveDefaultAccountId(cn, null, "ACC_DEFAULT_TAX_RECEIVABLE");
                if (taxInputAccountId <= 0)
                    taxInputAccountId = ResolveDefaultAccountId(cn, null, SettingKeys.DefaultSalesTaxAccount);

                int settlementAccountId = ResolvePurchaseSettlementAccountId(cn, purchaseHeader, cashAccountId, payableAccountId);
                if (inventoryAccountId <= 0 || settlementAccountId <= 0)
                    return null;

                int? supplierId = string.Equals(purchaseHeader.purchase_type, "Credit", StringComparison.OrdinalIgnoreCase)
                    && purchaseHeader.supplier_id > 0
                    ? (int?)purchaseHeader.supplier_id
                    : null;

                int? bankId = IsBankPaymentMethod(purchaseHeader.payment_method_text) && purchaseHeader.bank_id > 0
                    ? (int?)purchaseHeader.bank_id
                    : null;

                string narration = purchaseHeader.description;
                decimal amount = purchaseHeader.total_amount;
                decimal discount = purchaseHeader.total_discount;
                decimal tax = purchaseHeader.total_tax;

                List<JVLineModel> lines = new List<JVLineModel>();

                if (amount > 0)
                {
                    AddPurchaseAutoLine(lines, inventoryAccountId, 0m, amount, narration, null, null);
                    AddPurchaseAutoLine(lines, settlementAccountId, amount, 0m, narration, supplierId, bankId);
                }

                if (discount > 0 && purchaseDiscountAccountId > 0)
                {
                    AddPurchaseAutoLine(lines, purchaseDiscountAccountId, discount, 0m, narration, null, null);
                    AddPurchaseAutoLine(lines, settlementAccountId, 0m, discount, narration, supplierId, bankId);
                }

                if (tax > 0 && taxInputAccountId > 0)
                {
                    AddPurchaseAutoLine(lines, taxInputAccountId, 0m, tax, narration, null, null);
                    AddPurchaseAutoLine(lines, settlementAccountId, tax, 0m, narration, supplierId, bankId);
                }

                if (lines.Count == 0)
                    return null;

                return new AutoJVModel
                {
                    ModuleName = "PURCHASE_RETURN",
                    RefModule = "pos_purchases",
                    RefId = 0,
                    VoucherDate = purchaseHeader.purchase_date,
                    ReferenceNo = purchaseHeader.invoice_no,
                    Narration = narration,
                    IsAutoPosted = true,
                    Lines = lines
                };
            }
        }

        private int ResolvePurchaseSettlementAccountId(SqlConnection cn, PurchaseModalHeader purchaseHeader, int cashAccountId, int payableAccountId)
        {
            if (purchaseHeader == null)
                return 0;

            if (string.Equals(purchaseHeader.purchase_type, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                if (IsBankPaymentMethod(purchaseHeader.payment_method_text))
                {
                    int bankGl = ResolveSelectedBankGlAccountId(cn, purchaseHeader.bankGLAccountID);
                    if (bankGl > 0)
                        return bankGl;
                }

                return cashAccountId;
            }

            return payableAccountId;
        }

        private int ResolveSelectedBankGlAccountId(SqlConnection cn, string bankGlAccountValue)
        {
            if (string.IsNullOrWhiteSpace(bankGlAccountValue))
                return 0;

            string raw = bankGlAccountValue.Trim();
            int accountId;
            if (int.TryParse(raw, out accountId) && accountId > 0)
                return accountId;

            using (SqlCommand accountCmd = new SqlCommand("SELECT TOP 1 id FROM acc_accounts WHERE LTRIM(RTRIM(code)) = @code", cn))
            {
                accountCmd.Parameters.AddWithValue("@code", raw);
                object idObj = accountCmd.ExecuteScalar();
                if (idObj != null && idObj != DBNull.Value)
                {
                    int resolvedId;
                    if (int.TryParse(Convert.ToString(idObj), out resolvedId) && resolvedId > 0)
                        return resolvedId;
                }
            }

            return 0;
        }

        private void AddPurchaseAutoLine(List<JVLineModel> lines, int accountId, decimal debit, decimal credit, string narration, int? supplierId, int? bankId)
        {
            if (accountId <= 0)
                return;

            if (debit <= 0m && credit <= 0m)
                return;

            lines.Add(new JVLineModel
            {
                AccountId = accountId,
                Narration = narration,
                Debit = debit,
                Credit = credit,
                ModuleName = "PURCHASES",
                SupplierId = supplierId,
                BankId = bankId
            });
        }

        private bool IsBankPaymentMethod(string paymentMethodText)
        {
            return !string.IsNullOrWhiteSpace(paymentMethodText)
                && paymentMethodText.IndexOf("bank", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool GetBoolSetting(SqlConnection cn, SqlTransaction tx, string key, bool defaultValue)
        {
            using (SqlCommand settingCmd = tx == null
                ? new SqlCommand("SELECT TOP 1 setting_value FROM pos_settings WHERE setting_key = @key", cn)
                : new SqlCommand("SELECT TOP 1 setting_value FROM pos_settings WHERE setting_key = @key", cn, tx))
            {
                settingCmd.Parameters.AddWithValue("@key", key);
                object raw = settingCmd.ExecuteScalar();
                return ParseBoolSetting(raw, defaultValue);
            }
        }

        private int ResolveDefaultAccountId(SqlConnection cn, SqlTransaction tx, string settingKey)
        {
            string settingValue = string.Empty;

            using (SqlCommand settingCmd = tx == null
                ? new SqlCommand("SELECT TOP 1 setting_value FROM pos_settings WHERE setting_key = @key", cn)
                : new SqlCommand("SELECT TOP 1 setting_value FROM pos_settings WHERE setting_key = @key", cn, tx))
            {
                settingCmd.Parameters.AddWithValue("@key", settingKey);
                object raw = settingCmd.ExecuteScalar();
                settingValue = Convert.ToString(raw);
            }

            if (string.IsNullOrWhiteSpace(settingValue))
                return 0;

            using (SqlCommand accountCmd = tx == null
                ? new SqlCommand("SELECT TOP 1 id FROM acc_accounts WHERE LTRIM(RTRIM(code)) = @code", cn)
                : new SqlCommand("SELECT TOP 1 id FROM acc_accounts WHERE LTRIM(RTRIM(code)) = @code", cn, tx))
            {
                accountCmd.Parameters.AddWithValue("@code", settingValue.Trim());
                object idObj = accountCmd.ExecuteScalar();
                if (idObj != null && idObj != DBNull.Value)
                {
                    int resolvedId;
                    if (int.TryParse(Convert.ToString(idObj), out resolvedId) && resolvedId > 0)
                        return resolvedId;
                }
            }

            return 0;
        }

        private bool ParseBoolSetting(object raw, bool defaultValue)
        {
            if (raw == null || raw == DBNull.Value)
                return defaultValue;

            string text = Convert.ToString(raw);
            if (string.IsNullOrWhiteSpace(text))
                return defaultValue;

            text = text.Trim();
            if (string.Equals(text, "1", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "yes", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(text, "0", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "false", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "no", StringComparison.OrdinalIgnoreCase))
                return false;

            return defaultValue;
        }

        private void UpdatePurchasePostedFlag(string invoiceNo, bool posted)
        {
            if (string.IsNullOrWhiteSpace(invoiceNo))
                return;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                UpdatePurchasePostedFlag(cn, null, invoiceNo, posted);
            }
        }

        private void UpdatePurchasePostedFlag(SqlConnection cn, SqlTransaction tx, string invoiceNo, bool posted)
        {
            if (string.IsNullOrWhiteSpace(invoiceNo))
                return;

            string sql = @"
IF COL_LENGTH('pos_purchases', 'posted') IS NOT NULL
BEGIN
    UPDATE pos_purchases
    SET posted = @posted
    WHERE invoice_no = @invoice_no
      AND branch_id = @branch_id;
END";

            using (SqlCommand updateCmd = tx == null
                ? new SqlCommand(sql, cn)
                : new SqlCommand(sql, cn, tx))
            {
                updateCmd.Parameters.AddWithValue("@posted", posted ? 1 : 0);
                updateCmd.Parameters.AddWithValue("@invoice_no", invoiceNo);
                updateCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                updateCmd.ExecuteNonQuery();
            }
        }

        public int ReplacePurchases(string oldInvoiceNo, List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
        {
            if (string.IsNullOrWhiteSpace(oldInvoiceNo))
                throw new ArgumentException("Old invoice number is required.", nameof(oldInvoiceNo));

            if (purchases == null || purchases.Count == 0)
                throw new ArgumentException("Purchase header is required.", nameof(purchases));

            if (purchase_detail == null || purchase_detail.Count == 0)
                throw new ArgumentException("Purchase detail is required.", nameof(purchase_detail));

            int deleteResult = DeletePurchases(oldInvoiceNo);
            if (deleteResult <= 0)
                throw new Exception("Unable to delete old purchase invoice before replacement.");

            int newPurchaseId = Insertpurchases(purchases, purchase_detail);
            if (newPurchaseId <= 0)
                throw new Exception("Unable to save replacement purchase invoice.");

            return newPurchaseId;
        }

        public int InsertpurchasesItems(PurchasesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchase_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@PO_invoice_no", obj.po_invoice_no);
                        cmd.Parameters.AddWithValue("@PO_status", obj.po_status);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code.ToUpper());

                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);

                        cmd.Parameters.AddWithValue("@OperationType", "1");

                    }

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(PurchasesModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        //cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@avg_cost", obj.cost_price);
                        cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                       
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int purchasesId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", purchasesId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Purchase", $"PurchasesId: {purchasesId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }


        public DataTable GetReturnPurchase(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price," +
                            //" SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            //" (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            //" P.name AS product_name," +
                            //" C.first_name AS customer_name" +
                            " FROM pos_purchases P" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.id=SI.item_code" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE P.invoice_no = @invoice_no AND P.branch_id=@branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetReturnPurchaseItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt_1 = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT CAST(1 AS BIT) AS chk, SI.invoice_no,SI.loc_code,SI.quantity AS return_qty,SI.packet_qty," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity,SI.unit_price,SI.cost_price,SI.tax_rate,SI.tax_id,SI.discount_value," +
                            " (((SI.cost_price*SI.quantity-ABS(SI.discount_value))*SI.tax_rate/100) + (SI.cost_price*SI.quantity-ABS(SI.discount_value))) AS total," +
                            " ((SI.cost_price*SI.quantity-discount_value)*SI.tax_rate/100) AS vat," +
                            " P.name AS product_name,(SI.quantity - ISNULL(r.TotalReturnedQty,0)) AS ReturnQty," +
                            " ISNULL(r.TotalReturnedQty,0) AS ReturnedQty,(SI.quantity - ISNULL(r.TotalReturnedQty,0)) AS ReturnableQty" +
                            //" C.first_name AS customer_name" +
                            " FROM pos_purchases_items SI" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN (SELECT ItemNumber, SUM(QtyReturned) AS TotalReturnedQty FROM pos_purchasesReturn WHERE OriginalInvoiceNo = @invoice_no GROUP BY ItemNumber) r ON r.ItemNumber = SI.item_number" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE SI.invoice_no = @invoice_no AND SI.branch_id=@branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt_1);

                    // Add input column for new return qty (not persisted yet)
                    if (!dt_1.Columns.Contains("ReturnQty"))
                        dt_1.Columns.Add("ReturnQty", typeof(decimal));
                    foreach (DataRow r in dt_1.Rows)
                        r["ReturnQty"] = 0m;

                    return dt_1;
                }
                catch
                {

                    throw;
                }
            }

        }

        public int InsertReturnPurchase(List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
        {
            Int32 newProdID = 0;
            bool isAutoPostPurchases = false;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    try
                    {
                        cmd = new SqlCommand("sp_Purchases", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@employee_id", purchase_header.employee_id);
                            cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_type", purchase_header.purchase_type);
                            //cmd.Parameters.AddWithValue("@supplier_invoice_no", purchase_header.supplier_invoice_no);
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@total_amount", purchase_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", purchase_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", purchase_header.total_discount);
                            // cmd.Parameters.AddWithValue("@discount_percent", purchase_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@purchase_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@account", purchase_header.account);
                            // cmd.Parameters.AddWithValue("@PO_invoice_no", purchase_header.po_invoice_no);
                            //cmd.Parameters.AddWithValue("@PO_status", 0);
                            cmd.Parameters.AddWithValue("@purchase_time", purchase_header.purchase_time);
                            //cmd.Parameters.AddWithValue("@shipping_cost", purchase_header.shipping_cost);

                            cmd.Parameters.AddWithValue("@OperationType", "2");
                        }

                        newProdID = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (PurchasesModal detail in purchase_detail)
                        {
                            cmd = new SqlCommand("sp_Purchase_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);

                            cmd.Parameters.AddWithValue("@old_invoice_no", purchases[0].old_invoice_no);
                            cmd.Parameters.AddWithValue("@returnReason", purchases[0].returnReason);

                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@purchase_id", newProdID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@quantity", detail.quantity);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@supplier_id", detail.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_date", detail.purchase_date);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code.ToUpper());
                            cmd.Parameters.AddWithValue("@purchase_type", detail.purchase_type);

                            cmd.Parameters.AddWithValue("@OperationType", "2");

                            cmd.ExecuteScalar();
                        }

                        isAutoPostPurchases = GetBoolSetting(cn, transaction, SettingKeys.AutoPostPurchases, false);

                        if (!isAutoPostPurchases)
                        {
                            foreach (PurchaseModalHeader purchase_header in purchases)
                            {
                                UpdatePurchasePostedFlag(cn, transaction, purchase_header.invoice_no, false);
                            }
                        }


                        //insert log when trans commit
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            Log.LogAction("Add Return Purchase", $"InvoiceNo: {purchase_header.invoice_no}, Purchase Date: {purchase_header.purchase_date}, Total Amount: {((purchase_header.total_amount + purchase_header.total_tax) - purchase_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                if (isAutoPostPurchases)
                {
                    PostReturnPurchaseJournalsAndUpdatePostedFlag(purchases);
                }

                return (int)newProdID;
            }
        }

        public int InsertReturnPurchaseItems(PurchasesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchase_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                        cmd.Parameters.AddWithValue("@OperationType", "2");


                    }

                    int result = Convert.ToInt32(cmd.ExecuteScalar());


                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int DeletePurchases(string invoice_no)
        {
            string SQL1 = "DELETE FROM pos_purchases WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL2 = "DELETE FROM pos_purchases_items WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL3 = "DELETE FROM acc_entries WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL4 = "DELETE FROM pos_suppliers_payments WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            //string SQL5 = "DELETE FROM pos_inventory WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            Int32 result = 0;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
               
                DataTable sales_dt = new DataTable();
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    try
                    {
                        String query_1 = "SELECT SI.*" +
                                        " FROM pos_purchases_items SI" +
                                        " WHERE SI.invoice_no = @invoice_no AND branch_id = @branch_id";

                        // cmd1 = new SqlCommand(query_1, cn, transaction);
                        using (SqlCommand cmd = new SqlCommand(query_1, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(sales_dt);


                        }

                        foreach (DataRow dr in sales_dt.Rows)
                        {
                            //String query_2 = "UPDATE pos_products SET qty= (SELECT qty FROM pos_products WHERE code = @item_code)-@quantity WHERE code=@item_code ";
                            String query_2 = "UPDATE pos_product_stocks SET qty= (SELECT TOP 1 qty FROM pos_product_stocks WHERE item_number = @item_number AND branch_id = @branch_id)-@quantity WHERE item_number=@item_number AND branch_id = @branch_id ";
                            //--Insert Location qty

                            //String query_2 = "INSERT INTO pos_product_stocks VALUES (0,0,'" + dr["loc_code"].ToString() + "'," + dr["item_code"].ToString() + "," + (double.Parse(dr["quantity"].ToString()) * -1) + ",0,'" + DateTime.Now.Date + "','" + DateTime.Now.Date + "')";
                            //String query_2 = "UPDATE pos_product_stocks SET qty= (SELECT qty FROM pos_product_stocks WHERE item_code = " + dr["item_code"].ToString() + " AND loc_code = '" + dr["loc_code"].ToString() + "')-" + double.Parse(dr["quantity"].ToString()) + " WHERE item_code=" + dr["item_code"].ToString() + " AND loc_code = '" + dr["loc_code"].ToString() + "'";

                            cmd = new SqlCommand(query_2, cn, transaction);
                            cmd.Parameters.AddWithValue("@item_number", dr["item_number"].ToString());
                            cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["quantity"].ToString()));
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            cmd.ExecuteNonQuery();
                            //using (SqlCommand cmd1 = new SqlCommand(query_2, cn)) { cmd.ExecuteNonQuery(); }

                            String query_3 = "INSERT INTO pos_inventory VALUES (@item_code,-@quantity,-@cost_price,-@unit_price,@branch_id,@user_id,'Purchase Delete',@invoice_no, @purchase_date,GETDATE(),0,0,0,@purchase_date,@location_code,-@packet_qty,@item_number)";
                            cmd = new SqlCommand(query_3, cn, transaction);
                            cmd.Parameters.AddWithValue("@item_code", dr["item_code"].ToString());
                            cmd.Parameters.AddWithValue("@item_number", dr["item_number"].ToString());
                            cmd.Parameters.AddWithValue("@location_code", dr["loc_code"].ToString());
                            cmd.Parameters.AddWithValue("@cost_price", double.Parse(dr["cost_price"].ToString()));
                            cmd.Parameters.AddWithValue("@unit_price", double.Parse(dr["unit_price"].ToString()));
                            cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["quantity"].ToString()));
                            cmd.Parameters.AddWithValue("@packet_qty", double.Parse(dr["packet_qty"].ToString()));
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@purchase_date", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(SQL1, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(SQL2, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(SQL3, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(SQL4, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        //using (SqlCommand cmd = new SqlCommand(SQL5, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }

                        //String query = "DELETE FROM pos_purchases WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM pos_purchases_items WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM acc_entries WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM pos_suppliers_payments WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM pos_inventory WHERE invoice_no = @invoice_no";

                        //cmd = new SqlCommand(query, cn);
                        //cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                        transaction.Commit();
                        Log.LogAction("Delete Purchase", $"InvoiceNo: {invoice_no}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        result = 1;
                    }
                    catch
                    {
                        transaction.Rollback();
                        
                        throw;

                    }

                }
                   
                    return result;
            }

        }

        public int DeleteHoldPurchases(string invoice_no)
        {
            string SQL1 = "DELETE FROM pos_hold_purchases WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL2 = "DELETE FROM pos_hold_purchases_items WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            Int32 result = 0;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(SQL2, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(SQL1, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Log.LogAction("Delete Hold Purchase", $"InvoiceNo: {invoice_no}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        result = 1;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return result;
            }
        }

        public DataTable PurchaseReceipt(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable PurchaseReceipt_dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity,SI.unit_price,SI.cost_price," +
                            " SI.discount_value,(SI.cost_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " ((SI.cost_price*SI.quantity-SI.discount_value)*SI.tax_rate/100) AS vat," +
                            " C.first_name AS supplier_name, C.vat_no AS supplier_vat," +
                            " P.name AS product_name, P.code, SI.loc_code" +
                            " FROM pos_purchases S" +
                            " LEFT JOIN pos_purchases_items SI ON S.invoice_no=SI.invoice_no" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_suppliers C ON C.id=S.supplier_id" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(PurchaseReceipt_dt);
                    return PurchaseReceipt_dt;
                }
                catch
                {

                    throw;
                }
            }

        }


        public DataTable GetAll_Hold_PurchaseByInvoice(string invoice_no)
        { 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account,S.supplier_id," +
                            "S.supplier_invoice_no,S.employee_id,S.description,S.account,S.shipping_cost,S.payment_terms_id,S.payment_method_id,S.currency_id," +
                            " SI.id,SI.item_code,SI.quantity,SI.unit_price,SI.cost_price,SI.serialnumber,SI.item_number,SI.discount_percent," +
                            " SI.discount_value,(SI.unit_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity*SI.tax_rate/100) AS vat," +
                            " P.name AS name,P.code,P.location_code,P.item_type,P.barcode,P.description," +
                            " U.name AS unit," +
                            " CT.name AS category" +
                            " FROM pos_hold_purchases S" +
                            " LEFT JOIN pos_hold_purchases_items SI ON S.id=SI.purchase_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code=P.category_code" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id"+
                            " ORDER BY SI.serialnumber ASC";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public int Insert_hold_purchases(PurchasesModal obj)
        {
            Int32 newProdID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Hold_Purchases", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@employee_id", obj.employee_id);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);
                        cmd.Parameters.AddWithValue("@supplier_invoice_no", obj.supplier_invoice_no);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                        cmd.Parameters.AddWithValue("@total_tax", obj.total_tax);
                        cmd.Parameters.AddWithValue("@discount_value", obj.total_discount);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@account", obj.account);
                        cmd.Parameters.AddWithValue("@PO_invoice_no", obj.po_invoice_no);
                        cmd.Parameters.AddWithValue("@PO_status", 0);
                        cmd.Parameters.AddWithValue("@purchase_time", obj.purchase_time);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                    }

                    newProdID = Convert.ToInt32(cmd.ExecuteScalar());

                    if (obj.currency_id > 0)
                    {
                        cmd = new SqlCommand(@"IF COL_LENGTH('dbo.pos_hold_purchases','currency_id') IS NOT NULL
                                               BEGIN
                                                   UPDATE dbo.pos_hold_purchases
                                                   SET currency_id = @currency_id
                                                   WHERE id = @id;
                                               END", cn);
                        cmd.Parameters.AddWithValue("@currency_id", obj.currency_id);
                        cmd.Parameters.AddWithValue("@id", newProdID);
                        cmd.ExecuteNonQuery();
                    }

                    Log.LogAction("Add Hold Purchase", $"InvoiceNo: {obj.invoice_no}, Purchase Date: {obj.purchase_date}, Total Amount: {((obj.total_amount + obj.total_tax) - obj.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                }
                catch
                {

                    throw;
                }
            }
            return newProdID;
        }

        public int Insert_hold_purchasesItems(PurchasesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Hold_Purchase_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@serialNo", obj.serialNo);
                        cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@PO_invoice_no", obj.po_invoice_no);
                        cmd.Parameters.AddWithValue("@PO_status", obj.po_status);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code.ToUpper());
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                    }

                    int result = Convert.ToInt32(cmd.ExecuteScalar());


                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }
        public int UpdateSupplierInPurchases(string invoice_no, string supplier_id, string supplierInvoiceNo)
        {
            int result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    String query1 = "UPDATE pos_purchases SET supplier_id=@supplier_id, supplier_invoice_no=@supplierInvoiceNo WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
                    String query2 = "UPDATE pos_inventory SET supplier_id=@supplier_id  WHERE invoice_no= @invoice_no AND branch_id = @branch_id";
                    String query3 = "UPDATE acc_entries SET supplier_id=@supplier_id  WHERE invoice_no= @invoice_no AND branch_id = @branch_id";

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        transaction = cn.BeginTransaction();

                        using (SqlCommand cmd = new SqlCommand(query1, cn, transaction)) 
                        { 
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no); 
                            cmd.Parameters.AddWithValue("@supplier_id", supplier_id); 
                            cmd.Parameters.AddWithValue("@supplierInvoiceNo", supplierInvoiceNo); 
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); 
                            cmd.ExecuteNonQuery(); 
                        }
                        using (SqlCommand cmd = new SqlCommand(query2, cn, transaction)) 
                        { 
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no); 
                            cmd.Parameters.AddWithValue("@supplier_id", supplier_id); 
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); 
                            cmd.ExecuteNonQuery(); 
                        }
                        using (SqlCommand cmd = new SqlCommand(query3, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    
                    transaction.Commit();
                    Log.LogAction("Update Supllier Name in Purchase Inv", $"InvoiceNo: {invoice_no}, SupplierId: {supplier_id}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    result = 1;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }
        public bool IsSupplierInvoiceNoExists(int supplierId, string supplierInvoiceNo, string excludeInvoiceNo = null)
        {
            if (string.IsNullOrWhiteSpace(supplierInvoiceNo))
                return false;

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();

                cmd.CommandText = @"
                SELECT TOP 1 1
                FROM pos_purchases
                WHERE branch_id = @branch_id
                  AND supplier_id = @supplier_id
                  AND supplier_invoice_no = @supplier_invoice_no
                  AND (@exclude_invoice_no IS NULL OR invoice_no <> @exclude_invoice_no);";

                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                cmd.Parameters.AddWithValue("@supplier_invoice_no", supplierInvoiceNo.Trim());
                cmd.Parameters.AddWithValue("@exclude_invoice_no", (object)excludeInvoiceNo ?? DBNull.Value);

                var found = cmd.ExecuteScalar();
                return found != null && found != DBNull.Value;
            }
        }

        public bool IsHoldSupplierInvoiceNoExists(int supplierId, string supplierInvoiceNo, string excludeInvoiceNo = null)
        {
            if (string.IsNullOrWhiteSpace(supplierInvoiceNo))
                return false;

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();

                cmd.CommandText = @"
                SELECT TOP 1 1
                FROM pos_hold_purchases
                WHERE branch_id = @branch_id
                  AND supplier_id = @supplier_id
                  AND supplier_invoice_no = @supplier_invoice_no
                  AND (@exclude_invoice_no IS NULL OR invoice_no <> @exclude_invoice_no);";

                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                cmd.Parameters.AddWithValue("@supplier_invoice_no", supplierInvoiceNo.Trim());
                cmd.Parameters.AddWithValue("@exclude_invoice_no", (object)excludeInvoiceNo ?? DBNull.Value);

                var found = cmd.ExecuteScalar();
                return found != null && found != DBNull.Value;
            }
        }

        public DataTable GetPurchaseDashboardKpis(DateTime fromDate, DateTime toDate, DateTime prevFromDate, DateTime prevToDate)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH CurrentPurchases AS (
                SELECT p.invoice_no,
                       ISNULL(p.purchase_type, '') AS purchase_type,
                       CAST(ISNULL(p.total_amount,0) + ISNULL(p.total_tax,0) - ISNULL(p.discount_value,0) AS decimal(18,2)) AS net_amount,
                       p.purchase_date
                FROM pos_purchases p
                WHERE p.branch_id = @branch_id
                  AND p.purchase_date BETWEEN @fromDate AND @toDate
                  AND LOWER(ISNULL(p.purchase_type,'')) <> 'hold'
            ),
            PrevPurchases AS (
                SELECT CAST(ISNULL(p.total_amount,0) + ISNULL(p.total_tax,0) - ISNULL(p.discount_value,0) AS decimal(18,2)) AS net_amount
                FROM pos_purchases p
                WHERE p.branch_id = @branch_id
                  AND p.purchase_date BETWEEN @prevFromDate AND @prevToDate
                  AND LOWER(ISNULL(p.purchase_type,'')) <> 'hold'
            ),
            PaidByInvoiceTotal AS (
                SELECT sp.invoice_no, SUM(ISNULL(sp.debit, 0)) AS paid_amount
                FROM pos_suppliers_payments sp
                WHERE sp.branch_id = @branch_id
                  AND sp.entry_date <= @toDate
                GROUP BY sp.invoice_no
            ),
            PaidByInvoicePeriod AS (
                SELECT sp.invoice_no, SUM(ISNULL(sp.debit, 0)) AS paid_amount
                FROM pos_suppliers_payments sp
                WHERE sp.branch_id = @branch_id
                  AND sp.entry_date BETWEEN @fromDate AND @toDate
                GROUP BY sp.invoice_no
            )
            SELECT
                ISNULL((SELECT SUM(net_amount) FROM CurrentPurchases), 0) AS total_purchases,
                ISNULL((SELECT SUM(net_amount) FROM PrevPurchases), 0) AS total_purchases_prev,
                ISNULL((SELECT COUNT(1) FROM CurrentPurchases), 0) AS total_bills,
                ISNULL((SELECT SUM(CASE
                                   WHEN LOWER(c.purchase_type) = 'cash' THEN c.net_amount
                                   WHEN LOWER(c.purchase_type) = 'credit' THEN ISNULL(pp.paid_amount, 0)
                                   ELSE 0
                                 END)
                        FROM CurrentPurchases c
                        LEFT JOIN PaidByInvoicePeriod pp ON pp.invoice_no = c.invoice_no), 0) AS amount_paid,
                ISNULL((SELECT SUM(CASE
                                   WHEN LOWER(c.purchase_type) = 'credit'
                                        AND c.net_amount - ISNULL(pt.paid_amount, 0) > 0
                                   THEN c.net_amount - ISNULL(pt.paid_amount, 0)
                                   ELSE 0
                                 END)
                        FROM CurrentPurchases c
                        LEFT JOIN PaidByInvoiceTotal pt ON pt.invoice_no = c.invoice_no), 0) AS payable_outstanding,
                ISNULL((SELECT SUM(CASE
                                   WHEN LOWER(c.purchase_type) = 'credit'
                                        AND DATEDIFF(DAY, c.purchase_date, GETDATE()) > 30
                                        AND c.net_amount - ISNULL(pt.paid_amount, 0) > 0
                                   THEN c.net_amount - ISNULL(pt.paid_amount, 0)
                                   ELSE 0
                                 END)
                        FROM CurrentPurchases c
                        LEFT JOIN PaidByInvoiceTotal pt ON pt.invoice_no = c.invoice_no), 0) AS overdue_outstanding,
                CASE WHEN ISNULL((SELECT COUNT(1) FROM CurrentPurchases), 0) = 0 THEN 0
                     ELSE ISNULL((SELECT SUM(net_amount) FROM CurrentPurchases), 0) / NULLIF((SELECT COUNT(1) FROM CurrentPurchases), 0)
                END AS avg_purchase_value;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@prevFromDate", prevFromDate);
                cmd.Parameters.AddWithValue("@prevToDate", prevToDate);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPurchaseDashboardMonthlyPurchases(int months, DateTime endDate)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH M AS
            (
                SELECT CAST(DATEFROMPARTS(YEAR(DATEADD(MONTH, -(@months - 1), @endDate)), MONTH(DATEADD(MONTH, -(@months - 1), @endDate)), 1) AS date) AS month_start
                UNION ALL
                SELECT DATEADD(MONTH, 1, month_start)
                FROM M
                WHERE month_start < DATEFROMPARTS(YEAR(@endDate), MONTH(@endDate), 1)
            )
            SELECT MONTH(m.month_start) AS month_no,
                   YEAR(m.month_start) AS year_no,
                   LEFT(DATENAME(MONTH, m.month_start), 3) AS month_label,
                   ISNULL(SUM(ISNULL(p.total_amount,0) + ISNULL(p.total_tax,0) - ISNULL(p.discount_value,0)), 0) AS amount
            FROM M
            LEFT JOIN pos_purchases p ON p.purchase_date >= m.month_start
                                     AND p.purchase_date < DATEADD(MONTH, 1, m.month_start)
                                     AND p.branch_id = @branch_id
                                     AND LOWER(ISNULL(p.purchase_type,'')) <> 'hold'
            GROUP BY m.month_start
            ORDER BY m.month_start
            OPTION (MAXRECURSION 400);", cn))
            {
                cmd.Parameters.AddWithValue("@months", months);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPurchaseDashboardSupplierSplit(DateTime fromDate, DateTime toDate, int top = 5)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH S AS
            (
                SELECT CONCAT(ISNULL(ps.first_name, 'Unknown'), ' ', ISNULL(ps.last_name, '')) AS supplier_name,
                       SUM(ISNULL(pp.total_amount,0) + ISNULL(pp.total_tax,0) - ISNULL(pp.discount_value,0)) AS total_amount
                FROM pos_purchases pp
                LEFT JOIN pos_suppliers ps ON pp.supplier_id = ps.id
                WHERE pp.purchase_date BETWEEN @fromDate AND @toDate
                  AND pp.branch_id = @branch_id
                  AND LOWER(ISNULL(pp.purchase_type,'')) <> 'hold'
                GROUP BY CONCAT(ISNULL(ps.first_name, 'Unknown'), ' ', ISNULL(ps.last_name, ''))
            )
            SELECT TOP (@top)
                supplier_name,
                total_amount
            FROM S
            ORDER BY total_amount DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@top", top);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPurchaseDashboardYearlyTrend(int year)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH M AS
            (
                SELECT 1 AS month_no
                UNION ALL
                SELECT month_no + 1 FROM M WHERE month_no < 12
            )
            SELECT m.month_no,
                   LEFT(DATENAME(MONTH, DATEFROMPARTS(@year, m.month_no, 1)), 3) AS month_name,
                   ISNULL(SUM(CASE WHEN YEAR(p.purchase_date) = @year THEN ISNULL(p.total_amount,0) + ISNULL(p.total_tax,0) - ISNULL(p.discount_value,0) ELSE 0 END), 0) AS current_year_amount,
                   ISNULL(SUM(CASE WHEN YEAR(p.purchase_date) = @year - 1 THEN ISNULL(p.total_amount,0) + ISNULL(p.total_tax,0) - ISNULL(p.discount_value,0) ELSE 0 END), 0) AS last_year_amount
            FROM M m
            LEFT JOIN pos_purchases p ON MONTH(p.purchase_date) = m.month_no
                                     AND p.branch_id = @branch_id
                                     AND YEAR(p.purchase_date) IN (@year, @year - 1)
                                     AND LOWER(ISNULL(p.purchase_type,'')) <> 'hold'
            GROUP BY m.month_no
            ORDER BY m.month_no
            OPTION (MAXRECURSION 12);", cn))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPurchaseDashboardTopSuppliers(DateTime fromDate, DateTime toDate, int top = 10)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH P AS
            (
                SELECT pp.invoice_no,
                       pp.supplier_id,
                       CONCAT(ISNULL(ps.first_name, 'Unknown'), ' ', ISNULL(ps.last_name, '')) AS supplier_name,
                       ISNULL(pp.purchase_type, '') AS purchase_type,
                       CAST(ISNULL(pp.total_amount,0) + ISNULL(pp.total_tax,0) - ISNULL(pp.discount_value,0) AS decimal(18,2)) AS net_amount
                FROM pos_purchases pp
                LEFT JOIN pos_suppliers ps ON pp.supplier_id = ps.id
                WHERE pp.purchase_date BETWEEN @fromDate AND @toDate
                  AND pp.branch_id = @branch_id
                  AND LOWER(ISNULL(pp.purchase_type,'')) <> 'hold'
            ),
            Paid AS
            (
                SELECT invoice_no, SUM(ISNULL(debit,0)) AS paid_amount
                FROM pos_suppliers_payments
                WHERE branch_id = @branch_id
                GROUP BY invoice_no
            ),
            Totals AS
            (
                SELECT p.supplier_id,
                       p.supplier_name,
                       SUM(p.net_amount) AS total_purchases,
                       SUM(CASE
                               WHEN LOWER(p.purchase_type) = 'credit' AND p.net_amount - ISNULL(pd.paid_amount, 0) > 0
                               THEN p.net_amount - ISNULL(pd.paid_amount, 0)
                               ELSE 0
                           END) AS payable_amount
                FROM P p
                LEFT JOIN Paid pd ON pd.invoice_no = p.invoice_no
                GROUP BY p.supplier_id, p.supplier_name
            )
            SELECT TOP (@top)
                ROW_NUMBER() OVER (ORDER BY total_purchases DESC) AS rank_no,
                supplier_name,
                total_purchases,
                CASE WHEN gt.grand_total = 0 THEN 0 ELSE (total_purchases * 100.0 / gt.grand_total) END AS share_percent,
                payable_amount
            FROM Totals
            CROSS JOIN (SELECT ISNULL(SUM(total_purchases), 0) AS grand_total FROM Totals) gt
            ORDER BY total_purchases DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@top", top);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPurchaseDashboardPendingBills(DateTime fromDate, DateTime toDate, int top = 50)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH Paid AS
            (
                SELECT invoice_no, SUM(ISNULL(debit, 0)) AS paid_amount
                FROM pos_suppliers_payments
                WHERE branch_id = @branch_id
                GROUP BY invoice_no
            )
            SELECT TOP (@top)
                pp.invoice_no AS bill_no,
                CONCAT(ISNULL(ps.first_name, 'Unknown'), ' ', ISNULL(ps.last_name, '')) AS supplier_name,
                pp.purchase_date AS bill_date,
                DATEADD(DAY, 30, pp.purchase_date) AS due_date,
                CAST((ISNULL(pp.total_amount,0) + ISNULL(pp.total_tax,0) - ISNULL(pp.discount_value,0)) - ISNULL(pd.paid_amount,0) AS decimal(18,2)) AS amount,
                CASE
                    WHEN DATEDIFF(DAY, DATEADD(DAY, 30, pp.purchase_date), GETDATE()) > 0
                    THEN DATEDIFF(DAY, DATEADD(DAY, 30, pp.purchase_date), GETDATE())
                    ELSE 0
                END AS days_overdue
            FROM pos_purchases pp
            LEFT JOIN pos_suppliers ps ON pp.supplier_id = ps.id
            LEFT JOIN Paid pd ON pd.invoice_no = pp.invoice_no
            WHERE pp.purchase_date BETWEEN @fromDate AND @toDate
              AND pp.branch_id = @branch_id
              AND LOWER(ISNULL(pp.purchase_type, '')) = 'credit'
              AND ((ISNULL(pp.total_amount,0) + ISNULL(pp.total_tax,0) - ISNULL(pp.discount_value,0)) - ISNULL(pd.paid_amount,0)) > 0
            ORDER BY amount DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@top", top);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public decimal GetPurchasePaymentTotal(DateTime fromDate, DateTime toDate)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            SELECT ISNULL(SUM(ISNULL(sp.debit, 0)), 0)
            FROM pos_suppliers_payments sp
            INNER JOIN pos_purchases pp ON pp.invoice_no = sp.invoice_no
                                       AND pp.branch_id = sp.branch_id
            WHERE sp.entry_date BETWEEN @fromDate AND @toDate
              AND sp.branch_id = @branch_id
              AND LOWER(ISNULL(pp.purchase_type, '')) = 'credit';", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                cn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
            }
        }
    }
}
