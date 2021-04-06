using Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models;

namespace WebServer.BusinessLogic {
    public class GameRoomManagement {

        private GameRoomsRepository gameRoomRepository = new GameRoomsRepository();

        public string CreateNewGameRoom(PostNewGame postNewGame) {

            return gameRoomRepository.AddNewGame(postNewGame);
        }
    }

}