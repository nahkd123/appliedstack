using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.AppliedStack.Objects.Drawables
{
    public class DrawableStackingResult : FillFlowContainer
    {
        public HitResult HitResult { get; set; }
        public StackingResultType StackType { get; set; }

        private OsuSpriteText judgementText;
        private OsuSpriteText stackText;

        public DrawableStackingResult()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;
            Direction = FillDirection.Vertical;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour osuColour)
        {
            if (HitResult != HitResult.None) AddInternal(judgementText = new OsuSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Text = HitResult switch
                {
                    HitResult.Miss => "Miss",
                    HitResult.Meh => "Meh",
                    HitResult.Ok => "Ok",
                    HitResult.Great => "Great",
                    _ => "Unknown"
                },
                Colour = osuColour.ForHitResult(HitResult),
                Font = OsuFont.Numeric.With(size: 12f),
            });
            if (StackType != StackingResultType.Regular) AddInternal(stackText = new OsuSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Text = StackType switch
                {
                    StackingResultType.Single => "Single",
                    StackingResultType.Double => "Double",
                    StackingResultType.Triple => "Triple",
                    StackingResultType.Quadruple => "Quadruple",
                    StackingResultType.TSpinSingle => "T-spin Single!",
                    StackingResultType.TSpinDouble => "T-spin Double!!",
                    StackingResultType.TSpinTriple => "T-spin Triple!!!",
                    _ => "727"
                },
                Colour = AppliedStackColours.ForStackingResult(StackType),
                Font = OsuFont.Numeric.With(size: 12f),
            });
        }

        private void HitAnimation()
        {
            using (this.BeginDelayedSequence(0))
            {
                this.FadeOut(1500);
                if (judgementText != null) judgementText.TransformSpacingTo(new Vector2(5f, 1f), 1500, Easing.OutQuint);
                if (stackText != null) stackText.TransformSpacingTo(new Vector2(5f, 1f), 1500, Easing.OutQuint);
            }
        }

        public void PlayAnimation()
        {
            if (HitResult == HitResult.Miss) return;
            else HitAnimation();
        }
    }

    public enum StackingResultType
    {
        Regular = 0,
        Single = 1,
        Double = 2,
        Triple = 3,
        Quadruple = 4,

        TSpinSingle = 5,
        TSpinDouble = 6,
        TSpinTriple = 7,
    }
}