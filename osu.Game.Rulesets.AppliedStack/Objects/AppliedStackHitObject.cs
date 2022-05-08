using osu.Game.Rulesets.AppliedStack.Judgements;
using osu.Game.Rulesets.AppliedStack.Scoring;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.AppliedStack.Objects
{
    public abstract class AppliedStackHitObject : HitObject
    {
        public int ObjectSeed { get; set; } = 0;
        public ActionsLockingMode LockAllActions { get; set; } = ActionsLockingMode.HardDrop;
    }

    public enum ActionsLockingMode
    {
        None,
        HardDrop,
        All,
    }
}
