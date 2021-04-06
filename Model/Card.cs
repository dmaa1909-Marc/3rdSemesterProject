using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {

    public class Card {
        public Card(Value value, Suit suit, Visibility visibility, int pileId, Player owner) {
            CardValue = value;
            CardSuit = suit;
            Visibility = visibility;
            PileId = pileId;
            Owner = owner;
            //TODO: playerID should be a player object instead
        }

        public Value CardValue { get; set; }
        public Suit CardSuit { get; set; }
        public Visibility Visibility { get; set; }
        public int PileId { get; set; }
        public Player Owner { get; set; }



        public override bool Equals(Object obj) {
            if(obj == null)
                return false;

            Card other = obj as Card;
            if((Object)other == null)
                return false;

            return this.CardValue == other.CardValue
                && this.CardSuit == other.CardSuit
                && this.Visibility == other.Visibility
                && this.PileId == other.PileId
                && this.Owner == other.Owner;
        }

        public override int GetHashCode() {
            return CardValue.GetHashCode() +
                CardSuit.GetHashCode() +
                Visibility.GetHashCode() +
                PileId.GetHashCode() +
                Owner.GetHashCode();
        }


        public enum Value {
            Ace = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eighth = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13
        }

        public enum Suit : byte {
            Hearts = (byte)'H',
            Diamonds = (byte)'D',
            Clubs = (byte)'C',
            Spades = (byte)'S'
        }

    }
}
