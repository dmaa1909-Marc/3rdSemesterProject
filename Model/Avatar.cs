using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class Avatar {
        public int Id { get; set; }
        public string Image { get; set; }

        public Avatar() {
        }

        public Avatar(int id, string image) {
            Id = id;
            Image = image;
        }
    }
}
