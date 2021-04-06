#define BREAK_MOVE
#define DELAY_MOVE

#region irrelevant for concurrency issue

using Model.SignalRDataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Model.Card;
using static Model.Pile;


namespace Model {
    public class GameRoom {
        private Dictionary<string, Player> Players = new Dictionary<string, Player>();
        public object PlayersLock = new object();
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserAccountId { get; set; }
        public Product TableSkinId { get; set; }
        public Product CardSkinId { get; set; }
        public int RulesSetId { get; set; }
        public string GamePin { get; private set; }
        public List<Pile> Piles;
        public bool IsStarted { get; private set; } = false;
        public int MaxPileNo { get; set; } = 0;
        private object MaxPileNoLock = new object();
        private static object ConcurrencyIssueLock = new object();
        public GameRoom(int id, string name, DateTime creationDate, int userAccountId, Product tableSkinId, Product cardSkinId, int rulesSetId, string gamePin, List<Pile> listOfPiles) {
            Id = id;
            Name = name;
            CreationDate = creationDate;
            UserAccountId = userAccountId;
            TableSkinId = tableSkinId;
            CardSkinId = cardSkinId;
            RulesSetId = rulesSetId;
            GamePin = gamePin;
            Piles = listOfPiles ?? new List<Pile>();
        }


        public GameRoom(string name, DateTime creationDate, int userAccountId, Product tableSkinId, Product cardSkinId, int rulesSetId, string gamePin, List<Pile> listOfPiles) {
            Name = name;
            CreationDate = creationDate;
            UserAccountId = userAccountId;
            TableSkinId = tableSkinId;
            CardSkinId = cardSkinId;
            RulesSetId = rulesSetId;
            GamePin = gamePin;
            Piles = listOfPiles ?? new List<Pile>();
        }

        public bool AddPlayer(string connectionId, Player newPlayer) {
            if(!IsStarted) {
                Players.Add(connectionId, newPlayer);
                return true;
            }
            else {
                return false;
            }
        }

        public IEnumerable<Player> GetPlayers() {
            IEnumerable<Player> listOfPlayers = Players.Values;
            return listOfPlayers;
        }

        public Player GetPlayerByConnectionId(string connectionId) {
            Player playerToReturn = null;
            Players.TryGetValue(connectionId, out playerToReturn);
            return playerToReturn;
        }

        public IEnumerable<Pile> GetPiles() {
            return Piles.AsReadOnly();
        }

        public void CreateDeck() {
            List<Card> deck = new List<Card>();

            foreach(Suit suit in Enum.GetValues(typeof(Suit))) {

                foreach(Value value in Enum.GetValues(typeof(Value))) {
                    deck.Add(new Card(value, suit, Visibility.VisibleToNoOne, 1, null));
                };
            };
            Pile pile = null;
            lock(MaxPileNoLock) {
                pile = new Pile(Visibility.VisibleToNoOne, PileType.Deck, Accessability.AccessableToAll, 50, 50, null, deck, MaxPileNo++);
            }
            AddPile(pile);
        }

        public bool StartGame() {
            if(!IsStarted) {
                SetPilesToGameStartState();

                IsStarted = true;

                return true;
            }
            else
                return false;
        }

        private void SetPilesToGameStartState() {
            //Create and shuffle deck
            CreateDeck();
            Piles[0].ShufflePile();

            //Create discard pile
            Pile discardPile = new Pile(Visibility.VisibleToAll, PileType.Discard, Accessability.AccessableToAll, 55, 55, null, null, MaxPileNo++);
            AddPile(discardPile);

            //Create hand for each player
            lock(MaxPileNoLock) {
                foreach(Player player in Players.Values) {
                    Pile newPile = new Pile(Visibility.VisibleToOwner, PileType.Hand, Accessability.AccessableToOwner, 0, 0, player, null, MaxPileNo++);
                    AddPile(newPile);
                }
            }
        }

        public bool RemovePile(Pile oldPile) {
            bool succeded = Piles.Remove(oldPile);

            return succeded;
        }
        #endregion

