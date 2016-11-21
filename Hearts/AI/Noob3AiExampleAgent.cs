﻿using System.Collections.Generic;
using System.Linq;
using Hearts.Extensions;
using Hearts.Factories;
using Hearts.Model;

namespace Hearts.AI
{
    public class Noob3AiExampleAgent : IAgent
    {
        public string AgentName { get { return "Noob 2 AI"; } }
        
        public IEnumerable<Card> ChooseCardsToPass(IEnumerable<Card> startingCards, Pass pass)
        {
            // Basic "pass your highest cards" strategy
            var cards = startingCards.OrderByDescending(i => i.Kind).ThenBy(i => i.Suit).ToList();

            return new List<Card>
            {
                cards[0],
                cards[1],
                cards[2]
            };
        }

        public Card ChooseCardToPlay(Round gameState, IEnumerable<Card> startingCards, IEnumerable<Card> availableCards, IEnumerable<Card> legalCards)
        {
            if (legalCards.Count() == 1)
            {
                return legalCards.First();
            }

            // Cut cards down to matching suit if appropriate
            if (!gameState.IsLeadTurn)
            {
                var constrainedSuit = gameState.CurrentTrick.First().Card.Suit;

                if (availableCards.Any(i => i.Suit == constrainedSuit))
                {
                    availableCards = availableCards.Where(i => i.Suit == constrainedSuit).ToList();
                }
                else
                {
                    // Let's make our noob AI at least slightly viscious
                    // Queen someone at the first opportunity
                    if (legalCards.Any(i => i.Kind == Kind.Queen && i.Suit == Suit.Spades))
                    {
                        return legalCards.Single(i => i.Kind == Kind.Queen && i.Suit == Suit.Spades);
                    }
                    else
                    {
                        // Noob 2 is slightly improved in that it punishes people with Hearts at the first opportunity
                        if (legalCards.Any(i => i.Suit == Suit.Hearts))
                        {
                            return legalCards.Where(i => i.Suit == Suit.Hearts).OrderByDescending(i => i.Kind).First();
                        }
                    }
                }
            }

            // Noob 3 will win suit if it's safe
            if (gameState.CurrentTrick.Count == 3)
            {
                var suit = gameState.CurrentTrick.First().Card.Suit;

                if (legalCards.Any(i => i.Suit == suit) && gameState.CurrentTrick.SelectCards().Score() == 0)
                {
                    return legalCards.Where(i => i.Suit == suit).Highest();
                }
            }

            // Return any low card
            // Terrible plan in long term for a game, but gives a half chance of dodging the queen against other noob AIs
            return legalCards.OrderBy(i => i.Kind).ThenByDescending(i => i.Suit).First();
        }
    }
}