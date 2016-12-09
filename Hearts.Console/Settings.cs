﻿
using System.Collections.Generic;
using Hearts.AI;
using Hearts.Model;
using QueenCatcherBot;
using Hearts.Events;
using BenRead.Hearts;
using HeartsCrusher.Agents;
using Hearts.Console.Simulations;

namespace Hearts.Console
{
    public class Settings
    {
        // Controlled Randomisation
        public static bool UseFixedSeed = true;
        public static int FixedSeed = 8048609;
        public static EventNotifier Notifier = new EventNotifier();

        // Note: This simulates at about 300 games per second, without console outputs, depending on the efficiency of the bots used
        //
        //      1 game  - 0s
        //  1,000 games - 3s
        // 10,000 games - 30s
        public static int GameSimulationCount = 100;

        public static SimulationType SimulationType = SimulationType.Standard;

        // True:    Shows full game breakdown
        // True:    Shows summary
        public static bool ShowFullOutput = GameSimulationCount == 1 
            && SimulationType == SimulationType.Standard;

        // Available bots: (* = Doesn't currently build against current IAgent signature)
        // 
        //    • TerribleRandomAiAgent                                       0ms
        //    • Noob1AiExampleAgent                                         0ms
        //    • Noob2AiExampleAgent                                         0ms
        //    • Noob3AiExampleAgent                                         0ms
        //    • SavageBeast                                 Adam Hill       3ms
        //    • NoobCrusherV1                               Tony Beasley    0ms
        //    • NoobCrusherV2                               Tony Beasley    0ms
        //    • NoobCrusherV3                               Tony Beasley    0ms
        //    • ShootCrusher                                Tony Beasley    0ms
        //    • Craghoul                                    Craig Rowe      0ms
        //    • Deathstar                                   James Robinson  0ms
        //    • QueenCatcher                                Dan White       0ms
        //    • DefensiveAfter90							Ben Read		0ms  
        //    • TrashBot                                    Mary Hyde       0ms

        public static HeartsPlayerList Bots = new HeartsPlayerList
            {
				new ShootCrusher(allowShoot: true, allowAntiShoot: true),
				new Deathstar(allowShoot: true, allowAntiShoot: true),
				new DefensiveAfter90(),
                new SavageBeast(Notifier, allowShoot: true, allowAntiShoot: true, allowParallel: false, useNewShootAlgorithm: true)
            };
    }
}
