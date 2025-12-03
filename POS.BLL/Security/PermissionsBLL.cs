using System;
using System.Data;
using pos.DAL;

namespace POS.BLL
{
    public class PermissionsBLL
    {
        private readonly PermissionsDAL _dal;

        // Inject your connection string (from config)
        public PermissionsBLL()
        {
            _dal = new PermissionsDAL();
        }

        public DataTable GetAll() => _dal.GetAll();

        public DataTable Search(string keyword) => _dal.Search(keyword);

        public int Create(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName)) throw new ArgumentException("Permission name required.");
            return _dal.Insert(permissionName.Trim());
        }

        public int Update(int id, string permissionName)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(permissionName)) throw new ArgumentException("Permission name required.");
            return _dal.Update(id, permissionName.Trim());
        }

        public int Delete(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            return _dal.Delete(id);
        }
    }
}