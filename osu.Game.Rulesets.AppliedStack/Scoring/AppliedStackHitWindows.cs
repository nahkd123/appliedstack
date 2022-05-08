using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.AppliedStack.Scoring
{
    public class AppliedStackHitWindows : HitWindows
    {
        public override bool IsHitResultAllowed(HitResult result)
        {
            switch (result)
            {
                case HitResult.Great:
                case HitResult.Ok:
                case HitResult.Meh:
                case HitResult.Miss:
                case HitResult.SmallBonus:
                    return true;
                
                default:
                    return false;
            }
        }

        protected override DifficultyRange[] GetRanges() => new[]
        {
            new DifficultyRange(HitResult.Great, 80, 50, 20),
            new DifficultyRange(HitResult.Ok, 140, 100, 60),
            new DifficultyRange(HitResult.Meh, 200, 150, 100),
            new DifficultyRange(HitResult.Miss, 400, 400, 400),
        };
    }
}