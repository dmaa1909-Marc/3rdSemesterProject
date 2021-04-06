using Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebServer.BusinessLogic;

namespace WebServer.Controllers {
    public class GameController : Controller {

        private GameRoomManagement gameRoomManagement = new GameRoomManagement();

        //[HttpGet]
        //public ActionResult Get(string id) {
        //    Debug.WriteLine(id);

        //    return View("Index", null, id);
        //}

        // GET: Game
        public ActionResult Index(string id) {
            return View("Index", null, id);
        }


        // POST: Game/Create
        [HttpPost]
        public ActionResult Create(PostNewGame postNewGame) {

            string gamePin = gameRoomManagement.CreateNewGameRoom(postNewGame);

            if(gamePin != null)
                return RedirectToAction("Index", new { id = gamePin });
            else
                return RedirectToAction("CreateGame");
        }

        // GET: Game
        public ActionResult CreateGame() {

            return View();
        }

    }
}