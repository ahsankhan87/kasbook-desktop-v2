using POS.Core;
using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class DiscountSchemesBLL
    {
        private readonly DiscountSchemesDLL _dll = new DiscountSchemesDLL();

        public DataTable GetAll(int branchId)
        {
            try { return _dll.GetAll(branchId); }
            catch { throw; }
        }

        public DataTable GetAllActive(int branchId)
        {
            try { return _dll.GetAllActive(branchId); }
            catch { throw; }
        }

        public DataTable GetById(int id)
        {
            try { return _dll.GetById(id); }
            catch { throw; }
        }

        public int Insert(DiscountSchemeModal info)
        {
            try
            {
                int id = _dll.Insert(info);
                Log.LogAction(
                    "Insert Discount Scheme",
                    $"Name: {info.name}",
                    UsersModal.logged_in_userid,
                    UsersModal.logged_in_branch_id);
                return id;
            }
            catch { throw; }
        }

        public int Update(DiscountSchemeModal info)
        {
            try
            {
                int id = _dll.Update(info);
                Log.LogAction("Update Discount Scheme", $"ID: {info.id}, Name: {info.name}",
                    UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return id;
            }
            catch { throw; }
        }

        public int Delete(int id)
        {
            try
            {
                _dll.Delete(id);
                Log.LogAction("Delete Discount Scheme", $"ID: {id}",
                    UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return id;
            }
            catch { throw; }
        }

        public int ToggleActive(int id)
        {
            try
            {
                _dll.ToggleActive(id);
                Log.LogAction("Toggle Discount Scheme", $"ID: {id}",
                    UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return id;
            }
            catch { throw; }
        }
    }
}
