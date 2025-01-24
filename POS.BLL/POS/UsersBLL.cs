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
    public class UsersBLL
    {
        public DataTable GetAll()
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }

        public DataTable GetUser(int user_id)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.GetUser(user_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetUserModules(int user_id)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.GetUserModules(user_id);
            }
            catch
            {

                throw;
            }
        }
        
        public int Login(UsersModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.Login(obj);
            }
            catch
            {

                throw;
            }
        }
        /// <summary>
        /// Generic Search from database
        /// </summary>
        /// <param name="condition">i.e. first_name="Ahsan"</param>
        /// <returns>DataTable</returns>
        public DataTable SearchRecord(String condition)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByUsersID(int Users_id)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.SearchRecordByUsersID(Users_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetUserRights(int Users_id)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.GetUserRights(Users_id);
            }
            catch
            {

                throw;
            }
        }

        public bool IsUsernameExist(string username)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.IsUsernameExist(username);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(UsersModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int InsertUserModules(UsersModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.InsertUserModules(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(UsersModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int UpdateUserRights(UsersModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.UpdateUserRights(obj);
            }
            catch
            {

                throw;
            }
        }

        public int UpdatePassword(UsersModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.UpdatePassword(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int UsersId)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.Delete(UsersId);
            }
            catch
            {

                throw;
            }
        }
        public int DeleteUserModules(int UsersId)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.DeleteUserModules(UsersId);
            }
            catch
            {

                throw;
            }
        }
        public int DeleteUserRights(int UsersId)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.DeleteUserRights(UsersId);
            }
            catch
            {

                throw;
            }
        }

        public int InsertUserCommission(JournalsModal obj)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.InsertUserCommission(obj);
            }
            catch
            {

                throw;
            }
        }

        public long GetUserCommissionBalance(int UserId)
        {
            try
            {
                UsersDLL objDLL = new UsersDLL();
                return objDLL.GetUserCommissionBalance(UserId);
            }
            catch
            {

                throw;
            }
        }
    }
}
