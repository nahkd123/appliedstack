using osu.Game.Rulesets.AppliedStack.Judgements;
using osu.Game.Rulesets.AppliedStack.Scoring;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.AppliedStack.Objects
{
    public class HardAction : AppliedStackHitObject
    {
        public override Judgement CreateJudgement() => new HardJudgement();
        protected override HitWindows CreateHitWindows() => new AppliedStackHitWindows();
    }
}
