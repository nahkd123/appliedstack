using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Rulesets.AppliedStack.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModHidden : ModHidden
    {
        public override string Name => "Hidden";
        public override string Acronym => "HD";
        public override string Description => "Sneaky little notes.";
        public override double ScoreMultiplier => 1.15;

        public const int fade_duration_ms = 200;
        public const int fade_start_offset = 600;

        protected override void ApplyNormalVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
            double hitStart = hitObject.HitObject.StartTime;
            double hideStart = hitStart - fade_start_offset;

            using (hitObject.BeginAbsoluteSequence(hideStart))
            {
                hitObject.FadeOut(fade_duration_ms);
            }
        }

        protected override void ApplyIncreasedVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {}
    }
}