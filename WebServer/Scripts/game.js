const ACCESSIBILITY = {
    ACCESSIBLE_TO_OWNER: 0,
    ACCESSIBLE_TO_OTHERS: 1,
    ACCESSIBLE_TO_ALL: 2,
    ACCESSIBLE_TO_NOONE: 3
}

const VISIBILITY = {
    VISIBLE_TO_OWNER: 0,
    VISIBLE_TO_OTHERS: 1,
    VISIBLE_TO_ALL: 2,
    VISIBLE_TO_NOONE: 3
}

const PILETYPE = {
    DECK: 0,
    HAND: 1,
    DISCARD: 2,
    RUN: 3
}

$(function () {
    //Centimental card spike. (slowly dying 😢 - it's now dead.. 😭)
    //var moved = false
    //var coord
    //var card = $("<div />")
    //    .addClass("card")
    //    .draggable({
    //        drag: function (e) {
    //            coord = card.offset();
    //            moved = true;
    //        }
    //    })
    //    .appendTo("body")


    buildTable();
    pickNickName();

    var nickName;  //declared nickName as global variable for use in putMeAtIndexZero() method

    var players = [];
    var piles = [];

    var connection = $.hubConnection("https://localhost:44328/signalR", { useDefaultPath: false });
    var gameHub = connection.createHubProxy("gameHub");

    gameHub.on("GameIsStarted", function () {//Development function to redirect on reload...
        window.location.href = "https://localhost:44305/game/creategame";
    });

    gameHub.on("moveCard", function (moveCardModel) {
        console.log(moveCardModel);
        //console.log(moveCardModel.SourcePileVersionNo + " : " + moveCardModel.SourcePileNo + " : " + moveCardModel.SourceIndex + " : " + moveCardModel.TargetPileVersionNo + " : " + moveCardModel.TargetPileNo + " : " + moveCardModel.TargetIndex);

        var sourcePile = piles.find(function (pile) {
            return pile.pileNo == moveCardModel.SourcePileNo;
        });

        var cardsToMove = sourcePile.removeCards(moveCardModel.SourceIndex);

        if (moveCardModel.TargetPileNo != moveCardModel.SourcePileNo) {
            var targetPile = piles.find(function (pile) {
                return pile.pileNo == moveCardModel.TargetPileNo;
            });

            targetPile.insertCards(cardsToMove, moveCardModel.TargetIndex);

            sourcePile.setVersionNo(moveCardModel.SourcePileVersionNo);
            targetPile.setVersionNo(moveCardModel.TargetPileVersionNo);
        } else {
            sourcePile.insertCards(cardsToMove, moveCardModel.TargetIndex);

            sourcePile.setVersionNo(moveCardModel.SourcePileVersionNo)
        }
    });

    gameHub.on("errorMessage", function (message) {
        alert("Encountered error, error code is: " + message);
    });

    gameHub.on("newPlayerJoining", function (player) {
        player.Name = DOMPurify.sanitize(player.Name);

        addPlayerToGame(player);
    });

    gameHub.on("updatePlayersList", function (updatedPlayersList) {
        players = [];

        for (var i = 0; i < updatedPlayersList.length; i++) {
            var player = updatedPlayersList[i];

            player.Name = DOMPurify.sanitize(player.Name);

            addPlayerToGame(player);
        }
    });

    gameHub.on("startGame", function (receivedPiles) {
        console.log(receivedPiles);
        piles = [];
        seatPlayers();

        for (let i = 0; i < receivedPiles.length; i++) { //For each pile
            addPile(receivedPiles[i]);
        }

        console.log(piles);
        removeLayerMask();
    });

    gameHub.on("AddPile", function (pile) {
        console.log(pile);
    });




    /* ======================  GameHub Invokers ======================= */

    connection.start().done(function () {
        gameHub.invoke("IsGameStarted", gamePin); //Redirect if game is started
        $("#NicknameBtn").click(joinGame);
    });

    function joinGame() {
        nickName = DOMPurify.sanitize($("#Nickname").val());
        gameHub.invoke("joinGame", gamePin, nickName);
        showLobby();
    }

    function startGame() {
        gameHub.invoke("startGame", gamePin);
    }

    function moveCard(sourcePile, sourceIndex, targetPile, targetIndex) {
        gameHub.invoke("MoveCard", gamePin, {
            sourcePileVersionNo: sourcePile.VersionNo,
            sourcePileNo: sourcePile.PileNo,
            sourceIndex: sourceIndex,
            targetPileVersionNo: targetPile.VersionNo,
            targetPileNo: targetPile.PileNo,
            targetIndex: targetIndex
        });
    }

    function addNewPile(pile, cardsToMove) {
        //Remove JQ objects before sending the pile, without removing them from the actual pile and card objects
        //To do so shallow copies are used here. 
        var shallowPileCopy = { ...pile };
        shallowPileCopy.pileJQObject = undefined;
        shallowPileCopy.cards = undefined;

        gameHub.invoke("AddNewPile", gamePin, shallowPileCopy, cardsToMove);
    }

    function addPlayerToGame(player) {
        players.push(player);

        sortPlayersByPosition(players)

        seatPlayers();

        appendPlayerToPlayersList(player);
    }

    function appendPlayerToPlayersList(player) {
        $("#PlayersList").append($("<li />").text(player.Name).css("background-color", player.Color));
    }





    /* ======================  LOBBY RELATED ======================= */

    function buildTable() {

        $('#table').html('<div class="table-grid"></div>');
        $('#table').append('<div class="table-grid" id="table-top"></div>');
        $('#table').append('<div class="table-grid"></div>');

        $('#table').append('<div class="table-grid" id="table-left"></div>');
        $('#table').append('<div class="table-grid" id="table-center"></div>');
        $('#table').append('<div class="table-grid" id="table-right"></div>');

        $('#table').append('<div class="table-grid"></div>');
        $('#table').append('<div class="table-grid" id="table-bottom"></div>');
        $('#table').append('<div class="table-grid"></div>');
    }



    function pickNickName() {
        $('body').append('<div id="layer-mask"></div>');
        $('#layer-mask').html('<div class="menu-container"></div>');
        $('.menu-container').append('<div class="container-logo" >Decks</div>');
        $('.menu-container').append(' <div class="container-content"></div>');
        $('.container-content').append('<div class="inner-container"><div>');
        $('.inner-container').append('<form></form>');
        $('.inner-container').append('<div class"content-lbl"><label class="decks-lbl">Nickname:</label></div>');
        $('.inner-container').append('<div class="content-txt"><input type="text" id="Nickname" class="text-field" placeholder="Enter nickname..." /></div>');
        $('.inner-container').append('<div class="content-btn"><input id="NicknameBtn" class="decks-btn" type="button" value="Connect to lobby"/></div>');
        $('#layer-mask').append('<div id = "suit-logo"></div>');

    }



    function seatPlayers() {

        buildTable();
        putMeAtIndexZero(players, nickName);

        var player;

        if (players.length >= 1) {
            player = players[0];
            player.tableJQObject = $("<div class='player-table player-table-bottom' />");
            player.playerJQObject = $("<div class='player player-bottom " + player.Color + "'>" + player.Name + "</div>")
                .append(player.tableJQObject)
                .data("playerData", player);

            $("#table-bottom").append(player.playerJQObject);
        }

        var PlayersToSeat = players.length - 1;
        var sidesPlayersAmount = Math.floor(PlayersToSeat / 3);
        var topPlayerAmount = sidesPlayersAmount + PlayersToSeat % 3;

        var totalIndex = 1;

        for (var i = 0; i < sidesPlayersAmount; i++) {
            player = players[totalIndex];
            player.tableJQObject = $("<div class='player-table player-table-left' />");
            player.playerJQObject = $("<div class='player player-left " + player.Color + "'>" + player.Name + "</div>")
                .append(player.tableJQObject)
                .data("playerData", player)

            $("#table-left").prepend(player.playerJQObject);
            totalIndex++;
        }

        for (var i = 0; i < topPlayerAmount; i++) {
            player = players[totalIndex];
            player.tableJQObject = $("<div class='player-table player-table-top' />");
            player.playerJQObject = $("<div class='player player-top " + player.Color + "'>" + player.Name + "</div>")
                .append(player.tableJQObject)
                .data("playerData", player);

            $("#table-top").append(player.playerJQObject);
            totalIndex++;
        }

        for (var i = 0; i < sidesPlayersAmount; i++) {
            player = players[totalIndex];
            player.tableJQObject = $("<div class='player-table player-table-right' />");
            player.playerJQObject = $("<div class='player player-right " + player.Color + "'>" + player.Name + "</div>")
                .append(player.tableJQObject)
                .data("playerData", player);

            $("#table-right").append(player.playerJQObject);
            totalIndex++;
        }
    }

    function showLobby() {

        buildTable();

        $('#layer-mask').html('<div class="menu-container"></div>');
        $('.menu-container').append('<div class="container-logo">Decks</div>');
        $('.menu-container').append('<div class="container-content"></div>');
        $('.container-content').append('<div class="inner-container"><div>');
        //$('.inner-container').append('<div class"lobby-lbl"><div id="game-name">Room Name:</div></div>');
        //$('.inner-container').append('<div class="lobby-lbl">Players:</div>');
        $('.inner-container').append('<div class="lobby-list"><ul id="PlayersList"></ul><div>');
        $('.inner-container').append('<div class"lobby-btn"><input id="start-game-btn" class="decks-btn" type="button" value="Start game"/></div>');
        $('#start-game-btn').click(startGame);
        $('#layer-mask').append('<div id = "suit-logo"></div>');
    };

    function removeLayerMask() {
        $('#layer-mask').css('display', 'none');
    }


    function addPile(pile) {
        var newPile = new Pile(pile);

        var newJQPile = newPile.pileJQObject;

        piles.push(newPile);

        var newCards = []

        for (let j = 0; j < pile.Cards.length; j++) { //For each card in pile
            var card = pile.Cards[j];
            newCards.push(new Card(card));
        }

        newPile.insertCards(newCards);

        if (pile.PileType == 0) { //DECK
            newJQPile.addClass("deck");
            $('#table-center').append(newJQPile);

        }
        else if (pile.PileType == 1) { //HAND
            newJQPile.addClass("hand");

            $('.player').each(function (i, element) {
                var currentName = $(this).data("playerData").Name;
                if (currentName === pile.Owner.Name) {
                    $(this).append(newJQPile);
                }
            })

        } else if (pile.PileType == 2) {//DISCARD
            newJQPile.addClass("discard");
            $('#table-center').append(newJQPile);

        } else if (pile.PileType == 3) {//RUN
            //TODO
        }
    }

    function createNewJQPile(pile) {
        return $('<div />')
            .addClass('pile')
            .css({
                "position": "absolute",
                "left": pile.VAlignPercent + "%",
                "top": pile.HAlignPercent + "%"
            })
            .sortable({
                tolerance: "pointer",
                //revert: 'true',
                //hoverClass: 'hover',
                //accept: '.card',// .pile',
                cancel: ".deck :not(:last-child), .discard :not(:last-child)",
                cursor: "pointer",
                cursorAt: { left: 10, top: 10 },
                containment: "#table",
                connectWith: ".pile",
                activate: function (e, ui) {
                    $(this).addClass('dropZone');
                },
                deactivate: function (e, ui) {
                    $(this).removeClass('dropZone');
                    if ($(this).is(".deck, .discard")) {
                        $(this).sortable({ items: ":last" });
                    }
                    if (!$(this).is('.hand, .deck, .discard') && $(this).is(':empty')) {
                        $(this).remove();
                    }
                },
                start: function (e, ui) {
                    ///////////// GHOST PILES WIP //////////////////
                    //$("#table-center, .player-table").append(createNewJQPile({
                    //    valignpercent: 0,
                    //    halignpercent: 0,

                    //}).sortable({
                    //    connectWith: ".pile",
                    //    update: function (e, ui) {
                    //        var pile = new Pile({
                    //            vAlignPercent: 0,
                    //            hAlignPercent: 0,
                    //        })
                    //        var cardsToMove = [{
                    //            sourcePileVersionNo: ui.sender.data("pileData").VersionNo,
                    //            sourcePileNo: ui.sender.data("pileData").PileNo,
                    //            sourceIndex: ui.item.data("sourceIndex")
                    //        }]

                    //        addNewPile(pile, cardsToMove);
                    //    },
                    //    deactivate: function (e, ui) {
                    //        if ($(this).is(':empty')) {
                    //            $(this).remove();
                    //        }
                    //    }
                    //}));

                    ui.item.data("sourceIndex", ui.item.index());
                },
                update: function (e, ui) {
                    if ($(this).has(ui.item).length > 0) {
                        if (ui.sender != null) {
                            moveCard(ui.sender.data("pileData"), ui.item.data("sourceIndex"), $(this).data("pileData"), ui.item.index());
                        } else {
                            moveCard($(this).data("pileData"), ui.item.data("sourceIndex"), $(this).data("pileData"), ui.item.index());
                        }
                    }
                }
            })
            .draggable({
                cancel: ".hand",
                containment: "#table",
            })
            .disableSelection()
            .data("pileData", {
                PileNo: pile.PileNo,
                VersionNo: pile.VersionNo
            })
    }

    class Pile {

        cards = [];
        pileJQObject;
        owner = null;
        pileType;
        visibility;
        accessibility;
        vAlignPercent;
        hAlignPercent;
        pileNo;
        versionNo;

        constructor(pile) {
            this.pileJQObject = createNewJQPile(pile);

            this.owner = pile.Owner;
            this.pileType = pile.PileType;
            this.visibility = pile.Visibility;
            this.accessibility = pile.Accessibility;
            this.vAlignPercent = pile.VAlignPercent;
            this.hAlignPercent = pile.HAlignPercent;
            this.pileNo = pile.PileNo;
            this.versionNo = pile.VersionNo;
        }

        removeCards(index, amount = 1) {
            var removedCards = this.cards.splice(index, amount);
            return removedCards;
        }

        insertCards(cardsToInsert, index = this.cards.length) {
            this.cards.splice(index, 0, ...cardsToInsert); //...(three dots) spreads/inserts array items as individual arguments

            for (var i = 0; i < cardsToInsert.length; i++) {
                var currentCard = cardsToInsert[i];
                if (this.pileType === PILETYPE.HAND || this.pileType === PILETYPE.DECK) {
                    currentCard.setOwner(this.owner);
                }
                currentCard.setVisibility(this.visibility);
                insertAtIndex(this.pileJQObject, currentCard.cardJQObject, index + i);
            }
        }

        setVersionNo(versionNo) {
            this.versionNo = versionNo;
            this.pileJQObject.data('pileData').VersionNo = this.versionNo;
        }
    }

    class Card {
        backImageSrc;
        faceImageSrc;
        cardJQObject;

        cardValue;
        cardSuit;
        visibility = VISIBILITY.VISIBLE_TO_NOONE;
        owner = null;

        constructor(card) {
            this.backImageSrc = "/Content/images/red_back.png";
            this.faceImageSrc = "/Content/images/" + card.CardValue + String.fromCharCode(card.CardSuit).toLowerCase() + ".png";
            var image = new Image();
            image.src = this.backImageSrc;

            this.cardJQObject = $(image)
                .addClass('card');

            this.setVisibility(card.Visibility);
            this.setOwner(card.Owner);
            this.cardValue = card.CardValue;
            this.cardSuit = card.CardSuit;
        }

        setVisibility(visibility) {
            this.visibility = visibility;
            this.switchImage();
        }

        setOwner(owner) {
            if (this.owner != null) {
                this.cardJQObject.removeClass(this.owner.Color + "-border");
            }

            if (owner != null) {
                this.cardJQObject.addClass(owner.Color + "-border");
            }

            this.owner = owner;
            this.switchImage();
        }

        switchImage() {
            switch (this.visibility) {
                case VISIBILITY.VISIBLE_TO_OWNER:
                    if (this.owner === null || this.owner.Name !== nickName) {
                        this.cardJQObject.attr("src", this.backImageSrc);
                        break;
                    }

                case VISIBILITY.VISIBLE_TO_ALL:
                    this.cardJQObject.attr("src", this.faceImageSrc);
                    break;

                case VISIBILITY.VISIBLE_TO_OTHERS:
                    if (this.owner === null || this.owner.Name !== nickName) {
                        this.cardJQObject.attr("src", this.faceImageSrc);
                        break;
                    }

                case VISIBILITY.VISIBLE_TO_NOONE:

                default:
                    this.cardJQObject.attr("src", this.backImageSrc);
            }
        }
    }

    //class player {
    //    Name;
    //    Position;
    //    Color;
    //    playerJQObject;

    //    constructor(player) {
    //        this.player = player;
    //    }
    //}

})

