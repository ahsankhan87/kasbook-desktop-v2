using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.BLL
{
    public class StockAdjustmentBLL
    {
        private readonly StockAdjustmentDLL _dal;

        public StockAdjustmentBLL()
        {
            _dal = new StockAdjustmentDLL();
        }

        // ── Session ──────────────────────────────────────────────────

        public AdjSessionCreateResult CreateAdjSession(AdjSessionModel model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (model.AdjDate == DateTime.MinValue) model.AdjDate = DateTime.Today;
            if (string.IsNullOrWhiteSpace(model.AdjType))
                throw new ArgumentException("Adjustment type is required.", "model");
            if (model.WarehouseId <= 0)
                throw new ArgumentException("Warehouse is required.", "model");
            if (model.CreatedBy <= 0)
                throw new ArgumentException("Created-by user is required.", "model");

            return _dal.CreateAdjSession(model);
        }

        public void UpdateAdjSessionStatus(int adjId, string status, int userId)
        {
            if (adjId <= 0)                          throw new ArgumentException("Invalid adjId.",  "adjId");
            if (string.IsNullOrWhiteSpace(status))   throw new ArgumentException("status required.", "status");
            if (userId <= 0)                         throw new ArgumentException("Invalid userId.", "userId");

            _dal.UpdateAdjSessionStatus(adjId, status, userId);
            Log.LogAction("Stock Adjustment Status → " + status, "AdjId: " + adjId, userId, UsersModal.logged_in_branch_id);
        }

        public AdjSessionModel GetAdjSessionById(int adjId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");
            return _dal.GetAdjSessionById(adjId);
        }

        public DataTable GetAdjSessionList(DateTime? from, DateTime? to, string status)
        {
            return _dal.GetAdjSessionList(from, to, status);
        }

        public void DeleteDraftSession(int adjId, int userId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");
            _dal.DeleteDraftSession(adjId);
            Log.LogAction("Delete Draft Stock Adjustment", "AdjId: " + adjId, userId, UsersModal.logged_in_branch_id);
        }

        // ── Lines ─────────────────────────────────────────────────────

        public void SaveAdjLines(int adjId, List<AdjSessionLineModel> lines)
        {
            if (adjId <= 0)  throw new ArgumentException("Invalid adjId.", "adjId");
            if (lines == null) throw new ArgumentNullException("lines");
            foreach (AdjSessionLineModel l in lines)
            {
                if (l == null || l.ProductId <= 0)
                    throw new ArgumentException("Each line must have a valid ProductId.", "lines");
            }
            _dal.SaveAdjLines(adjId, lines);
        }

        public List<AdjSessionLineModel> GetAdjLines(int adjId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");
            return _dal.GetAdjLines(adjId);
        }

        public void UpdateLineVerified(int lineId, bool isVerified)
        {
            if (lineId <= 0) throw new ArgumentException("Invalid lineId.", "lineId");
            _dal.UpdateLineVerified(lineId, isVerified);
        }

        // ── Posting ───────────────────────────────────────────────────

        public AdjPostResult PostAdjustmentBatch(int adjId, int userId)
        {
            if (adjId <= 0)  throw new ArgumentException("Invalid adjId.", "adjId");
            if (userId <= 0) throw new ArgumentException("Invalid userId.", "userId");

            AdjPostResult result = _dal.PostAdjustmentBatch(adjId, userId);
            if (result.Success)
                Log.LogAction("Post Stock Adjustment", "AdjId: " + adjId + ", Lines: " + result.AffectedRows, userId, UsersModal.logged_in_branch_id);
            return result;
        }

        // ── Product ───────────────────────────────────────────────────

        public SqlDataReader GetProductsForIndex()
        {
            return _dal.GetProductsForIndex();
        }

        public ProductModal GetProductDetail(int productId)
        {
            if (productId <= 0) throw new ArgumentException("Invalid productId.", "productId");
            return _dal.GetProductDetail(productId);
        }

        public List<AdjAuditRow> GetProductStockHistory(int productId, DateTime from, DateTime to)
        {
            if (productId <= 0) throw new ArgumentException("Invalid productId.", "productId");
            return _dal.GetProductStockHistory(productId, from, to);
        }

        // ── Location ──────────────────────────────────────────────────

        public DataTable GetLocationHierarchy(int warehouseId)
        {
            if (warehouseId <= 0) throw new ArgumentException("Invalid warehouseId.", "warehouseId");
            return _dal.GetLocationHierarchy(warehouseId);
        }

        public DataTable GetProductLocation(int productId)
        {
            if (productId <= 0) throw new ArgumentException("Invalid productId.", "productId");
            return _dal.GetProductLocation(productId);
        }

        public void BulkUpdateLocations(DataTable locationChanges)
        {
            if (locationChanges == null || locationChanges.Rows.Count == 0) return;
            _dal.BulkUpdateLocations(locationChanges);
        }

        // ── Audit / Reports ───────────────────────────────────────────

        public AdjSessionSummaryModel GetSessionSummary(int adjId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");
            return _dal.GetSessionSummary(adjId);
        }

        public List<AdjAuditRow> GetAdjustmentHistory(int productId, DateTime fromDate, DateTime toDate)
        {
            return _dal.GetAdjustmentHistory(productId, fromDate, toDate);
        }

        public List<AdjSessionListRow> GetAdjustmentSessions(DateTime fromDate, DateTime toDate, string status = null)
        {
            return _dal.GetAdjustmentSessions(fromDate, toDate, status);
        }

        public List<StockVarianceRow> GetStockVarianceReport(DateTime fromDate, DateTime toDate)
        {
            return _dal.GetStockVarianceReport(fromDate, toDate);
        }

        public List<PriceChangeRow> GetPriceChangeReport(DateTime fromDate, DateTime toDate)
        {
            return _dal.GetPriceChangeReport(fromDate, toDate);
        }

        public void WriteAuditRow(int adjId, string adjNo, int productId, string changeType,
                                  string oldValue, string newValue, int userId, string reason)
        {
            _dal.WriteAuditRow(adjId, adjNo, productId, changeType, oldValue, newValue, userId, reason);
        }

        // ── Legacy shims (kept so existing call-sites compile) ─────────

        public AdjSessionCreateResult CreateSession(AdjSessionModel model, int userId)
        {
            if (model != null) model.CreatedBy = userId;
            return CreateAdjSession(model);
        }

        public void SaveDraft(int adjId, List<AdjSessionLineModel> lines, int userId)
        {
            SaveAdjLines(adjId, lines);
        }

        public void PostAdjustment(int adjId, int userId)
        {
            AdjPostResult r = PostAdjustmentBatch(adjId, userId);
            if (!r.Success) throw new ApplicationException(r.ErrorMessage);
        }

        public void ReverseAdjustment(int adjId, string reason, int userId)
        {
            if (adjId <= 0) throw new ArgumentException("Invalid adjId.", "adjId");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reverse reason is required.", "reason");
            _dal.ReverseAdjustment(adjId, reason, userId);
            Log.LogAction("Reverse Stock Adjustment", "AdjId: " + adjId + ", Reason: " + reason, userId, UsersModal.logged_in_branch_id);
        }
    }
}
