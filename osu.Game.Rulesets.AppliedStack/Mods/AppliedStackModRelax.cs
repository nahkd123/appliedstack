using System;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.AppliedStack.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModRelax : ModRelax, IApplicableToHitObject, IUpdatableByPlayfield
    {
        public override string Description => "Play with however you want.";

        public void ApplyToHitObject(HitObject hitObject)
        {
            if (!(hitObject is AppliedStackHitObject asho)) return;
            asho.LockAllActions = ActionsLockingMode.None;
        }

        public void Update(Playfield playfield)
        {
            if (!(playfield is AppliedStackPlayfield aspf)) return;
            if (aspf.HandlingActions.Length != 0) aspf.HandlingActions = new[]
            {
                AppliedStackAction.HardDrop,
                AppliedStackAction.Hold,
                AppliedStackAction.MoveLeft,
                AppliedStackAction.MoveRight,
                AppliedStackAction.RotateLeft,
                AppliedStackAction.RotateRight,
                AppliedStackAction.SoftDrop,
            };
        }
    }
}