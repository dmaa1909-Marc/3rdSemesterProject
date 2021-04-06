using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using ApplicationServer.GameLogic;
using Microsoft.AspNet.SignalR;
using Model;
using Model.SignalRDataModels;

namespace ApplicationServer.SignalRHubs {
    public class GameHub : Hub {

        private GameManagement GameManager = GameManagement.Instance;

        //========================================== Core functionality ==========================================// 
        public void JoinGame(string gamePin, string nickName) {
            Status result = GameManager.AddPlayerToGame(gamePin, nickName, Context.ConnectionId);

            if (result == Status.Success) {
                Groups.Add(Context.ConnectionId, gamePin);

                GameRoom currentGameRoom = GameManager.GetGameRoomWithPin(gamePin);

                IEnumerable<Player> currentlyConnectedPlayers = currentGameRoom.GetPlayers();

                Clients.Caller.UpdatePlayersList(currentlyConnectedPlayers);
                Clients.OthersInGroup(gamePin).NewPlayerJoining(currentGameRoom.GetPlayerByConnectionId(Context.ConnectionId));

            }
            else {
                SendErrorMessage(result);
            }
        }

        public void StartGame(string gamePin) {
            Status result = GameManager.StartGame(gamePin, Context.ConnectionId);

            if (result == Status.Success) {
                IEnumerable<Pile> piles = GameManager.GetGameState(gamePin);

                Clients.Group(gamePin).StartGame(piles);
                //Clients.Group(gamePin).UpdateAllPiles(piles);
            }
            else {
                SendErrorMessage(result);
            }
        }

        public void MoveCard(string gamePin, MoveCardModel moveCardModel) {
            Status status = GameManager.MoveCard(gamePin, moveCardModel);

            if (status == Status.Success) {
                Clients.Group(gamePin).MoveCard(moveCardModel);

                //Delete source pile if it is empty and a run pile
                Pile oldPile = GameManager.GetGameState(gamePin).Single((pile) => pile.PileNo == moveCardModel.SourcePileNo);
                if (oldPile.GetCards().Count() == 0 && oldPile.PileType == PileType.Run) {
                    DeleteEmptyRunPile(gamePin, oldPile);
                }
            }
            else {
                //TODO: Pile version issues...
                IEnumerable<Pile> piles = GameManager.GetGameState(gamePin);

                Clients.Caller.StartGame(piles); 
                //MoveRejected(moveCardModel.TargetPileVersionNo, moveCardModel.TargetPileNo, moveCardModel.TargetIndex,
                //    moveCardModel.SourcePileVersionNo, moveCardModel.SourcePileNo, moveCardModel.SourceIndex);
            }
        }

        public void AddNewPile(string gamePin, Pile pile, IEnumerable<MoveCardModel> cardsToMove) {
            Status result = GameManager.AddNewPile(gamePin, pile);

            if (result == Status.Success) {
                Clients.Group(gamePin).AddPile(pile);
            }
            else {
                SendErrorMessage(result);
            }
        }

        public void GetGameState(string gamePin) {
            IEnumerable<Pile> piles = GameManager.GetGameState(gamePin);

            if (piles != null) {
                Clients.Caller.UpdateAllPiles(piles);
            }
            else {
                Status errorMessage = Status.GameNotFoundError;
                SendErrorMessage(errorMessage);
            }
        }


        //========================================== Other game functionality ==========================================// 


        private void SendErrorMessage(Status result) {
            String message = result.ToString();
            Clients.Caller.ErrorMessage(message);
        }

        public void IsGameStarted(string gamePin) {
            GameRoom gameRoom = GameManager.GetGameRoomWithPin(gamePin);
            if (gameRoom != null ? gameRoom.IsStarted : false)
                Clients.Caller.GameIsStarted();
        }

        public void DeleteEmptyRunPile(string gamePin, Pile oldPile) {
            Status deleteStatus = GameManager.DeleteEmptyRunPile(gamePin, oldPile);

            if (deleteStatus == Status.Success) {
                Clients.Group(gamePin).DeleteEmptyRunPile(oldPile.PileNo);
            }
            else {
                SendErrorMessage(deleteStatus);
            }
        }

        public void RestartGame(string gamePin) {
            //Only lobby owner should be able to do this?  (player with pos = 1)
            Status result = GameManager.RestartGame(gamePin, Context.ConnectionId);

            if (result == Status.Success) {
                IEnumerable<Pile> piles = GameManager.GetGameState(gamePin);

                Clients.Group(gamePin).UpdateAllPiles(piles);
            }
            else {
                SendErrorMessage(result);
            }
        }

        public Task LeaveGame(string gamePin) {
            //TODO: remove player from game...
            return Groups.Remove(Context.ConnectionId, gamePin);
        }

        public override Task OnDisconnected(bool stopCalled) {
            //TODO: OnReconnect...
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected() {
            //TODO: OnDisconnected...
            return base.OnReconnected();
        }

        public void lockCard(string gamePin, int pileNo, int cardIndex) {

        }

        public void MoveCards(string gamePin, List<Card> cardsToMove, Object TargetPile) {

        }
    }
}
