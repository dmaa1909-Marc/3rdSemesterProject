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
    public class UserAccountsDB : IUserAccountsDB {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public void AddUserAccount(UserAccount account) {
            string query = "INSERT INTO " +
                "UserAccount(Id, Username, Email, RegistrationDate, AvatarId, PremiumAccount, PaymentDetailsId, Banned, RowVersion) " +
                "VALUES (@Id, @Username, @Email, @RegistrationDate, @AvatarId, @PremiumAccount, @PaymentDetailsId, @Banned, DEFAULT)";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    con.Execute(query, account);

                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<UserAccount> FindUserAccountByUsernameOrEmail(string searchString) {
            string query = "SELECT * FROM UserAccount WHERE Username LIKE @Email OR Email LIKE @Email";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    return con.Query<UserAccount>(query, new { Email = searchString + '%' });
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }

        public UserAccount GetUserAccountById(Guid id) {
            string query = "SELECT * FROM UserAccount WHERE Id = @Id";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    return con.QuerySingle<UserAccount>(query, new { Id = id });
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<UserAccount> GetAllUserAccounts() {
            string query = "SELECT * FROM UserAccount";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    return con.Query<UserAccount>(query);

                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }


        public void UpdateUserAccount(UserAccount userAccount) {
            string query = "UPDATE UserAccount SET " +
                               "AvatarId = @AvatarId, " +
                               "PremiumAccount = @PremiumAccount, " +
                               "PaymentDetailsId = @PaymentDetailsId, " +
                               "Banned = @Banned " +
                           //"OUTPUT inserted.RowVersion " +
                           "WHERE Id = @Id AND RowVersion = @RowVersion";

            try {
                //TODO: Return if update is rejected due to bad row version
                using(var con = new SqlConnection(connectionString)) {
                    con.Open();

                    //var trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                    //using(trans) {
                    int rowsAffected = con.Execute(query, userAccount/*, trans*/);
                    //trans.Commit();
                    Debug.WriteLine($"Rows updated: {rowsAffected}");
                    //}
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }

        public bool DeleteUserAccount(Guid id) {
            string query = "DELETE FROM UserAccount WHERE Id = @Id";

            try {
                using(var con = new SqlConnection(connectionString)) {
                    con.Open();

                    //using(var trans = con.BeginTransaction(IsolationLevel.ReadUncommitted)) {
                    con.Execute(query, new { Id = id }/*, trans*/);
                    //trans.Commit();
                    return true;
                    //}
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }
    }

}


