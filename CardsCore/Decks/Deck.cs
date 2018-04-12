using System;
using System.Collections.Generic;
using CardsCore.Cards;

namespace CardsCore.Decks
{
    public class Deck
    {
        private Stack<Card> cards;

        public Deck()
        {
            cards = new Stack<Card>();
        }

        /// <summary>
        /// Shuffles the deck three times;
        /// </summary>
        public void Shuffle()
        {
            Shuffle(3);
        }

        public void Shuffle(int numShuffles) {
            Random rng = new Random();
            var cardList = new List<Card>(cards);
            cards.Clear();
            for (int shuffleCount = 0; shuffleCount < numShuffles; shuffleCount++)
            {
                for (int cardIndex = cardList.Count - 1; cardIndex > 0; cardIndex--)
                {
                    //indexToMove can be any index of a card that has not moved and
                    // is not the current card to move.
                    int indexToMove = rng.Next(0, cardIndex);
                    Card card = cardList[indexToMove];
                    cardList[indexToMove] = cardList[cardIndex];
                    cardList[cardIndex] = card;
                }
            }

            for (int index = 0; index < cardList.Count; index++)
            {
                Push(cardList[index]);
            }
        }

        public void Push(Card card){
            cards.Push(card);
        }

        public int NumCards { get { return cards.Count; } }

        public Card Pop()
        {
            if (NumCards <= 0){
                throw new InvalidOperationException("Deck is empty");   
            }
            return cards.Pop();
        }

        public void PrintDeck()
        {
            int i = 1;
            foreach (Card card in cards)
            {
                Console.WriteLine("Card {0}: {1} of {2}. Value: {3}", i, card.Face, card.Suit, card.Value);
                i++;
            }
        }



    }
}
