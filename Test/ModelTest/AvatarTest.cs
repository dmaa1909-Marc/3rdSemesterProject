using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Model;

namespace Test.ModelTest {
    [TestClass]
    public class AvatarTest {

        [TestMethod]
        public void ConstructorSetsPropertiesCorrectTest() {
            //Arrange
            int id = 5;
            string base64Image = "SomeRandomImageGibberish";

            //Act
            Avatar avatar = new Avatar(id, base64Image);

            //Assert
            Assert.IsNotNull(avatar);
            Assert.AreEqual(id, avatar.Id);
            Assert.AreEqual(base64Image, avatar.Image);
        }
    }
}
