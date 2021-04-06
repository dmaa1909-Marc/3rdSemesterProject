using ApplicationServer;
using ApplicationServer.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Test.ApplicationServerTest {
    [TestClass]
    public class ColorsDBTest {
        [TestMethod]
        public void GetMin8ColorsFromDBTest() {
            //Arrange
            IColorsDB colorsDB = new ColorsDB();
            int minNumberOfColors = 8;

            //Act
            //colorsDB.AddColor(Color.Blue);
            IEnumerable<Color> colors = colorsDB.GetColors();

            //Assert
            Assert.IsTrue(colors.Count() >= minNumberOfColors);
        }
    }
}
