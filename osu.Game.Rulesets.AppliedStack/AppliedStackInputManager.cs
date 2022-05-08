using System.ComponentModel;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.AppliedStack
{
    public class AppliedStackInputManager : RulesetInputManager<AppliedStackAction>
    {
        public AppliedStackInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum AppliedStackAction
    {
        [Description("Rotate Left")]
        RotateLeft,

        [Description("Rotate Right")]
        RotateRight,

        [Description("Move Left")]
        MoveLeft,

        [Description("Move Right")]
        MoveRight,

        [Description("Soft Drop")]
        SoftDrop,

        [Description("Hard Drop")]
        HardDrop,
        [Description("Hold")]
        Hold,
    }
}
