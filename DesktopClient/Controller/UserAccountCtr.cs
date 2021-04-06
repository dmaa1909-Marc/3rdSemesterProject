using Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Controller {
    class UserAccountCtr {
       
        UserAccountsRepository UserAccountsRepository = new UserAccountsRepository();

        IEnumerable<UserAccount> UserAccounts = new List<UserAccount>();

        public IEnumerable<UserAccount> FindUserAccountByUsernameOrEmail(string searchString) {
            UserAccounts = UserAccountsRepository.FindUserAccountByUsernameOrEMail(searchString);
            if(UserAccounts != null) {
                return UserAccounts.ToList();
            } else {
                return null;
            }
        }

        //public bool UpdateUserAccount(Guid id) {
        //    UserAccount userAccountToUpdate = UserAccounts.First(userAccount => userAccount.Id == id);
        //    return UserAccountsRepository.UpdateUserAccount(userAccountToUpdate);
        //}

        public bool UpdateUserAccount(UserAccount userAccount) {
            return UserAccountsRepository.UpdateUserAccount(userAccount);
        }

        public IEnumerable<UserAccount> GetAllUserAccounts() {
            UserAccounts = UserAccountsRepository.GetAllUserAccounts();
            if(UserAccounts != null) {
                return UserAccounts.ToList();
            }
            else {
                return null;
            }
        }

        public bool DeleteUserAccount(Guid id) {
            return UserAccountsRepository.DeleteUserAccount(id);
        }

    }
}
