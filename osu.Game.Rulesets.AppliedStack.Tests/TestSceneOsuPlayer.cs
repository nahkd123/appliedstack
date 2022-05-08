using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.AppliedStack.Tests
{
    [TestFixture]
    public class TestSceneOsuPlayer : PlayerTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new AppliedStackRuleset();
    }
}
