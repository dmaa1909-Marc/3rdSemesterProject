using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Test.ApplicationServerTest {
    [TestClass()]
    public class UserAccountsDBTest {

        private static UserAccountsDB userAccountsDB;

        [ClassInitialize]
        public static void InstantiateUserAccountsDBClassTest(TestContext testContext) {
            userAccountsDB = new UserAccountsDB();
        }

        //[TestMethod()]
        //public void AddUserAccountTest() {
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void findUserAccountByEmailTest() {
            //Arragne
            string emailToSearchFor = "Test1@mail.dk";

            //Act
            IEnumerable<UserAccount> foundUserAccounts = userAccountsDB.FindUserAccountByUsernameOrEmail(emailToSearchFor);

            //Assert
            Assert.IsNotNull(foundUserAccounts);
            Assert.AreEqual(1, foundUserAccounts.Count<UserAccount>(userAccount => userAccount.Email == emailToSearchFor));
        }
    }
}