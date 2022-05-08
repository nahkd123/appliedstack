using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.AppliedStack
{
    public class AppliedStackDifficultyCalculator : DifficultyCalculator
    {
        public AppliedStackDifficultyCalculator(IRulesetInfo ruleset, IWorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, double clockRate)
        {
            if (!(beatmap is AppliedStackBeatmap asBeatmap)) return new DifficultyAttributes(mods, 0);
            
            double starRating = 0.0;
            {
                double lowestLocalSr = 727.0;
                double highestLocalSr = 0.0;
                double lastSoftActionsTime = asBeatmap.HitObjects[0].StartTime;

                for (int i = 1; i < asBeatmap.HitObjects.Count; i++)
                {
                    AppliedStackHitObject obj = asBeatmap.HitObjects[i];
                    double timeDeltaSoft = obj.StartTime - lastSoftActionsTime;
                    if (Math.Abs(timeDeltaSoft) <= 5) continue;

                    double srLocal = Math.Min(1.0 / (timeDeltaSoft / 1000.0), 727);
                    if (srLocal < lowestLocalSr) lowestLocalSr = srLocal;
                    if (srLocal > highestLocalSr) highestLocalSr = srLocal;
                    lastSoftActionsTime = obj.StartTime;
                }

                starRating = (lowestLocalSr * 0.4 + highestLocalSr * 0.6) / 2.3;
            }

            return new DifficultyAttributes(mods, starRating);
        }

        protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, double clockRate) => Enumerable.Empty<DifficultyHitObject>();

        protected override Skill[] CreateSkills(IBeatmap beatmap, Mod[] mods, double clockRate) => Array.Empty<Skill>();
    }
}
