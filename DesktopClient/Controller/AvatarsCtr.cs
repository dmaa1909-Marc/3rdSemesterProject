using Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Controller {
    class AvatarsCtr {
        AvatarsRepository avatarsRepository = new AvatarsRepository();

        public IEnumerable<Avatar> GetAllAvatars() {
            return avatarsRepository.GetAllUserAvatars();
        }
    }
}
