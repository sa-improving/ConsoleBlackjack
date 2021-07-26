using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Deck
    {
        public List<Card> cards { get; set; }
        public static List<Card> Primer()
        {
            var values = new List<int>{2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11};
            var suits = new List<String>{ "♠", "♦", "♥", "♣" };
            List<Card> primer = new List<Card>();
            values.ForEach(v =>
            {
                int tenCount = 0;
                suits.ForEach(s =>
                {
                    switch(v)
                    {
                        case 11:
                            primer.Add(new Card { value = v, suit = s, display = "Ace" });
                            break;
                        case 10:
                            primer.Add(new Card { value = v, suit = s, display = FaceCardDisplay(tenCount) });
                            tenCount++;
                            break;
                        default:
                            primer.Add(new Card { value = v, suit = s, display = v.ToString() });
                            break;
                    }
                    
                });
            });
            return primer;
        }

        private static string FaceCardDisplay(int count)
        {
            switch(count)
            {
                case 0:
                    return "10";
                case 1:
                    return "Jack";
                case 2:
                    return "Queen";
                case 3:
                    return "King";
                default:
                    return "Joker";
            }
        }

        public static List<Card> ShuffledCards()
        {
            var primer = Deck.Primer();
            var cards = new List<Card>();
            Random rnd = new Random();
            var count = 0;
            while(count < 52)
            {
                var selectedCard = primer[rnd.Next(primer.Count)];
                if (!cards.Contains(selectedCard))
                {
                    cards.Add(selectedCard);
                    count++;
                }
            }        
            return cards;
        }
    }
}
