using osu.Framework.Configuration.Tracking;
using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.AppliedStack.Configuration
{
    public class AppliedStackConfigManager : RulesetConfigManager<AppliedStackSetting>
    {
        public AppliedStackConfigManager(SettingsStore store, RulesetInfo ruleset, int? variant = null) : base(store, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetDefault(AppliedStackSetting.AutoRepeatRate, 50.0, 0.0, 1000.0);
            SetDefault(AppliedStackSetting.DelayedAutoShift, 150.0, 0.0, 1000.0);
            SetDefault(AppliedStackSetting.SoftDropRate, 100.0, 0.0, 1000.0);
            SetDefault(AppliedStackSetting.FancyAnimations, true);
        }

        public override TrackedSettings CreateTrackedSettings() => new TrackedSettings
        {
            new TrackedSetting<double>(AppliedStackSetting.AutoRepeatRate, ms => new SettingDescription(ms, "Auto repeat rate", $"ARR: {ms}ms")),
            new TrackedSetting<double>(AppliedStackSetting.DelayedAutoShift, ms => new SettingDescription(ms, "Delayed auto shift", $"DAS: {ms}ms")),
            new TrackedSetting<double>(AppliedStackSetting.SoftDropRate, ms => new SettingDescription(ms, "Soft drop rate", $"SDR: {ms}ms"))
        };
    }

    public enum AppliedStackSetting
    {
        AutoRepeatRate,
        DelayedAutoShift,
        SoftDropRate,
        FancyAnimations,
    }
}