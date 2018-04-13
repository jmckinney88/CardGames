using System;
using System.Text;
using CardsCore.Decks;
using CardsCore.Decks.Factories;
namespace Blackjack
{
    public class Player
    {
        public int Chips { get; set; }
        public Deck Hand { get; private set; }
        public String Name { get; private set; }

        public Player(String name)
        {
            Hand = new EmptyDeckFactory().CreateDeck();
            Chips = 0;
            Name = name;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(String.Format("Chips: {0}", Chips));
            if (Hand.NumCards > 0) {
                stringBuilder.AppendLine("[Hand]");
                stringBuilder.AppendLine(Hand.ToString());
            }
            return stringBuilder.ToString();
		}
	}
}
