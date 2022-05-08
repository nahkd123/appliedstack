using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.AppliedStack.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Mods;
using osu.Game.Rulesets.AppliedStack.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Configuration;
using osu.Game.Configuration;
using osu.Game.Rulesets.AppliedStack.Configuration;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.AppliedStack
{
    public class AppliedStackRuleset : Ruleset
    {
        public const int BOARD_WIDTH = 10;
        public const int BOARD_HEIGHT = 20;
        public const int PIECE_SPAWN_HEIGHT = 22;

        public override string ShortName => "appliedstack";

        public override string Description => "applied!stack";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableAppliedStackRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new AppliedStackBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new AppliedStackDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new AppliedStackModUnlockJudgement(),
                    };
                    
                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new AppliedStackModStrictMode(),
                        new AppliedStackModHidden(),
                    };

                case ModType.Automation:
                    return new Mod[]
                    {
                        new AppliedStackModAutoplay(),
                        new AppliedStackModRelax(),
                    };

                case ModType.Fun:
                    return new Mod[]
                    {
                        new AppliedStackModToTheBeat(),
                        new AppliedStackModFadingTiles(),
                        new AppliedStackModUpsideDown(),
                    };

                default:
                    return new Mod[] { null };
            }
        }

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Z, AppliedStackAction.RotateLeft),
            new KeyBinding(InputKey.Z, AppliedStackAction.RotateLeft),
            new KeyBinding(InputKey.X, AppliedStackAction.RotateRight),
            new KeyBinding(InputKey.Up, AppliedStackAction.RotateRight),
            new KeyBinding(InputKey.C, AppliedStackAction.Hold),
            new KeyBinding(InputKey.LShift, AppliedStackAction.Hold),
            new KeyBinding(InputKey.Left, AppliedStackAction.MoveLeft),
            new KeyBinding(InputKey.Right, AppliedStackAction.MoveRight),
            new KeyBinding(InputKey.Down, AppliedStackAction.SoftDrop),
            new KeyBinding(InputKey.Space, AppliedStackAction.HardDrop),
        };

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new AppliedStackConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new AppliedStackSettingsSubsection(this);

        public override Drawable CreateIcon() => new AppliedStackIcon();
    }
}
