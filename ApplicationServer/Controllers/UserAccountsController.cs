using ApplicationServer.BusinessLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApplicationServer.Controllers {

    [Authorize]
    public class UserAccountsController : ApiController {

        private UserAccountManagement userAccountManager = new UserAccountManagement();

        // GET: api/UserAccount
        public IHttpActionResult Get() {

            try {
                return Ok(userAccountManager.GetAllUserAccounts());
            }
            catch {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get([FromUri] string searchString) {

            try {
                return Ok(userAccountManager.findUserAccountByUsernameOrEmail(searchString));
            }
            catch {
                return InternalServerError();
            }
        }

        //GET: api/UserAccount/5
        public IHttpActionResult Get([FromUri] Guid id) {
            try {
                return Ok(userAccountManager.GetUserAccountById(id));
            }
            catch {
                return InternalServerError();
            }
        }

        // POST: api/UserAccount
        public IHttpActionResult Post([FromBody] UserAccount newUserAccount) {
            try {
                userAccountManager.AddUserAccount(newUserAccount);
                return Ok();
            }
            catch {
                return InternalServerError();
            }
        }


        // PUT: api/UserAccount/5
        public IHttpActionResult Put(Guid id, [FromBody] UserAccount userAccount) {

            if(id == userAccount.Id) {
                userAccountManager.UpdateUserAccount(userAccount);
                return Ok();
            }
            else {
                return BadRequest("id and object id are inconsistent");
            }
        }

        // DELETE: api/UserAccount/5
        public void Delete(Guid id) {
            userAccountManager.DeleteUserAccount(id);
        }
    }
}
