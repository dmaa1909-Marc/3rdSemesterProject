using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class Player {
        public int Position { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public int GameId { get; set; }
        public Guid UserAccountId { get; set; }
        public int Id { get; set; }

        public Player(string name, int position, Color color) {
            Name = name;
            Position = position;
            Color = color;
        }

        public Player() {
           
        }
    }
}
