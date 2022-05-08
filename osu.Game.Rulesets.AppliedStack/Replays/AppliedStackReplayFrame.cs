using System.Collections.Generic;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.AppliedStack.Replays
{
    public class AppliedStackReplayFrame : ReplayFrame
    {
        public List<AppliedStackAction> Actions = new List<AppliedStackAction>();

        public AppliedStackReplayFrame(int rotationDir, int moveDir)
        {
            switch (rotationDir)
            {
                case -1:
                    Actions.Add(AppliedStackAction.RotateLeft);
                    break;
                case 1:
                    Actions.Add(AppliedStackAction.RotateRight);
                    break;
                default: break;
            }
            switch (moveDir)
            {
                case -1:
                    Actions.Add(AppliedStackAction.MoveLeft);
                    break;
                case 1:
                    Actions.Add(AppliedStackAction.MoveRight);
                    break;
                default: break;
            }
        }

        public AppliedStackReplayFrame(AppliedStackAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }
    }
}
