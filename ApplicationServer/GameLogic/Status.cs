using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationServer.GameLogic {
    public enum Status {
        None,
        Success,
        DuplicateNickNameError,
        InvalidGamePinError,
        GameIsFullError,
        GameIsStartedError,
        UnknownError,
        InvalidMoveError,
        NotInGameError,
        AddPileError,
        PileNotRemovedError,
        GameNotFoundError
    }
}