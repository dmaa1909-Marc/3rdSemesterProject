using ApplicationServer.GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Model.Card;
using static Model.Pile;

namespace Test.ApplicationServerTest {
    [TestClass]
    public class PileTest {


        [TestMethod]
        public void CheckIfDeckIsShuffled() {

            // Arrange
            
            List<Card> testDeck = new List<Card>();

            testDeck.Add(new Card(Value.Two, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 01,  2H
            testDeck.Add(new Card(Value.Three, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 02,  3H
            testDeck.Add(new Card(Value.Four, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 03,  4H
            testDeck.Add(new Card(Value.Five, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 04,  5H
            testDeck.Add(new Card(Value.Six, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 05,  6H
            testDeck.Add(new Card(Value.Seven, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 06,  7H
            testDeck.Add(new Card(Value.Eighth, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));     // card 07,  8H
            testDeck.Add(new Card(Value.Nine, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 08,  9H
            testDeck.Add(new Card(Value.Ten, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 09, 10H
            testDeck.Add(new Card(Value.Jack, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 10, 11H
            testDeck.Add(new Card(Value.Queen, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));      // card 11, 12H
            testDeck.Add(new Card(Value.King, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));       // card 12, 13H
            testDeck.Add(new Card(Value.Ace, Suit.Hearts, Visibility.VisibleToNoOne, 1, null));        // card 13, 14H
                                  
            testDeck.Add(new Card(Value.Two, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 14,  2D
            testDeck.Add(new Card(Value.Three, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 15,  3D
            testDeck.Add(new Card(Value.Four, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 16,  4D
            testDeck.Add(new Card(Value.Five, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 17,  5D
            testDeck.Add(new Card(Value.Six, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 18,  6D
            testDeck.Add(new Card(Value.Seven, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 19,  7D
            testDeck.Add(new Card(Value.Eighth, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));   // card 20,  8D
            testDeck.Add(new Card(Value.Nine, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 21,  9D
            testDeck.Add(new Card(Value.Ten, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 22, 10D
            testDeck.Add(new Card(Value.Jack, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 23, 11D
            testDeck.Add(new Card(Value.Queen, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));    // card 24, 12D
            testDeck.Add(new Card(Value.King, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));     // card 25, 13D
            testDeck.Add(new Card(Value.Ace, Suit.Diamonds, Visibility.VisibleToNoOne, 1, null));      // card 26, 14D
                                  
            testDeck.Add(new Card(Value.Two, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 27,  2C
            testDeck.Add(new Card(Value.Three, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 28,  3C
            testDeck.Add(new Card(Value.Four, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 29,  4C
            testDeck.Add(new Card(Value.Five, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 30,  5C
            testDeck.Add(new Card(Value.Six, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 31,  6C
            testDeck.Add(new Card(Value.Seven, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 32,  7C
            testDeck.Add(new Card(Value.Eighth, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));      // card 33,  8C
            testDeck.Add(new Card(Value.Nine, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 34,  9C
            testDeck.Add(new Card(Value.Ten, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 35, 10C
            testDeck.Add(new Card(Value.Jack, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 36, 11C
            testDeck.Add(new Card(Value.Queen, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));       // card 37, 12C
            testDeck.Add(new Card(Value.King, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));        // card 38, 13C
            testDeck.Add(new Card(Value.Ace, Suit.Clubs, Visibility.VisibleToNoOne, 1, null));         // card 39, 14C
                                  
            testDeck.Add(new Card(Value.Two, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 40,  2S
            testDeck.Add(new Card(Value.Three, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 41,  3S
            testDeck.Add(new Card(Value.Four, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 42,  4S
            testDeck.Add(new Card(Value.Five, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 43,  5S
            testDeck.Add(new Card(Value.Six, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 44,  6S
            testDeck.Add(new Card(Value.Seven, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 45,  7S
            testDeck.Add(new Card(Value.Eighth, Suit.Spades, Visibility.VisibleToNoOne, 1, null));     // card 46,  8S
            testDeck.Add(new Card(Value.Nine, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 47,  9S
            testDeck.Add(new Card(Value.Ten, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 48, 10S
            testDeck.Add(new Card(Value.Jack, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 49, 11S
            testDeck.Add(new Card(Value.Queen, Suit.Spades, Visibility.VisibleToNoOne, 1, null));      // card 50, 12S
            testDeck.Add(new Card(Value.King, Suit.Spades, Visibility.VisibleToNoOne, 1, null));       // card 51, 13S
            testDeck.Add(new Card(Value.Ace, Suit.Spades, Visibility.VisibleToNoOne, 1, null));        // card 52, 14S

            int id = 1;
            string name = "Test Room";
            DateTime creationDate = DateTime.Now;
            int userAccountId = 1;
            int rulesSetId = 1;
            string gamePin = "1111";

            GameRoom gameRoomTest = new GameRoom(id, name, creationDate, userAccountId, null, null, rulesSetId, gamePin, null);

            gameRoomTest.CreateDeck();

            Pile deck = gameRoomTest.Piles[0];

            //Act
            deck.ShufflePile();

            //Assert
            CollectionAssert.AreNotEqual(testDeck, deck.GetCards().ToList());

        }

        [TestMethod]
        public void CheckIfPileIsCreatedWithTheCorrectParameters() {
            //Arrange
            Visibility pileVisibility = Visibility.VisibleToNoOne;
            PileType pileType = PileType.Deck;
            Accessability pileAccessability = Accessability.AccessableToAll;
            float hAlignPercent = 0.5f;
            float vAlignPercent = 0.6f;
            Player player = new Player("testName", 1, Color.Red);
            int pileNo = 0;

            //Act
            Pile pile = new Pile(pileVisibility, pileType, pileAccessability, hAlignPercent, vAlignPercent, player, null, pileNo);

            //Assert
            Assert.AreEqual(pileVisibility, pile.Visibility);
            Assert.AreEqual(pileType, pile.PileType);
            Assert.AreEqual(pileAccessability, pile.Accessibility);
            Assert.AreEqual(hAlignPercent, pile.HAlignPercent);
            Assert.AreEqual(vAlignPercent, pile.VAlignPercent);
            Assert.AreEqual(player, pile.Owner);
            Assert.IsNotNull(pile.GetCards());

        }
    }
}
