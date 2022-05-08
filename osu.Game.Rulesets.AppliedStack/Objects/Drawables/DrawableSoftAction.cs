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
    public class DrawableSoftAction : DrawableAppliedStackHitObject, IKeyBindingHandler<AppliedStackAction>
    {
        public DrawableSoftAction()
            : this(null)
        {
        }

        public DrawableSoftAction(SoftAction hitObject)
            : base(hitObject)
        {
            Size = new Vector2(40);
            Origin = Anchor.Centre;

            // todo: add visuals.
            AddInternal(new Box
            {
                Colour = new Color4(1f, 1f, 1f, 0.5f),
                Width = 10, Height = 100
            });
        }
        
        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (HitObject.LockAllActions == ActionsLockingMode.None)
                {
                    if (timeOffset >= 0) ApplyResult(r => r.Type = HitResult.LargeBonus);
                }
                else
                {
                    if (!HitObject.HitWindows.CanBeHit(timeOffset)) ApplyResult(r => r.Type = HitResult.IgnoreMiss);
                }
                return;
            }

            HitResult result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None) return;

            LastAction = HoldingAction;
            ApplyResult(r => r.Type = HitResult.LargeBonus);
        }

        public bool OnPressed(KeyBindingPressEvent<AppliedStackAction> e)
        {
            if (!IsActionValid(e.Action)) return false;
            
            HoldingAction = e.Action;
            return UpdateResult(true);
        }

        public void OnReleased(KeyBindingReleaseEvent<AppliedStackAction> e)
        {}

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            const double duration = 1000;

            switch (state)
            {
                case ArmedState.Hit:
                    this.FadeOut(duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, duration);
                    this.FadeOut(duration, Easing.InQuint).Expire();
                    break;
            }
        }
    }
}
