using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.AppliedStack.Configuration;
using osu.Game.Rulesets.AppliedStack.Objects;
using osu.Game.Rulesets.AppliedStack.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.AppliedStack.UI
{
    public class AppliedStackSettingsSubsection : RulesetSettingsSubsection
    {
        protected override LocalisableString Header => "AppliedStack";
        
        public AppliedStackSettingsSubsection(AppliedStackRuleset ruleset) : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            if (!(Config is AppliedStackConfigManager manager)) return;

            Children = new Drawable[]
            {
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Auto repeat rate",
                    Current = manager.GetBindable<double>(AppliedStackSetting.AutoRepeatRate),
                    KeyboardStep = 5,
                },
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Delayed auto shift",
                    Current = manager.GetBindable<double>(AppliedStackSetting.DelayedAutoShift),
                    ShowsDefaultIndicator = true,
                    KeyboardStep = 5,
                },
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Soft drop rate",
                    Current = manager.GetBindable<double>(AppliedStackSetting.SoftDropRate),
                    ShowsDefaultIndicator = true,
                    KeyboardStep = 5,
                },
                new SettingsCheckbox
                {
                    LabelText = "Fancy animations",
                    Current = manager.GetBindable<bool>(AppliedStackSetting.FancyAnimations),
                    ShowsDefaultIndicator = true,
                }
            };
        }
    }
}