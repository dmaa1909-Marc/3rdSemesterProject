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
    public class AvatarsController : ApiController {

        AvatarsManagement avatarsManager = new AvatarsManagement();

        // GET: api/Avatars
        public IHttpActionResult Get() {

            try {
                IEnumerable<Avatar> avatars = avatarsManager.GetAllAvatars();

                if(avatars != null) {
                    return Ok(avatars);
                }
                else {
                    return InternalServerError();
                }
            }
            catch {
                return InternalServerError();
            }
        }

        // GET: api/Avatars/5
        public IHttpActionResult Get(int id) {
            try {
            Avatar avatar = avatarsManager.GetAvatarById(id);

                if(avatar != null) {
                    return Ok(avatar);
                }
                else {
                    return InternalServerError();
                }
            }
            catch {
                return InternalServerError();
            }
        }

        //// POST: api/Avatars
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Avatars/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Avatars/5
        //public void Delete(int id)
        //{
        //}
    }
}
