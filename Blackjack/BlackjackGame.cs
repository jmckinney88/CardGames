using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using CardsCore.Cards;
using CardsCore.Decks;
using CardsCore.Decks.Factories;

namespace Blackjack
{
    public class BlackjackGame
    {
        Deck deck;
        Deck discard;
        Player player;
        Player dealer;
        IDeckFactory standardDeckFactory;
        IDeckFactory emptyDeckFactory;

        protected void Init() {
            player = new Player("You");
            player.Chips = 500;
            player.Hand.Clear();
            standardDeckFactory = new StandardDeckFactory();
            emptyDeckFactory = new EmptyDeckFactory();
            deck = standardDeckFactory.CreateDeck();
            deck.Shuffle();
            discard = emptyDeckFactory.CreateDeck();
            dealer = new Player("Dealer");
            dealer.Hand.Clear();
        }

        public void Run() {
            Console.Title = "♠♥♣♦ House of Payne Blackjack ♦♣♥♠";
            Console.WriteLine("House of Payne Blackjack!!\n");

            Init();

            while (player.Chips > 0)
            {
                PlayRound();
                Console.WriteLine("\nPress any key to continue...\n");
                ConsoleKeyInfo userInput = Console.ReadKey(true);
            }

            Console.WriteLine("Bankrupt!  Come back to the House of Payne when you have more cash! \n");
            Console.ReadLine();
        }

        protected void PrepareDecks() {
            while(player.Hand.NumCards > 0)
            {
                discard.Push(player.Hand.Pop());
            }

            while (dealer.Hand.NumCards > 0)
            {
                discard.Push(dealer.Hand.Pop());
            }


            if (deck.NumCards < 20)
            {
                while (discard.NumCards > 0)
                {
                    deck.Push(discard.Pop());
                }
                deck.Shuffle();
            }
        }

        protected void DisplayPlayerState() {
            Console.WriteLine("Remaining Cards: {0}", deck.NumCards);
            Console.WriteLine(player);
        }

        protected int PromptPlayerForBet() {
            Console.WriteLine("How much would you like to bet? (1 - {0})", player.Chips);
            string input = Console.ReadLine().Trim().Replace(" ", "");
            int betAmount;
            while (!Int32.TryParse(input, out betAmount) || betAmount < 1 || betAmount > player.Chips)
            {
                Console.WriteLine("Try Again. How much would you like to bet? (1 - {0})", player.Chips);
                input = Console.ReadLine().Trim().Replace(" ", "");
            }
            Console.WriteLine();
            return betAmount;
        }

        protected void Deal(Deck hand) {
            hand.Push(deck.Pop());
            hand.Push(deck.Pop());

            foreach (Card card in hand)
            {
                if (card.Face == Face.Ace)
                {
                    card.Value += 10;
                    break;
                }
            }
        }

        protected static void DisplayHandState(Deck hand, String playerName, int numHole=0) {
            Console.WriteLine("[{0}]", playerName);
            List<Card> cards = new List<Card>(hand);
            int handValue = 0;
            for (int index = 0; index < cards.Count - numHole; index++){
                Console.WriteLine("Card {0}: {1}", index + 1, cards[index]);
                handValue += cards[index].Value;
            }
            for (int index = cards.Count - numHole; index < cards.Count; index++){
                Console.WriteLine("Card {0}: [Hole Card]", index + 1);
            }
            Console.WriteLine("Total: {0}\n", handValue);
        }

        protected static bool PromptPlayerForInsurance() {
            Console.WriteLine("Would you care for Insurance? (y / n)");
            string userInput = Console.ReadLine();
            bool insurance = false;
            while (userInput != "y" && userInput != "n")
            {
                Console.WriteLine("I did not understand. Insurance? (y / n)");
                userInput = Console.ReadLine();
            }

            if (userInput == "y")
            {
                insurance = true;
                Console.WriteLine("\n[Insurance Accepted!]\n");
            }
            else
            {
                insurance = false;
                Console.WriteLine("\n[Insurance Rejected]\n");
            }
            return insurance;
        }

