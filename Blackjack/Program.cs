using System;
using System.Collections.Generic;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playing = true;
            int playerChips = 100;
            Console.WriteLine("Welcome! here are some chips on the house!");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while(playing)
            {
                if (playerChips == 0)
                {
                    Console.WriteLine("Oops, looks like you're out of chips!");
                    return;
                }
                Console.WriteLine($"Current chips: {playerChips}");
                Console.WriteLine("Whats your wager?");
                int playerBet;
                if (!int.TryParse(Console.ReadLine(), out playerBet))
                {
                    Console.WriteLine("Please enter a proper amount of chips!");
                    break;
                }
                if(playerBet > playerChips)
                {
                    Console.WriteLine("You can't bet chips you don't have!");
                    continue;
                }
                Deck newDeck = new Deck { cards = Deck.ShuffledCards() };
                var playerHand = new List<Card> { newDeck.cards[0], newDeck.cards[2] };
                var dealerHand = new List<Card> { newDeck.cards[1], newDeck.cards[3] };
                playerHand.ForEach(pc => Console.WriteLine($"Players Card: {pc.display} of {pc.suit}"));
                Console.WriteLine($"Player Value : {PlayerHandValue(playerHand)}\n");
                Console.WriteLine($"Dealer: {dealerHand[0].display} of {dealerHand[0].suit} and 1 face down.\nDealer Known Value : {dealerHand[0].value}\n");

                int nextCard = 4;
                bool bust = false;
                bool activeDeal = true;
                bool instantWinner = false;

                while (activeDeal)
                {                  
                    int activeHandValue = PlayerHandValue(playerHand);
                    Console.WriteLine($"Player Value : {activeHandValue}\n");
                    Console.WriteLine($"Dealer Value: {dealerHand[0].value}\n");
                    if (activeHandValue == 21 && playerHand.Count == 2)
                    {
                        instantWinner = true;
                        break;
                    }
                    if (activeHandValue > 21)
                    {
                        bust = true;
                        activeDeal = false;
                        break;
                    }
                    Console.WriteLine("Hit or Stand?");
                    var response = Console.ReadLine().ToLower();

                    if (response.Equals("stand"))
                    {
                        activeDeal = false;
                        break;
                    }
                    if (response.ToLower().Equals("hit"))
                    {
                        playerHand.Add(newDeck.cards[nextCard]);
                        nextCard++;
                    }
                    if (!response.ToLower().Equals("hit") && !response.ToLower().Equals("stand"))
                    {
                        break;
                    }
                    for (var i = 0; i < playerHand.Count; i++)
                    {
                        if (activeHandValue > 21)
                        {
                            if (playerHand[i].value == 11)
                            {
                                playerHand[i].value = 1;
                            }
                        }
                    }
                    

                }
                int playerFinalHandValue = PlayerHandValue(playerHand);
                if (bust)
                {
                    Console.WriteLine($"Oof, you busted with {playerFinalHandValue}");
                    Console.WriteLine($"Dealer had {PlayerHandValue(dealerHand)}");
                }

                bool dealerPlays = true;
                bool dealerBust = false;

                while (dealerPlays && !instantWinner && !bust)
                {
                    int activeDealerValue = PlayerHandValue(dealerHand);
                    if (activeDealerValue == 21)
                    {
                        dealerPlays = false;
                        break;
                    }
                    if (activeDealerValue > 21)
                    {
                        dealerBust = true;
                        dealerPlays = false;
                        break;
                    }
                    if (activeDealerValue < 17)
                    {
                        dealerHand.Add(newDeck.cards[nextCard]);
                        nextCard++;
                    }
                    if (activeDealerValue >= 17)
                    {
                        for (var i = 0; i < dealerHand.Count; i++)
                        {
                            if (activeDealerValue > 21)
                            {
                                if (playerHand[i].value == 11)
                                {
                                    playerHand[i].value = 1;
                                }
                            }
                        }
                        if (activeDealerValue >= 17)
                        {
                            dealerPlays = false;
                            break;
                        }
                    }


                }
                int dealerFinalHandValue = PlayerHandValue(dealerHand);
                if (instantWinner)
                {
                    Console.WriteLine("Blackjack! Winner Winner Chicken Dinner!");
                    playerChips += playerBet;
                }
                Console.WriteLine("Finished Dealing!");
                if ((playerFinalHandValue > dealerFinalHandValue || dealerBust) && !bust)
                {
                    if (dealerBust)
                    {
                        Console.WriteLine("DEALER BUSTED!");
                    }
                    Console.WriteLine("Congrats you won!");                 
                    playerChips += playerBet;
                }
                if((playerFinalHandValue < dealerFinalHandValue || bust) && !dealerBust)
                {
                    if(bust)
                    {
                        Console.WriteLine("YOU BUSTED!");
                    }
                    Console.WriteLine("Maybe Next Time!");
                    playerChips -= playerBet;
                }
                if (playerFinalHandValue == dealerFinalHandValue)
                {
                    Console.WriteLine("PUSH!");
                }
                Console.WriteLine($"Player Value: {playerFinalHandValue}");
                Console.WriteLine($"Dealer Value: {dealerFinalHandValue}");
                
                Console.WriteLine("Would you like to keep playing?[Y/N]");
                var keepPlaying = Console.ReadLine().ToLower();
                switch(keepPlaying)
                {
                    case "y":
                    case "yes":
                        break;
                    case "n":
                    case "no":
                        Console.WriteLine("Thanks for playing!");
                        return;
                    default:
                        Console.WriteLine("Well if you can't give me a straight answer, seems like you're playing again!");
                        break;
                }
            }
            Console.WriteLine("Finished Playing");            
        }

        static int PlayerHandValue(List<Card> hand)
        {
            int value = 0;
            hand.ForEach(c => value += c.value);
            return value;
        }
    }
}
