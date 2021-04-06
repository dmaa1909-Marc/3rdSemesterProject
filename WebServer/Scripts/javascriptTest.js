$(function () {
    $("#layer-mask").remove();
    var test = $("#Test")

    testSortPlayersByPosition()
    testPutMeAtIndexZero()

    function addTestResult(testName, testSucceded) {
        var color = testSucceded ? "green" : "red"
        test.append($("<div />").text(testName).css("background-color", color))
    }

    function testSortPlayersByPosition() {
        //Arrange
        var testArray = [
            { Position: 2 },{ Position: 1 },{ Position: 4 },{ Position: 3 }
        ]
        
        var expectedArray = [
            { Position: 1 },{ Position: 2 },{ Position: 3 },{ Position: 4 }
        ]

        var testSucceded = true;

        //Act
        sortPlayersByPosition(testArray)
        
        //Assert
        expectedArray.forEach((a, i) =>
            a.Position != testArray[i].Position ? testSucceded = false : null)
        addTestResult("testArraySort", testSucceded)
    }


    function testPutMeAtIndexZero() {
        //Arrange
        var nickName  = "3"
        var testArray = [
            { Name: "1" },
            { Name: "2" },
            { Name: "3" },
            { Name: "4" }
        ]

        var expectedArray = [
            { Name: "3" },
            { Name: "4" },
            { Name: "1" },
            { Name: "2" }
        ]

        var testSucceded = true;

        //Act
        putMeAtIndexZero(testArray, nickName)
        
        //Assert
        expectedArray.forEach((a, i) => a.Name != testArray[i].Name ? testSucceded = false : null)
        addTestResult("testPutMeAtIndexZero", testSucceded)
    }
})
