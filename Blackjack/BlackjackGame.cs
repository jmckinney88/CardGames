using System;
using System.Threading;
using System.Collections.Generic;
using CardsCore.Cards;
using CardsCore.Decks;

namespace Blackjack
{
    public static class BlackjackGame
    {
        static int chips;
        static Deck deck;
        static List<Card> userHand;
        static List<Card> dealerHand;

        public static void Run(){
            Console.Title = "♠♥♣♦ House of Payne Blackjack ♦♣♥♠";
            Console.WriteLine("House of Payne Blackjack!!\n");

            chips = 500;
            deck = new Deck();
            deck.Shuffle();

            while (chips > 0)
            {
                DealHand();
                Console.WriteLine("\nPress any key to continue...\n");
                ConsoleKeyInfo userInput = Console.ReadKey(true);
            }

            Console.WriteLine("Bankrupt!  Come back to the House of Payne when you have more cash! \n");
            Console.ReadLine();
        }

        static void DealHand()
        {
            if (deck.GetAmountOfRemainingCrads() < 20)
            {
                deck.Initialize();
                deck.Shuffle();
            }

            Console.WriteLine("Remaining Cards: {0}", deck.GetAmountOfRemainingCrads());
            Console.WriteLine("Current Chips: {0}", chips);
            Console.WriteLine("How much would you like to bet? (1 - {0})", chips);
            string input = Console.ReadLine().Trim().Replace(" ", "");
            int betAmount;
            while (!Int32.TryParse(input, out betAmount) || betAmount < 1 || betAmount > chips)
            {
                Console.WriteLine("Try Again. How much would you like to bet? (1 - {0})", chips);
                input = Console.ReadLine().Trim().Replace(" ", "");
            }
            Console.WriteLine();

            userHand = new List<Card>();
            userHand.Add(deck.DrawACard());
            userHand.Add(deck.DrawACard());

            foreach (Card card in userHand)
            {
                if (card.Face == Face.Ace)
                {
                    card.Value += 10;
                    break;
                }
            }

            Console.WriteLine("[You]");
            Console.WriteLine("Card 1: {0} of {1}", userHand[0].Face, userHand[0].Suit);
            Console.WriteLine("Card 2: {0} of {1}", userHand[1].Face, userHand[1].Suit);
            Console.WriteLine("Total: {0}\n", userHand[0].Value + userHand[1].Value);

            dealerHand = new List<Card>();
            dealerHand.Add(deck.DrawACard());
            dealerHand.Add(deck.DrawACard());

            foreach (Card card in dealerHand)
            {
                if (card.Face == Face.Ace)
                {
                    card.Value += 10;
                    break;
                }
            }

            Console.WriteLine("[Dealer]");
            Console.WriteLine("Card 1: {0} of {1}", dealerHand[0].Face, dealerHand[1].Suit);
            Console.WriteLine("Card 2: [Hole Card]");
            Console.WriteLine("Total: {0}\n", dealerHand[0].Value);

            bool insurance = false;

            if (dealerHand[0].Face == Face.Ace)
            {
                Console.WriteLine("Would you care for Insurance? (y / n)");
                string userInput = Console.ReadLine();

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
            }

            if (dealerHand[0].Face == Face.Ace || dealerHand[0].Value == 10)
            {
                Console.WriteLine("Dealer checks if they have blackjack...\n");
                Thread.Sleep(2000);
                if (dealerHand[0].Value + dealerHand[1].Value == 21)
                {
                    Console.WriteLine("[Dealer]");
                    Console.WriteLine("Card 1: {0} of {1}", dealerHand[0].Face, dealerHand[1].Suit);
                    Console.WriteLine("Card 2: {0} of {1}", dealerHand[1].Face, dealerHand[1].Suit);
                    Console.WriteLine("Total: {0}\n", dealerHand[0].Value + dealerHand[1].Value);

                    Thread.Sleep(2000);

                    int amountLost = 0;

                    if (userHand[0].Value + userHand[1].Value == 21 && insurance)
                    {
                        amountLost = betAmount / 2;
                    }
                    else if (userHand[0].Value + userHand[1].Value != 21 && !insurance)
                    {
                        amountLost = betAmount + betAmount / 2;
                    }

                    chips -= amountLost;

                    Console.WriteLine("Good Try! However, you lost {0} chips", amountLost);

                    Thread.Sleep(1000);

                    return;
                }
                else
                {
                    Console.WriteLine("Dealer does not have a blackjack, moving on...\n");
                }
            }

            if (userHand[0].Value + userHand[1].Value == 21)
            {
                Console.WriteLine("Blackjack!! You Won! ({0} chips)\n", betAmount + betAmount / 2);
                chips += betAmount + betAmount / 2;
                return;
            }

            do
            {
                Console.WriteLine("Please choose a valid option: [(S)tand (H)it]");
                ConsoleKeyInfo userOption = Console.ReadKey(true);
                while (userOption.Key != ConsoleKey.H && userOption.Key != ConsoleKey.S)
                {
                    Console.WriteLine("I don't understand, Would you like to Stand or Hit?: [(S)tand (H)it]");
                    userOption = Console.ReadKey(true);
                }
                Console.WriteLine();

                switch (userOption.Key)
                {
                    case ConsoleKey.H:
                        userHand.Add(deck.DrawACard());
                        Console.WriteLine("You Hit {0} of {1}", userHand[userHand.Count - 1].Face, userHand[userHand.Count - 1].Suit);
                        int totalCardsValue = 0;
                        foreach (Card card in userHand)
                        {
                            totalCardsValue += card.Value;
                        }
                        Console.WriteLine("Total cards value now: {0}\n", totalCardsValue);
                        if (totalCardsValue > 21)
                        {
                            Console.Write("Busted!\n");
                            chips -= betAmount;
                            Thread.Sleep(2000);
                            return;
                        }
                        else if (totalCardsValue == 21)
                        {
                            Console.WriteLine("Great Job! I highly recommend you Stand...\n");
                            Thread.Sleep(2000);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case ConsoleKey.S:

                        Console.WriteLine("[Dealer]");
                        Console.WriteLine("Card 1: {0} of {1}", dealerHand[0].Face, dealerHand[1].Suit);
                        Console.WriteLine("Card 2: {0} of {1}", dealerHand[1].Face, dealerHand[1].Suit);

                        int dealerCardsValue = 0;
                        foreach (Card card in dealerHand)
                        {
                            dealerCardsValue += card.Value;
                        }

                        while (dealerCardsValue < 17)
                        {
                            Thread.Sleep(2000);
                            dealerHand.Add(deck.DrawACard());
                            dealerCardsValue = 0;
                            foreach (Card card in dealerHand)
                            {
                                dealerCardsValue += card.Value;
                            }
                            Console.WriteLine("Card {0}: {1} of {2}", dealerHand.Count, dealerHand[dealerHand.Count - 1].Face, dealerHand[dealerHand.Count - 1].Suit);
                        }
                        dealerCardsValue = 0;
                        foreach (Card card in dealerHand)
                        {
                            dealerCardsValue += card.Value;
                        }
                        Console.WriteLine("Total: {0}\n", dealerCardsValue);

                        if (dealerCardsValue > 21)
                        {
                            Console.WriteLine("Dealer busts! You win! ({0} chips)", betAmount);
                            chips += betAmount;
                            return;
                        }
                        else
                        {
                            int playerCardValue = 0;
                            foreach (Card card in userHand)
                            {
                                playerCardValue += card.Value;
                            }

                            if (dealerCardsValue > playerCardValue)
                            {
                                Console.WriteLine("Dealer has {0} and you have {1}, dealer wins!", dealerCardsValue, playerCardValue);
                                chips -= betAmount;
                                return;
                            }
                            else
                            {
                                Console.WriteLine("You have {0} and dealer has {1}, you win!", playerCardValue, dealerCardsValue);
                                chips += betAmount;
                                return;
                            }
                        }
                        break;

                    default:
                        break;
                }

                Console.ReadLine();
            }
            while (true);
        }
    }
}
