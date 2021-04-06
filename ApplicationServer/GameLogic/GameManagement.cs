//#define BREAK_JOIN
//#define DELAY_JOIN

#region irrelevant for concurrency issue

using ApplicationServer.DB;
using Model;
using Model.SignalRDataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace ApplicationServer.GameLogic {

    // Thread Safety Singleton using Double Check Locking

    public sealed class GameManagement {

        private Dictionary<string, GameRoom> GameRooms = new Dictionary<string, GameRoom>();
        private Random random = new Random();

        private static Color[] Colors;

        private GameManagement() {
            IColorsDB colorsDB = new ColorsDB();
            Colors = colorsDB.GetColors().ToArray();
        }

        private static readonly object padlock = new object();
        private static GameManagement instance = null;

        public static GameManagement Instance {
            get {
                if(instance == null) {
                    lock(padlock) {
                        if(instance == null) {
                            instance = new GameManagement();
                        }
                    }
                }
                return instance;
            }
        }
#endregion

        public Status AddPlayerToGame(string gamePin, string nickName, string connectionId) {
            Status result = Status.None;

            //Add player to game
            GameRoom updatedGameRoom = GetGameRoomWithPin(gamePin);

            if(updatedGameRoom != null) {

#if !BREAK_JOIN
                lock (updatedGameRoom.PlayersLock) {
#else
                { //To match closing bracket
#endif
                    int amountOfPlayers = updatedGameRoom.GetPlayers().Count();


#if BREAK_JOIN || DELAY_JOIN
                    if(amountOfPlayers == 1 && Monitor.TryEnter(updatedGameRoom.PlayersLock)) {
                        Debug.WriteLine("Paused with player " + nickName);
                        Thread.Sleep(5000);
                        Debug.WriteLine("Player " + nickName + " continuing from pause");

                        Monitor.Exit(updatedGameRoom.PlayersLock);
                    }
                    else {
                        Debug.WriteLine("Skipping pause for player " + nickName);
                    }
#endif

                    bool isDublicateNickName = false;
                    foreach(Player p in updatedGameRoom.GetPlayers()) {
                        if(p.Name.Equals(nickName)) {
                            isDublicateNickName = true;
                        }
                    }
                    if(updatedGameRoom.IsStarted) {
                        result = Status.GameIsStartedError;
                    }
                    else if(isDublicateNickName) {
                        result = Status.DuplicateNickNameError;
                    }
                    else if(amountOfPlayers >= 8) {
                        result = Status.GameIsFullError;
                    }
                    else {

                        Color playerColor = Colors[amountOfPlayers];
                        Player newPlayer = new Player(nickName, amountOfPlayers, playerColor);
                        if(updatedGameRoom.AddPlayer(connectionId, newPlayer)) {
                            result = Status.Success;
#if BREAK_JOIN || DELAY_JOIN
                            Debug.WriteLine("Player " + nickName + " joined the game with position: " + amountOfPlayers);
#endif
                        }
                        else
                            result = Status.UnknownError;
                    }
                }
            }
            else {
                result = Status.GameNotFoundError;
            }
            return result;
        }

        public Status MoveCard(string gamePin, MoveCardModel moveCardModel) {
            Status status = Status.None;
            GameRoom gameRoom = GetGameRoomWithPin(gamePin);

            if(gameRoom != null) {
                if(gameRoom.MoveCard(moveCardModel))
                    status = Status.Success;
                else
                    status = Status.InvalidMoveError;
            }
            else {
                status = Status.GameNotFoundError;
            }

            return status;
        }

        public IEnumerable<Pile> GetGameState(string gamePin) {
            return GetGameRoomWithPin(gamePin).GetPiles();
        }

        public Status DeleteEmptyRunPile(string gamePin, Pile oldPile) {
            Status result = Status.None;
            GameRoom gameRoomToRemovePile = GetGameRoomWithPin(gamePin);
            if(gameRoomToRemovePile != null) {

                if(gameRoomToRemovePile.RemovePile(oldPile)) {
                    result = Status.Success;
                }
                else {
                    result = Status.PileNotRemovedError;
                }
            }
            else {
                result = Status.GameNotFoundError;
            }

            return result;
        }

        public Status StartGame(string gamePin, string ConnectionId) {
            Status result = Status.None;
            GameRoom gameToStart = GetGameRoomWithPin(gamePin);

            if(gameToStart != null) {
                if(!gameToStart.IsStarted)
                    if(gameToStart.GetPlayerByConnectionId(ConnectionId) == null)
                        result = Status.NotInGameError;
                    else if(gameToStart.StartGame())
                        result = Status.Success;
                    else
                        result = Status.UnknownError;
                else
                    result = Status.GameIsStartedError;
            }
            else {
                result = Status.GameNotFoundError;
            }
            return result;
        }

        public Status AddNewPile(string gamePin, Pile pile) {
            Status result = Status.None;
            GameRoom targetGameRoom = GetGameRoomWithPin(gamePin);

            if(targetGameRoom.AddPile(pile)) {
                result = Status.Success;
            }
            else {
                result = Status.AddPileError;
            }
            return result;
        }

        public GameRoom GetGameRoomWithPin(string newGamePin) {
            GameRoom returnGameRoom = null;

            GameRooms.TryGetValue(newGamePin, out returnGameRoom);

            return returnGameRoom;
        }

        public Status RestartGame(string gamePin, string connectionId) {
            //Connection ID is also sent if only the lobby owner should be able to restart the game
            Status result = Status.None;

            GameRoom gameToRestart = GetGameRoomWithPin(gamePin);

            if(gameToRestart != null) {
                gameToRestart.RestartGame();
                result = Status.Success;
            }
            else {
                result = Status.GameNotFoundError;
            }



            return result;
        }

        public string CreateNewGame(string name) {
            lock(GameRooms) { //Lock so that you don't get colliding gamePins 
                //TODO: make lock lighther so that only a single gamePin is locked somehow
                int maxPinNo = GameRooms.Count() * 10;

                int newPin = 0;

                bool validPin = false;
                while(!validPin) {
                    newPin = random.Next(maxPinNo);
                    validPin = !GameRooms.ContainsKey(newPin.ToString());
                }

                GameRoom newGameRoom = new GameRoom(name, DateTime.Now, 0, null, null, 0, newPin.ToString(), null);

                GameRooms.Add(newGameRoom.GamePin, newGameRoom);

                return newGameRoom.GamePin;
            }
        }
    }
}