﻿using Hearts.Factories;
using Hearts.Model;
using System.Collections.Generic;

namespace Hearts.AI
{
    public interface IAgent
    {
        // An fixed (optionally arbitrary) name for your AI, that allows other AIs to adjust to it
        string AgentName { get; }

        // The owning player
        Player Player { get; set; }

        IEnumerable<Card> ChooseCardsToPass(IEnumerable<Card> startingCards, Pass pass); // TODO: Add player I'm passing to, Add player I'm receiving from

        Card ChooseCardToPlay(Round gameState, PlayerHolding holding);
    }
}