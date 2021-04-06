using ApplicationServer.DB;
using Model;
using System;
using System.Collections.Generic;

namespace ApplicationServer.BusinessLogic {
    class AvatarsManagement {
        AvatarsDB avatarsDB = new AvatarsDB();

        internal IEnumerable<Avatar> GetAllAvatars() {
            return avatarsDB.GetAllAvatars();
        }

        internal Avatar GetAvatarById(int id) {
            return avatarsDB.GetAvatarById(id);
        }
    }
}