using osu.Game.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Beatmaps;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModUnlockJudgement : Mod, IApplicableToBeatmapConverter
    {
        public override string Name => "Unlock Judgement";
        public override string Acronym => "UJ";
        public override string Description => "Allow hard drop without being forced to do it every beat.";
        public override ModType Type => ModType.DifficultyReduction;
        public override double ScoreMultiplier => 0.5;

        public void ApplyToBeatmapConverter(IBeatmapConverter converter)
        {
            if (!(converter is AppliedStackBeatmapConverter asbc)) return;
            asbc.AllSoft = true;
        }
    }
}