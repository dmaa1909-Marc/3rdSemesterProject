using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using static Model.Card;

namespace Test {
    [TestClass]
    public class GameRoomTest {

        private Product TestProduct;
        private GameRoom EmptyGameRoom;
        private Player PlayerOne;
        private string ConnIdPlayerOne;


        public GameRoomTest() {
            ResetFieldsAfterEachTestMethod();
        }

        [TestInitialize]
        public void ResetFieldsAfterEachTestMethod() {
            TestProduct = new Product(1, "Test Product", "Product for testing", 15, Currency.DKK, ProductType.CardSkin, 20, 20, true);
            EmptyGameRoom = new GameRoom(1, "Test Room", DateTime.Now, 1, TestProduct, TestProduct, 1, "1111", null);
            PlayerOne = new Player("Bo", 1, Color.Red);
            ConnIdPlayerOne = "connTest";
        }
        

        [TestMethod]
        public void CheckIFGameRoomIsCreatedWithTheCorrectParameters() {
            // Arrange
            int productIid = 1;
            string productName = "Test Product";
            string description = "Product for testing";
            double price = 15;
            Currency currency = Currency.DKK;
            ProductType productType = ProductType.CardSkin;
            int amountAvailable = 20;
            int amountTotal = 20;
            bool active = true;

            Product testProduct = new Product(productIid, productName, description, price, currency, productType, amountAvailable, amountTotal, active);

            Pile pile1 = new Pile();
            Pile pile2 = new Pile();
            List<Pile> listOfPiles = new List<Pile>() {
                pile1,
                pile2
            };

            int id = 1;
            string name = "Test Room";
            DateTime creationDate = DateTime.Now;
            int userAccountId = 1;
            Product tableSkin = testProduct;
            Product cardSkin = testProduct;
            int rulesSetId = 1;
            string gamePin = "1111";


            //Act
            GameRoom gameRoomTest = new GameRoom(id, name, creationDate, userAccountId, tableSkin, cardSkin, rulesSetId, gamePin, listOfPiles);


            //Assert
            Assert.AreEqual(gameRoomTest.Id, id);
            Assert.AreEqual(gameRoomTest.Name, name);
            Assert.AreEqual(gameRoomTest.CreationDate, creationDate);
            Assert.AreEqual(gameRoomTest.UserAccountId, userAccountId);
            Assert.AreEqual(gameRoomTest.TableSkinId, tableSkin);
            Assert.AreEqual(gameRoomTest.CardSkinId, cardSkin);
            Assert.AreEqual(gameRoomTest.RulesSetId, rulesSetId);
            Assert.AreEqual(gameRoomTest.GamePin, gamePin);
            Assert.AreEqual(pile1, gameRoomTest.GetPiles().ElementAt(0));
            Assert.AreEqual(pile2, gameRoomTest.GetPiles().ElementAt(1));

        }

        [TestMethod]
        public void AddPlayerShouldReturnTrueIfPlayerIsAddedTest() {
            //Arrange
            GameRoom gameRoomTest = EmptyGameRoom;

            //Act
            bool result = gameRoomTest.AddPlayer(ConnIdPlayerOne, PlayerOne);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(this.PlayerOne, gameRoomTest.GetPlayers().ElementAt(0));

        }

