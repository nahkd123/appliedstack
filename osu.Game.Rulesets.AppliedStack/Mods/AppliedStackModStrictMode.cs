using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.AppliedStack.Beatmaps;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModStrictMode : Mod, IApplicableToBeatmapConverter
    {
        [SettingSource("Beat Divisor", "Forced hard drops frequency")]
        public Bindable<int> BeatDivisor { get; } = new BindableInt(2) { MinValue = 2, MaxValue = 16, Precision = 1 };

        public override string Name => "Strict Mode";
        public override string Acronym => "SM";
        public override string Description => "Increase forced hard drop hit objects density.";
        public override ModType Type => ModType.DifficultyIncrease;
        public override double ScoreMultiplier => 1.25;

        public void ApplyToBeatmapConverter(IBeatmapConverter converter)
        {
            if (!(converter is AppliedStackBeatmapConverter asbc)) return;
            asbc.HardBeatDivisor = BeatDivisor.Value;
        }
    }
}