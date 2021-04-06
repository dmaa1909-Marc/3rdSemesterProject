using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.Card;

namespace Model {
    public class Pile {
        public Visibility Visibility { get; set; }
        public PileType PileType { get; set; }
        public Accessability Accessibility { get; set; }

        public float HAlignPercent { get; set; }
        public float VAlignPercent { get; set; }

        public Player Owner { get; set; }

        private static Random random = new Random();
        public List<Card> Cards { get; set; }
        public int VersionNo { get; set; } = 0;
        public int PileNo { get; set; }

        public Pile(Visibility visibility, PileType pileType, Accessability accessibility, float hAlignPercent, float vAlignpercent, Player owner, List<Card> listOfCards, int pileNo) {
            Visibility = visibility;
            PileType = pileType;
            Accessibility = accessibility;
            HAlignPercent = hAlignPercent;
            VAlignPercent = vAlignpercent;
            Owner = owner;
            Cards = listOfCards ?? new List<Card>();
            PileNo = pileNo;
        }

        public Pile() {
        }

        public void ShufflePile() {

            // Durstenfeld's modern version of Fisher-Yates shuffle algorithm
            for(int i = Cards.Count - 1; i > 0; --i) {
                int randomCard = random.Next(i + 1);
                Card temp = Cards[i];
                Cards[i] = Cards[randomCard];
                Cards[randomCard] = temp;
            }
        }

        public Card RemoveCard(int sourceIndex) {
            Card removedCard = Cards[sourceIndex];
            Cards.RemoveAt(sourceIndex);
            return removedCard;
        }

        public void AddCard(int targetIndex, Card cardToMove) {
            Cards.Insert(targetIndex, cardToMove);
            if(PileType == PileType.Hand) {
                cardToMove.Owner = Owner;
            }
        }

        public ICollection<Card> GetCards() {
            return Cards;
        }
    }
}