var sortPlayersByPosition = function (players) {

    players.sort((player1, player2) => (player1.Position > player2.Position) ? 1 : -1);

}

var putMeAtIndexZero = function (players, nickName) {

    for (var i = 0; i < players.length && players[0].Name !== nickName; i++) {
        players.push(players.splice(0, 1)[0]);
    }
}

var insertAtIndex = function (elementToInsertTo, elementToInsert, index) {
    var offset = elementToInsert.offset();
    elementToInsert.remove();

    //TODO: Make a test
    if (index === 0)
        elementToInsertTo.prepend(elementToInsert);
    else
        elementToInsertTo.children().eq(index - 1).after(elementToInsert);

    elementToInsert.offset(offset);
    elementToInsert.animate({ left: 0, top: 0 }, { queue: false });
}









//TRASHCAN #####################################################################################
//##############################################################################################
//##############################################################################################


    //var pickRoomName = function () {
    //    $('#layer-mask').html('<div id="lobby"></div>')
    //    $('#lobby').append('<form action="/Game/Create" method="post">')
    //    $('#lobby').append('<label>Roomname:</label>')
    //    $('#lobby').append('<input type="text" id="gameName" name="gameName" placeholder="choose room name..." />')
    //    $('#lobby').append('<input id="room-name-btn" class="lobby-btn" type="submit" value="create room" onclick="pickNickName()"/>')
    //    $('#lobby').append('</form>')
    //} 


    //function makeCardsDraggable() {

    //    $('.card').draggable({
    //        containment: "#table",
    //        scroll: false,
    //        addClasses: false,
    //        stack: ".card",
    //        start: function (event, ui) {
    //            currElement = $(this); // capturing this exact element for further global use
    //        },
    //        stop: function (event, ui) {

    //            var left = (100 * parseFloat($(currElement).position().left / parseFloat($(currElement).parent().width())));
    //            var top = (100 * parseFloat($(currElement).position().top / parseFloat($(currElement).parent().height())));

    //            //$(this).append("left: " + left.toFixed(2) + "%");
    //            //$(this).append("right: " + right.toFixed(2) + "%");

    //            //$(this).css("left", left.toFixed(2) + "%");
    //            //$(this).css("top", top.toFixed(2) + "%");
    //            ////$(this).html(card.value);
    //            //$('#table-center').html(screenWidth + "x" + screenHeight);
    //        }
    //    });

    //}


//Old card.draggable
//.draggable({
//    revert: 'invalid',
//    //drag: function (e, ui) {
//    //    let tmpTop = ui.offset.top
//    //    //ui.offset.top = -ui.offset.left
//    //    //ui.offset.left = tmpTop
//    //    let tmpPosTop = ui.position.top
//    //    ui.position.top = /*- ui.offset.left*/ - ui.position.left
//    //    ui.position.left = /*tmpTop +*/ tmpPosTop

//    //},
//    connectToSortable: ".pile",
//    start: function (e, ui) {
//        console.log(ui.item)

//        $(this).data("sourcePile", $(this).closest(".pile").data("pileData"))
//        $(this).data("sourceindex", null)
//    },
//    //stop: function (e, ui) {
//    //    $(this).css({ left: '0px', top: '0px' })
//    //}
//    //create: function (event, ui) {
//    //    var index = ui.item.index() + 1;
//    //    console.log(index);
//    //}
//})