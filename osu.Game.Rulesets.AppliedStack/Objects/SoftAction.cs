using osu.Game.Rulesets.AppliedStack.Judgements;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.AppliedStack.Objects
{
    public class SoftAction : AppliedStackHitObject
    {
        public override Judgement CreateJudgement() => new SoftJudgement();
        protected override HitWindows CreateHitWindows() => new HitWindows();
    }
}
