using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.AppliedStack.Objects.Drawables;
using osu.Game.Rulesets.AppliedStack.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.AppliedStack.UI
{
    [Cached]
    public class DrawableAppliedStackRuleset : DrawableScrollingRuleset<AppliedStackHitObject>
    {
        public DrawableAppliedStackRuleset(AppliedStackRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 6000;
        }

        protected override Playfield CreatePlayfield() => new AppliedStackPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new AppliedStackFramedReplayInputHandler(replay);

        public override DrawableHitObject<AppliedStackHitObject> CreateDrawableRepresentation(AppliedStackHitObject h)
        {
            switch (h)
            {
                case SoftAction soft: return new DrawableSoftAction(soft);
                case HardAction hard: return new DrawableHardAction(hard);
                default: return null;
            }
        }

        protected override PassThroughInputManager CreateInputManager() => new AppliedStackInputManager(Ruleset?.RulesetInfo);
    }
}
