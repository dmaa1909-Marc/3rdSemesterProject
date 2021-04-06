using ApplicationServer.DB;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationServer.BusinessLogic {
    public class UserAccountManagement {

        private UserAccountsDB userAccountsDB = new UserAccountsDB();

        public void AddUserAccount(UserAccount newUserAccount) {
            userAccountsDB.AddUserAccount(newUserAccount);
        }

        public void UpdateUserAccount(UserAccount userAccount) {
            userAccountsDB.UpdateUserAccount(userAccount);
        }

        public IEnumerable<UserAccount> findUserAccountByUsernameOrEmail(string searchString) {
            return userAccountsDB.FindUserAccountByUsernameOrEmail(searchString);
        }

        public IEnumerable<UserAccount> GetAllUserAccounts() {
            return userAccountsDB.GetAllUserAccounts();
        }

        public bool DeleteUserAccount(Guid id) {
            return userAccountsDB.DeleteUserAccount(id);
        }

        public UserAccount GetUserAccountById(Guid id) {
            return userAccountsDB.GetUserAccountById(id);
        }
    }
}