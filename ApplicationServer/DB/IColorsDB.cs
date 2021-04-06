using System.Collections.Generic;
using System.Drawing;

namespace ApplicationServer.DB {
    public interface IColorsDB {
        IEnumerable<Color> GetColors();
        void AddColor(Color color);
    }
}