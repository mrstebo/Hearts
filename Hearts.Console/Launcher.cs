﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hearts;
using Hearts.Extensions;

namespace Hearts.Console
{
    public class Launcher
    { 
        public static void Main()
        {
            var game = new Game();

            var card = game.Players.First().RemainingCards.First();

            var test = card.ToAbbreviation();
            int i = game.Players.First().RemainingCards.Count();
        }
    }
}
