using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.AppliedStack.Objects.Drawables;
using osu.Game.Rulesets.AppliedStack.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModUpsideDown : Mod, IUpdatableByPlayfield
    {
        public override string Name => "Upside Down";
        public override string Acronym => "UD";
        public override string Description => "Reverse stacking.";
        public override ModType Type => ModType.Fun;
        public override double ScoreMultiplier => 1.0;
        public override IconUsage? Icon => FontAwesome.Solid.Cloud;

        public void Update(Playfield playfield)
        {
            if (!(playfield is AppliedStackPlayfield aspf)) return;
            aspf.ScaledContainer.Rotation = 180f;
        }
    }
}