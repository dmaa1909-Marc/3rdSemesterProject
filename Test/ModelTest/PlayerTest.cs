using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Model;
using System.Drawing;

namespace Test.ModelTest {
    [TestClass]
    public class PlayerTest {
        [TestMethod]
        public void CreatePlayerWithPropertiesTest() {
            string Name = "Hans";
            int Position = 1;
            Color color = Color.FromArgb(0xff, 0x00, 0x00);
            
            Player player = new Player(Name, Position, color);

            Assert.AreEqual(Name, player.Name);
            Assert.AreEqual(Position, player.Position);
            Assert.AreEqual(color, player.Color);
        }
    }

    
}
