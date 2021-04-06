using ApplicationServer.GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Model;
using System.Collections;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using Model.SignalRDataModels;

namespace Test.ApplicationServerTest {
    [TestClass]
    public class GameManagementTest {
        private GameManagement GameManager;
        private string GamePin;
        private GameRoom GameRoom;
        private string NickNamePlayerOne;
        private string ConnIdPlayerOne;
        private string NickNamePlayerTwo;
        private string ConnIdPlayerTwo;
        private int MaxAmountOfPlayers = 8;

        public GameManagementTest() {
            GameManager = GameManagement.Instance;
        }

        [TestInitialize]
        public void InitializeVariables() {
            GamePin = GameManager.CreateNewGame("testGame");
            GameRoom = GameManager.GetGameRoomWithPin(GamePin);
            NickNamePlayerOne = "Bo";
            ConnIdPlayerOne = "1";
            NickNamePlayerTwo = "Kaj";
            ConnIdPlayerTwo = "2";
        }

        [TestMethod]
        public void CheckIfSingletonIsOnlyCreatingOneInstance() {
            // Arrange
            GameManagement secondGameManager = null;

            // Act
            secondGameManager = GameManagement.Instance;

            // Assert
            Assert.AreEqual(GameManager, secondGameManager);

        }

        [TestMethod]
        public void CreateNewGameShouldCreateANewGameWithTheGivenNameTest() {
            //Arrange
            string gameName = "gameNameTest";

            //Act
            string gamePin = GameManager.CreateNewGame(gameName);

            //Assert
            Assert.AreEqual(gameName, GameManager.GetGameRoomWithPin(gamePin).Name);
        }

        [TestMethod]
        public void AddPlayerToGameShouldAddValidPlayerAndReturnSuccessTest() {
            //Arrange

            //Act
            Status result = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);

            //Assert
            Assert.IsNotNull(GameRoom.GetPlayers());
            Assert.AreEqual(NickNamePlayerOne, GameRoom.GetPlayers().ElementAt(0).Name);
            Assert.AreEqual(Status.Success, result);
        }

        [TestMethod]
        public void AddPlayerToGameShouldRejectPlayerWithDubplicateNickNameTest() {
            //Arrange
            string DuplicateNickNamePlayerTwo = NickNamePlayerOne;

            //Act
            Status result1 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);
            Status result2 = GameManager.AddPlayerToGame(GamePin, DuplicateNickNamePlayerTwo, ConnIdPlayerTwo);

