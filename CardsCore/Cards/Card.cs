using System;
namespace CardsCore.Cards
{
    public class Card
    {
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public int Value { get; set; }

        public override String ToString(){
            return String.Format("{0} of {1}. Value: {2}", this.Face, this.Suit, this.Value);
        }
    }
}
