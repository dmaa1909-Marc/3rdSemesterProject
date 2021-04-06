using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public enum Accessability {
        AccessableToOwner,
        AccessableToOthers,
        AccessableToAll,
        AccessableToNoOne
    }

    public enum Visibility {
        VisibleToOwner,
        VisibleToOthers,
        VisibleToAll,
        VisibleToNoOne
    }

    public enum PileType {
        Deck,
        Hand,
        Discard,
        Run
    }
}
