using System;
namespace CardsCore.Decks.Factories
{
    public class EmptyDeckFactory : IDeckFactory
    {
        public EmptyDeckFactory()
        {
        }

        public Deck CreateDeck()
        {
            return new Deck();
        }
    }
}
