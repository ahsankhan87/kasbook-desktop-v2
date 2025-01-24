using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.DLL;
using System.Data;
using POS.Core;

namespace POS.BLL
{
    public class ProductGroupsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }

        public DataTable SearchAlternateProducts(int alt_no)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.SearchAlternateProducts(alt_no);
            }
            catch
            {

                throw;
            }
        }

        public int GetMaxAlternateNo()
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.GetMaxAlternateNo();
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByName(String condition)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.SearchRecordByName(condition);
            }
            catch
            {

                throw;
            }
        }
        
        public DataTable SearchRecord(String condition)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.SearchRecordByGroup(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByProductGroupsID(int ProductGroups_id)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.SearchRecordByProductGroupsID(ProductGroups_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(ProductGroupsModal obj)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }
        public string InsertProductGroupDetail(ProductGroupsModal obj)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.InsertProductGroupDetail(obj);
            }
            catch
            {

                throw;
            }
        }
        public string InsertProductAlternate(ProductGroupsModal obj)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.InsertProductAlternate(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(ProductGroupsModal obj)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int ProductGroupsId)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.Delete(ProductGroupsId);
            }
            catch
            {

                throw;
            }
        }

        public int DeleteAltNo(int ProductId)
        {
            try
            {
                ProductGroupsDLL objDLL = new ProductGroupsDLL();
                return objDLL.DeleteAltNo(ProductId);
            }
            catch
            {

                throw;
            }
        }
    }
}
