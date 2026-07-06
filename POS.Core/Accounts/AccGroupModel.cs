using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POS.Core
{
    public class AccGroupModel : INotifyPropertyChanged
    {
        private int _id;
        private int _parentGroupId;
        private int _accountTypeId;
        private int _level;
        private string _groupCode;
        private string _groupName;
        private string _groupNameAr;
        private string _description;
        private bool _isActive = true;
        private int _companyId;
        private int _branchId;
        private DateTime _createdAt = DateTime.Now;
        private DateTime? _updatedAt;

        public int Id
        {
            get { return _id; }
            set { SetField(ref _id, value); }
        }

        public int ParentGroupId
        {
            get { return _parentGroupId; }
            set { SetField(ref _parentGroupId, value); }
        }

        public int AccountTypeId
        {
            get { return _accountTypeId; }
            set { SetField(ref _accountTypeId, value); }
        }

        public int Level
        {
            get { return _level; }
            set { SetField(ref _level, value); }
        }

        public string GroupCode
        {
            get { return _groupCode; }
            set { SetField(ref _groupCode, value); }
        }

        public string GroupName
        {
            get { return _groupName; }
            set { SetField(ref _groupName, value); }
        }

        public string GroupNameAr
        {
            get { return _groupNameAr; }
            set { SetField(ref _groupNameAr, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetField(ref _isActive, value); }
        }

        public int CompanyId
        {
            get { return _companyId; }
            set { SetField(ref _companyId, value); }
        }

        public int BranchId
        {
            get { return _branchId; }
            set { SetField(ref _branchId, value); }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { SetField(ref _createdAt, value); }
        }

        public DateTime? UpdatedAt
        {
            get { return _updatedAt; }
            set { SetField(ref _updatedAt, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