        [TestMethod]
        public void AddPlayerShouldReturnFalseIfGameIsStartedTest() {
            //Arrange
            GameRoom startedGame = EmptyGameRoom;
            startedGame.StartGame();

            //Act
            bool result = startedGame.AddPlayer(ConnIdPlayerOne, PlayerOne);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetPlayersShouldReturnEmptyListIfNoPlayersAreAddedTest() {
            //Arrange
            GameRoom gameRoomTest = EmptyGameRoom;

            //Act
            IEnumerable<Player> listOfPlayers = gameRoomTest.GetPlayers();

            //Assert
            Assert.IsNotNull(listOfPlayers);
            Assert.AreEqual(0, listOfPlayers.Count());
        }
        
        [TestMethod]
        public void GetPlayersShouldReturnAddedPlayersTest() {
            //Arrange
            GameRoom gameRoomTest = EmptyGameRoom;
            gameRoomTest.AddPlayer(ConnIdPlayerOne, PlayerOne);

            //Act
            IEnumerable<Player> listOfPlayers = gameRoomTest.GetPlayers();

            //Assert
            Assert.AreEqual(1, listOfPlayers.Count());
            Assert.AreEqual(PlayerOne, listOfPlayers.ElementAt(0));
        }

        [TestMethod]
        public void GetPlayerByConnectionIdTest() {
            //Arrange
            GameRoom gameRoomTest = EmptyGameRoom;
            gameRoomTest.AddPlayer(ConnIdPlayerOne, PlayerOne);

            //Act
            Player foundPlayer = EmptyGameRoom.GetPlayerByConnectionId(ConnIdPlayerOne);

            //Assert
            Assert.AreEqual(PlayerOne, foundPlayer);
        }

        [TestMethod]
        public void CheckIfADeckIsCreatedCorrectly() {
            // Arrange

            List<Card> testDeck = new List<Card>();

            GameRoom gameRoomTest = EmptyGameRoom;

            foreach(Suit suit in Enum.GetValues(typeof(Suit))) {
                foreach(Value value in Enum.GetValues(typeof(Value))) {
                    testDeck.Add(new Card(value, suit, Visibility.VisibleToNoOne, 1, null));
                };
            };

            //testDeck.Add(new Card(Value.Ace, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 13, 14H
            //testDeck.Add(new Card(Value.Two, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 01,  2H
            //testDeck.Add(new Card(Value.Three, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 02,  3H
            //testDeck.Add(new Card(Value.Four, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 03,  4H
            //testDeck.Add(new Card(Value.Five, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 04,  5H
            //testDeck.Add(new Card(Value.Six, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 05,  6H
            //testDeck.Add(new Card(Value.Seven, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 06,  7H   
            //testDeck.Add(new Card(Value.Eighth, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 07,  8H
            //testDeck.Add(new Card(Value.Nine, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 08,  9H
            //testDeck.Add(new Card(Value.Ten, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 09, 10H
            //testDeck.Add(new Card(Value.Jack, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 10, 11H
            //testDeck.Add(new Card(Value.Queen, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 11, 12H
            //testDeck.Add(new Card(Value.King, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 12, 13H

            //testDeck.Add(new Card(Value.Ace, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 26, 14D
            //testDeck.Add(new Card(Value.Two, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 14,  2D
            //testDeck.Add(new Card(Value.Three, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 15,  3D
            //testDeck.Add(new Card(Value.Four, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 16,  4D
            //testDeck.Add(new Card(Value.Five, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 17,  5D
            //testDeck.Add(new Card(Value.Six, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 18,  6D
            //testDeck.Add(new Card(Value.Seven, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 19,  7D
            //testDeck.Add(new Card(Value.Eighth, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 20,  8D
            //testDeck.Add(new Card(Value.Nine, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 21,  9D
            //testDeck.Add(new Card(Value.Ten, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 22, 10D
            //testDeck.Add(new Card(Value.Jack, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 23, 11D
            //testDeck.Add(new Card(Value.Queen, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 24, 12D
            //testDeck.Add(new Card(Value.King, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 25, 13D

            //testDeck.Add(new Card(Value.Ace, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 39, 14C
            //testDeck.Add(new Card(Value.Two, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 27,  2C
            //testDeck.Add(new Card(Value.Three, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 28,  3C
            //testDeck.Add(new Card(Value.Four, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 29,  4C
            //testDeck.Add(new Card(Value.Five, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 30,  5C
            //testDeck.Add(new Card(Value.Six, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 31,  6C
            //testDeck.Add(new Card(Value.Seven, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 32,  7C
            //testDeck.Add(new Card(Value.Eighth, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 33,  8C
            //testDeck.Add(new Card(Value.Nine, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 34,  9C
            //testDeck.Add(new Card(Value.Ten, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 35, 10C
            //testDeck.Add(new Card(Value.Jack, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 36, 11C
            //testDeck.Add(new Card(Value.Queen, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 37, 12C
            //testDeck.Add(new Card(Value.King, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 38, 13C

            //testDeck.Add(new Card(Value.Ace, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 52, 14S    
            //testDeck.Add(new Card(Value.Two, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 40,  2S
            //testDeck.Add(new Card(Value.Three, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 41,  3S
            //testDeck.Add(new Card(Value.Four, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 42,  4S
            //testDeck.Add(new Card(Value.Five, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 43,  5S
            //testDeck.Add(new Card(Value.Six, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 44,  6S
            //testDeck.Add(new Card(Value.Seven, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 45,  7S
            //testDeck.Add(new Card(Value.Eighth, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 46,  8S
            //testDeck.Add(new Card(Value.Nine, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 47,  9S
            //testDeck.Add(new Card(Value.Ten, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 48, 10S
            //testDeck.Add(new Card(Value.Jack, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 49, 11S
            //testDeck.Add(new Card(Value.Queen, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 50, 12S
            //testDeck.Add(new Card(Value.King, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 51, 13S


            //Act
            gameRoomTest.CreateDeck();


            // Assert
            CollectionAssert.AreEqual(testDeck, gameRoomTest.Piles.ElementAt(0).GetCards().ToList());

        }

        [TestMethod]
        public void StartGameCreatesPileOfTypeDeckTest() {
            //Arrange
            GameRoom gameRoomTest = EmptyGameRoom;

            //Act
            gameRoomTest.StartGame();
            Pile deck = null;
            deck = gameRoomTest.Piles.Single((pile) => pile.PileType == PileType.Deck);

            //Assert
            Assert.AreEqual(PileType.Deck, gameRoomTest.Piles[0].PileType);
        }

        [TestMethod]
        public void StartGameCreatesPlayersHandsTest() {
            //Arrange
            GameRoom gameRoomTest = EmptyGameRoom;

            Player player1 = PlayerOne;
            Player player2 = new Player("Bo", 2, Color.Red);
            string player1ConnId = ConnIdPlayerOne;
            string player2ConnId = "connId2";

            gameRoomTest.AddPlayer(player1ConnId, player1);
            gameRoomTest.AddPlayer(player2ConnId, player2);

            //Act
            gameRoomTest.StartGame();

            bool player1HandFound = gameRoomTest.Piles.Exists((pile) => pile.PileType == PileType.Hand && pile.Owner == player1);
            bool player2HandFound = gameRoomTest.Piles.Exists((pile) => pile.PileType == PileType.Hand && pile.Owner == player2);

            //Assert
            Assert.AreEqual(PileType.Deck, gameRoomTest.Piles[0].PileType);
            Assert.IsTrue(player1HandFound);
            Assert.IsTrue(player2HandFound);

        }

        [TestMethod]
        public void AddPileShouldReturnTrueIfPileIsAddedTest() {
            //Arrange
            EmptyGameRoom.StartGame();
            int oldPileAmount = EmptyGameRoom.GetPiles().Count();

            //Act
            Pile newPile = new Pile(Visibility.VisibleToAll, PileType.Run, Accessability.AccessableToAll, 50, 50, PlayerOne, null, EmptyGameRoom.MaxPileNo++);
            bool succeded = EmptyGameRoom.AddPile(newPile);

            //Assert
            Assert.AreEqual(oldPileAmount + 1, EmptyGameRoom.GetPiles().Count());
            Assert.IsTrue(succeded);
        }

        public void RemovePileShouldReturnTrueIfPileIsRemovedTest() {
            //Arrange
            EmptyGameRoom.StartGame();
            List<Pile> piles = EmptyGameRoom.GetPiles().ToList();
            int oldPileAmount = piles.Count();
            Pile removePile = piles.Last();

            //Act
            bool succeded = EmptyGameRoom.RemovePile(removePile);

            //Assert
            Assert.IsTrue(succeded);
            CollectionAssert.DoesNotContain(piles, removePile);
            Assert.AreEqual(oldPileAmount - 1, piles.Count());
        }
    }
}


