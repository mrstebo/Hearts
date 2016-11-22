﻿using Hearts.Deal;
using Hearts.Factories;
using Hearts.Model;
using Hearts.Passing;
using Hearts.Rules;
using Hearts.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using Hearts.Extensions;
using Hearts.Logging;

namespace Hearts
{
    public class GameManager
    {
        private PlayerCircle playerCircle;
        private Dictionary<Player, PlayerCards> playerCards;
        private Round round;
        private Dealer dealer;

        public GameManager(IEnumerable<Player> players)
        {
            this.playerCircle = new PlayerCircle();
            this.AddPlayers(players);
            this.Reset();
        }

        public void Reset()
        {
            this.dealer = new Dealer(new StandardDeckFactory(), new EvenHandDealAlgorithm());
            this.playerCards = this.playerCircle.AllPlayers.ToDictionary(i => i, i => new PlayerCards());

            if (this.round != null)
            {
                this.round.Reset();
            }
        }

        public RoundResult Play(int roundIndex)
        {
            this.Reset();
            var players = this.playerCircle.AllPlayers;
            this.round = new Round(players.Count, roundIndex);
            var startingHands = this.dealer.DealStartingHands(players);

            foreach (var startingHand in startingHands)
            {
                this.playerCards[startingHand.Key].StartingHands = startingHand.Value;
                this.playerCards[startingHand.Key].RemainingCards = startingHand.Value.ToList();
            }

            //this.remainingCards = this.startingHands.ToDictionary(i => i.Key, i => i.Value.ToList().AsEnumerable()); // Simple Clone

            // TODO: Logging - reinstate
            //Log.StartingHands(this.startingHands);

            //this.postPassHands = new PassService().OrchestratePassing(roundIndex, players, startingHands, this.playerCircle.FirstPlayer);

            foreach (var postPassHand in new PassService().OrchestratePassing(roundIndex, startingHands, this.playerCircle.FirstPlayer))
            {
                this.playerCards[postPassHand.Key].PostPassHands = postPassHand.Value;
            }

            // TODO: Logging - reinstate
            //Log.HandsAfterPass(this.postPassHands);

            var handEvaluator = new HandWinEvaluator();
            var rulesEngine = new GameRulesEngine();
            var startingPlayer = this.playerCircle.GetStartingPlayer(this.playerCards);

            while (this.playerCards.Select(i => i.Value.RemainingCards.Count()).Sum() > 0)
            {
                this.round.BeginTrick();

                foreach (var player in this.playerCircle.GetOrderedPlayersStartingWith(startingPlayer))
                {
                    var playerRemainingCards = this.playerCards[player].RemainingCards;
                    this.playerCards[player].LegalCards = rulesEngine.GetPlayableCards(playerRemainingCards, this.round);
                    var card = player.Agent.ChooseCardToPlay(this.round, this.playerCards[player]);

                    if (!this.playerCards[player].LegalCards.Contains(card))
                    {
                        // TODO: Handle illegal move
                        Log.IllegalPlay(player, card);
                        player.AgentHasMadeIllegalMove = true;
                        card = this.playerCards[player].LegalCards.First();
                    }

                    this.playerCards[player].RemainingCards = playerRemainingCards.ExceptCard(card);

                    this.round.Play(player, card);
                }

                this.round.EndTrick();
                var trick = this.round.PlayedTricks.Last();
                var trickWinner = handEvaluator.EvaluateWinner(trick);
                trick.Winner = trickWinner;
                startingPlayer = trick.Winner;

                Log.TrickSummary(trick);
            }

            var scores = players.ToDictionary(i => i, i => new ScoreEvaluator().CalculateScore(this.round.PlayedTricks.Where(j => j.Winner == i)));
            var tricks = players.ToDictionary(i => i, i => this.round.PlayedTricks.Where(j => j.Winner == i).ToList());

            Log.PointsForRound(scores);

            return new RoundResult
            {
                Scores = scores,
                Tricks = tricks
            };
        }

        private void AddPlayers(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                this.playerCircle.AddPlayer(player);
            }
        }
    }
}
