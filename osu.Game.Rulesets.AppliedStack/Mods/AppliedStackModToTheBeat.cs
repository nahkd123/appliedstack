using System;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.AppliedStack.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModToTheBeat : Mod, IApplicableToHitObject, IUpdatableByPlayfield
    {
        public override string Name => "To The Beat";
        public override string Acronym => "TB";
        public override string Description => "Do every actions to the beat.";
        public override ModType Type => ModType.Fun;
        public override double ScoreMultiplier => 1.0;
        public override Type[] IncompatibleMods => new[] {
            typeof(AppliedStackModAutoplay)
        };

        public void ApplyToHitObject(HitObject hitObject)
        {
            if (!(hitObject is AppliedStackHitObject asho)) return;
            asho.LockAllActions = ActionsLockingMode.All;
        }

        public void Update(Playfield playfield)
        {
            if (!(playfield is AppliedStackPlayfield aspf)) return;
            aspf.arr = aspf.das = aspf.sdr = 100_000;
            if (aspf.HandlingActions.Length != 0) aspf.HandlingActions = new AppliedStackAction[0];
        }
    }
}