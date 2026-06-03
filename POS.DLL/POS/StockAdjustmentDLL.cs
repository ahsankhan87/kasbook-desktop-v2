using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class StockAdjustmentDLL
    {
        // ─────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────

        private static SqlConnection OpenConnection()
        {
            var cn = new SqlConnection(dbConnection.ConnectionString);
            cn.Open();
            return cn;
        }

        private static T ReadField<T>(SqlDataReader r, string col, T fallback = default(T))
        {
            int ord = r.GetOrdinal(col);
            if (r.IsDBNull(ord)) return fallback;
            return (T)Convert.ChangeType(r.GetValue(ord), typeof(T));
        }

        // ─────────────────────────────────────────────────────────────
        // SESSION METHODS
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates a new Draft adjustment session.
        /// Calls sp_StockAdjustment_CreateSession which internally calls
        /// sp_GenerateAdjNo and returns the new adj_id + adj_no.
        /// </summary>
        public AdjSessionCreateResult CreateAdjSession(AdjSessionModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StockAdjustment_CreateSession", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@adj_date",     model.AdjDate == DateTime.MinValue ? (object)DBNull.Value : model.AdjDate.Date);
                cmd.Parameters.AddWithValue("@adj_type",     (object)model.AdjType     ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@warehouse_id", model.WarehouseId);
                cmd.Parameters.AddWithValue("@notes",        (object)model.Notes       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@created_by",   model.CreatedBy);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new ApplicationException("sp_StockAdjustment_CreateSession returned no rows.");

                    return new AdjSessionCreateResult
                    {
                        AdjId = Convert.ToInt32(r["adj_id"]),
                        AdjNo = Convert.ToString(r["adj_no"])
                    };
                }
            }
        }

        /// <summary>
        /// Updates session status (Draft → Posted / Reversed) and the matching timestamp column.
        /// </summary>
        public void UpdateAdjSessionStatus(int adjId, string status, int userId)
        {
            if (adjId <= 0)     throw new ArgumentException("Invalid adjId.",  "adjId");
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("status required.", "status");

            // Build SET fragment safely — status is validated by the DB CHECK constraint
            string sql = @"UPDATE stk_adj_sessions
                           SET    status      = @status,
                                  modified_at = GETDATE(),
                                  posted_by   = CASE WHEN @status = 'Posted'   THEN @userId ELSE posted_by   END,
                                  posted_at   = CASE WHEN @status = 'Posted'   THEN GETDATE() ELSE posted_at   END,
                                  reversed_by = CASE WHEN @status = 'Reversed' THEN @userId ELSE reversed_by END,
                                  reversed_at = CASE WHEN @status = 'Reversed' THEN GETDATE() ELSE reversed_at END
                           WHERE  adj_id = @adjId";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@adjId",  adjId);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns a single session header (without lines).
        /// </summary>
        public AdjSessionModel GetAdjSessionById(int adjId)
        {
            const string sql = @"
                SELECT s.adj_id, s.adj_no, s.adj_date, s.adj_type, s.warehouse_id, s.status,
                       s.notes, s.created_by, s.created_at, s.modified_at,
                       s.posted_by, s.posted_at, s.reversed_by, s.reversed_at,
                       s.total_lines, s.qty_increases, s.qty_decreases,
                       s.price_changes, s.location_changes, s.reversal_reason
                FROM   stk_adj_sessions s
                WHERE  s.adj_id = @adjId";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@adjId", adjId);
                using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!r.Read()) return null;
                    return MapSessionHeader(r);
                }
            }
        }

        /// <summary>
        /// Returns a filterable DataTable of sessions for a grid.
        /// All parameters are optional; pass null to skip the filter.
        /// </summary>
        public DataTable GetAdjSessionList(DateTime? from, DateTime? to, string status)
        {
            const string sql = @"
                SELECT s.adj_id, s.adj_no,
                       CONVERT(VARCHAR(10), s.adj_date, 120) AS adj_date,
                       s.adj_type, s.status,
                       s.total_lines, s.qty_increases, s.qty_decreases,
                       s.price_changes, s.location_changes,
                       ISNULL(uc.full_name, uc.username) AS created_by,
                       ISNULL(up.full_name, up.username) AS posted_by,
                       s.notes, s.created_at
                FROM   stk_adj_sessions s
                JOIN   users uc ON uc.id = s.created_by
                LEFT   JOIN users up ON up.id = s.posted_by
                WHERE  (@from   IS NULL OR s.adj_date >= @from)
                  AND  (@to     IS NULL OR s.adj_date <= @to)
                  AND  (@status IS NULL OR s.status   = @status)
                ORDER  BY s.adj_date DESC, s.adj_id DESC";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@from",   from.HasValue   ? (object)from.Value.Date   : DBNull.Value);
                cmd.Parameters.AddWithValue("@to",     to.HasValue     ? (object)to.Value.Date     : DBNull.Value);
                cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status);

                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Deletes a Draft session and all its lines. Throws if session is not in Draft status.
        /// </summary>
        public void DeleteDraftSession(int adjId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");

            const string checkSql = "SELECT status FROM stk_adj_sessions WHERE adj_id = @adjId";
            const string delLines = "DELETE FROM stk_adj_lines    WHERE adj_id = @adjId";
            const string delSess  = "DELETE FROM stk_adj_sessions WHERE adj_id = @adjId AND status = 'Draft'";

            using (SqlConnection cn = OpenConnection())
            using (SqlTransaction tx = cn.BeginTransaction())
            {
                try
                {
                    using (SqlCommand chk = new SqlCommand(checkSql, cn, tx))
                    {
                        chk.Parameters.AddWithValue("@adjId", adjId);
                        object s = chk.ExecuteScalar();
                        if (s == null || s == DBNull.Value)
                            throw new ApplicationException("Adjustment session not found.");
                        if (Convert.ToString(s) != "Draft")
                            throw new InvalidOperationException("Only Draft sessions can be deleted.");
                    }

                    using (SqlCommand cmd = new SqlCommand(delLines, cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@adjId", adjId);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand(delSess, cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@adjId", adjId);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        // ─────────────────────────────────────────────────────────────
        // LINE METHODS
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Saves adjustment lines using an upsert pattern:
        ///   1. Delete all existing lines for the session.
        ///   2. Bulk-insert via SqlBulkCopy for large batches (≥ BULK_THRESHOLD rows).
        ///   3. Fall back to a single batched VALUES INSERT for small sets.
        /// </summary>
        public void SaveAdjLines(int adjId, List<AdjSessionLineModel> lines)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");
            if (lines == null) lines = new List<AdjSessionLineModel>();

            const int BULK_THRESHOLD = 500;
            const string delSql = "DELETE FROM stk_adj_lines WHERE adj_id = @adjId";
            const string updateHeader = @"UPDATE stk_adj_sessions SET modified_at = GETDATE() WHERE adj_id = @adjId";

            using (SqlConnection cn = OpenConnection())
            using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    // Delete existing lines
                    using (SqlCommand del = new SqlCommand(delSql, cn, tx))
                    {
                        del.Parameters.AddWithValue("@adjId", adjId);
                        del.ExecuteNonQuery();
                    }

                    if (lines.Count > 0)
                    {
                        if (lines.Count >= BULK_THRESHOLD)
                            BulkInsertLines(cn, tx, adjId, lines);
                        else
                            BatchInsertLines(cn, tx, adjId, lines);
                    }

                    using (SqlCommand upd = new SqlCommand(updateHeader, cn, tx))
                    {
                        upd.Parameters.AddWithValue("@adjId", adjId);
                        upd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        private static void BulkInsertLines(SqlConnection cn, SqlTransaction tx, int adjId, List<AdjSessionLineModel> lines)
        {
            using (var dt = BuildLinesDataTable(adjId, lines))
            using (var bulk = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, tx))
            {
                bulk.DestinationTableName = "stk_adj_lines";
                bulk.BatchSize            = 1000;
                bulk.BulkCopyTimeout      = 120;

                bulk.ColumnMappings.Add("adj_id",             "adj_id");
                bulk.ColumnMappings.Add("product_id",         "product_id");
                bulk.ColumnMappings.Add("system_qty",         "system_qty");
                bulk.ColumnMappings.Add("physical_qty",       "physical_qty");
                bulk.ColumnMappings.Add("current_sale_price", "current_sale_price");
                bulk.ColumnMappings.Add("new_sale_price",     "new_sale_price");
                bulk.ColumnMappings.Add("current_location",   "current_location");
                bulk.ColumnMappings.Add("new_location",       "new_location");
                bulk.ColumnMappings.Add("reason",             "reason");
                bulk.ColumnMappings.Add("notes",              "notes");
                bulk.ColumnMappings.Add("is_verified",        "is_verified");

                bulk.WriteToServer(dt);
            }
        }

        private static void BatchInsertLines(SqlConnection cn, SqlTransaction tx, int adjId, List<AdjSessionLineModel> lines)
        {
            // Single INSERT … SELECT … UNION ALL pattern in batches of 100
            const int BATCH = 100;
            for (int offset = 0; offset < lines.Count; offset += BATCH)
            {
                int count = Math.Min(BATCH, lines.Count - offset);
                var sb = new System.Text.StringBuilder(
                    "INSERT INTO stk_adj_lines (adj_id,product_id,system_qty,physical_qty," +
                    "current_sale_price,new_sale_price,current_location,new_location,reason,notes,is_verified) VALUES ");

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection  = cn;
                    cmd.Transaction = tx;

                    for (int i = 0; i < count; i++)
                    {
                        string p = "@p" + i;
                        if (i > 0) sb.Append(",");
                        sb.AppendFormat("(@adjId,{0}pid,{0}sq,{0}pq,{0}csp,{0}nsp,{0}cl,{0}nl,{0}rs,{0}nt,{0}iv)", p);

                        AdjSessionLineModel l = lines[offset + i];
                        cmd.Parameters.AddWithValue(p + "pid", l.ProductId);
                        cmd.Parameters.AddWithValue(p + "sq",  l.SystemQty);
                        cmd.Parameters.AddWithValue(p + "pq",  l.PhysicalQty);
                        cmd.Parameters.AddWithValue(p + "csp", l.CurrentSalePrice);
                        cmd.Parameters.AddWithValue(p + "nsp", l.NewSalePrice);
                        cmd.Parameters.AddWithValue(p + "cl",  (object)l.CurrentLocation ?? DBNull.Value);
                        cmd.Parameters.AddWithValue(p + "nl",  (object)l.NewLocation     ?? DBNull.Value);
                        cmd.Parameters.AddWithValue(p + "rs",  (object)l.Reason          ?? DBNull.Value);
                        cmd.Parameters.AddWithValue(p + "nt",  (object)l.Notes           ?? DBNull.Value);
                        cmd.Parameters.AddWithValue(p + "iv",  l.IsVerified);
                    }

                    cmd.Parameters.AddWithValue("@adjId", adjId);
                    cmd.CommandText = sb.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static DataTable BuildLinesDataTable(int adjId, List<AdjSessionLineModel> lines)
        {
            var dt = new DataTable();
            dt.Columns.Add("adj_id",             typeof(int));
            dt.Columns.Add("product_id",         typeof(int));
            dt.Columns.Add("system_qty",         typeof(decimal));
            dt.Columns.Add("physical_qty",       typeof(decimal));
            dt.Columns.Add("current_sale_price", typeof(decimal));
            dt.Columns.Add("new_sale_price",     typeof(decimal));
            dt.Columns.Add("current_location",   typeof(string));
            dt.Columns.Add("new_location",       typeof(string));
            dt.Columns.Add("reason",             typeof(string));
            dt.Columns.Add("notes",              typeof(string));
            dt.Columns.Add("is_verified",        typeof(bool));

            foreach (AdjSessionLineModel l in lines)
            {
                dt.Rows.Add(
                    adjId,
                    l.ProductId,
                    l.SystemQty,
                    l.PhysicalQty,
                    l.CurrentSalePrice,
                    l.NewSalePrice,
                    (object)l.CurrentLocation ?? DBNull.Value,
                    (object)l.NewLocation     ?? DBNull.Value,
                    (object)l.Reason          ?? DBNull.Value,
                    (object)l.Notes           ?? DBNull.Value,
                    l.IsVerified);
            }
            return dt;
        }

        /// <summary>
        /// Returns all lines for a session.
        /// </summary>
        public List<AdjSessionLineModel> GetAdjLines(int adjId)
        {
            const string sql = @"
                SELECT l.line_id, l.adj_id, l.product_id,
                       l.system_qty, l.physical_qty, l.qty_difference,
                       l.current_sale_price, l.new_sale_price, l.price_difference,
                       l.current_location, l.new_location,
                       l.reason, l.notes, l.is_verified
                FROM   stk_adj_lines l
                WHERE  l.adj_id = @adjId
                ORDER  BY l.line_id";

            var list = new List<AdjSessionLineModel>();
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@adjId", adjId);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new AdjSessionLineModel
                        {
                            LineId           = ReadField<int>(r, "line_id"),
                            AdjId            = adjId,
                            ProductId        = ReadField<int>(r, "product_id"),
                            SystemQty        = ReadField<decimal>(r, "system_qty"),
                            PhysicalQty      = ReadField<decimal>(r, "physical_qty"),
                            QtyDifference    = ReadField<decimal>(r, "qty_difference"),
                            CurrentSalePrice = ReadField<decimal>(r, "current_sale_price"),
                            NewSalePrice     = ReadField<decimal>(r, "new_sale_price"),
                            CurrentLocation  = ReadField<string>(r, "current_location"),
                            NewLocation      = ReadField<string>(r, "new_location"),
                            Reason           = ReadField<string>(r, "reason"),
                            Notes            = ReadField<string>(r, "notes"),
                            IsVerified       = ReadField<bool>(r, "is_verified")
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Updates the is_verified flag on a single line — used by scan-mode verification.
        /// </summary>
        public void UpdateLineVerified(int lineId, bool isVerified)
        {
            if (lineId <= 0) throw new ArgumentException("Invalid lineId.", "lineId");

            const string sql = "UPDATE stk_adj_lines SET is_verified = @v WHERE line_id = @id";
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", lineId);
                cmd.Parameters.AddWithValue("@v",  isVerified);
                cmd.ExecuteNonQuery();
            }
        }

        // ─────────────────────────────────────────────────────────────
        // POSTING METHOD
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Calls sp_PostStockAdjustment in an explicit transaction.
        /// The stored procedure performs all stock/price/location updates and
        /// writes the full audit trail atomically.
        /// Returns an AdjPostResult with success flag, affected rows, and error message.
        /// </summary>
        public AdjPostResult PostAdjustmentBatch(int adjId, int userId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");

            using (SqlConnection cn = OpenConnection())
            using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PostStockAdjustment", cn, tx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300; // large batches may take time
                        cmd.Parameters.AddWithValue("@adj_id",  adjId);
                        cmd.Parameters.AddWithValue("@user_id", userId);

                        // sp_PostStockAdjustment returns @@ROWCOUNT of affected product lines
                        SqlParameter affected = new SqlParameter("@rows_affected", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(affected);

                        cmd.ExecuteNonQuery();
                        tx.Commit();

                        int rows = affected.Value == DBNull.Value ? 0 : Convert.ToInt32(affected.Value);
                        return new AdjPostResult { Success = true, AffectedRows = rows };
                    }
                }
                catch (SqlException ex)
                {
                    try { tx.Rollback(); } catch { }
                    return new AdjPostResult
                    {
                        Success      = false,
                        AffectedRows = 0,
                        ErrorMessage = ex.Message
                    };
                }
                catch (Exception ex)
                {
                    try { tx.Rollback(); } catch { }
                    return new AdjPostResult
                    {
                        Success      = false,
                        AffectedRows = 0,
                        ErrorMessage = ex.Message
                    };
                }
            }
        }

        // ─────────────────────────────────────────────────────────────
        // PRODUCT METHODS
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns a forward-only DataReader for bulk loading the in-memory
        /// product search index (50 k+ rows). Caller must dispose the reader
        /// AND the connection when done. Use CommandBehavior.CloseConnection.
        /// </summary>
        public SqlDataReader GetProductsForIndex()
        {
            const string sql = @"
                SELECT p.id, p.code, p.name, p.name_ar,
                       p.unit_price, p.cost_price, p.qty,
                       p.status, p.category_id,
                       ISNULL(c.name,'') AS category_name,
                       ISNULL(p.location_code,'') AS location_code,
                       ISNULL(p.barcode,'') AS barcode,
                       ISNULL(p.item_number,'') AS item_number
                FROM   mst_products p WITH (NOLOCK)
                LEFT   JOIN categories c WITH (NOLOCK) ON c.id = p.category_id
                WHERE  p.status = 1
                ORDER  BY p.name";

            // Connection deliberately left open — caller owns it via CloseConnection
            SqlConnection cn = new SqlConnection(dbConnection.ConnectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 120;
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Returns full product detail for the drawer panel.
        /// </summary>
        public ProductModal GetProductDetail(int productId)
        {
            const string sql = @"
                SELECT p.id, p.code, p.name, p.name_ar, p.item_number, p.part_number,
                       p.barcode, p.unit_price, p.unit_price_2, p.cost_price, p.avg_cost,
                       p.qty, p.status, p.category_id,
                       ISNULL(c.name,'') AS category,
                       ISNULL(c.code,'') AS category_code,
                       ISNULL(p.location_code,'') AS location_code,
                       ISNULL(u.name,'') AS unit_name,
                       p.unit_id, p.tax_id,
                       p.description, p.date_created, p.date_updated
                FROM   mst_products p
                LEFT   JOIN categories c ON c.id = p.category_id
                LEFT   JOIN units      u ON u.id = p.unit_id
                WHERE  p.id = @productId";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@productId", productId);
                using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!r.Read()) return null;
                    return new ProductModal
                    {
                        id             = ReadField<int>(r,    "id"),
                        code           = ReadField<string>(r, "code"),
                        name           = ReadField<string>(r, "name"),
                        name_ar        = ReadField<string>(r, "name_ar"),
                        item_number    = ReadField<string>(r, "item_number"),
                        part_number    = ReadField<string>(r, "part_number"),
                        unit_price     = ReadField<double>(r, "unit_price"),
                        unit_price_2   = ReadField<double>(r, "unit_price_2"),
                        cost_price     = ReadField<double>(r, "cost_price"),
                        avg_cost       = ReadField<double>(r, "avg_cost"),
                        qty            = ReadField<double>(r, "qty"),
                        status         = ReadField<bool>(r,   "status"),
                        category_id    = ReadField<int>(r,    "category_id"),
                        category       = ReadField<string>(r, "category"),
                        category_code  = ReadField<string>(r, "category_code"),
                        location_code  = ReadField<string>(r, "location_code"),
                        unit_id        = ReadField<int>(r,    "unit_id"),
                        tax_id         = ReadField<int>(r,    "tax_id"),
                        description    = ReadField<string>(r, "description"),
                        date_created   = ReadField<string>(r, "date_created"),
                        date_updated   = ReadField<string>(r, "date_updated")
                    };
                }
            }
        }

        /// <summary>
        /// Returns the audit history for a single product between two dates.
        /// </summary>
        public List<AdjAuditRow> GetProductStockHistory(int productId, DateTime from, DateTime to)
        {
            return GetAdjustmentHistory(productId, from, to);
        }

        // ─────────────────────────────────────────────────────────────
        // LOCATION METHODS
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns a three-level location hierarchy (aisle / shelf / bin)
        /// for a warehouse, suitable for cascading dropdowns.
        /// </summary>
        public DataTable GetLocationHierarchy(int warehouseId)
        {
            const string sql = @"
                SELECT DISTINCT
                       aisle_code,
                       shelf_code,
                       bin_code,
                       location_code
                FROM   mst_product_locations
                WHERE  warehouse_id = @wid
                ORDER  BY aisle_code, shelf_code, bin_code";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@wid", warehouseId);
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Returns the current location rows for a product across all warehouses.
        /// </summary>
        public DataTable GetProductLocation(int productId)
        {
            const string sql = @"
                SELECT l.loc_id, l.product_id, l.warehouse_id,
                       l.aisle_code, l.shelf_code, l.bin_code,
                       l.location_code, l.qty_at_location, l.is_primary,
                       ISNULL(w.name,'') AS warehouse_name
                FROM   mst_product_locations l
                LEFT   JOIN warehouses w ON w.id = l.warehouse_id
                WHERE  l.product_id = @productId
                ORDER  BY l.is_primary DESC, l.location_code";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@productId", productId);
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Bulk-updates product locations via SqlBulkCopy into a staging table,
        /// then merges into mst_product_locations using a TVP-style UPDATE.
        /// Expected DataTable columns: product_id, warehouse_id,
        ///   aisle_code, shelf_code, bin_code, qty_at_location, is_primary.
        /// </summary>
        public void BulkUpdateLocations(DataTable locationChanges)
        {
            if (locationChanges == null || locationChanges.Rows.Count == 0) return;

            const string createStaging = @"
                CREATE TABLE #loc_staging (
                    product_id      INT           NOT NULL,
                    warehouse_id    INT           NOT NULL,
                    aisle_code      VARCHAR(10)   NOT NULL,
                    shelf_code      VARCHAR(10)   NOT NULL,
                    bin_code        VARCHAR(10)   NOT NULL,
                    qty_at_location DECIMAL(18,3) NOT NULL DEFAULT 0,
                    is_primary      BIT           NOT NULL DEFAULT 0
                )";

            const string mergeSQL = @"
                MERGE mst_product_locations AS tgt
                USING #loc_staging           AS src
                   ON  tgt.product_id   = src.product_id
                  AND  tgt.warehouse_id = src.warehouse_id
                  AND  tgt.aisle_code   = src.aisle_code
                  AND  tgt.shelf_code   = src.shelf_code
                  AND  tgt.bin_code     = src.bin_code
                WHEN MATCHED THEN
                    UPDATE SET qty_at_location = src.qty_at_location,
                               is_primary      = src.is_primary
                WHEN NOT MATCHED BY TARGET THEN
                    INSERT (product_id, warehouse_id, aisle_code, shelf_code,
                            bin_code, qty_at_location, is_primary)
                    VALUES (src.product_id, src.warehouse_id, src.aisle_code,
                            src.shelf_code, src.bin_code,
                            src.qty_at_location, src.is_primary);

                DROP TABLE #loc_staging;";

            using (SqlConnection cn = OpenConnection())
            using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    // Create temp staging table
                    using (SqlCommand create = new SqlCommand(createStaging, cn, tx))
                        create.ExecuteNonQuery();

                    // Bulk-copy into staging
                    using (var bulk = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, tx))
                    {
                        bulk.DestinationTableName = "#loc_staging";
                        bulk.BatchSize            = 1000;
                        bulk.BulkCopyTimeout      = 120;
                        bulk.ColumnMappings.Add("product_id",      "product_id");
                        bulk.ColumnMappings.Add("warehouse_id",    "warehouse_id");
                        bulk.ColumnMappings.Add("aisle_code",      "aisle_code");
                        bulk.ColumnMappings.Add("shelf_code",      "shelf_code");
                        bulk.ColumnMappings.Add("bin_code",        "bin_code");
                        bulk.ColumnMappings.Add("qty_at_location", "qty_at_location");
                        bulk.ColumnMappings.Add("is_primary",      "is_primary");
                        bulk.WriteToServer(locationChanges);
                    }

                    // Merge staging → permanent table
                    using (SqlCommand merge = new SqlCommand(mergeSQL, cn, tx))
                    {
                        merge.CommandTimeout = 120;
                        merge.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        // ─────────────────────────────────────────────────────────────
        // AUDIT / REPORT METHODS  (unchanged, kept for compatibility)
        // ─────────────────────────────────────────────────────────────

        public List<AdjAuditRow> GetAdjustmentHistory(int productId, DateTime fromDate, DateTime toDate)
        {
            var list = new List<AdjAuditRow>();
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetAdjustmentHistory", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@FromDate",  fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate",    toDate.Date);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new AdjAuditRow
                        {
                            AuditId    = ReadField<int>(r,      "audit_id"),
                            AdjNo      = ReadField<string>(r,   "adj_no"),
                            ChangedAt  = ReadField<DateTime>(r, "changed_at"),
                            ChangeType = ReadField<string>(r,   "change_type"),
                            OldValue   = ReadField<string>(r,   "old_value"),
                            NewValue   = ReadField<string>(r,   "new_value"),
                            ChangedBy  = ReadField<string>(r,   "changed_by"),
                            Reason     = ReadField<string>(r,   "reason"),
                            AdjType    = ReadField<string>(r,   "adj_type")
                        });
                    }
                }
            }
            return list;
        }

        public List<AdjSessionListRow> GetAdjustmentSessions(DateTime fromDate, DateTime toDate, string status)
        {
            var list = new List<AdjSessionListRow>();
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetAdjustmentSessions", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate",   toDate.Date);
                cmd.Parameters.AddWithValue("@Status",   string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new AdjSessionListRow
                        {
                            AdjId           = ReadField<int>(r,    "adj_id"),
                            AdjNo           = ReadField<string>(r, "adj_no"),
                            AdjDate         = ReadField<string>(r, "adj_date"),
                            AdjType         = ReadField<string>(r, "adj_type"),
                            Status          = ReadField<string>(r, "status"),
                            ProductCount    = ReadField<int>(r,    "product_count"),
                            QtyIncreases    = ReadField<int>(r,    "qty_increases"),
                            QtyDecreases    = ReadField<int>(r,    "qty_decreases"),
                            PriceChanges    = ReadField<int>(r,    "price_changes"),
                            LocationChanges = ReadField<int>(r,    "location_changes"),
                            CreatedBy       = ReadField<string>(r, "created_by"),
                            PostedBy        = ReadField<string>(r, "posted_by"),
                            Notes           = ReadField<string>(r, "notes")
                        });
                    }
                }
            }
            return list;
        }

        public List<StockVarianceRow> GetStockVarianceReport(DateTime fromDate, DateTime toDate)
        {
            var list = new List<StockVarianceRow>();
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StockVarianceReport", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate",   toDate.Date);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new StockVarianceRow
                        {
                            ProductCode        = ReadField<string>(r,   "product_code"),
                            ProductName        = ReadField<string>(r,   "product_name"),
                            CategoryName       = ReadField<string>(r,   "category_name"),
                            TotalAdjustments   = ReadField<int>(r,      "total_adjustments"),
                            TotalQtyAdjusted   = ReadField<decimal>(r,  "total_qty_adjusted"),
                            TotalValueImpact   = ReadField<decimal>(r,  "total_value_impact"),
                            LastAdjustmentDate = ReadField<DateTime>(r, "last_adjustment_date")
                        });
                    }
                }
            }
            return list;
        }

        public List<PriceChangeRow> GetPriceChangeReport(DateTime fromDate, DateTime toDate)
        {
            var list = new List<PriceChangeRow>();
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_PriceChangeReport", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate",   toDate.Date);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new PriceChangeRow
                        {
                            ProductCode = ReadField<string>(r,  "product_code"),
                            ProductName = ReadField<string>(r,  "product_name"),
                            CategoryName= ReadField<string>(r,  "category_name"),
                            AdjNo       = ReadField<string>(r,  "adj_no"),
                            ChangeDate  = ReadField<string>(r,  "change_date"),
                            OldPrice    = ReadField<decimal>(r, "old_price"),
                            NewPrice    = ReadField<decimal>(r, "new_price"),
                            PctChange   = ReadField<decimal>(r, "pct_change"),
                            ApprovedBy  = ReadField<string>(r,  "approved_by"),
                            Reason      = ReadField<string>(r,  "reason")
                        });
                    }
                }
            }
            return list;
        }

        public void WriteAuditRow(int adjId, string adjNo, int productId, string changeType,
                                  string oldValue, string newValue, int userId, string reason)
        {
            const string sql = @"
                INSERT INTO stk_adj_audit
                    (adj_id, adj_no, product_id, change_type, old_value, new_value, changed_by, changed_at, reason)
                VALUES
                    (@adj_id,@adj_no,@product_id,@change_type,@old_value,@new_value,@changed_by,GETDATE(),@reason)";

            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@adj_id",      adjId);
                cmd.Parameters.AddWithValue("@adj_no",      (object)adjNo      ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@product_id",  productId);
                cmd.Parameters.AddWithValue("@change_type", changeType);
                cmd.Parameters.AddWithValue("@old_value",   (object)oldValue   ?? string.Empty);
                cmd.Parameters.AddWithValue("@new_value",   (object)newValue   ?? string.Empty);
                cmd.Parameters.AddWithValue("@changed_by",  userId);
                cmd.Parameters.AddWithValue("@reason",      (object)reason     ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public void ReverseAdjustment(int adjId, string reason, int userId)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StockAdjustment_Reverse", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@adj_id",  adjId);
                cmd.Parameters.AddWithValue("@reason",  (object)reason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.ExecuteNonQuery();
            }
        }

        public AdjSessionSummaryModel GetSessionSummary(int adjId)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StockAdjustment_GetSessionSummary", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@adj_id", adjId);
                using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!r.Read()) return new AdjSessionSummaryModel();
                    return new AdjSessionSummaryModel
                    {
                        TotalLines           = ReadField<int>(r,     "total_lines"),
                        QtyIncrease          = ReadField<decimal>(r, "qty_increase"),
                        QtyDecrease          = ReadField<decimal>(r, "qty_decrease"),
                        PriceChangeProducts  = ReadField<int>(r,     "price_change_products"),
                        RelocatedProducts    = ReadField<int>(r,     "relocated_products"),
                        TotalStockValueImpact= ReadField<decimal>(r, "total_stock_value_impact")
                    };
                }
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Private mapping helpers
        // ─────────────────────────────────────────────────────────────

        private static AdjSessionModel MapSessionHeader(SqlDataReader r)
        {
            return new AdjSessionModel
            {
                AdjId       = ReadField<int>(r,      "adj_id"),
                AdjNo       = ReadField<string>(r,   "adj_no"),
                AdjDate     = ReadField<DateTime>(r, "adj_date"),
                AdjType     = ReadField<string>(r,   "adj_type"),
                WarehouseId = ReadField<int>(r,      "warehouse_id"),
                Status      = ReadField<string>(r,   "status"),
                Notes       = ReadField<string>(r,   "notes"),
                CreatedBy   = ReadField<int>(r,      "created_by"),
                CreatedAt   = ReadField<DateTime>(r, "created_at"),
                ModifiedAt  = r.IsDBNull(r.GetOrdinal("modified_at")) ? (DateTime?)null
                              : Convert.ToDateTime(r["modified_at"]),
                PostedBy    = r.IsDBNull(r.GetOrdinal("posted_by"))   ? (int?)null
                              : Convert.ToInt32(r["posted_by"]),
                PostedAt    = r.IsDBNull(r.GetOrdinal("posted_at"))   ? (DateTime?)null
                              : Convert.ToDateTime(r["posted_at"])
            };
        }
    }
}
