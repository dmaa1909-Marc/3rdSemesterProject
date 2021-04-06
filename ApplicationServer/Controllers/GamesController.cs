using ApplicationServer.GameLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApplicationServer.Controllers {

    [Authorize]
    public class GamesController : ApiController {
        GameManagement gameManager = GameManagement.Instance;
        //// GET: api/Games
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Games/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Games
        public IHttpActionResult Post(PostNewGame game) {
            //Create a game
            string newGamePin = gameManager.CreateNewGame(game.GameName);

            //Return game pin
            return Ok<string>(newGamePin);
        }

        //// PUT: api/Games/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Games/5
        //public void Delete(int id)
        //{
        //}
    }
}