        protected bool CheckForDealerBlackjack(int betAmount, bool insurance) {
            Console.WriteLine("Dealer checks if they have blackjack...\n");
            Thread.Sleep(2000);
            if (dealer.Hand.Sum(card => card.Value) == 21)
            {
                DisplayHandState(dealer.Hand, "Dealer");

                Thread.Sleep(2000);

                int amountLost = 0;

                int playerScore = player.Hand.Sum(card => card.Value);

                if (playerScore == 21 && insurance)
                {
                    amountLost = betAmount / 2;
                }
                else if (playerScore != 21 && !insurance)
                {
                    amountLost = betAmount + betAmount / 2;
                }

                player.Chips -= amountLost;

                Console.WriteLine("Good Try! However, you lost {0} chips", amountLost);

                Thread.Sleep(1000);

                return true;
            }
            return false;
        }

        protected ConsoleKeyInfo PromptPlayerForAction() {
            Console.WriteLine("Please choose a valid option: [(S)tand (H)it]");
            ConsoleKeyInfo userOption = Console.ReadKey(true);
            while (userOption.Key != ConsoleKey.H && userOption.Key != ConsoleKey.S)
            {
                Console.WriteLine("I don't understand, Would you like to Stand or Hit?: [(S)tand (H)it]");
                userOption = Console.ReadKey(true);
            }
            Console.WriteLine();
            return userOption;
        }

        /// <summary>
        /// Returns true if the player has not busted.
        /// </summary>
        protected bool Hit(int betAmount){
            Card newCard = deck.Pop();
            player.Hand.Push(newCard);
            Console.WriteLine("You Hit: {0}", newCard);
            int totalCardsValue = player.Hand.Sum(card => card.Value);
            DisplayHandState(player.Hand, player.Name);
            if (totalCardsValue > 21)
            {
                Console.Write("Busted!\n");
                player.Chips -= betAmount;
                Thread.Sleep(2000);
                return false;
            }
            else if (totalCardsValue == 21)
            {
                Console.WriteLine("Great Job! I highly recommend you Stand...\n");
                Thread.Sleep(2000);
            }
            return true;
        }

        protected void PlayRound() {
            PrepareDecks();
            DisplayPlayerState();

            int betAmount = PromptPlayerForBet();


            Deal(player.Hand);
            DisplayHandState(player.Hand, "You");

            Deal(dealer.Hand);
            DisplayHandState(dealer.Hand, "Dealer", 1);

            bool insurance = false;

            if (dealer.Hand.Peek().Face == Face.Ace)
            {
                insurance = PromptPlayerForInsurance();
            }

            if (dealer.Hand.Peek().Face == Face.Ace || dealer.Hand.Peek().Value == 10)
            {
                if(CheckForDealerBlackjack(betAmount, insurance)){
                    return;
                }
                else
                {
                    Console.WriteLine("Dealer does not have a blackjack, moving on...\n");
                }
            }

            if (player.Hand.Sum(card => card.Value) == 21)
            {
                Console.WriteLine("Blackjack!! You Won! ({0} chips)\n", betAmount + betAmount / 2);
                player.Chips += betAmount + betAmount / 2;
                return;
            }

            do
            {
                ConsoleKeyInfo userOption = PromptPlayerForAction();

                switch (userOption.Key)
                {
                    case ConsoleKey.H:
                        if(!Hit(betAmount)){
                            return;
                        }
                        break;

                    case ConsoleKey.S:

                        DisplayHandState(dealer.Hand, dealer.Name);

                        int dealerCardsValue = dealer.Hand.Sum(card => card.Value);

                        while (dealerCardsValue < 17)
                        {
                            Thread.Sleep(2000);
                            Card newCard = deck.Pop();
                            dealer.Hand.Push(newCard);
                            dealerCardsValue += newCard.Value;
                            Console.WriteLine("Card {0}: {1}", dealer.Hand.NumCards, newCard);
                        }
                        Console.WriteLine("Total: {0}\n", dealerCardsValue);

                        if (dealerCardsValue > 21)
                        {
                            Console.WriteLine("Dealer busts! You win! ({0} chips)", betAmount);
                            player.Chips += betAmount;
                            return;
                        }
                        else
                        {
                            int playerCardValue = player.Hand.Sum(card => card.Value);

                            if (dealerCardsValue > playerCardValue)
                            {
                                Console.WriteLine("Dealer has {0} and you have {1}, dealer wins!", dealerCardsValue, playerCardValue);
                                player.Chips -= betAmount;
                                return;
                            }
                            else
                            {
                                Console.WriteLine("You have {0} and dealer has {1}, you win!", playerCardValue, dealerCardsValue);
                                player.Chips += betAmount;
                                return;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            while (true);
        }
    }
}