        public bool MoveCard(MoveCardModel moveCardModel) {
            bool result = false;

            Pile sourcePile = Piles.Find(pile => pile.PileNo == moveCardModel.SourcePileNo);
            Pile targetPile = Piles.Find(pile => pile.PileNo == moveCardModel.TargetPileNo);

            try {
                if(Monitor.TryEnter(sourcePile)) {
#if DELAY_MOVE
                    if(Monitor.TryEnter(ConcurrencyIssueLock)) {
                        Debug.WriteLine("Paused with move " + moveCardModel.ToString());
                        Thread.Sleep(5000);
                        Debug.WriteLine("Continueing paused move...");
                        Monitor.Exit(ConcurrencyIssueLock);
                    }
#endif
#if BREAK_MOVE
                }
                else {
                    Debug.WriteLine("Skipping pause for move " + moveCardModel.ToString());
                }
                { //To match closing bracket
#endif
                    if(targetPile != sourcePile) {

                        if(Monitor.TryEnter(targetPile) && validateMove(sourcePile, targetPile, moveCardModel)) {
                            Card cardToMove = sourcePile.RemoveCard(moveCardModel.SourceIndex);
                            targetPile.AddCard(moveCardModel.TargetIndex, cardToMove);
                            sourcePile.VersionNo++;
                            targetPile.VersionNo++;
                            moveCardModel.SourcePileVersionNo = sourcePile.VersionNo;
                            moveCardModel.TargetPileVersionNo = targetPile.VersionNo;
                            result = true;
#if BREAK_MOVE
                            Debug.WriteLine("Made move " + moveCardModel.ToString());
#endif
                        }
                    }
                    else { //Source and Target pile are the same

                        if(validateMove(sourcePile, targetPile, moveCardModel)) {
                            Card cardToMove = sourcePile.RemoveCard(moveCardModel.SourceIndex);
                            sourcePile.AddCard(moveCardModel.TargetIndex, cardToMove);
                            sourcePile.VersionNo++;
                            moveCardModel.SourcePileVersionNo = sourcePile.VersionNo;
                            moveCardModel.TargetPileVersionNo = sourcePile.VersionNo;
                            result = true;
#if BREAK_MOVE
                            Debug.WriteLine("Made move " + moveCardModel.ToString());
#endif
                        }
                    }
                }
            }
            catch(Exception e) {
#if BREAK_MOVE
                Debug.WriteLine("Excepiton during move " + moveCardModel.ToString());
#endif
                Debug.WriteLine(e.Message);
            }
            finally {
                if(Monitor.IsEntered(sourcePile)) {
                    Monitor.Exit(sourcePile);
                }

                if(Monitor.IsEntered(targetPile)) {
                    Monitor.Exit(targetPile);
                }
            }

            return result;
        }

        private bool validateMove(Pile sourcePile, Pile targetPile, MoveCardModel moveToValidate) {

            bool sourcePileVersionIsValid = moveToValidate.SourcePileVersionNo == sourcePile.VersionNo;
            bool targetPileVersionIsValid = moveToValidate.TargetPileVersionNo == targetPile.VersionNo;

            bool sourceIndexIsValid = moveToValidate.SourceIndex >= 0 && moveToValidate.SourceIndex <= sourcePile.Cards.Count() - 1;
            bool targetIndexIsValid;

            if(targetPile != sourcePile) {
                targetIndexIsValid = moveToValidate.TargetIndex >= 0 && moveToValidate.TargetIndex <= targetPile.Cards.Count();
            }
            else { //Source and Target pile are the same
                targetIndexIsValid = moveToValidate.TargetIndex >= 0 && moveToValidate.TargetIndex <= targetPile.Cards.Count() - 1;
            }

#if BREAK_MOVE
            return true;
#else
            return sourcePileVersionIsValid && targetPileVersionIsValid && sourceIndexIsValid && targetIndexIsValid;
#endif
        }

        public void RestartGame() {
            SetPilesToGameStartState();
        }

        public bool AddPile(Pile pile) {
            bool succeded = false;
            int oldPilesAmount = Piles.Count();

            Piles.Add(pile);
            if(Piles.Count() == oldPilesAmount + 1) {
                succeded = true;
            }

            return succeded;
        }
    }
}
