using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.AppliedStack.Judgements
{
    public class HardJudgement : Judgement
    {
        protected override double HealthIncreaseFor(HitResult result) => 0.005;

        public override HitResult MaxResult => HitResult.Great;
    }
}