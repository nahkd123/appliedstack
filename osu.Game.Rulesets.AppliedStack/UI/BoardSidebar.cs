using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.UI
{
    public class BoardSidebar : Container
    {
        public BoardSidebar()
        {
            Width = 4;
            Height = AppliedStackRuleset.BOARD_HEIGHT;
        }
    }
}