            //Assert
            Assert.AreEqual(1, GameRoom.GetPlayers().Count());
            Assert.AreEqual(NickNamePlayerOne, GameRoom.GetPlayers().ElementAt(0).Name);
            Assert.AreEqual(DuplicateNickNamePlayerTwo, GameRoom.GetPlayers().ElementAt(0).Name);
            Assert.AreEqual(Status.Success, result1);
            Assert.AreEqual(Status.DuplicateNickNameError, result2);
        }

        [TestMethod]
        public void AddPlayerToGameShouldRejectPlayerIfGameIsStartedTest() {
            //Arrange
            GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);

            GameManager.StartGame(GamePin, ConnIdPlayerOne);
            
            //Act
            Status result = GameManager.AddPlayerToGame(GamePin, NickNamePlayerTwo, ConnIdPlayerTwo);


            //Assert
            Assert.AreEqual(1, GameManager.GetGameRoomWithPin(GamePin).GetPlayers().Count());
            Assert.AreEqual(Status.GameIsStartedError, result);
        }

        [TestMethod]
        public void AddPlayerToGameNonExistingGameTestShouldReturnErrorTest() {
            //Arrange
            string InvalidGamePin = "invalidPin";

            //Act
            Status result = GameManager.AddPlayerToGame(InvalidGamePin, NickNamePlayerOne, ConnIdPlayerOne);

            //Assert
            Assert.IsNull(GameManager.GetGameRoomWithPin(InvalidGamePin));
            Assert.AreEqual(Status.GameNotFoundError, result);
        }

        [TestMethod]
        public void AddPlayerToGameShouldAddUniquePlayersToGameTest() {
            //Arrange

            //Act
            Status result1 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);
            Status result2 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerTwo, ConnIdPlayerTwo);

            //Assert
            Assert.AreEqual(2, GameRoom.GetPlayers().Count());
            Assert.AreEqual(NickNamePlayerOne, GameRoom.GetPlayers().ElementAt(0).Name);
            Assert.AreEqual(NickNamePlayerTwo, GameRoom.GetPlayers().ElementAt(1).Name);

            Assert.AreEqual(Status.Success, result1);
            Assert.AreEqual(Status.Success, result2);
        }

        [TestMethod]
        public void AddMaxAmountOfPlayersTest() {
            //Arrange
            int maxAmountOfPlayers = MaxAmountOfPlayers;

            string NickNameTestPlayerThree = "Jens";
            string ConnectionIdPlayerThree = "3";

            string NickNameTestPlayerFour = "Karsten";
            string ConnectionIdPlayerFour = "4";

            string NickNameTestPlayerFive = "Per";
            string ConnectionIdPlayerFive = "5";

            string NickNameTestPlayerSix = "Peter";
            string ConnectionIdPlayerSix = "6";

            string NickNameTestPlayerSeven = "Karl";
            string ConnectionIdPlayerSeven = "7";

            string NickNameTestPlayerEight = "Torsten";
            string ConnectionIdPlayerEight = "8";

            //Act
            Status result1 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);
            Status result2 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerTwo, ConnIdPlayerTwo);
            Status result3 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerThree, ConnectionIdPlayerThree);
            Status result4 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerFour, ConnectionIdPlayerFour);
            Status result5 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerFive, ConnectionIdPlayerFive);
            Status result6 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerSix, ConnectionIdPlayerSix);
            Status result7 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerSeven, ConnectionIdPlayerSeven);
            Status result8 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerEight, ConnectionIdPlayerEight);

            //Assert
            Assert.IsNotNull(GameRoom.GetPlayers());
            Assert.AreEqual(maxAmountOfPlayers, GameRoom.GetPlayers().Count());
            Assert.AreEqual(Status.Success, result1);
            Assert.AreEqual(Status.Success, result2);
            Assert.AreEqual(Status.Success, result3);
            Assert.AreEqual(Status.Success, result4);
            Assert.AreEqual(Status.Success, result5);
            Assert.AreEqual(Status.Success, result6);
            Assert.AreEqual(Status.Success, result7);
            Assert.AreEqual(Status.Success, result8);
        }

        [TestMethod]
        public void AddMaxAmountOfPlayersPlusOneTest() {
            //Arrange
            int maxAmountOfPlayers = MaxAmountOfPlayers;

            string NickNameTestPlayerThree = "Jens";
            string ConnectionIdPlayerThree = "3";

            string NickNameTestPlayerFour = "Karsten";
            string ConnectionIdPlayerFour = "4";

            string NickNameTestPlayerFive = "Per";
            string ConnectionIdPlayerFive = "5";

            string NickNameTestPlayerSix = "Peter";
            string ConnectionIdPlayerSix = "6";

            string NickNameTestPlayerSeven = "Karl";
            string ConnectionIdPlayerSeven = "7";

            string NickNameTestPlayerEight = "Torsten";
            string ConnectionIdPlayerEight = "8";

            string NickNameTestPlayerNine = "Torben";
            string ConnectionIdPlayerNine = "9";

            //Act
            Status result1 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);
            Status result2 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerTwo, ConnIdPlayerTwo);
            Status result3 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerThree, ConnectionIdPlayerThree);
            Status result4 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerFour, ConnectionIdPlayerFour);
            Status result5 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerFive, ConnectionIdPlayerFive);
            Status result6 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerSix, ConnectionIdPlayerSix);
            Status result7 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerSeven, ConnectionIdPlayerSeven);
            Status result8 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerEight, ConnectionIdPlayerEight);
            Status result9 = GameManager.AddPlayerToGame(GamePin, NickNameTestPlayerNine, ConnectionIdPlayerNine);

            //Assert
            Assert.AreEqual(maxAmountOfPlayers, GameRoom.GetPlayers().Count());
            Assert.AreEqual(Status.Success, result1);
            Assert.AreEqual(Status.Success, result2);
            Assert.AreEqual(Status.Success, result3);
            Assert.AreEqual(Status.Success, result4);
            Assert.AreEqual(Status.Success, result5);
            Assert.AreEqual(Status.Success, result6);
            Assert.AreEqual(Status.Success, result7);
            Assert.AreEqual(Status.Success, result8);
            Assert.AreEqual(Status.GameIsFullError, result9);
        }

        [TestMethod]
        public void AddPlayerToGameAssignCorrectPositionTest() {
            //Arrange

            //Act
            Status result1 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);
            Status result2 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerTwo, ConnIdPlayerTwo);

            //Assert
            Assert.AreEqual(Status.Success, result1);
            Assert.AreEqual(Status.Success, result2);

            Assert.AreEqual(0, GameRoom.GetPlayers().ElementAt(0).Position);
            Assert.AreEqual(1, GameRoom.GetPlayers().ElementAt(1).Position);
        }

        [TestMethod]
        public void AddPlayerToGameAssignDifferentColorTest() {
            //Arrange

            //Act
            Status result1 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);
            Status result2 = GameManager.AddPlayerToGame(GamePin, NickNamePlayerTwo, ConnIdPlayerTwo);

            Color player1Color = GameRoom.GetPlayers().ElementAt(0).Color;
            Color player2Color = GameRoom.GetPlayers().ElementAt(1).Color;

            //Assert
            Assert.AreEqual(Status.Success, result1);
            Assert.AreEqual(Status.Success, result2);
            Assert.AreNotEqual(player1Color, player2Color);
        }

        [TestMethod]
        public void StartGameShouldReturnSuccessIfGameIsStartedTest() {
            //Arrange
            GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);

            //Act
            Status result = GameManager.StartGame(GamePin, ConnIdPlayerOne);

            //Assert
            Assert.AreEqual(Status.Success, result);
        }

        [TestMethod]
        public void StartGameShouldReturnErrorIfGameIsStartedTest() {
            //Arrange
            GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);

            GameManager.StartGame(GamePin, ConnIdPlayerOne);

            //Act
            Status result = GameManager.StartGame(GamePin, ConnIdPlayerOne);

            //Assert
            Assert.AreEqual(Status.GameIsStartedError, result);
        }

        [TestMethod]
        public void MoveCardShouldMoveCardFromSourcePileToTargetPileTest() {
            //Arrange
            GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);

            GameManager.StartGame(GamePin, ConnIdPlayerOne);

            IEnumerable<Pile> piles = GameManager.GetGameState(GamePin);
            Pile deck = piles.First((pile) => pile.PileType == PileType.Deck);
            Pile BoHand = piles.First((pile) => pile.PileType == PileType.Hand);

            int sourcePileVersionNo = deck.VersionNo;
            int sourcePileNo = deck.PileNo;
            int sourceIndex = deck.GetCards().Count() - 1;
            int targetPileVersionNo = BoHand.VersionNo;
            int targetPileNo = BoHand.PileNo;
            int targetIndex = 0;

            Card deckCard0 = deck.GetCards().ElementAt(sourceIndex);

            //Act
            GameManager.MoveCard(GamePin, new MoveCardModel(sourcePileVersionNo, sourcePileNo, sourceIndex, targetPileVersionNo, targetPileNo, targetIndex));

            //Assert
            CollectionAssert.DoesNotContain(deck.GetCards().ToList(), deckCard0);
            Assert.AreEqual(deckCard0, BoHand.GetCards().ElementAt(0));
        }

        [TestMethod]
        public void MoveCardShouldUpdateVersionNumberOfSourcePileAndTargetPileTest() {
            //Arrange
            GameManager.AddPlayerToGame(GamePin, NickNamePlayerOne, ConnIdPlayerOne);

            GameManager.StartGame(GamePin, ConnIdPlayerOne);

            IEnumerable<Pile> piles = GameManager.GetGameState(GamePin);
            Pile deck = piles.First((pile) => pile.PileType == PileType.Deck);
            Pile BoHand = piles.First((pile) => pile.PileType == PileType.Hand);

            int sourcePileVersionNo = deck.VersionNo;
            int sourcePileNo = deck.PileNo;
            int sourceIndex = deck.GetCards().Count()-1;
            int targetPileVersionNo = BoHand.VersionNo;
            int targetPileNo = BoHand.PileNo;
            int targetIndex = 0;

            MoveCardModel moveCardModel = new MoveCardModel(sourcePileVersionNo, sourcePileNo, sourceIndex, targetPileVersionNo, targetPileNo, targetIndex);

            Card deckCard0 = deck.GetCards().ElementAt(sourceIndex);

            //Act
            GameManager.MoveCard(GamePin, moveCardModel);

            //Assert
            Assert.AreEqual(1, deck.VersionNo);
            Assert.AreEqual(1, BoHand.VersionNo);

            Assert.AreEqual(deck.VersionNo, moveCardModel.SourcePileVersionNo);
            Assert.AreEqual(BoHand.VersionNo, moveCardModel.TargetPileVersionNo);

            CollectionAssert.DoesNotContain(deck.GetCards().ToList(), deckCard0);
            Assert.AreEqual(deckCard0, BoHand.GetCards().ElementAt(0));
        }

        [TestMethod]
        public void MoveCardSamePileTest() {
            //Arrange
            string gamePin = GameManager.CreateNewGame("testName");

            GameRoom GameRoom = GameManager.GetGameRoomWithPin(gamePin);
            string connId = "connId";
            GameManager.AddPlayerToGame(gamePin, "Bo", connId);

            GameManager.StartGame(gamePin, connId);

            int sourcePileVersionNo = 0,
                sourcePileNo = 0,
                sourceIndex = 0,
                targetPileVersionNo = 0,
                targetPileNo = 0,
                targetIndex = 1;

            MoveCardModel moveCardModel = new MoveCardModel(sourcePileVersionNo, sourcePileNo, sourceIndex, targetPileVersionNo, targetPileNo, targetIndex);

            //Act
            Card deckCard0 = GameRoom.Piles.ElementAt(0).GetCards().ElementAt(0);
            Card deckCard1 = GameRoom.Piles.ElementAt(0).GetCards().ElementAt(1);


            GameManager.MoveCard(gamePin, moveCardModel);

            IEnumerable<Pile> piles = GameManager.GetGameState(gamePin);

            Pile deck = piles.ElementAt(0);
            Pile boHand = piles.ElementAt(1);

            //Assert
            Assert.AreEqual(1, deck.VersionNo);
            Assert.AreEqual(0, boHand.VersionNo);

            Assert.AreEqual(deck.VersionNo, moveCardModel.SourcePileVersionNo);
            Assert.AreEqual(deck.VersionNo, moveCardModel.TargetPileVersionNo);

            Assert.AreEqual(0, boHand.GetCards().Count());
            Assert.AreEqual(deckCard0, deck.GetCards().ElementAt(1));
            Assert.AreEqual(deckCard1, deck.GetCards().ElementAt(0));
        }

        [TestMethod]
        public void MoveCardToLastIndexSamePileTest() {
            //Arrange
            string gamePin = GameManager.CreateNewGame("testName");

            GameRoom GameRoom = GameManager.GetGameRoomWithPin(gamePin);
            string connId = "connId";
            GameManager.AddPlayerToGame(gamePin, "Bo", connId);

            GameManager.StartGame(gamePin, connId);

            int sourcePileVersionNo = 0,
                sourcePileNo = 0,
                sourceIndex = 0,

                targetPileVersionNo = 0,
                targetPileNo = 1,
                targetIndex = 0;

            MoveCardModel moveCardModel = new MoveCardModel(sourcePileVersionNo, sourcePileNo, sourceIndex, targetPileVersionNo, targetPileNo, targetIndex);

            GameManager.MoveCard(gamePin, moveCardModel);
            GameManager.MoveCard(gamePin, moveCardModel);
            GameManager.MoveCard(gamePin, moveCardModel);

            moveCardModel.SourcePileNo = 1;
            moveCardModel.TargetIndex = 2;

            IEnumerable<Pile> piles = GameManager.GetGameState(gamePin);
            
            //Act
            Card boHandCard0 = piles.ElementAt(1).GetCards().ElementAt(0);
            Card boHandCard1 = piles.ElementAt(1).GetCards().ElementAt(1);
            Card boHandCard2 = piles.ElementAt(1).GetCards().ElementAt(2);

            GameManager.MoveCard(gamePin, moveCardModel);

            Pile deck = piles.ElementAt(0);
            Pile boHand = piles.ElementAt(1);

            //Assert
            Assert.AreEqual(3, deck.VersionNo);
            Assert.AreEqual(4, boHand.VersionNo);

            Assert.AreEqual(boHand.VersionNo, moveCardModel.SourcePileVersionNo);
            Assert.AreEqual(boHand.VersionNo, moveCardModel.TargetPileVersionNo);

            Assert.AreEqual(3, boHand.GetCards().Count());
            Assert.AreEqual(boHandCard0, boHand.GetCards().ElementAt(2));
            Assert.AreEqual(boHandCard1, boHand.GetCards().ElementAt(0));
            Assert.AreEqual(boHandCard2, boHand.GetCards().ElementAt(1));
        }

        [TestMethod]
        public void GetGameRoomWithPinShouldReturnNullIfGameRoomDoesNotExistTest() {
            //Arrange
            string InvalidGamePin = "InvalidGamePin";

            //Act
            GameRoom gameRoomTest = GameManager.GetGameRoomWithPin(InvalidGamePin);

            //Assert
            Assert.IsNull(gameRoomTest);
        }
    }
}
