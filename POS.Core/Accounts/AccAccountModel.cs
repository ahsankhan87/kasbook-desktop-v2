using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POS.Core
{
    public class AccAccountModel : INotifyPropertyChanged
    {
        private int _id;
        private int _parentGroupId;
        private string _accountCode;
        private string _accountName;
        private string _accountNameAr;
        private string _accountType;
        private string _normalBalance;
        private decimal _openingBalance;
        private DateTime _openingBalanceDate = DateTime.Today;
        private bool _isBankAccount;
        private bool _isCashAccount;
        private string _bankName;
        private string _bankBranch;
        private string _accountNo;
        private string _iban;
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

        public string AccountCode
        {
            get { return _accountCode; }
            set { SetField(ref _accountCode, value); }
        }

        public string AccountName
        {
            get { return _accountName; }
            set { SetField(ref _accountName, value); }
        }

        public string AccountNameAr
        {
            get { return _accountNameAr; }
            set { SetField(ref _accountNameAr, value); }
        }

        public string AccountType
        {
            get { return _accountType; }
            set { SetField(ref _accountType, value); }
        }

        public string NormalBalance
        {
            get { return _normalBalance; }
            set { SetField(ref _normalBalance, value); }
        }

        public decimal OpeningBalance
        {
            get { return _openingBalance; }
            set { SetField(ref _openingBalance, value); }
        }

        public DateTime OpeningBalanceDate
        {
            get { return _openingBalanceDate; }
            set { SetField(ref _openingBalanceDate, value); }
        }

        public bool IsBankAccount
        {
            get { return _isBankAccount; }
            set { SetField(ref _isBankAccount, value); }
        }

        public bool IsCashAccount
        {
            get { return _isCashAccount; }
            set { SetField(ref _isCashAccount, value); }
        }

        public string BankName
        {
            get { return _bankName; }
            set { SetField(ref _bankName, value); }
        }

        public string BankBranch
        {
            get { return _bankBranch; }
            set { SetField(ref _bankBranch, value); }
        }

        public string AccountNo
        {
            get { return _accountNo; }
            set { SetField(ref _accountNo, value); }
        }

        public string Iban
        {
            get { return _iban; }
            set { SetField(ref _iban, value); }
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
