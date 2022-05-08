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
    public class AppliedStackModFadingTiles : Mod, IUpdatableByPlayfield
    {
        [SettingSource("Fade Delay", "Amount of time in milliseconds before fading occurs.")]
        public Bindable<double> FadeDelay { get; } = new BindableDouble(100) { MinValue = 100, MaxValue = 5000, Precision = 5 };
        
        [SettingSource("Fade Duration", "Fading duration in milliseconds.")]
        public Bindable<double> FadeDuration { get; } = new BindableDouble(1000) { MinValue = 100, MaxValue = 5000, Precision = 5 };

        public override string Name => "Fading Tiles";
        public override string Acronym => "FT";
        public override string Description => "Where are my tiles?";
        public override ModType Type => ModType.Fun;
        public override double ScoreMultiplier => 1.0;
        public override IconUsage? Icon => FontAwesome.Regular.Eye;

        public void Update(Playfield playfield)
        {
            if (!(playfield is AppliedStackPlayfield aspf)) return;
            DrawableBoard board = aspf.DrawableBoard;
            board.TrackNewTiles = true;

            if (board.NewTiles.Count > 0)
            {
                foreach (DrawableTileObject tile in board.NewTiles)
                {
                    using (tile.BeginDelayedSequence(FadeDelay.Value))
                    {
                        tile.FadeOut(FadeDuration.Value);
                    }
                }
                board.NewTiles.Clear();
            }
        }
    }
}