using ApplicationServer.GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using static Model.Card;

namespace Test.ModelTest {
    [TestClass]
    public class CardTest {
        [TestMethod]
        public void CheckIfCardIsCreatedWithTheCorrectParameters() {

            // Arrange
            //int id = 1;
            Value value = Value.King;
            Suit suit = Suit.Hearts;
            Visibility visibility = Visibility.VisibleToAll;
            int pileId = 1;
            Player player = new Player("Bo", 0, Color.Red);

            //Act
            Card testCard = new Card(value, suit, visibility, pileId, player);

            // Assert
            //Assert.AreEqual(testCard.Id, id);
            Assert.AreEqual(testCard.CardValue, value);
            Assert.AreEqual(testCard.CardSuit, suit);
            Assert.AreEqual(testCard.Visibility, visibility);
            Assert.AreEqual(testCard.PileId, pileId);
            Assert.AreEqual(testCard.Owner, player);
        }


        

    }
}

