using System;
using CardsCore.Cards;
namespace CardsCore.Decks.Factories
{
    /// <summary>
    /// Produces a standard deck of the 52 unique french playing cards.
    /// </summary>
    public class StandardDeckFactory : IDeckFactory
    {
        public StandardDeckFactory()
        {
        }

        public Deck CreateDeck()
        {
            var deck = new Deck();

            for (int i = 0; i < Enum.GetValues(typeof(Suit)).Length; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var card = new Card() { Suit = (Suit)i, Face = (Face)j };
                    if (j <= 8)
                        card.Value = j + 1;
                    else
                        card.Value = 10;
                    deck.Push(card);
                }
            }

            return deck;
        }
    }
}
