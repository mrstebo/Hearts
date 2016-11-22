﻿using Hearts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearts.Rules
{
    public class CannotLeadHeartsUntilBrokenRule : IGameRule
    {
        public IEnumerable<Card> FilterCards(IEnumerable<Card> cards, Round round)
        {
            var nonHearts = cards.Where(i => i.Suit != Suit.Hearts).ToList();

            return nonHearts.Count > 0 ? nonHearts : cards;
        }

        public bool Applies(Round round)
        {
            return round.IsLeadTurn && !round.IsHeartsBroken;
        }
    }
}
