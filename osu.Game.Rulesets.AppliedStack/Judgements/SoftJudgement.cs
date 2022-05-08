using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.AppliedStack.Judgements
{
    public class SoftJudgement : Judgement
    {
        protected override double HealthIncreaseFor(HitResult result) => 0.002;

        public override HitResult MaxResult => HitResult.LargeBonus;
    }
}