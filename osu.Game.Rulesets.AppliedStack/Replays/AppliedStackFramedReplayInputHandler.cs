using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.StateChanges;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;
using static osu.Game.Input.Handlers.ReplayInputHandler;

namespace osu.Game.Rulesets.AppliedStack.Replays
{
    public class AppliedStackFramedReplayInputHandler : FramedReplayInputHandler<AppliedStackReplayFrame>
    {
        public AppliedStackFramedReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        protected override bool IsImportant(AppliedStackReplayFrame frame) => frame.Actions.Any();

        protected override void CollectReplayInputs(List<IInput> inputs)
        {
            inputs.Add(new AppliedStackReplayState
            {
                PressedActions = CurrentFrame?.Actions ?? new List<AppliedStackAction>(),
            });
        }
    }

    public class AppliedStackReplayState : ReplayState<AppliedStackAction>
    {
    }
}
