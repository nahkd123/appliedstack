using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects.Drawables
{
    public abstract class DrawableAppliedStackHitObject : DrawableHitObject<AppliedStackHitObject>
    {
        // Ok istg I'll add empty action enum. Also this value doesn't accepts move +
        // soft drop anyways.
        public AppliedStackAction LastAction { get; set; }
        protected AppliedStackAction HoldingAction { get; set; }

        public DrawableAppliedStackHitObject(AppliedStackHitObject hitObject)
            : base(hitObject)
        {
        }

        public AppliedStackAction[] ValidActions
        {
            get {
                return HitObject.LockAllActions? new[]
                {
                    AppliedStackAction.HardDrop,
                    AppliedStackAction.Hold,
                    AppliedStackAction.MoveLeft,
                    AppliedStackAction.MoveRight,
                    AppliedStackAction.RotateLeft,
                    AppliedStackAction.RotateRight,
                    AppliedStackAction.SoftDrop,
                } : new[]
                {
                    AppliedStackAction.HardDrop
                };
            }
        }

        public bool IsActionValid(AppliedStackAction action) => ValidActions.Where(v => v == action).Any();
    }
}