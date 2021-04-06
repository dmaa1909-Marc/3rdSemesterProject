using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ApplicationServer.DB {
    public interface IUserAccountsDB {

        void AddUserAccount(UserAccount account);
        IEnumerable<UserAccount> FindUserAccountByUsernameOrEmail(string searchString);
        UserAccount GetUserAccountById(Guid id);
        IEnumerable<UserAccount> GetAllUserAccounts();
        void UpdateUserAccount(UserAccount userAccount);
        bool DeleteUserAccount(Guid id);
    }
}


